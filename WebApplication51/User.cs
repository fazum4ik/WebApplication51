using Supabase.Postgrest.Models;
using Supabase.Postgrest.Attributes;

namespace WebApplication51
{
    [Table("users")]
    public class User : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("login")]
        public string Login { get; set; }

        [Column("age")]
        public string Age { get; set; }

        [Column("city_id")]
        public string city_id { get; set; }


    }
}


namespace WebApplication51
{
    [Table("city")]
    public class City : BaseModel
    {

        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("name")]
        public string name { get; set; }

        [Column("population")]
        public string population { get; set; }

    }
}

