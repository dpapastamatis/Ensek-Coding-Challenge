using EnsekCodingChallenge.Implementations;
using EnsekCodingChallenge.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EnsekCodingChallenge.Tests
{
    public class CSVFileMeterReadingProcessTests
    {

        private CSVFileMeterReadingProcess service;

        public CSVFileMeterReadingProcessTests()
        {
            var accountsSample = new List<Account>
            {
                new Account { AccountID = 1234, First_name = "Freya", Last_name = "Test" },
                new Account { AccountID = 1240, First_name = "Archie", Last_name = "Test" },
                new Account { AccountID = 1239, First_name = "Nody", Last_name = "Test" }
            }.AsQueryable();

            var mockAccountsSet = new Mock<DbSet<Account>>();
            mockAccountsSet.As<IQueryable<Account>>().Setup(m => m.Provider).Returns(accountsSample.Provider);
            mockAccountsSet.As<IQueryable<Account>>().Setup(m => m.Expression).Returns(accountsSample.Expression);
            mockAccountsSet.As<IQueryable<Account>>().Setup(m => m.ElementType).Returns(accountsSample.ElementType);
            mockAccountsSet.As<IQueryable<Account>>().Setup(m => m.GetEnumerator()).Returns(accountsSample.GetEnumerator());

            var meterReadingsSample = new List<MeterReading>
            {
                new MeterReading { AccountID = 1234, MeterReadingDateTime = new DateTime(2019, 12, 31), MeterReadValue = 1002 },
                new MeterReading { AccountID = 1240, MeterReadingDateTime = new DateTime(2019, 11, 15), MeterReadValue = 998 },
                new MeterReading { AccountID = 1239, MeterReadingDateTime = new DateTime(2020, 9, 1), MeterReadValue = 800 },
                new MeterReading { AccountID = 1240, MeterReadingDateTime = new DateTime(2021, 3, 4), MeterReadValue = 2003 },
                new MeterReading { AccountID = 1239, MeterReadingDateTime = new DateTime(2021, 9, 8), MeterReadValue = 1542 },
            }.AsQueryable();

            var mockMeterReadingsSet = new Mock<DbSet<MeterReading>>();
            mockMeterReadingsSet.As<IQueryable<MeterReading>>().Setup(m => m.Provider).Returns(meterReadingsSample.Provider);
            mockMeterReadingsSet.As<IQueryable<MeterReading>>().Setup(m => m.Expression).Returns(meterReadingsSample.Expression);
            mockMeterReadingsSet.As<IQueryable<MeterReading>>().Setup(m => m.ElementType).Returns(meterReadingsSample.ElementType);
            mockMeterReadingsSet.As<IQueryable<MeterReading>>().Setup(m => m.GetEnumerator()).Returns(meterReadingsSample.GetEnumerator());

            var mockContext = new Mock<ApplicationDbContext>();
            mockContext.Setup(c => c.Accounts).Returns(mockAccountsSet.Object);
            mockContext.Setup(c => c.MeterReadingSet).Returns(mockMeterReadingsSet.Object);

            service = new CSVFileMeterReadingProcess(mockContext.Object);
        }

        [Fact]
        public void ValidateExistingAccount_ReturnsTrueIfValidAccount()
        {
            //Arrange
            bool expected = true;
            var newMeterReading = new MeterReading { AccountID = 1234, MeterReadingDateTime = new DateTime(2020, 12, 31), MeterReadValue = 1000 };

            //Act
            bool actual = service.ValidateExistingAccount(newMeterReading.AccountID);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ValidateExistingAccount_ReturnsFalseIfInvalidAccount()
        {
            //Arrange
            bool expected = false;
            var newMeterReading = new MeterReading { AccountID = 1299, MeterReadingDateTime = new DateTime(2020, 12, 31), MeterReadValue = 1000 };

            //Act
            bool actual = service.ValidateExistingAccount(newMeterReading.AccountID);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsDuplicate_ReturnsTrueIfDuplicate()
        {
            //Arrange
            bool expected = true;
            var newMeterReading = new MeterReading { AccountID = 1234, MeterReadingDateTime = new DateTime(2019, 12, 31), MeterReadValue = 1000 };

            //Act
            bool actual = service.IsDuplicate(newMeterReading);

            //Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(CSVFileMeterReadingProcessTests.GetMeterReadingTestObjects), MemberType = typeof(CSVFileMeterReadingProcessTests))]
        public void IsDuplicate_ReturnsFalseIfNotDuplicate(MeterReading testObject1, MeterReading testObject2)
        {
            //Arrange
            bool expected = false;

            //Act
            bool testObject1actual = service.IsDuplicate(testObject1);
            bool testObject2actual = service.IsDuplicate(testObject2);

            //Assert
            Assert.Equal(expected, testObject1actual);
            Assert.Equal(expected, testObject2actual);
        }

        [Theory]
        [InlineData(1299)]
        [InlineData(1234)]
        public void ValidateNewEntryIsNotOlderThanCurrent_ReturnsTrueWhenThereIsNoMoreRecentReading(int accountID)
        {
            //Arrange
            bool expected = true;
            var newMeterReading = new MeterReading { AccountID = accountID, MeterReadingDateTime = new DateTime(2020, 12, 31), MeterReadValue = 1000 };

            //Act
            bool actual = service.ValidateNewEntryIsNotOlderThanCurrent(newMeterReading);

            //Assert
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ValidateNewEntryIsNotOlderThanCurrent_ReturnsFalseWhenThereIsMoreRecentReading()
        {
            //Arrange
            bool expected = true;
            var newMeterReading = new MeterReading { AccountID = 1234, MeterReadingDateTime = new DateTime(2020, 12, 31), MeterReadValue = 1000 };

            //Act
            bool actual = service.ValidateNewEntryIsNotOlderThanCurrent(newMeterReading);

            //Assert
            Assert.Equal(expected, actual);
        }


        public static IEnumerable<object[]> GetMeterReadingTestObjects()
        {
            yield return new object[]
            {
            new MeterReading { AccountID = 1299, MeterReadingDateTime = new DateTime(2019, 11, 15), MeterReadValue = 1000 },
            new MeterReading {AccountID = 1234, MeterReadingDateTime = new DateTime(2020, 12, 31), MeterReadValue = 1000 }
            };
        }
    }
}
