using System.ComponentModel.DataAnnotations;

namespace AuthApp.Core.Dtos
{
    public class UpdatePermissionDto
    {

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

    }
}
