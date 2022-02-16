

using CsvHelper;
using CsvHelper.Configuration;
using EnsekCodingChallenge.Interfaces;
using EnsekCodingChallenge.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EnsekCodingChallenge.Implementations
{
    public class CSVFileMeterReadingProcess : IProcessService
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public CSVFileMeterReadingProcess(ApplicationDbContext context)
        {
            _context = context;
        }

        public (ReturnData, IEnumerable<BaseModel>) ProcessData(HttpPostedFile file)
        {
            List<MeterReading> records = new List<MeterReading>();
            int succesfullReadings = 0;
            int failedReadings = 0;
            bool success;
            var config = new CsvConfiguration(CultureInfo.CurrentCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null,
                HasHeaderRecord = true
            };

            using (var reader = new StreamReader(file.InputStream))
            {
                using (var csv = new CsvReader(reader, config))
                {
                    csv.Read(); //ignore header
                    while (csv.Read())
                    {
                        int accountID;
                        DateTime meterReadingDateTime;
                        long meterReadValue;
                        success = false;
                        var csvFieldAccountID = csv.GetField(0);
                        if (!String.IsNullOrEmpty(csvFieldAccountID) && int.TryParse(csvFieldAccountID, out accountID))
                        {
                            var csvFieldMeterReadingDateTime = csv.GetField(1);
                            if (!String.IsNullOrEmpty(csvFieldMeterReadingDateTime) && DateTime.TryParse(csvFieldMeterReadingDateTime, out meterReadingDateTime))
                            {
                                var csvFieldMeterReadValue = csv.GetField(2);
                                if (!String.IsNullOrEmpty(csvFieldMeterReadValue) && long.TryParse(csvFieldMeterReadValue, out meterReadValue))
                                {
                                    var newMeterReading = new MeterReading
                                                                (
                                                                    accountID,
                                                                    meterReadingDateTime,
                                                                    meterReadValue
                                                                );

                                    if (ValidateMeterReading(newMeterReading))
                                    {
                                        records.Add(
                                                    new MeterReading
                                                        (
                                                            accountID,
                                                            meterReadingDateTime,
                                                            meterReadValue
                                                        )
                                                    );
                                        success = true;
                                    }
                                }
                            }
                        }
                        if (success)
                        {
                            succesfullReadings++;
                        }
                        else
                        {
                            failedReadings++;
                        }
                    }
                }
            }
            return (new ReturnData
                        (
                            succesfullReadings,
                            failedReadings
                        ), (IEnumerable<BaseModel>)(records))
                    ;
        }

        public async Task SaveData(IEnumerable<BaseModel> records)
        {

               foreach (var record in records)
               {
                   _context.MeterReadingSet.Add((MeterReading)(record));
               }
               await _context.SaveChangesAsync();
        }

        public bool ValidateMeterReading(MeterReading meterReading)
        {
            if (ValidateExistingAccount(meterReading.AccountID) && !IsDuplicate(meterReading) && (ValidateNewEntryIsNotOlderThanCurrent(meterReading)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ValidateExistingAccount(int accountID)
        {
            return (_context.Accounts.Where(account => account.AccountID == accountID).Count() > 0) ? true : false;
        }

        public bool IsDuplicate(MeterReading meterReading)
        {
            return (_context.MeterReadingSet.Where(record => record.AccountID == meterReading.AccountID && record.MeterReadingDateTime == meterReading.MeterReadingDateTime).Count() > 0) ? true : false;
        }

        public bool ValidateNewEntryIsNotOlderThanCurrent(MeterReading meterReading)
        {
            var current = _context.MeterReadingSet.Where(record => record.AccountID == meterReading.AccountID).OrderByDescending(record => record.MeterReadingDateTime).FirstOrDefault();
            if (current != null)
            {
                if (meterReading.MeterReadingDateTime < current.MeterReadingDateTime)
                {
                    return false;
                }
            }
            return true;
        }

    }
}