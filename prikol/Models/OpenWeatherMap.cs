using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace prikol.Models
{
    public class OpenWeatherMap
    {
        public string apiResponce { get; set; }

        public List<string> cities { get; set; }
    }
}