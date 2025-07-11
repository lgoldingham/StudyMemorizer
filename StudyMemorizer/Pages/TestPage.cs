using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific.AppCompat;
using StudyMemorizer.Classes;
using System.Text.RegularExpressions;

namespace StudyMemorizer.Pages;

public class TestPage : ContentPage
{
    // Section: Dataset Selection
    Label datasetLabel = new Label
    {
        Text = "Select Datasets:",
        FontSize = 20,
        HorizontalOptions = LayoutOptions.Start
    };
    private StackLayout datasetsStackLayout = new StackLayout
    {
        Orientation = StackOrientation.Vertical,
    };
    private List<CheckBox> datasetCheckBoxes = new List<CheckBox>();


    // Section: Shuffle Method
    private List<RadioButton> shuffleMethodsRadioButtons = new List<RadioButton>
    {
        new RadioButton
        {
            Content = "Full, AB Swapping",
            GroupName = "ShuffleMethodGroup",
            Value = ShuffleMethod.FullABSwapping,
            IsChecked = true,
            VerticalOptions = LayoutOptions.Center
        },
        new RadioButton
        {
            Content = "Half, AB Swapping",
            GroupName = "ShuffleMethodGroup",
            Value = ShuffleMethod.HalfABSwapping,
            VerticalOptions = LayoutOptions.Center
        },
        new RadioButton
        {
            Content = "Half, A Only",
            GroupName = "ShuffleMethodGroup",
            Value = ShuffleMethod.HalfAOnly,
            VerticalOptions = LayoutOptions.Center
        },
        new RadioButton
        {
            Content = "Half, B Only",
            GroupName = "ShuffleMethodGroup",
            Value = ShuffleMethod.HalfBOnly,
            VerticalOptions = LayoutOptions.Center
        }

    };

    // Section: Testing Method
    private List<RadioButton> testingMethodsRadioButtons = new List<RadioButton>
    {
        new RadioButton
        {
            Content = "Queue card",
            GroupName = "PresentationMode",
            Value = TestingMethod.QueueCard,
            IsChecked = true,
            VerticalOptions = LayoutOptions.Center
        },
        new RadioButton
        {
            Content = "List",
            GroupName = "PresentationMode",
            Value = TestingMethod.List,
            VerticalOptions = LayoutOptions.Center
        }

    };

    // Section: Number of times to repeat when incorrect
    private Label repeatConsecutiveStepperLabel = new Label
    {
        Text = "Repeat Consecutive: 1",
        FontSize = 20
    };
    private Stepper repeatConsecutiveStepper = new Stepper
    {
        Minimum = 0,
        Maximum = 10,
        Increment = 1,
        Value = 1
    };
    private Label repeatRandomStepperLabel = new Label
    {
        Text = "Repeat Random: 1",
        FontSize = 20
    };
    private Stepper repeatRandomStepper = new Stepper
    {
        Minimum = 0,
        Maximum = 10,
        Increment = 1,
        Value = 1
    };


    public TestPage()
    {
        Title = "Test Page";

        // Section: Dataset Selection
        datasetsStackLayout_LoadData();

        // Section: Shuffle Method
        Label shuffleMethodsLabel = new Label
        {
            Text = "Shuffle Method:",
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Start
        };
        StackLayout shuffleMethods = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            Children =
            {
                shuffleMethodsLabel
            }
        };
        foreach (RadioButton rb in shuffleMethodsRadioButtons)
        {
            shuffleMethods.Children.Add(rb);
        }

        // Section: Testing Method
        Label testingMethodLabel = new Label
        {
            Text = "Testing Method:",
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Start
        };

        StackLayout testingMethod = new StackLayout
        {
            Children =
            {
                testingMethodLabel
            }
        };
        foreach (RadioButton rb in testingMethodsRadioButtons)
        {
            testingMethod.Children.Add(rb);
        }

