using Akka.Actor;
using PrisonersDilema;
using PrisonersDilema.managementActors;
using static PrisonersDilema.Delegates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton(_ => ActorSystem.Create("system"));
builder.Services.AddSingleton<IPunishmentCalculator, PunsimentCalculator>();

builder.Services.AddSingleton<PManagementProvider>(p => {
    var actorSystem = p.GetService<ActorSystem>();
    var pManagement = actorSystem.ActorOf<PlayerManagementActor>();
    return () => pManagement;
});

builder.Services.AddSingleton<APIProvider>(p => {
    var actorSystem = p.GetService<ActorSystem>();
    var apiActor = actorSystem.ActorOf<APIActor>($"{nameof(APIActor)}");
    return () => apiActor;
});

builder.Services.AddSingleton<WardProvider>(p => {
    var actorSystem = p.GetService<ActorSystem>();
    var apiActor = p.GetService<APIProvider>()();
    var pManagment = p.GetService<PManagementProvider>()();
    var c = p.GetService<IPunishmentCalculator>();

    var w = actorSystem.ActorOf(Props.Create(() => new Ward(c, pManagment, apiActor)), $"{nameof(Ward)}");
    return () => w;
});



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

app.Lifetime.ApplicationStarted.Register(() =>
{
    app.Services.GetService<ActorSystem>();
    app.Services.GetService<WardProvider>()().Tell(Ward.LifeCycles.NEWGAME);
});

app.Lifetime.ApplicationStopped.Register(() => {
    app.Services.GetService<ActorSystem>().Terminate().Wait();
});

app.Run();
