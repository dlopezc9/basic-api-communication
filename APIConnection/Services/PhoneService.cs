using APIConnection.Interfaces;
using APIConnection.Models;
using Newtonsoft.Json;

namespace APIConnection.Services
{
    public class PhoneService : IPhoneService
    {
        public async Task<IEnumerable<Phone>> GetPhones()
        {
            //Api Client SetUp.
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.restful-api.dev/");
            string endpoint = $"{client.BaseAddress.AbsoluteUri}objects";
            var endPointAddress = new Uri(endpoint);

            //Calling the API.
            var response = await client.GetAsync(endPointAddress);

            //Decoding the response.
            var responseBody = await response.Content.ReadAsStringAsync();
            var phones = JsonConvert.DeserializeObject<IEnumerable<Phone>>(responseBody);

            return phones;
        }
    }
}
