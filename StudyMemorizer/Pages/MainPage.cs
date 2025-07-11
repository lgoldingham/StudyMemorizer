using StudyMemorizer.Classes;

namespace StudyMemorizer.Pages;

public class MainPage : ContentPage
{
    public MainPage()
    {
        Button reloadButton = new Button
        {
            Text = "Load Data",
            HorizontalOptions = LayoutOptions.Fill
        };
        reloadButton.Clicked += reloadButton_Clicked;



        ScrollView content = new ScrollView { };
        content.Content = new VerticalStackLayout
        {
            Padding = new Thickness(30, 0),
            Spacing = 25,
            Children =
            {
                reloadButton
            }
        };
        Content = content;
    }
    public void reloadButton_Clicked(object? sender, EventArgs e)
    {
        if (sender is Button button)
        {
            button.Text = $"Loaded {DataHandler.GetInstance().LoadData()} items at {DateTime.Now}";
        }
    }
}