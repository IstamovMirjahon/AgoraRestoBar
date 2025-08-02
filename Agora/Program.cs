using Agora.Application.Interfaces;
using Agora.Infrastructure;
using Agora.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Swagger
builder.Services.AddEndpointsApiExplorer(); // <-- BU SHART!
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Agora API",
        Version = "v1"
    });
});

// ✅ Controllers va JSON sozlamalar
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

// ✅ Kestrel HTTP port sozlovi
builder.WebHost.UseUrls("http://0.0.0.0:5043");

// ✅ CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// ✅ Custom service'lar
builder.Services.AddInfrastructureRegisterServices(builder.Configuration);
builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

// ✅ Middlewarelar
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Agora API v1");
    c.RoutePrefix = "swagger";
});

app.UseStaticFiles();
app.UseAuthorization(); // Faqat kerak bo‘lsa
app.MapControllers();

app.Run();
