using System.ComponentModel.DataAnnotations;

namespace DeploymentPipeline.Models
{
    public enum DotNetTargetFrameworks
    {
        [Display(Name = "netcoreapp3.1")]
        ThreePointOne,
        [Display(Name = "net5.0")]
        Five,
    }
}
