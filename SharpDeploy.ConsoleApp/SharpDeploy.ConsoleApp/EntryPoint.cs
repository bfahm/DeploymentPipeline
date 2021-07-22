using System;
using System.Threading.Tasks;

namespace SharpDeploy.ConsoleApp
{
    class EntryPoint
    {
        private readonly Deployer _deployer;

        public EntryPoint(Deployer deployer)
        {
            _deployer = deployer;
        }

        private void Index()
        {
            var listOfApps = _deployer.GetListOfApplications();

            Console.WriteLine("Available Projects:");
            Console.WriteLine("");


            foreach (var app in listOfApps)
            {
                Console.WriteLine($"ID: {app.Id}");
                Console.WriteLine($"Name: {app.Name}");
                Console.WriteLine($"Title: {app.Title}");
                
                Console.WriteLine("");
            }
        }

        public async Task<string> Deploy()
        {
            Console.WriteLine("----- SHARP DEPLOYER -----");
            Console.WriteLine("");
            Console.WriteLine("");

            Index();

            Console.WriteLine("Enter the following required data:");
            Console.Write("Project Id:\t\t");
            var projectId = Console.ReadLine();

            Console.Write("Git Email:\t\t");
            var gitEmail = Console.ReadLine();
            Console.Write("Git Personal Token:\t");
            var gitToken = Console.ReadLine();
            

            return await _deployer.DeployDotNet(projectId, gitEmail, gitToken);
        }
    }
}
