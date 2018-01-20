using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CircuitPlanification.RepositoryModels
{
    public class Fare
    {
        public string iataDepart { get; set; }
        public string iataArrive { get; set; }
        public string date { get; set; }
        public string precio { get; set; }
    }
}
