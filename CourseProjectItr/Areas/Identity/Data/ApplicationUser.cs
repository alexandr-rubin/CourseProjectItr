using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CourseProjectItr.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        [PersonalData]
        public DateTimeOffset RegistrationDate { get; set; }
        [PersonalData]
        public DateTimeOffset LastLoginDate { get; set; }
        [PersonalData]
        public bool LockoutStatus { get; set; }
    }
}
