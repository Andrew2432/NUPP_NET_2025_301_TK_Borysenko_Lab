using Microsoft.AspNetCore.Identity;

namespace University.Common
{
    // Наслідуємося від базового класу користувача IdentityUser
    public class AppUser : IdentityUser
    {
        // Тут можна додати власні поля, якщо треба (наприклад, ім'я)
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}