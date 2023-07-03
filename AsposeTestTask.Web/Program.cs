using AsposeTestTask.Web.Configuration;
using AsposeTestTask.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddMvc();
builder.Services.AddSingleton<IConfigProvider, ConfigProvider>();
builder.AddDataBaseContext();   ///Using extension method.
builder.AddServices();          ///Using extension method.

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.UseAuthorization();

app.MapRazorPages();

app.Run();
