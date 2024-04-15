using BambooCard.Abstract;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IHackerNews, BambooCard.Services.HackerNews>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/beststories", async (
        int quantity,
        [FromServices] IHttpClientFactory httpClientFactory,
        [FromServices] IHackerNews hackerNewsService,
        CancellationToken cancellationToken) =>
    {
        return await hackerNewsService
            .GetBestStoriesDetails(quantity, cancellationToken);
    })
    .WithName("BestStories")
    .WithOpenApi();



app.Run();

