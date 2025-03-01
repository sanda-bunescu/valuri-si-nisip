using Domain;
using Microsoft.EntityFrameworkCore;
using Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddControllersAsServices();

builder.Services
    .AddDbContext(builder.Configuration, builder.Environment)
    .AddAuth(builder.Configuration)
    .AddSwagger()
    .AddRepositories()
    .AddServices();

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandlerMiddleware();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    var seeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    seeder.Seed();
}
app.UseSwagger();
app.UseSwaggerUI();

app.Run();