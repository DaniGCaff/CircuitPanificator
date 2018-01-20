using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using CircuitPlanification.RepositoryModels;

namespace CircuitPlanificator.Repositories
{
    public class FaresRepository
    {
        private static string currency = "EUR";
        private static string API_KEY = "8chSr10sSK0bAaGDzTx4jLpo4VPWgcbj";
        private static string API_URL = "https://api.ryanair.com/farefinder/3/oneWayFares/{0}/{1}/cheapestPerDay?outboundWeekOfDate={2}&currency={3}&apikey={4}";

        public static async Task<List<Fare>> GetChipestFaresFor(string iataArrive, string iataDepart, DateTime date)
        {
            string apiUrl = string.Format(API_URL, iataDepart, iataArrive, date.ToString("yyyy-MM-dd"), currency, API_KEY);

            var client = new HttpClient();
            HttpResponseMessage mensaje = await client.GetAsync(apiUrl);


            // Get the response content.
            HttpContent responseContent = mensaje.Content;
            List<Fare> result = new List<Fare>();
            // Get the stream of the content.
            using (var reader = new StreamReader(await responseContent.ReadAsStreamAsync()))
            {
                // Write the output.
                Task<string> read = reader.ReadToEndAsync();
                var JObject = Newtonsoft.Json.Linq.JObject.Parse(read.Result);
                List<Newtonsoft.Json.Linq.JToken> listaFares = JObject["outbound"]["fares"].ToList();
                foreach(var fare in listaFares)
                {
                    if(fare["soldOut"].ToString().Equals("False") && fare["unavailable"].ToString().Equals("False"))
                    {
                        if(fare["day"].ToString().Equals(date.ToString("dd/MM/yyyy")))
                        {
                            Fare aux = new Fare();
                            aux.iataArrive = iataArrive;
                            aux.iataDepart = iataDepart;
                            aux.precio = fare["price"]["value"].ToString();
                            aux.date = fare["day"].ToString();
                            result.Add(aux);
                        }
                    }
                }
            }
            return result;
        }
    }
}
