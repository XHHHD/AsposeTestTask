using AsposeTestTask.Web.Configuration;
using AsposeTestTask.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddMvc();
builder.Services.AddScoped<IConfigProvider, ConfigProvider>();
builder.AddDataBaseContext();
builder.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

//app.Map(
//    "{controller=Home}/{action=Index}/{id?}",
//    (string controller, string action, string? id) =>
//        $"Controller: {controller} \nAction: {action} \nId: {id}");

app.MapRazorPages();

app.Run();
