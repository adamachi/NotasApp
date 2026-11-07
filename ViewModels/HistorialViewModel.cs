using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotasApp.Models;
using NotasApp.Services;

namespace NotasApp.ViewModels;

public partial class HistorialViewModel : ObservableObject
{
    private readonly NotasDatabase _database;

    public ObservableCollection<Historial> Historial { get; } = [];

    [ObservableProperty]
    private bool estaCargando;

    public HistorialViewModel(NotasDatabase database)
    {
        _database = database;
    }

    [RelayCommand]
    private async Task CargarHistorialAsync()
    {
        if (EstaCargando)
        {
            return;
        }

        try
        {
            EstaCargando = true;
            var historial = await _database.ObtenerHistorialAsync();
            Historial.Clear();
            foreach (var item in historial.OrderByDescending(item => item.Fecha))
            {
                Historial.Add(item);
            }
        }
        finally
        {
            EstaCargando = false;
        }
    }
}
