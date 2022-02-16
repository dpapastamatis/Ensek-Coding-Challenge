using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using CsvHelper;
using CsvHelper.Configuration;
using EnsekCodingChallenge.Implementations;
using EnsekCodingChallenge.Interfaces;
using EnsekCodingChallenge.Models;

namespace EnsekCodingChallenge.Controllers.Api
{
    public class MeterReadingController : ApiController
    {
        private readonly IProcessService _processService;

        public MeterReadingController(IProcessService processService)
        {
            _processService = processService;
        }

        
        [HttpPost]
        public async Task<IHttpActionResult> ImportMeterReadings()
        {
            ReturnData returnSet;

            var file = HttpContext.Current.Request.Files.Count > 0 ?
                HttpContext.Current.Request.Files[0] : null;

            if (file == null)
            {
                ModelState.AddModelError("Error", "File was not provided.");
                return BadRequest(ModelState);
            }
            else
            {
                try
                {
                    var result = _processService.ProcessData(file);
                    returnSet = result.ReturnSet;
                    if (result.Records != null && result.Records.Count() > 0)
                    {
                        await _processService.SaveData(result.Records);
                    }
                }
                catch
                {
                    var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        Content = new StringContent("Unhandled Exception while processing the data.")
                    };
                    throw new HttpResponseException(response);
                }

                return Ok(returnSet);
            }
        }
    }
}