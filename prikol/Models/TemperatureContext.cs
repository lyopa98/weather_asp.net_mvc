using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace prikol.Models
{
    public class TemperatureContext: DbContext
    {
        public DbSet<Temperature> temperatures { get; set; }
    }
}