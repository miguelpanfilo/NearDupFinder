using NearDupFinder_Interfaz.Components;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using NearDupFinder_Almacenamiento;
using NearDupFinder_Almacenamiento.Repositorios;
using NearDupFinder_Interfaces;
using NearDupFinder_Interfaz;
using NearDupFinder_LogicaDeNegocio.Servicios;
using NearDupFInder_LogicaDeNegocio.Servicios.Auditorias;
using NearDupFInder_LogicaDeNegocio.Servicios.Catalogos;
using NearDupFInder_LogicaDeNegocio.Servicios.Clusters;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados;
using NearDupFinder_LogicaDeNegocio.Servicios.Duplicados.ProcesamientoTexto;
using NearDupFinder_LogicaDeNegocio.Servicios.Importacion;
using NearDupFinder_LogicaDeNegocio.Servicios.Items;
using NearDupFinder_LogicaDeNegocio.Servicios.Usuarios;
using NearDupFInder_LogicaDeNegocio.Servicios.Usuarios;
using NearDupFinder_LogicaDeNegocio.Servicios.Exportacion;
using NearDupFinder_LogicaDeNegocio.Servicios.Notificaciones;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IProcesadorTexto, ProcesadorTexto>();
builder.Services.AddSingleton<SesionUsuarioActual>();
builder.Services.AddSingleton<GestorDuplicados>(sp =>
{
    var procesador = sp.GetRequiredService<IProcesadorTexto>();
    return new GestorDuplicados(procesador);
});
builder.Services.AddScoped<IEstrategiaExportacionAuditoria, EstrategiaExportarCsv>();
builder.Services.AddScoped<GestorExportacionAuditoria>();
builder.Services.AddScoped<GestorUsuarios>();
builder.Services.AddScoped<GestorCatalogos>();
builder.Services.AddScoped<IRepositorioAuditorias, RepositorioAuditorias>();
builder.Services.AddScoped<GestorAuditoria>();
builder.Services.AddScoped<GestorNotificaciones>();
builder.Services.AddScoped<GestorControlClusters>(sp =>
{
    var gestorCatalogo = sp.GetRequiredService<GestorCatalogos>();
    var gestorAuditoria = sp.GetRequiredService<GestorAuditoria>();
    var gestorNotificaciones = sp.GetRequiredService<GestorNotificaciones>();
    var sesionUsuarioActual = sp.GetRequiredService<SesionUsuarioActual>();
    var repoClusters = sp.GetRequiredService<IRepositorioClusters>();   
    var repoItems = sp.GetRequiredService<IRepositorioItems>();      

    return new GestorControlClusters(gestorCatalogo, gestorAuditoria, gestorNotificaciones,sesionUsuarioActual, repoClusters, repoItems);
});
builder.Services.AddScoped<ControladorDuplicados>(sp =>
{
    var gestorAuditoria = sp.GetRequiredService<GestorAuditoria>();
    var gestorDuplicados = sp.GetRequiredService<GestorDuplicados>();
    var gestorCatalogo = sp.GetRequiredService<GestorCatalogos>();
    var gestorControlClusters = sp.GetRequiredService<GestorControlClusters>();    
    var repoDuplicados = sp.GetRequiredService<IRepositorioDuplicados>();

    return new ControladorDuplicados(
        gestorAuditoria,
        gestorDuplicados,
        gestorCatalogo,
        gestorControlClusters,
        repoDuplicados);
});
builder.Services.AddScoped<GestorItems>(sp =>
{
    var repositorioItems = sp.GetRequiredService<IRepositorioItems>();
    return new GestorItems(repositorioItems);
});
builder.Services.AddScoped<IRepositorioCatalogos, RepositorioCatalogos>();
builder.Services.AddScoped<IRepositorioItems, RepositorioItems>();
builder.Services.AddScoped<ControladorItems>(sp =>
{
    var gestorItems = sp.GetRequiredService<GestorItems>();
    var gestorCatalogos = sp.GetRequiredService<GestorCatalogos>();
    var controladorDuplicados = sp.GetRequiredService<ControladorDuplicados>();
    var gestorControlClusters = sp.GetRequiredService<GestorControlClusters>();
    var gestorAuditoria = sp.GetRequiredService<GestorAuditoria>();

    return new ControladorItems(
        gestorItems,
        gestorCatalogos,
        controladorDuplicados,
        gestorControlClusters,
        gestorAuditoria);
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
builder.Services.AddScoped<GestorInicializacion>();

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

builder.Services.AddScoped<IRepositorioClusters, RepositorioClusters>();
builder.Services.AddScoped<IRepositorioSincronizacionIds, RepositorioSincronizacionIds>();
builder.Services.AddScoped<IRepositorioUsuarios, RepositorioUsuarios>();
builder.Services.AddScoped<IRepositorioDuplicados, RepositorioDuplicados>();
builder.Services.AddScoped<IRepositorioNotificaciones, RepositorioNotificaciones>();


builder.Services.AddServerSideBlazor()
    .AddCircuitOptions(options => { options.DetailedErrors = true; });


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var servicios = scope.ServiceProvider;
    var gestorInit = servicios.GetRequiredService<GestorInicializacion>();
    gestorInit.AsegurarInicializacion();
}

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

app.MapeoAuditoriasExportacionEndpoints();

app.Run();