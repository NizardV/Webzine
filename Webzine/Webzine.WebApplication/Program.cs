var builder = WebApplication.CreateBuilder(args);

// Ajoute le support des contrôleurs et des vues (indispensable pour MVC)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Ajoute les fichiers statiques (Bootstrap, CSS, JS)
app.UseStaticFiles();

app.UseRouting();

// Configure le routage pour les zones (areas)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Configure le routage par défaut
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// C'est cette ligne qui maintient le serveur web allumé !
app.Run();