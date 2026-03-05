var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

// Active le middleware permettant le routage des requêtes entrantes.
app.UseRouting();

// Ajoute un endpoint permettant de router les urls
// avec la forme /area/controller/action/id(optionnel).
app.MapControllerRoute(
  name: "areas",
  pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"); 

// Ajoute un endpoint permettant de router les urls
// avec la forme /controller/action/id(optionnel).
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
