using InvoiceManager.Data;
using InvoiceManager.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración de DbContext con SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=invoices.db"));

// Agregar servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DataImporter>();

var app = builder.Build();

// Configuración Swagger (habilitado siempre para pruebas)
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

// ✅ Importación automática al inicio con manejo seguro de ruta y errores
await using (var scope = app.Services.CreateAsyncScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    var importer = scope.ServiceProvider.GetRequiredService<DataImporter>();

    // Ruta absoluta para el archivo JSON
    var jsonPath = Path.Combine(AppContext.BaseDirectory, "bd_exam_invoices.json");

    if (File.Exists(jsonPath))
    {
        try
        {
            Console.WriteLine($"Importando datos desde: {jsonPath}");
            await importer.ImportDataAsync(jsonPath);
            Console.WriteLine("✅ Datos importados correctamente.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al importar datos: {ex.Message}");
        }
    }
    else
    {
        Console.WriteLine($"⚠ No se encontró el archivo JSON en: {jsonPath}");
    }
}

app.Run();
