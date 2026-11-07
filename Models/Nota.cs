using SQLite;

namespace NotasApp.Models;

public sealed class Nota
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public string Contenido { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; }
}
