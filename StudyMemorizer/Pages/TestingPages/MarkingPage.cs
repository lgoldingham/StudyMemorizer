using StudyMemorizer.Classes;
using System.Numerics;

namespace StudyMemorizer.Pages.TestingPages;

public class MarkingPage : ContentPage
{
    Label scorePercentTotalLabel = new Label
    {
        FontSize = 20
    };
    Label scorePercentAnsweredLabel = new Label
    {
        FontSize = 20
    };
    Label scoreFractionTotalLabel = new Label
    {
        FontSize = 20
    };
    Label scoreFractionAnsweredLabel = new Label
    {
        FontSize = 20
    };
    Label correctAnswerLabel = new Label
    {
        Text = "Questions Answered Correctly:",
        FontSize = 20
    };
    Editor correctAnswerEditor = new Editor
    {
        IsReadOnly = true,
        AutoSize = EditorAutoSizeOption.TextChanges,
        FontSize = 20
    };
    Label incorrectAnswerLabel = new Label
    {
        Text = "Questions Answered Incorrectly:",
        FontSize = 20
    };
    Editor incorrectAnswerEditor = new Editor
    {
        IsReadOnly = true,
        AutoSize = EditorAutoSizeOption.TextChanges,
        FontSize = 20
    };
    Label notAnsweredLabel = new Label
    {
        Text = "Questions Not Answered:",
        FontSize = 20
    };
    Editor notAnsweredEditor = new Editor
    {
        IsReadOnly = true,
        AutoSize = EditorAutoSizeOption.TextChanges,
        FontSize = 20
    };

    public MarkingPage()
	{
		Title = "Marking";
        Initialize();


        RefreshView refreshView = new RefreshView
        {
            Content = new ScrollView
            {
                Orientation = ScrollOrientation.Vertical,
                Content = new StackLayout
                {
                    Children =
                    {
                        scorePercentTotalLabel,
                        scorePercentAnsweredLabel,
                        scoreFractionTotalLabel,
                        scoreFractionAnsweredLabel,
                        correctAnswerLabel,
                        correctAnswerEditor,
                        incorrectAnswerLabel,
                        incorrectAnswerEditor,
                        notAnsweredLabel,
                        notAnsweredEditor
                    }
                }
            }
        };
        refreshView.Refreshing += refreshView_Refreshing;

        Content = refreshView;
    }

    private void refreshView_Refreshing(object? sender, EventArgs e)
    {
        if (sender is RefreshView refreshView)
        {
            Initialize();
            refreshView.IsRefreshing = false;
        }
    }

    private void Initialize()
	{
		QuestionSet questionSet = QuestionSet.GetInstance();
		questionSet.Mark();

        scorePercentTotalLabel.Text = $"Percent of Total: {questionSet.PercentageTotal()}";
        scorePercentAnsweredLabel.Text = $"Percent of Answered: {questionSet.PercentageAnswered()}";
        scoreFractionTotalLabel.Text = $"Fraction of Total: {questionSet.FractionTotal()}";
        scoreFractionAnsweredLabel.Text = $"Fraction of Answered: {questionSet.FractionAnswered()}";
        correctAnswerEditor.Text = String.Join(Environment.NewLine, questionSet.GetCorrect());
        incorrectAnswerEditor.Text = String.Join(Environment.NewLine, questionSet.GetIncorrect());
        notAnsweredEditor.Text = String.Join(Environment.NewLine, questionSet.GetNotAnswered());

    }
}