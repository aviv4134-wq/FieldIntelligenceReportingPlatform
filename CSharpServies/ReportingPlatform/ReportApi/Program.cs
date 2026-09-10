using Elastic.Clients.Elasticsearch;
using ReportApi.middleware;
using ReportApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var settings = new ElasticsearchClientSettings(new Uri("http://localhost:9200"));
    
var client = new ElasticsearchClient(settings);

builder.Services.AddSingleton(client);

builder.Services.AddScoped<IRepositoryReports, RepositoryReports>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
