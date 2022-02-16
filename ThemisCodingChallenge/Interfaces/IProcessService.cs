using EnsekCodingChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace EnsekCodingChallenge.Interfaces
{
    public interface IProcessService
    {
        (ReturnData ReturnSet, IEnumerable<BaseModel> Records) ProcessData(HttpPostedFile file);
        Task SaveData(IEnumerable<BaseModel> records);
    }
}
