using StudyMemorizer.Classes;

namespace StudyMemorizer.Pages;

public class DeleteDatasetsPage : ContentPage
{
    private Picker picker = new Picker
    {
        HorizontalOptions = LayoutOptions.Fill,
    };
    public DeleteDatasetsPage()
    {
        Title = "Delete Datasets";

        Button deleteButton = new Button
        {
            Text = "Delete Dataset"
        };
        deleteButton.Clicked += deleteButton_Clicked;

        RefreshView refreshView = new RefreshView
        {
            Content = new ScrollView
            {
                Orientation = ScrollOrientation.Vertical,
                Content = new StackLayout
                {
                    Children =
                    {
                        picker,
                        deleteButton
                    }
                }
            }
        };
        refreshView.Refreshing += refreshView_Refreshing;
        refreshView_Refreshing(refreshView, null);
        Content = refreshView;
    }

    private void deleteButton_Clicked(object? sender, EventArgs e)
    {
        if (picker.SelectedItem is Dataset dataset)
        {
            DataHandler.GetInstance().Delete(dataset);
            picker.SelectedIndex = -1;
            picker.ItemsSource = null;
            picker.ItemsSource = DataHandler.GetInstance().GetDatasets();
        }
    }

    private void refreshView_Refreshing(object? sender, EventArgs e)
    {
        if (sender is RefreshView refreshView)
        {
            picker.ItemsSource = DataHandler.GetInstance().GetDatasets();
            refreshView.IsRefreshing = false;
        }
    }
}