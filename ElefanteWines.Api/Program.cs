using ElefanteWines.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ── Servicios ─────────────────────────────────────────────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Serializa los enums como strings ("Pending") en lugar de números (0)
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Elefante Wines API",
        Version = "v1",
        Description = "API de e-commerce para Elefante Wines — proyecto final Backend 2026."
    });
});

// Registra TODO Infrastructure (DbContext + repositorios) en una sola línea
builder.Services.AddInfrastructure(builder.Configuration);

// ── Pipeline ──────────────────────────────────────────────────────────────────
var app = builder.Build();

// Swagger SIEMPRE habilitado (en producción podría ser solo en Development)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Elefante Wines API v1");
    c.RoutePrefix = "swagger";  // Swagger disponible en /swagger
});

app.UseAuthorization();
app.MapControllers();

app.Run();