using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPS.Data;


using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;



namespace RPS.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class PtItemsController : ControllerBase
    {
        private readonly IPtItemsRepository rpsItemsRepo;

        public PtItemsController(IPtItemsRepository rpsItemsData)
        {
            rpsItemsRepo = rpsItemsData;
        }

        [HttpGet]
        public DataSourceResult Get([DataSourceRequest]DataSourceRequest request)
        {
            var items = rpsItemsRepo.GetAll();

            return items.ToDataSourceResult(request);
        }

        //public IActionResult Index(DataSourceRequest request)
        //{
        //    var items = rpsItemsRepo.GetAll();
        //    return new JsonResult(items.ToDataSourceResult(request));
        //}

    }
}