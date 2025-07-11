using StudyMemorizer.Classes;

namespace StudyMemorizer.Pages;

public class CreateModifyPage : ContentPage
{
    // Section: Dataset Selection
    private List<Dataset> datasets;
    private Picker selectDatasetPicker = new Picker
    {
        HorizontalOptions = LayoutOptions.Fill
    };

    // Section: Dataset Modification
    Entry datasetNameEntry = new Entry
    {
        FontSize = 20,
        HorizontalOptions = LayoutOptions.Fill
    };
    Editor datasetModificationEditor = new Editor
    {
        FontSize = 20,
        HorizontalOptions = LayoutOptions.Fill,
        AutoSize = EditorAutoSizeOption.TextChanges
    };

    public CreateModifyPage()
	{
		Title = "Create/Modify Dataset";

        // Section: Dataset Selection
        Label selectDatasetLabel = new Label
        {
            Text = "Select Dataset:",
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Start
        };
        selectDatasetPicker.SelectedIndexChanged += selectDatasetPicker_SelectedIndexChanged;

        // Section: Dataset Modification
        Label datasetModificationLabel = new Label
        {
            Text = "Dataset Modification:",
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Start
        };
        Button saveButton = new Button
        {
            Text = "Save",
            HorizontalOptions = LayoutOptions.Fill
        };
        saveButton.Clicked += saveButton_Clicked;


        RefreshView refreshView = new RefreshView
        {
            Content = new ScrollView
            {
                Orientation = ScrollOrientation.Vertical,
                Content = new StackLayout
                {
                    Children =
                    {
                        selectDatasetLabel,
                        selectDatasetPicker,
                        datasetNameEntry,
                        datasetModificationEditor,
                        saveButton
                    }
                }
            }
        };
        refreshView.Refreshing += refreshView_Refreshing;
        refreshView_Refreshing(refreshView, null);

        Content = refreshView;
    }

    private void refreshView_Refreshing(object? sender, EventArgs e)
    {
        if (sender is RefreshView refreshView)
        {
            datasets = new List<Dataset> { new Dataset() };
            datasets.AddRange(DataHandler.GetInstance().GetDatasets());
            selectDatasetPicker.ItemsSource = datasets;
            selectDatasetPicker.SelectedIndex = 0;
            refreshView.IsRefreshing = false;
        }
    }

    private void saveButton_Clicked(object? sender, EventArgs e)
    {
        if (selectDatasetPicker.SelectedItem is Dataset dataset && datasetNameEntry.Text != "" && datasetModificationEditor.Text != "")
        {
            dataset.Save(datasetNameEntry.Text, datasetModificationEditor.Text);
            if (selectDatasetPicker.SelectedIndex == 0)
            {
                DataHandler.GetInstance().Add(dataset);
                datasets = new List<Dataset> { new Dataset() };
                datasets.AddRange(DataHandler.GetInstance().GetDatasets());
                selectDatasetPicker.ItemsSource = datasets;
                selectDatasetPicker.SelectedIndex = datasets.Count-1;
            }
        }
    }

    private void selectDatasetPicker_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (sender is Picker picker)
        {
            if (picker.SelectedItem is Dataset dataset)
            {
                datasetNameEntry.Text = dataset.GetName();
                datasetModificationEditor.Text = dataset.GetData();
            }
        }
    }
}