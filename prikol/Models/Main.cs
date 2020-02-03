using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prikol.Models
{
    public class Main
    {
        public Main()
        {

        }
        public double temp { get; set; }
        public double pressure { get; set; }
        public double humidity { get; set; }
        public double temp_min { get; set; }
        public double temp_max { get; set; }
        public double sea_lavel {get;set;}
    }
}