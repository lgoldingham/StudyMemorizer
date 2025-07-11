using StudyMemorizer.Classes;

namespace StudyMemorizer.Pages;

public class MassImportPage : ContentPage
{
    private Editor datasetEditor = new Editor
    {
        AutoSize = EditorAutoSizeOption.TextChanges,
        FontSize = 20
    };
    public MassImportPage()
    {
        Title = "Mass Import";

        Button importButton = new Button
        {
            Text = "Import"
        };
        importButton.Clicked += importButton_Clicked;

        RefreshView refreshView = new RefreshView
        {
            Content = new ScrollView
            {
                Orientation = ScrollOrientation.Vertical,
                Content = new StackLayout
                {
                    Children =
                    {
                        importButton,
                        datasetEditor
                    }
                }
            }
        };
        refreshView.Refreshing += refreshView_Refreshing;
        refreshView_Refreshing(refreshView, null);

        Content = refreshView;
    }

    private void importButton_Clicked(object? sender, EventArgs e)
    {
        if (datasetEditor.Text != "")
        {
            DataHandler.GetInstance().ImportData(datasetEditor.Text);
        }
    }

    private void refreshView_Refreshing(object? sender, EventArgs e)
    {
        if (sender is RefreshView refreshView)
        {
            refreshView.IsRefreshing = false;
        }
    }
}