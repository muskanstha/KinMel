using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace KinMel.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string ProfilePictureUrl { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public string FullName
        {
            get
            {
                string dspFirstName =
                    string.IsNullOrWhiteSpace(this.FirstName) ? "" : this.FirstName;
                string dspLastName =
                    string.IsNullOrWhiteSpace(this.LastName) ? "" : this.LastName;
               

                return $"{dspFirstName} {dspLastName}";
            }
        }

        public virtual ICollection<ClassifiedAd> ClassifiedAds { get; set; }


        public virtual ICollection<Rating> Ratings { get; set; }

        public double AverageStars
        {
            get
            {
                if (Ratings?.Count > 0)
                {
                    return Ratings.Average(r => r.Stars);
                }

                return 0;
            }
        }

        public bool AcceptedTerms { get; set; }

        public DateTime JoinDate { get; set; }
    }
}
