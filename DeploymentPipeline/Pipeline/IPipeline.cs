using System.Threading.Tasks;

namespace DeploymentPipeline.Pipeline
{
    public interface IPipeline
    {
        Task Execute();
    }
}
