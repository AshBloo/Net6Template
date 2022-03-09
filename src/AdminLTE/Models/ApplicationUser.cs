using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminLTE.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarURL { get; set; }
        public string DateRegistered { get; set; }
        public string Position { get; set; }
        public string NickName { get; set; }
        [NotMapped]
        public IFormFile file { get; set; }
        [Display(Name = "Full Name")]
        public string FullName
        {
            get
            {
                if (FirstName != null)
                {
                    return FirstName + " " + LastName;
                }
                else
                {
                    return null;
                }

            }
        }



    }
}
