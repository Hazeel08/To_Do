using CapaDatos;
using CapaDatos.Interfaces;
using CapaDatos.Persistencia;
using CapaDatos.Repositorio;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<DBConexion>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddScoped<IToDo, ToDoRepositorio>();
builder.Services.AddScoped<IToDoEstado, ToDoEstadoRepositorio>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
