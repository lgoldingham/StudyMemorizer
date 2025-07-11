using StudyMemorizer.Classes;

namespace StudyMemorizer.Pages.TestingPages;

public class QueueCardViewPage : ContentPage
{
    private Label queueCardQuestionLabel = new Label
    {
        FontSize = 20,
        HorizontalOptions = LayoutOptions.Center
    };
    private Entry queueCardAnswerEntry = new Entry
    {
        FontSize = 20,
        HorizontalOptions = LayoutOptions.Fill,
        HorizontalTextAlignment = TextAlignment.Center
    };
    Button submitButton = new Button
    {
        Text = "Answer Later",
        HorizontalOptions = LayoutOptions.Fill
    };
    Label correctAnswerLabel = new Label
    {
        FontSize = 20,
        HorizontalOptions = LayoutOptions.Center
    };
    public QueueCardViewPage()
    {
        Title = "Test Page";

        // Section: Button Control
        Button skipButton = new Button
        {
            Text = "Skip",
            HorizontalOptions = LayoutOptions.Fill
        };
        skipButton.Clicked += skipButton_Clicked;
        submitButton.Clicked += queueCardAnswerEntry_Completed;
        Button markButton = new Button
        {
            Text = "Mark",
            HorizontalOptions = LayoutOptions.Fill
        };
        markButton.Clicked += markButton_Clicked;
        StackLayout buttonControlStackLayout = new StackLayout
        {
            Orientation = StackOrientation.Vertical,
            Children =
            {
                skipButton,
                submitButton,
                markButton
            }
        };

        queueCardAnswerEntry.Completed += queueCardAnswerEntry_Completed;
        queueCardAnswerEntry.TextChanged += queueCardAnswerEntry_TextChanged;

        ReloadCard();

        Content = new ScrollView
        {
            Orientation = ScrollOrientation.Vertical,
            Content = new StackLayout
            {
                Children =
                {
                    queueCardQuestionLabel,
                    queueCardAnswerEntry,
                    buttonControlStackLayout,
                    correctAnswerLabel
                }
            }
        };
    }

    private void queueCardAnswerEntry_TextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            submitButton.Text = entry.Text == "" ? "Answer Later" : "Submit";
        }
    }

    private void markButton_Clicked(object? sender, EventArgs e)
    {
        Mark();
    }

    private void queueCardAnswerEntry_Completed(object? sender, EventArgs e)
    {
        if (queueCardAnswerEntry.Text == "")
        {
            QuestionSet.GetInstance().AnswerFirstLater();
        }
        else
        {
            correctAnswerLabel.Text = String.Join('\n', QuestionSet.GetInstance().Answer(queueCardAnswerEntry.Text));
        }
        if (QuestionSet.GetInstance().HasNext())
        {
            ReloadCard();
        }
        else
        {
            Mark();
        }
    }

    private void skipButton_Clicked(object? sender, EventArgs e)
    {
        bool a = QuestionSet.GetInstance().HasNext();
        if (QuestionSet.GetInstance().HasNext())
        {
            QuestionSet.GetInstance().SkipFirst();
            ReloadCard();
        }
        else
        {
            Mark();
        }
    }

    private void ReloadCard()
    {
        QuestionSet questionSet = QuestionSet.GetInstance();
        if (questionSet.HasNext())
        {
            queueCardQuestionLabel.Text = questionSet.GetNext();
            queueCardAnswerEntry.Text = "";
            queueCardAnswerEntry.Focus();
        }
        else
        {
            Mark();
        }
    }

    private void Mark()
    {
        Navigation.PushAsync(new StudyMemorizer.Pages.TestingPages.MarkingPage());
    }
}