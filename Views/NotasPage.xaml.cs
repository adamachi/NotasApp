using NotasApp.ViewModels;

namespace NotasApp.Views;

public partial class NotasPage : ContentPage
{
    private readonly NotasViewModel _viewModel;

    public NotasPage(NotasViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        ThemeSwitch.IsToggled = Application.Current?.UserAppTheme == AppTheme.Dark ||
            (Application.Current?.UserAppTheme == AppTheme.Unspecified && Application.Current?.RequestedTheme == AppTheme.Dark);
    }

    private void OnThemeSwitchToggled(object? sender, ToggledEventArgs e)
    {
        if (Application.Current is not null)
        {
            Application.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarNotasCommand.ExecuteAsync(null);
    }
}
