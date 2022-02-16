using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsekCodingChallenge.Models
{
    public class ReturnData
    {
        public int SuccesfullReadings { get; set; }
        public int FailedReadings { get; set; }

        public ReturnData(int succesfullReadings, int failedReadings)
        {
            this.SuccesfullReadings = succesfullReadings;
            this.FailedReadings = failedReadings;
        }

    }
}
