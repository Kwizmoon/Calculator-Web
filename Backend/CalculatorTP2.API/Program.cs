using CalculatriceLibrary;
using CalculatriceLibrary.Data;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);


// CORS POUR PERMETTRE LES REQUETES DEPUIS LE FRONTEND
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});


// AJOUTE SERVICES ICI
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddScoped<Calculator>(); 

var app = builder.Build();

// CONFIGURATION DU SERVEUR
app.UseDefaultFiles(); // Pour chercher index.html
app.UseStaticFiles();  // Pour lire le dossier wwwroot

/*
// MIGRATION AUTOMATIQUE 
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}
*/



app.UseCors();
app.MapControllers();
app.Run();