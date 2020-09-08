using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace UcsfTestHarness.Controllers
{
    [ApiController]
    public class HDKController : ControllerBase
    {

        private class AuthResponse
        {
            public string code { get; set; }
            public string state { get; set; }
        }
        private class LaunchCode
        {
            public string launchcode { get; set; }
        }
        [Route("api/hdk/getAuth")]
        public async Task<IActionResult> GetAuth([FromQuery] string launchCode)
        {
            if (launchCode == null)
            {
                string hdkLaunchUrl = $"https://dev-unified-api.ucsf.edu/clinical/hdk/2.0/oauth2/launchcode";

                LaunchCode launchResponse = await this.GetLaunchCode<LaunchCode>(hdkLaunchUrl);

                if (launchResponse == null)
                {
                    return Unauthorized("Error while retrieving Launch Code.");
                }
                launchCode = launchResponse.launchcode;
            }

            string hdkAuthorizeUrl = $"https://dev-unified-api.ucsf.edu/clinical/hdk/2.0/oauth2/authorize?response_type=code&client_id=820a491727ea4bc3a205e525acfe8764&redirect_uri=https://dev-unified-api.ucsf.edu/clinical/hdk/2.0/oauth2/app/redirect&scope=launch+patient%2FObservation.read+patient%2FPatient.read+openid+profile&state=:PatientAppstate&aud=https%3A%2F%2Fdev-unified-api.ucsf.edu%2Fclinical%2Fhdk%2F2.0&launch={launchCode}&code_challenge=YWE4MjU3MDJkNDNlODIzY2RmMGU2YTBjYWM3Yjc1NmI2MDA1NTVlZDUzOWY3Zjg3NWY0NTQyZGJmNTJkN2IzMA==&code_challenge_method=s256";

            AuthResponse response = await this.GetAuthCode<AuthResponse>(hdkAuthorizeUrl);

            if (response != null)
                return Ok(response);
            else
                return Unauthorized("Error while retrieving Auth Code.");
        }


        private async Task<T> GetAuthCode<T>(string hdkUrl)
        {
            T details = default(T);

            RestClient client = new RestClient(hdkUrl);
            IRestRequest request = new RestRequest(Method.GET);

            int attempts = 5;
            while (attempts > 0)
            {
                IRestResponse response = await client.ExecuteAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    details = JsonConvert.DeserializeObject<T>(response.Content);
                    attempts = 0;
                }
                attempts--;
            }
            return details;
        }


        private async Task<T> GetLaunchCode<T>(string hdkUrl)
        {
            T details = default(T);

            RestClient client = new RestClient(hdkUrl);
            IRestRequest request = new RestRequest(Method.POST)
                .AddJsonBody("{\"client_secret\":\"0cC9Ea076748430a9aC0b14E4C394164\",\"client_id\":\"820a491727ea4bc3a205e525acfe8764\",\"ehr_context\":{\"dept_id\":\"5201107\",\"preferred_lang\":\"en\",\"referral_id\":\"4645\"},\"patient_mrn\":\"96806795\",\"access_token\":\"xoqwJbxO3gqNkNydbnVvpxhQOXBUsgzTgvMK2VeSmE1KRUVEQHiNbFxRymz5d\",\"partner_appcode\":\"RCAPP\"}");

            int attempts = 5;
            while (attempts > 0)
            {
                IRestResponse response = await client.ExecuteAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    details = JsonConvert.DeserializeObject<T>(response.Content);
                    attempts = 0;
                }
                attempts--;
            }
            return details;
        }
    }
}