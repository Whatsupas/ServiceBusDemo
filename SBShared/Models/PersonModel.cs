using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBShared.Models
{
    public class PersonModel
    {
        [Required]
        public int FirstName { get; set; }
        [Required]
        public int LastName { get; set; }

    }
}
