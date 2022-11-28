using System.Diagnostics;
using MauiApp1.ViewModel;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
	public MainPageViewModel ViewModel { get; set; }

	public MainPage()
	{
		ViewModel = new MainPageViewModel();

		InitializeComponent();

        BindingContext = ViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        ViewModel.OnAppearing();
    }

    private void ServerPicker_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        var selectedIndex = picker.SelectedIndex;

        if (selectedIndex != -1)
        {
            ViewModel.ServerName = ViewModel.Servers[selectedIndex].Server;
        }
    }

    private async void ServerClickButton_OnClicked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ViewModel.ServerName))
        {
            var result = await ViewModel.DoTrace(ViewModel.ServerName);

            Debug.WriteLine(result ? "Trace successful!" : "Trace failed!");
        }
    }
}

