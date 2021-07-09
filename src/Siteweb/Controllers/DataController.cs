using Bb.DataDeep.Models.Manifests;
using Bb.DataDeep.Models.Mpd;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Siteweb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;

        public DataController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ManifestModel Get()
        {

            return Program.Instance.Manifest;

        }

        //[HttpGet]
        //public Package Get1(string id)
        //{

        //    var manifest = Program.Instance.Manifest;

        //    var result = manifest.Items.FirstOrDefault(c => c.Id == id);

        //    return result.Path;

        //}

    }
}
