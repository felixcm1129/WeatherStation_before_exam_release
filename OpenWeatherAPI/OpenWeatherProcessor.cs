using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace OpenWeatherAPI
{
    /// <summary>
    /// Classe permettant de faire des appels au service Open Weather
    /// Il faut avoir une clé API pour pouvoir l'utiliser
    /// </summary>
    public class OpenWeatherProcessor
    {
        /// <summary>
        /// Singleton
        /// </summary>
        private static readonly Lazy<OpenWeatherProcessor> lazy = new Lazy<OpenWeatherProcessor>(() => new OpenWeatherProcessor());

        public static OpenWeatherProcessor Instance { get { return lazy.Value; } }


        public string BaseURL { get; set; }
        public string EndPoint { get; set; }

        public string Longitude { get; set; } = "-72.7491"; // Shawinigan par défaut
        public string Latitude { get; set; } = "46.5668";

        private string longUrl;

        public string ApiKey { get; set; }

        public string City { get; set; } = "Shawinigan";

        private OpenWeatherProcessor()
        {
            BaseURL = $"https://api.openweathermap.org/data/2.5";
            EndPoint = $"/weather?";
        }

        
        /// <summary>
        /// Appel le endpoint One Call API
        /// </summary>
        /// <returns></returns>
        public async Task<OpenWeatherOneCallModel> GetOneCallAsync()
        {
            
            EndPoint = $"/onecall?";

            /// Src : https://stackoverflow.com/a/14517976/503842
            var uriBuilder = new UriBuilder($"{BaseURL}{EndPoint}");

            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["lat"] = Latitude; // Shawinigan
            query["lon"] = Longitude;
            query["units"] = "metric";
            query["appid"] = ApiKey;


            uriBuilder.Query = query.ToString();
            longUrl = uriBuilder.ToString();

            return await doOneCall();
        }

        /// <summary>
        /// Appel le endpoint weather
        /// </summary>
        /// <returns></returns>
        public async Task<OWCurrentWeaterModel> GetCurrentWeatherAsync()
        {

            if (ApiKey == null || ApiKey == "")
            {
                throw new ArgumentException("ApiKey est vide ou null");
            }
            else
            {
                EndPoint = $"/weather?";

                /// Src : https://stackoverflow.com/a/14517976/503842
                var uriBuilder = new UriBuilder($"{BaseURL}{EndPoint}");

                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["q"] = City; // Shawinigan
                query["units"] = "metric";
                query["appid"] = ApiKey;

                uriBuilder.Query = query.ToString();
                longUrl = uriBuilder.ToString();

                return await doCurrentWeatherCall();
            }
            
        }

        private async Task<OpenWeatherOneCallModel> doOneCall()
        {

            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(longUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    OpenWeatherOneCallModel result = await response.Content.ReadAsAsync<OpenWeatherOneCallModel>();
                    return result;
                }

                return null;
            }
        }

        private async Task<OWCurrentWeaterModel> doCurrentWeatherCall()
        {            
            using (HttpResponseMessage response = await ApiHelper.ApiClient.GetAsync(longUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    OWCurrentWeaterModel result = await response.Content.ReadAsAsync<OWCurrentWeaterModel>();
                    return result;
                }

                return null;

            }
        }
    }
}
