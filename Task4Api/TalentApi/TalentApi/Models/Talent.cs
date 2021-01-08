using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductStore.Models
{
    public class Talent
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortName { get; set; }
        [Required]
        public string Reknown { get; set; }
        [Required]
        public string Bio { get; set; }
    }
}
