using AlmaMedical.Web.Client.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Registrar servicio de permisos
builder.Services.AddSingleton<AlmaMedical.Web.Client.Services.RolePermissionService>();
builder.Services.AddScoped<AlmaMedical.Web.Client.Services.CurrentUserService>();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
