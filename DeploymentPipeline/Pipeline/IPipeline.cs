using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeploymentPipeline.Pipeline
{
    public interface IPipeline
    {
        Task Execute();
    }
}
