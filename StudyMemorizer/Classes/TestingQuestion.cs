using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyMemorizer.Classes
{
    internal class TestingQuestion
    {
        // decides if A is the question or B is the question
        private bool _questionIsA = true;
        // holds the question object
        private Question _question;
        // holds the question that is displayed
        private string _displayQuestion = "";
        // holds the user's answer
        private string _answer = "";

        private TestingQuestion() { }
        public TestingQuestion(Question question, bool questionIsA)
        {
            _question = question;
            _questionIsA = questionIsA;
            _displayQuestion = question.GetQuestion(questionIsA);
        }
        public TestingQuestion(Question question, bool questionIsA, string displayQuestion)
        {
            _question = question;
            _questionIsA = questionIsA;
            _displayQuestion = displayQuestion;
        }

        public void SetQuestionIsA(bool quesitonIsA)
        {
            _questionIsA = quesitonIsA;
        }

        public void Answer(string answer)
        {
            _answer = answer;
        }

        public bool CheckAnswer()
        {
            return _question.CheckAnswer(_answer, _questionIsA);
        }

        public bool CheckAnswer(string answer)
        {
            return _question.CheckAnswer(answer, _questionIsA);
        }

        public bool Equals(TestingQuestion other)
        {
            return _questionIsA == other._questionIsA && _question == other._question;
        }

        public TestingQuestion Clone()
        {
            return new TestingQuestion(_question, _questionIsA, _displayQuestion);
        }

        public bool IsQuestionA()
        {
            return _questionIsA;
        }

        public string GetQuestion()
        {
            return _displayQuestion;
        }

        public List<string> GetAnswer()
        {
            return _question.GetAnswers(_questionIsA);
        }

        public bool HasAnswer()
        {
            return _answer != "";
        }

        public override string ToString()
        {
            string q = GetQuestion();
            if (HasAnswer())
            {
                if (CheckAnswer())
                {
                    return $"{q} = {_answer}";
                }
                return $"{q} ≠ {_answer}\n    {String.Join("\n    ", _question.GetAnswers(_questionIsA))}";
            }
            return $"{q}\n    {String.Join("\n    ", _question.GetAnswers(_questionIsA))}";
        }
    }
}
