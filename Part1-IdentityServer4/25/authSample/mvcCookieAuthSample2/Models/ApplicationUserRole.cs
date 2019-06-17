using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace mvcCookieAuthSample.Models
{
    public class ApplicationUserRole:IdentityRole<int>
    {
        public string Avatar { get; set; }
    }
}
