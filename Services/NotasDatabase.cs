using NotasApp.Models;
using SQLite;

namespace NotasApp.Services;

public sealed class NotasDatabase
{
    private readonly SQLiteAsyncConnection _connection;
    private readonly Task _initializationTask;

    public NotasDatabase()
    {
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "notas.db3");
        _connection = new SQLiteAsyncConnection(databasePath);
        _initializationTask = InitializeAsync();
    }

    public async Task<IReadOnlyList<Nota>> ObtenerTodasAsync()
    {
        await EnsureInitializedAsync();
        return await _connection.Table<Nota>()
            .OrderByDescending(nota => nota.FechaCreacion)
            .ToListAsync();
    }

    public async Task<Nota?> ObtenerPorIdAsync(int id)
    {
        await EnsureInitializedAsync();
        return await _connection.Table<Nota>()
            .Where(nota => nota.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<int> GuardarOActualizarAsync(Nota nota)
    {
        ArgumentNullException.ThrowIfNull(nota);
        await EnsureInitializedAsync();

        var esNueva = nota.Id == 0;
        if (esNueva)
        {
            if (nota.FechaCreacion == default)
            {
                nota.FechaCreacion = DateTime.Now;
            }

            await _connection.InsertAsync(nota);
        }
        else
        {
            await _connection.UpdateAsync(nota);
        }

        await RegistrarAccionAsync(
            esNueva ? "Creación" : "Edición",
            $"{nota.Titulo} (#{nota.Id})");

        return nota.Id;
    }

    public async Task<int> EliminarAsync(int id)
    {
        await EnsureInitializedAsync();
        var nota = await ObtenerPorIdAsync(id);
        if (nota is null)
        {
            return 0;
        }

        var result = await _connection.DeleteAsync(nota);
        if (result > 0)
        {
            await RegistrarAccionAsync("Eliminación", $"{nota.Titulo} (#{nota.Id})");
        }

        return result;
    }

    public async Task<IReadOnlyList<Historial>> ObtenerHistorialAsync()
    {
        await EnsureInitializedAsync();
        return await _connection.Table<Historial>()
            .OrderByDescending(item => item.Fecha)
            .ToListAsync();
    }

    public async Task<int> RegistrarAccionAsync(string accion, string detalleNota)
    {
        await EnsureInitializedAsync();
        return await _connection.InsertAsync(new Historial
        {
            Accion = accion,
            Fecha = DateTime.Now,
            DetalleNota = detalleNota
        });
    }

    private async Task InitializeAsync()
    {
        await _connection.CreateTableAsync<Nota>();
        await _connection.CreateTableAsync<Historial>();
    }

    private Task EnsureInitializedAsync() => _initializationTask;
}
