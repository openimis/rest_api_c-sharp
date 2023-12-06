using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenImis.Security.Security;
using OpenImis.ModulesV3;
using OpenImis.ModulesV3.MainItemModule;
using Newtonsoft.Json;

namespace OpenImis.RestApi.Controllers.V3 { 
    
    [ApiVersion("3")]
    [AllowAnonymous]
    [Route("api/")]
    [ApiController]

    public class MainItemController : Controller
    {
        public MainItemController()
        {
        }
        //[HasRights(Rights.DiagnosesDownload)]
        [HttpGet]   //To allow the sending of data that could be used by the consumers
        [Route("GetListMainItemItems")]


        public IActionResult GetListMainItemItems()
        {
            ClassMainItemRequest getRequest = new ClassMainItemRequest();
            var response = getRequest.SerializeDr();
            return Json(response);
        }

    }
}
