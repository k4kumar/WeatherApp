using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApp.Models
{
    public class CityViewModel
    {
        [Required]
        [StringLength(30,ErrorMessage ="City Name must be less than or equal to 30 characters")]
        [MinLength(3,ErrorMessage ="City Name must be greater than or equal to 3 characters")]
        public String CityName { get; set; }
    }
}
