using System.ComponentModel.DataAnnotations;

namespace SharpDeploy.Models
{
    public enum DotNetTargetFrameworks
    {
        [Display(Name = "netcoreapp3.1")]
        ThreePointOne,
        [Display(Name = "net5.0")]
        Five,
    }
}
