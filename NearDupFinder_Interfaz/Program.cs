using NearDupFinder_Interfaz.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NearDupFinder_Almacenamiento;
using NearDupFinder_LogicaDeNegocio.Servicios;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;
using NearDupFInder_LogicaDeNegocio.Servicios.Catalogos;
using NearDupFInder_LogicaDeNegocio.Servicios.Clusters;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;
using NearDupFinder_LogicaDeNegocio.Servicios.Importacion;
using NearDupFinder_LogicaDeNegocio.Servicios.Items;
using NearDupFinder_LogicaDeNegocio.Servicios.Usuarios;
using NearDupFInder_LogicaDeNegocio.Servicios.Usuarios;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<AlmacenamientoDeDatos>();
builder.Services.AddSingleton<AppState>();
builder.Services.AddSingleton<GestorDuplicados>();
builder.Services.AddSingleton<GestorAuditoria>();
builder.Services.AddScoped<GestorUsuarios>();
builder.Services.AddScoped<GestorCatalogos>();


builder.Services.AddScoped<GestorControlClusters>(sp =>
{
    var cat = sp.GetRequiredService<GestorCatalogos>();
    var aud = sp.GetRequiredService<GestorAuditoria>();
    return new GestorControlClusters(cat, aud);
});

builder.Services.AddScoped<ControladorDuplicados>(sp =>
{
    var auditoria = sp.GetRequiredService<GestorAuditoria>();
    var detector = sp.GetRequiredService<GestorDuplicados>();
    var gestorCat = sp.GetRequiredService<GestorCatalogos>();
    var gestorControlClusters = sp.GetRequiredService<GestorControlClusters>();
    var state = sp.GetRequiredService<AppState>();
    return new ControladorDuplicados(auditoria, detector, gestorCat,gestorControlClusters, state.DuplicadosGlobales);
});

builder.Services.AddScoped<GestorItems>(sp =>
{
    var state = sp.GetRequiredService<AppState>();
    return new GestorItems( state.IdsItemsGlobal);
});


builder.Services.AddScoped<ControladorItems>(sp =>
{
    var gestorItems = sp.GetRequiredService<GestorItems>();
    var gestorCatalogos = sp.GetRequiredService<GestorCatalogos>();
    var controladorDuplicados = sp.GetRequiredService<ControladorDuplicados>();
    var gestorAuditoria = sp.GetRequiredService<GestorAuditoria>();
    var appState = sp.GetRequiredService<AppState>();

    return new ControladorItems(
        gestorItems,
        gestorCatalogos,
        controladorDuplicados,
        gestorAuditoria,
        appState.IdsItemsGlobal
    );
});

builder.Services.AddScoped<GestorLectorCsv>(sp =>
{
    var gestorCatalogos = sp.GetRequiredService<GestorCatalogos>();
    var gestorItems = sp.GetRequiredService<GestorItems>();
    var controladorItems= sp.GetRequiredService<ControladorItems>();
    return new GestorLectorCsv(gestorCatalogos, gestorItems, controladorItems);
});

builder.Services.AddScoped<ControladorLectorCsv>();
builder.Services.AddScoped<GestorAutenticacionUsuario>();
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

builder.Services.AddDbContext<SqlContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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