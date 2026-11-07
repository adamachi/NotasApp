using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using NotasApp.Models;
using NotasApp.Services;

namespace NotasApp.ViewModels;

[QueryProperty(nameof(NotaId), "id")]
public partial class DetalleNotaViewModel : ObservableObject
{
    private readonly NotasDatabase _database;
    private Nota? _nota;

    [ObservableProperty]
    private string notaId = string.Empty;

    [ObservableProperty]
    private string titulo = string.Empty;

    [ObservableProperty]
    private string contenido = string.Empty;

    [ObservableProperty]
    private string mensajeValidacion = string.Empty;

    [ObservableProperty]
    private bool estaGuardando;

    public int CaracteresTitulo => Titulo.Length;

    public DetalleNotaViewModel(NotasDatabase database)
    {
        _database = database;
    }

    partial void OnNotaIdChanged(string value)
    {
        if (int.TryParse(value, out var id) && id > 0)
        {
            _ = CargarNotaAsync(id);
        }
    }

    partial void OnTituloChanged(string value)
    {
        OnPropertyChanged(nameof(CaracteresTitulo));
        ValidarTitulo();
        GuardarCommand.NotifyCanExecuteChanged();
    }

    partial void OnContenidoChanged(string value)
    {
        GuardarCommand.NotifyCanExecuteChanged();
    }

    partial void OnEstaGuardandoChanged(bool value)
    {
        GuardarCommand.NotifyCanExecuteChanged();
    }

    public bool PuedeGuardar() => !EstaGuardando && EsTituloValido;

    private bool EsTituloValido => !string.IsNullOrWhiteSpace(Titulo) && Titulo.Trim().Length <= 100;

    [RelayCommand(CanExecute = nameof(PuedeGuardar))]
    private async Task GuardarAsync()
    {
        if (!ValidarTitulo())
        {
            return;
        }

        try
        {
            EstaGuardando = true;
            GuardarCommand.NotifyCanExecuteChanged();

            _nota ??= new Nota { FechaCreacion = DateTime.Now };
            _nota.Titulo = Titulo.Trim();
            _nota.Contenido = Contenido.Trim();
            await _database.GuardarOActualizarAsync(_nota);
            await Shell.Current.GoToAsync("..");
        }
        finally
        {
            EstaGuardando = false;
            GuardarCommand.NotifyCanExecuteChanged();
        }
    }

    [RelayCommand]
    private static async Task CancelarAsync()
    {
        await Shell.Current.GoToAsync("..");
    }

    private async Task CargarNotaAsync(int id)
    {
        _nota = await _database.ObtenerPorIdAsync(id);
        if (_nota is null)
        {
            return;
        }

        Titulo = _nota.Titulo;
        Contenido = _nota.Contenido;
    }

    private bool ValidarTitulo()
    {
        var tituloSinEspacios = Titulo.Trim();
        MensajeValidacion = string.IsNullOrWhiteSpace(tituloSinEspacios)
            ? "El título es obligatorio."
            : tituloSinEspacios.Length > 100
                ? "El título no puede superar los 100 caracteres."
                : string.Empty;

        return string.IsNullOrEmpty(MensajeValidacion);
    }
}
