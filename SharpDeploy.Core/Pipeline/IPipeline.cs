using System.Threading.Tasks;

namespace SharpDeploy.Pipeline
{
    public interface IPipeline
    {
        Task Execute();
    }
}
