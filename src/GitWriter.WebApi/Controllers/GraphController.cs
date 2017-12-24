﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GitWriter.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class GraphController : Controller
    {
        // GET api/graph
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var image = System.IO.File.OpenRead(@"C:\dev\WinGitWriter\bin\full.svg");
            return File(image, "image/svg+xml");
        }
    }
}