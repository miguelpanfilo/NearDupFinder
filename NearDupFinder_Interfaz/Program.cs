using NearDupFinder_Interfaz.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using NearDupFinder_Almacenamiento;
using NearDupFinder_LogicaDeNegocio.Servicios;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<AlmacenamientoDeDatos>();
builder.Services.AddSingleton<AppState>();
builder.Services.AddSingleton<GestorDuplicados>();
builder.Services.AddSingleton<GestorAuditoria>();
builder.Services.AddScoped<GestorUsuarios>();
builder.Services.AddScoped<GestorCatalogos>();

builder.Services.AddScoped<GestorControlDuplicados>(sp =>
{
    var auditoria = sp.GetRequiredService<GestorAuditoria>();
    var detector = sp.GetRequiredService<GestorDuplicados>();
    var gestorCat = sp.GetRequiredService<GestorCatalogos>();
    var state = sp.GetRequiredService<AppState>();
    return new GestorControlDuplicados(auditoria, detector, gestorCat, state.DuplicadosGlobales);
});

builder.Services.AddScoped<GestorItems>(sp =>
{
    var cat = sp.GetRequiredService<GestorCatalogos>();
    var dup = sp.GetRequiredService<GestorControlDuplicados>();
    var aud = sp.GetRequiredService<GestorAuditoria>();
    var state = sp.GetRequiredService<AppState>();
    return new GestorItems(cat, dup, aud, state.IdsItemsGlobal);
});

builder.Services.AddScoped<GestorControlClusters>(sp =>
{
    var cat = sp.GetRequiredService<GestorCatalogos>();
    var dup = sp.GetRequiredService<GestorControlDuplicados>();
    var aud = sp.GetRequiredService<GestorAuditoria>();
    return new GestorControlClusters(cat, dup, aud);
});


builder.Services.AddScoped<LectorCsv>(sp =>
{
    var gestorCatalogos = sp.GetRequiredService<GestorCatalogos>();
    var gestorItems = sp.GetRequiredService<GestorItems>();
    return new LectorCsv(gestorCatalogos, gestorItems);
});

builder.Services.AddScoped<GestorControlLectorCsv>();
builder.Services.AddScoped<Login>();
builder.Services.AddSingleton<GestorInicializacion>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();


builder.Services.AddRazorComponents().AddInteractiveServerComponents();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.LogoutPath = "/logout";
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();

public class AppState
{
    public List<ParDuplicado> DuplicadosGlobales { get; } = new();
    public HashSet<int> IdsItemsGlobal { get; } = new();
}