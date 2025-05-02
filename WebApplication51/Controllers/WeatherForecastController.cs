using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using WebApplication51;

namespace WebApplication51.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly SupaBaseContext _supabaseContext;

        public WeatherForecastController(Supabase.Client supabaseClient, SupaBaseContext supaBaseContext)
        {
            _supabaseClient = supabaseClient;
            _supabaseContext = supaBaseContext;
        }
        [HttpGet("GetAllUsers", Name = "GetAllUsers")]
        public async Task<string> GetAllUsers()
        {
            try
            {
                var result = await _supabaseClient.From<User>().Get();
                return JsonConvert.SerializeObject(result.Models, Formatting.Indented);
            }
            catch (Exception)
            {
                return "";
            }
        }

        [HttpPost("InsertUser", Name = "InsertUser")]
        public async Task<ActionResult> InsertUser([FromBody] UserData userData)
        {
            try
            {
                if (string.IsNullOrEmpty(userData.Login) || string.IsNullOrEmpty(userData.Password))
                {
                    return BadRequest("логин или пароль пустой.");
                }
                else
                {
                    User newUser = new User
                    {
                        Id = 0,
                        Login = userData.Login,
                        Password = userData.Password,
                        Age = userData.age ?? "",
                        city_id = userData.city_id ?? ""
                    };

                    bool result = await _supabaseContext.InsertUser(_supabaseClient, newUser);

                    if (result == true)
                    {
                        return Ok("регестрация прошла успешно.");
                    }
                    else
                    {
                        return BadRequest("не удалось добавить в бд");
                    }
                }
            }
            catch (Exception)
            {
                return BadRequest("ошибка");
            }
        }

        [HttpPut("UpdateUser", Name = "UpdateUser")]
        public async Task<string> UpdateUser(int id, [FromBody] UserData userData)
        {
            try
            {
                var result = await _supabaseClient.From<User>().Where(x => x.Id == id).Get();
                var user = result.Models.FirstOrDefault();

                if (user == null)
                {
                    return "нету пользователя";
                }

                user.Login = userData.Login;
                user.Password = userData.Password;
                user.Age = userData.age;
                user.city_id = userData.city_id;

                await _supabaseClient.From<User>().Update(user);
                return "ок";
            }
            catch (Exception)
            {
                return "ошикба";
            }
        }

        [HttpDelete("DeleteUser", Name = "DeleteUser")]
        public async Task<string> DeleteUser(int id)
        {
            try
            {
                var result = await _supabaseClient.From<User>().Where(x => x.Id == id).Get();
                var user = result.Models.FirstOrDefault();

                if (user == null)
                {
                    return "пользователя нет";
                }

                await _supabaseClient.From<User>().Delete(user);
                return "ок";
            }
            catch (Exception)
            {
                return "ошибка";
            }
        }
        

        [HttpGet("GetAllCities", Name = "GetAllCities")]
        public async Task<string> GetAllCities()
        {
            try
            {
                var cities = await _supabaseContext.GetCities(_supabaseClient);
                return JsonConvert.SerializeObject(cities, Formatting.Indented);
            }
            catch (Exception)
            {
                return "ошибка при получение городов";
            }
        }

        [HttpPost("InsertCity", Name = "InsertCity")]
        public async Task<ActionResult> InsertCity([FromBody] CityData cityData)
        {
            try
            {
                if (string.IsNullOrEmpty(cityData.Name))
                {
                    return BadRequest("название города не должно быть пустым");
                }

                if (cityData.Population <= 0)
                {
                    return BadRequest("население должго быть больше 0");
                }
                var nextId = 1;
                var cities = await _supabaseClient.From<City>().Get();
                if (cities.Models.Any())
                {
                    nextId = cities.Models.Max(c => c.Id) + 1;
                }

                City newCity = new City
                {
                    Id = nextId,
                    Name = cityData.Name,
                    Population = cityData.Population
                };

                await _supabaseClient.From<City>().Insert(newCity);
                return Ok("город добавлен");
            }
            catch (Exception)
            {
                return BadRequest("не удалось добавитьв бд");
            }
        }

        [HttpPut("UpdateCity", Name = "UpdateCity")]
        public async Task<string> UpdateCity(int id, [FromBody] CityData cityData)
        {
            try
            {
                var result = await _supabaseClient.From<City>().Where(x => x.Id == id).Get();
                var city = result.Models.FirstOrDefault();
                
                if (city == null)
                {
                    return "город не найден";
                }

                city.Name = cityData.Name;
                city.Population = cityData.Population;

                await _supabaseClient.From<City>().Update(city);
                return "город добавлен";
            }
            catch (Exception)
            {
                return "ошибка при обновлении ";
            }
        }

        [HttpDelete("DeleteCity", Name = "DeleteCity")]
        public async Task<string> DeleteCity(int id)
        {
            try
            {
                var result = await _supabaseClient.From<City>().Where(x => x.Id == id).Get();
                var city = result.Models.FirstOrDefault();

                if (city == null)
                {
                    return "города нету";
                }

                await _supabaseClient.From<City>().Delete(city);
                return "город удален";
            }
            catch (Exception)
            {
                return "ошибка при удалении ";
            }
        }
    }

    public class UserData
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("age")]
        public string age { get; set; }

        [JsonProperty("city_id")]
        public string city_id { get; set; }
    }
    
    public class CityData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("population")]
        public int Population { get; set; }
    }
}