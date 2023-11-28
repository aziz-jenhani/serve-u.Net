
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace serveu.Models
{


    public class ApplicationUser : IdentityUser
    {

        public static string ADMIN_ROLE = "ADMIN";
        public static string RESTAURANT_ROLE = "RESTAURANT";

        [Required]
        public string Name { get; set; }

    }

    public static class UserRole
    {

        public const string ADMIN = "ADMIN";
        public const string RESTAURANT = "RESTAURANT";
    }

}