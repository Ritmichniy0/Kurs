using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GBLesson4DTO
{
    public class UserAuthRequest
    {
        [Required] // делает поле обязательным
        [EmailAddress]  // проверяет, что это email
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

        public RoleType UserRole { get; set; } = RoleType.User;
    }
}
