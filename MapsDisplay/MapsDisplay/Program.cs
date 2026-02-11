using MapsDisplay.Components;
using MapsDisplay.Features.LocalAuthority.Services.Builders;
using MapsDisplay.Configurations;


var builder = WebApplication.CreateBuilder(args);

// Add Services to the container using extension methods
builder.Services.AddCustomServices(builder.Configuration);
builder.Services.AddCustomDatabase(builder.Configuration);
builder.Services.AddIdentityServices();
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddEmailSender();

var app = builder.Build();

// Build lookup.json (in Datasets folder) file if doesn't exits
DatasetBuilder.BuildAll();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// Configure interactive components
app.MapRazorComponents<App>()
   .AddInteractiveServerRenderMode()
   .AddInteractiveWebAssemblyRenderMode()
   .AddAdditionalAssemblies(typeof(MapsDisplay.Client._Imports).Assembly);

// Add additional Identity endpoints
app.MapAdditionalIdentityEndpoints();

app.Run();
