using SQLite;

namespace NotasApp.Models;

public sealed class Historial
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Accion { get; set; } = string.Empty;

    public DateTime Fecha { get; set; }

    public string DetalleNota { get; set; } = string.Empty;
}
