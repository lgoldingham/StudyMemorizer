using StudyMemorizer.Classes;
using System.Reflection;

namespace StudyMemorizer.Pages;

public class ViewDatasetPage : ContentPage
{
    private Label label = new Label { };
    private Picker picker = new Picker
    {
        HorizontalOptions = LayoutOptions.Fill,
        ItemsSource = DataHandler.GetInstance().GetDatasets()
    };

    public ViewDatasetPage()
    {
        ScrollView content = new ScrollView();
        Button viewButton = new Button
        {
            Text = "Load Data",
            HorizontalOptions = LayoutOptions.Fill
        };
        viewButton.Clicked += viewButton_Clicked;
        content.Content = new VerticalStackLayout
        {
            Padding = new Thickness(30, 0),
            Spacing = 25,
            Children =
            {
                picker,
                viewButton,
                label
            }
        };
        Content = content;
    }
    public void viewButton_Clicked(object? sender, EventArgs e)
    {
        label.Text = ((Dataset)picker.SelectedItem).LabelFormat();
    }
}