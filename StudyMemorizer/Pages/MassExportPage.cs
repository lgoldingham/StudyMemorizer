using StudyMemorizer.Classes;

namespace StudyMemorizer.Pages;

public class MassExportPage : ContentPage
{
    private Editor datasetEditor = new Editor
    {
        AutoSize = EditorAutoSizeOption.TextChanges,
        FontSize = 20
    };
	public MassExportPage()
    {
        Title = "Mass Export";

        Button copyButton = new Button
        {
            Text = "Copy"
        };
        copyButton.Clicked += copyButton_Clicked;

        RefreshView refreshView = new RefreshView
        {
            Content = new ScrollView
            {
                Orientation = ScrollOrientation.Vertical,
                Content = new StackLayout
                {
                    Children =
                    {
                        copyButton,
                        datasetEditor
                    }
                }
            }
        };
        refreshView.Refreshing += refreshView_Refreshing;
        refreshView_Refreshing(refreshView, null);

        Content = refreshView;
    }

    private void copyButton_Clicked(object? sender, EventArgs e)
    {
        Clipboard.Default.SetTextAsync(datasetEditor.Text);
    }

    private void refreshView_Refreshing(object? sender, EventArgs e)
    {
        if (sender is RefreshView refreshView)
        {
            datasetEditor.Text = String.Join('\n', DataHandler.GetInstance().ExportData());
            refreshView.IsRefreshing = false;
        }
    }
}