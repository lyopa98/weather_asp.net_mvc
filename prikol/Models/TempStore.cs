using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prikol.Models
{
    public static class TempStore
    {
        public static ResponseWeather response = new ResponseWeather();
        public static List<tempModel> temps = new List<tempModel>();
    }
}