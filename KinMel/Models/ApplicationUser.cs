using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        private string _profilePictureUrl;

        public string ProfilePictureUrl
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this._profilePictureUrl))
                {
                    return "/images/noimage.svg";
                }

                return this._profilePictureUrl;
            }
            set => _profilePictureUrl = value;
        }

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

        [InverseProperty("NotificationTo")]
        public virtual ICollection<Notification> Notifications { get; set; }

        [InverseProperty("RatedFor")]
        public virtual ICollection<Rating> Ratings { get; set; }
        public double AverageStars
        {
            get
            {
                if (Ratings?.Count > 0) { return Ratings.Average(r => r.Stars); }
                return 0;
            }
        }

        public bool AcceptedTerms { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMMM, yyyy}", ApplyFormatInEditMode = true)]
        public DateTimeOffset JoinDate { get; set; }
    }

    public class AccountDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        public string ProfilePictureUrl { get; set; }
        public string Address { get; set; }
        public string City { get; set; }

        public virtual ICollection<ClassifiedAd> ClassifiedAds { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }

        public double AverageStars { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMMM, yyyy}", ApplyFormatInEditMode = true)]
        public DateTimeOffset JoinDate { get; set; }
    }
}
