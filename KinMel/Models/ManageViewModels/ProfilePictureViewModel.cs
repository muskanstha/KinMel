using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace KinMel.Models.ManageViewModels
{
    public class ProfilePictureViewModel
    {

        public string ProfilePictureUrl { get; set; }

        public string StatusMessage { get; set; }
    }
}
