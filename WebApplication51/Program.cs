using WebApplication51;

class Program
{
    public static void Main()
    {
        var builder = WebApplication.CreateBuilder();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        string url = builder.Configuration["SupabaseSetting:ApiUrl"];
        string key = builder.Configuration["SupabaseSetting:ApiKey"];
        var options = new Supabase.SupabaseOptions
        {
            AutoConnectRealtime = true
        };
        
        var supabase = new Supabase.Client(url, key, options);
        var supabaseContext = new SupaBaseContext();
        
        builder.Services.AddSingleton(supabase);
        builder.Services.AddSingleton(supabaseContext);

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}