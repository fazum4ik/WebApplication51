using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace WebApplication51
{
    [Table("city")]
    public class City : BaseModel
    {
        [PrimaryKey("id")]
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("population")]
        public long Population { get; set; }
    }
}