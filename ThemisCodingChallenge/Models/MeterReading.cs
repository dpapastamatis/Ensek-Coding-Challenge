using CsvHelper.Configuration.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace EnsekCodingChallenge.Models
{
    public class MeterReading: BaseModel
    {
        public int MeterReadingID { get; set; }

        public int AccountID { get; set; }

        public ApplicationUser Linked_User { get; set; }

        [Required]
        [Display(Name = "Meter Reading Date and Time")]
        public DateTime MeterReadingDateTime { get; set; }

        [Required]
        [Display(Name = "Meter Read Value")]
        public long MeterReadValue { get; set; }

        public MeterReading(int accountID, DateTime meterReadingDateTime, long meterReadValue)
        {
            AccountID = accountID;
            MeterReadingDateTime = meterReadingDateTime;
            MeterReadValue = meterReadValue;
        }

        public MeterReading() { }


    }
}