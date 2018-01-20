using CircuitPlanification.RepositoryModels;
using CircuitPlanificator.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CircuitPlanificator.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString = "server=83.37.61.86;port=3306;database=circuitplanification;uid=backend;password=123456";
        public ActionResult Index()
        {


            // Create database if not exists
            //using (CircuitPlanificationContext contextDB = new CircuitPlanificationContext())
            //{
            //    contextDB.Database.CreateIfNotExists();
            //}

            // Create database if not exists
            //using (CircuitPlanificationContext contextDB = new CircuitPlanificationContext())
            //{
            //    var x = contextDB.Cities.ToList();
            //}

            //string connectionString = "server=localhost;port=3305;database=parking;uid=root";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Create database if not exists
                using (CircuitPlanificationContext contextDB = new CircuitPlanificationContext(connection, false))
                {
                    contextDB.Database.CreateIfNotExists();
                }

                connection.Open();



                // DbConnection that is already opened
                using (CircuitPlanificationContext context = new CircuitPlanificationContext(connection, false))
                {
                    var x = context.Cities.ToList();
                    var y = context.Airports.ToList();
                    var z = context.Routes.ToList();
                }


            }

            return View();
        }

        public async Task<ActionResult> GetChipestFaresFor(string code, string date)
        {
            if (code == null) code = "MAD";

            if (date == null) date = "01/04/2018";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Create database if not exists
                using (CircuitPlanificationContext contextDB = new CircuitPlanificationContext(connection, false))
                {
                    contextDB.Database.CreateIfNotExists();
                }

                connection.Open();
                List<Fare> result = new List<Fare>();
                // DbConnection that is already opened
                using (CircuitPlanificationContext context = new CircuitPlanificationContext(connection, false))
                {
                    foreach (Routes route in context.Routes.ToList().FindAll(p => p.iataCodeDepart == code))
                        result.AddRange(await CircuitPlanificator.Repositories.FaresRepository.GetChipestFaresFor(route.iataCodeArrive, route.iataCodeDepart, DateTime.ParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.CurrentCulture)));
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            return View();
        }

        public ActionResult GetAirportCoords(string code)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                // Create database if not exists
                using (CircuitPlanificationContext contextDB = new CircuitPlanificationContext(connection, false))
                {
                    contextDB.Database.CreateIfNotExists();
                }

                connection.Open();
                List<Fare> result = new List<Fare>();
                // DbConnection that is already opened
                using (CircuitPlanificationContext context = new CircuitPlanificationContext(connection, false))
                {
                    AirportCoords aux = new AirportCoords();
                    aux.latitud = context.Airports.ToList().Find(a => a.iataCode == code).latitude.ToString();
                    aux.longitud = context.Airports.ToList().Find(a => a.iataCode == code).longitude.ToString();
                    return Json(aux, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
}