        // Section: Number of times to repeat when incorrect
        Label repeatNumWhenIncorrectLabel = new Label
        {
            Text = "Number of times to repeat when Incorrect:",
            FontSize = 20,
            HorizontalOptions = LayoutOptions.Start
        };
        StackLayout repeatConsecutiveStackLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Children =
            {
                repeatConsecutiveStepper,
                repeatConsecutiveStepperLabel
            }
        };
        repeatConsecutiveStepper.ValueChanged += repeatConsecutiveStepper_ValueChanged;
        StackLayout repeatRandomStackLayout = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            Children =
            {
                repeatRandomStepper,
                repeatRandomStepperLabel
            }
        };
        repeatRandomStepper.ValueChanged += repeatRandomStepper_ValueChanged;

        // Section: Start Testing
        Button startTestingButton = new Button
        {
            Text = "Start Testing",
            HorizontalOptions = LayoutOptions.Fill
        };
        startTestingButton.Clicked += startTestingButton_Clicked;



        RefreshView refreshView = new RefreshView
        {
            Content = new ScrollView
            {
                Orientation = ScrollOrientation.Vertical,
                Content = new StackLayout
                {
                    Children =
                    {
                        datasetsStackLayout,
                        shuffleMethods,
                        testingMethod,
                        repeatNumWhenIncorrectLabel,
                        repeatConsecutiveStackLayout,
                        repeatRandomStackLayout,
                        startTestingButton
                    }
                }
            }
        };
        refreshView.Refreshing += refreshView_Refreshing;

        Content = refreshView;
    }

    private void repeatConsecutiveStepper_ValueChanged(object? sender, ValueChangedEventArgs e)
    {
        if (sender is Stepper stepper)
        {
            repeatConsecutiveStepperLabel.Text = $"Repeat Consecutive: {stepper.Value}";
        }
    }

    private void repeatRandomStepper_ValueChanged(object? sender, ValueChangedEventArgs e)
    {
        if (sender is Stepper stepper)
        {
            repeatRandomStepperLabel.Text = $"Repeat Random: {stepper.Value}";
        }
    }

    private void datasetsStackLayout_LoadData()
    {
        datasetsStackLayout.Children.Clear();
        datasetsStackLayout.Add(datasetLabel);
        foreach (Dataset dataset in DataHandler.GetInstance().GetDatasets())
        {
            CheckBox c = new CheckBox
            {
                IsChecked = false,
                BindingContext = dataset
            };
            datasetCheckBoxes.Add(c);
            datasetsStackLayout.Add(new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Children = { c, new Label { Text = dataset.ToString(), VerticalOptions = LayoutOptions.Center } }
            });
        }
    }

    private void refreshView_Refreshing(object? sender, EventArgs e)
    {
        if (sender is RefreshView refreshView)
        {
            datasetsStackLayout_LoadData();
            refreshView.IsRefreshing = false;
        }
    }

    public void startTestingButton_Clicked(object? sender, EventArgs e)
    {
        // get datasets
        List<Dataset> datasets = new List<Dataset>();
        bool noDatasetsSelected = true;
        foreach (CheckBox checkBox in datasetCheckBoxes)
        {
            if (checkBox.BindingContext is Dataset dataset)
            {
                if (noDatasetsSelected != checkBox.IsChecked)
                {
                    datasets.Add(dataset);
                }
                else if (noDatasetsSelected && checkBox.IsChecked)
                {
                    noDatasetsSelected = false;
                    datasets.Clear();
                    datasets.Add(dataset);
                }
            }
        }

        // get shuffle method and create question set
        foreach (RadioButton radioButton in shuffleMethodsRadioButtons)
        {
            if (radioButton.IsChecked == true && radioButton.Value is ShuffleMethod shuffleMethod)
            {
                QuestionSet.GetInstance().ResetQuestions(shuffleMethod, datasets, (int)repeatConsecutiveStepper.Value, (int)repeatRandomStepper.Value);
            }
        }

        // get testing method
        TestingMethod testingMethod;
        foreach (RadioButton radioButton in testingMethodsRadioButtons)
        {
            if (radioButton.IsChecked == true && radioButton.Value is TestingMethod tm)
            {
                testingMethod = tm;
            }
        }
        Navigation.PushAsync(new StudyMemorizer.Pages.TestingPages.QueueCardViewPage());
    }
}