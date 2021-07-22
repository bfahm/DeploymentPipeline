using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SharpDeploy.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        private readonly Deployer _deployer;

        public HomeController(Deployer deployer)
        {
            _deployer = deployer;
        }

        public IActionResult Index()
        {
            var listOfApps = _deployer.GetListOfApplications();
            return View(listOfApps);
        }

        [HttpPost]
        public async Task<string> Deploy(string git_email, string git_token, string project_id)
        {
            return await _deployer.DeployDotNet(project_id, git_email, git_token);
        }
    }
}
