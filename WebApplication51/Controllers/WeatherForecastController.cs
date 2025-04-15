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
                    return BadRequest("пароль или логин пустой.");
                }
                else
                {
                    User newUser = new User
                    {
                        Id = 0,
                        Login = userData.Login,
                        Password = userData.Password,
                        Age = userData.Age ?? ""
                    };

                    bool result = await _supabaseContext.InsertUser(_supabaseClient, newUser);

                    if (result == true)
                    {
                        return Ok("регистрация прошла успешно.");
                    }
                    else
                    {
                        return BadRequest("не удалось добавить пользователя в бд.");
                    }
                }
            }
            catch (Exception)
            {
                return BadRequest("неизвестная ошибка.");
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
                user.Age = userData.Age;

                await _supabaseClient.From<User>().Update(user);
                return "ок";
            }
            catch (Exception)
            {
                return "ошибка";
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
                    return "пользователя нету";
                }

                await _supabaseClient.From<User>().Delete(user);
                return "ок";
            }
            catch (Exception)
            {
                return "ошибка";
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
}