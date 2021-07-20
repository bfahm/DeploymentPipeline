using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpDeploy.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> Deploy(string git_email, string git_token, string project_id)
        {
            Deployer deployer = new Deployer();
            return await deployer.DeployDotNet(project_id, git_email, git_token);
        }
    }
}
