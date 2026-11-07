using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotasApp.Models;
using NotasApp.Services;
using NotasApp.Views;

namespace NotasApp.ViewModels;

public partial class NotasViewModel : ObservableObject
{
    private readonly NotasDatabase _database;
    private List<Nota> _todasLasNotas = [];

    public ObservableCollection<Nota> Notas { get; } = [];

    [ObservableProperty]
    private string textoBusqueda = string.Empty;

    [ObservableProperty]
    private bool estaCargando;

    public NotasViewModel(NotasDatabase database)
    {
        _database = database;
    }

    partial void OnTextoBusquedaChanged(string value) => AplicarFiltro(value);

    [RelayCommand]
    private async Task CargarNotasAsync()
    {
        if (EstaCargando)
        {
            return;
        }

        try
        {
            EstaCargando = true;
            _todasLasNotas = (await _database.ObtenerTodasAsync()).ToList();
            AplicarFiltro(TextoBusqueda);
        }
        finally
        {
            EstaCargando = false;
        }
    }

    [RelayCommand]
    private static async Task CrearNotaAsync()
    {
        await Shell.Current.GoToAsync(nameof(DetalleNotaPage));
    }

    [RelayCommand]
    private static async Task EditarNotaAsync(Nota? nota)
    {
        if (nota is not null)
        {
            await Shell.Current.GoToAsync(nameof(DetalleNotaPage), new Dictionary<string, object>
            {
                ["id"] = nota.Id.ToString()
            });
        }
    }

    [RelayCommand]
    private async Task EliminarNotaAsync(Nota? nota)
    {
        if (nota is null)
        {
            return;
        }

        var confirmar = await Shell.Current.DisplayAlertAsync(
            "Eliminar nota",
            $"¿Quieres eliminar «{nota.Titulo}»? Esta acción no se puede deshacer.",
            "Eliminar",
            "Cancelar");

        if (!confirmar)
        {
            return;
        }

        await _database.EliminarAsync(nota.Id);
        await CargarNotasAsync();
    }

    private void AplicarFiltro(string? texto)
    {
        var filtro = texto?.Trim() ?? string.Empty;
        var resultado = string.IsNullOrWhiteSpace(filtro)
            ? _todasLasNotas
            : _todasLasNotas.Where(nota => nota.Titulo.Contains(filtro, StringComparison.OrdinalIgnoreCase)).ToList();

        Notas.Clear();
        foreach (var nota in resultado)
        {
            Notas.Add(nota);
        }
    }
}
