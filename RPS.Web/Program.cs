using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RPS.Data;

var builder = WebApplication.CreateBuilder(args);

var tempDataContext = new PtInMemoryContext();

builder.Services.AddSingleton<IPtUserRepository>(c => new PtUserRepository(tempDataContext));
builder.Services.AddSingleton<IPtItemsRepository>(c => new PtItemsRepository(tempDataContext));
builder.Services.AddSingleton<IPtDashboardRepository>(c => new PtDashboardRepository(tempDataContext));
builder.Services.AddSingleton<IPtTasksRepository>(c => new PtTasksRepository(tempDataContext));
builder.Services.AddSingleton<IPtCommentsRepository>(c => new PtCommentsRepository(tempDataContext));

builder.Services.AddRazorPages();
builder.Services.AddMvc()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AddPageRoute("/Dashboard", "");
    });

builder.Services.AddKendo(options =>
{
    options.DeferToScriptFiles = true;
});

var app = builder.Build();

// Browser Link and browser refresh connect to other localhost ports/WebSockets in development only
var connectSrc = app.Environment.IsDevelopment()
    ? "'self' http://localhost:* https://localhost:* ws://localhost:* wss://localhost:*"
    : "'self'";

app.Use(async (context, next) =>
{
    var cspNonce = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(16));
    context.Items["CspNonce"] = cspNonce;

    context.Response.Headers["Content-Security-Policy"] =
        "default-src 'self'; base-uri 'self'; object-src 'none'; frame-ancestors 'self'; form-action 'self'; " +
        $"script-src 'self' 'nonce-{cspNonce}'; style-src 'self' 'nonce-{cspNonce}'; " +
        $"img-src 'self' data: blob:; font-src 'self' data:; connect-src {connectSrc}";

    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseMiddleware<Kendo.Mvc.KendoDeferredScriptsMiddleware>();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
