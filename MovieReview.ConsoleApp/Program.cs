using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieReview.Application;
using MovieReview.ConsoleApp;
using MovieReview.Infrastructure;
using MovieReview.Infrastructure.Data;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddPresentation();

using IHost host = builder.Build();
using (var scope = host.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;

	try
	{
		var appDbContext = scopedProvider.GetRequiredService<ApplicationDbContext>();

		await DbInitializer.SeedData(appDbContext);
	}
	catch (Exception e)
	{
        Console.WriteLine(e.Message);
	}
}

var app = host.Services.GetRequiredService<MovieReviewApp>();

await app.RunAsync();