using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApplication51
{
    public class SupaBaseContext
    {
        public async Task<List<User>> GetUsers(Supabase.Client _supabaseClient)
        {
            var result = await _supabaseClient.From<User>().Get();
            return result.Models;
        }

        public async Task<bool> InsertUser(Supabase.Client _supabaseClient, User user)
        {
            try
            {
                await _supabaseClient.From<User>().Insert(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<City>> GetCities(Supabase.Client _supabaseClient)
        {
            var result = await _supabaseClient.From<City>().Get();
            return result.Models;
        }

        public async Task<City> GetCityById(Supabase.Client _supabaseClient, int id)
        {
            var result = await _supabaseClient.From<City>().Where(x => x.Id == id).Get();
            return result.Models.FirstOrDefault();
        }

        public async Task<bool> InsertCity(Supabase.Client _supabaseClient, City city)
        {
            try
            {
                await _supabaseClient.From<City>().Insert(city);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateCity(Supabase.Client _supabaseClient, City city)
        {
            try
            {
                await _supabaseClient.From<City>().Update(city);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteCity(Supabase.Client _supabaseClient, City city)
        {
            try
            {
                await _supabaseClient.From<City>().Delete(city);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}










   