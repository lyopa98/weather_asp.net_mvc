using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prikol.Models
{
    public class tempModel
    {
        public int Id { get; set; }
        public Main main { get; set; }
        public Wind wind { get; set; }
        public List<Weather> weather { get; set; }
        public Clouds clouds { get; set; }

        public DateTime dt_txt { get; set; }

        //public double dt { get; set; }
        public tempModel()
        {
            main = new Main();
            weather = new List<Weather>();
            clouds = new Clouds();
            dt_txt = new DateTime();
            wind = new Wind();
        }

    }
}