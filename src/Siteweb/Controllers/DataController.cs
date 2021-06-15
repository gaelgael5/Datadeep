using Bb.DataDeep.Models.Manifests;
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

        [HttpGet]
        public ManifestModel Get1()
        {

            return Program.Instance.Manifest;

        }

    }
}
