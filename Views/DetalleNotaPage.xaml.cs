using NotasApp.ViewModels;

namespace NotasApp.Views;

public partial class DetalleNotaPage : ContentPage
{
    public DetalleNotaPage(DetalleNotaViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
