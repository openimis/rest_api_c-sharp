using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenImis.Security.Security;
using OpenImis.ModulesV3;
using OpenImis.ModulesV3.ServiceAllModule;
using Newtonsoft.Json;

namespace OpenImis.RestApi.Controllers.V3 { 
    
    [ApiVersion("3")]
    [AllowAnonymous]
    [Route("api/")]
    [ApiController]

    public class ServiceAllController : Controller
    {
        public ServiceAllController()
        {
        }
        //[HasRights(Rights.DiagnosesDownload)]
        [HttpGet]   //To allow the sending of data that could be used by the consumers
        [Route("GetListServiceAllItems")]


        public IActionResult GetListServiceAllItems()
        {
            ClassServiceAllRequest getRequest = new ClassServiceAllRequest();
            var response = getRequest.SerializeDr();
            return Json(response);
        }

    }
}