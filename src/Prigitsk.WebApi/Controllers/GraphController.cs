using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Prigitsk.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class GraphController : Controller
    {
        // GET api/graph
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            FileStream image = System.IO.File.OpenRead(@"C:\dev\WinPrigitsk\bin\full.svg");
            return File(image, "image/svg+xml");
        }
    }
}