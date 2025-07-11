using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyMemorizer.Classes
{
    internal class QuestionSet
    {
        private static QuestionSet _instance = null;
        private List<TestingQuestion> _questionSet = new List<TestingQuestion>();
        private List<TestingQuestion> _answeredQuestions = new List<TestingQuestion>();
        private List<TestingQuestion> _correctAnswers = new List<TestingQuestion>();
        private List<TestingQuestion> _incorrectAnswers = new List<TestingQuestion>();
        private List<TestingQuestion> _notAnswered = new List<TestingQuestion>();
        private int _repeatConsecutive = 0;
        private int _repeatRandom = 0;

        private QuestionSet() { }

        public static QuestionSet GetInstance()
        {
            if (_instance == null)
            {
                _instance = new QuestionSet();
            }
            return _instance;
        }

        public void ResetQuestions(ShuffleMethod shuffleMethod, List<Dataset> datasets, int repeatConsecutive, int repeatRandom)
        {
            _questionSet.Clear();
            _answeredQuestions.Clear();
            _correctAnswers.Clear();
            _incorrectAnswers.Clear();
            _notAnswered.Clear();
            _repeatConsecutive = repeatConsecutive;
            _repeatRandom = repeatRandom;
            switch (shuffleMethod)
            {
                case ShuffleMethod.FullABSwapping:
                    LoadHalf(datasets, true);
                    LoadHalf(datasets, false);
                    break;
                case ShuffleMethod.HalfABSwapping:
                    LoadHalf(datasets);
                    break;
                case ShuffleMethod.HalfAOnly:
                    LoadHalf(datasets, true);
                    break;
                case ShuffleMethod.HalfBOnly:
                    LoadHalf(datasets, false);
                    break;
            }
            Shuffle();
        }

        private void LoadHalf(List<Dataset> datasets, bool questionIsA)
        {
            foreach (Dataset dataset in datasets)
            {
                foreach (Question question in dataset.GetQuestions())
                {
                    _questionSet.Add(new TestingQuestion(question, questionIsA));
                }
            }
        }
        private void LoadHalf(List<Dataset> datasets)
        {
            Random rand = new Random();
            foreach (Dataset dataset in datasets)
            {
                foreach (Question question in dataset.GetQuestions())
                {
                    _questionSet.Add(new TestingQuestion(question, rand.Next(2) == 0));
                }
            }
        }
        private void Shuffle()
        {
            Random rand = new Random();
            for (int i = _questionSet.Count-1; i > 0; i--)
            {
                int j = rand.Next(i + 1);
                TestingQuestion temp = _questionSet[j];
                _questionSet[j] = _questionSet[i];
                _questionSet[i] = temp;
            }
        }

        public void AnswerFirstLater()
        {
            _questionSet.Add(_questionSet[0]);
            _questionSet.RemoveAt(0);
        }

        public void SkipFirst()
        {
            _answeredQuestions.Add(_questionSet[0]);
            _questionSet.RemoveAt(0);
        }

        public void AnswerFirst(string question)
        {
            _questionSet[0].Answer(question);
            SkipFirst();
        }

        public bool HasNext()
        {
            return _questionSet.Count > 0;
        }

        public string GetNext()
        {
            return _questionSet[0].GetQuestion();
        }

        public void Mark()
        {
            _notAnswered.Clear();
            _correctAnswers.Clear();
            _incorrectAnswers.Clear();
            foreach (TestingQuestion question in _answeredQuestions)
            {
                if (!question.HasAnswer())
                {
                    _notAnswered.Add(question);
                }
                else if (question.CheckAnswer())
                {
                    _correctAnswers.Add(question);
                }
                else
                {
                    _incorrectAnswers.Add(question);
                }
            }
            _notAnswered.AddRange(_questionSet);
        }

        public List<string> Answer(string answer)
        {
            TestingQuestion question = _questionSet[0];
            question.Answer(answer);
            SkipFirst();
            if (question.CheckAnswer(answer))
            {
                return new List<string>();
            }

            for (int i = 0; i < _repeatConsecutive; i++)
            {
                if (_questionSet.Count == 0 || !_questionSet[i].Equals(question))
                {
                    _questionSet.Insert(0, question.Clone());
                }
                
            }

            Random rand = new Random();
            for (int i = GetNumInstances(question)-_repeatConsecutive; i < _repeatRandom; i++)
            {
                _questionSet.Insert(rand.Next(_repeatConsecutive, _questionSet.Count), question.Clone());
            }

            return question.GetAnswer();
        }

        private int GetNumInstances(TestingQuestion question)
        {
            int output = 0;
            for (int i = 0; i < _questionSet.Count; i++)
            {
                if (_questionSet[i].Equals(question))
                {
                    output++;
                }
            }
            return output;
        }

        public string PercentageTotal()
        {
            return $"%{float.Round(((float)_correctAnswers.Count) / (_answeredQuestions.Count + _questionSet.Count) * 100, 2)}";
        }

        public string PercentageAnswered()
        {
            return $"%{float.Round(((float)_correctAnswers.Count) / _answeredQuestions.Count * 100, 2)}";
        }

        public string FractionTotal()
        {
            return $"{_correctAnswers.Count}/{_answeredQuestions.Count + _questionSet.Count}";
        }

        public string FractionAnswered()
        {
            return $"{_correctAnswers.Count}/{_answeredQuestions.Count}";
        }

        public List<string> GetCorrect()
        {
            List<string> output = new List<string> ();
            foreach (TestingQuestion question in _correctAnswers)
            {
                output.Add(question.ToString());
            }
            return output;
        }

        public List<string> GetIncorrect()
        {
            List<string> output = new List<string>();
            foreach (TestingQuestion question in _incorrectAnswers)
            {
                output.Add(question.ToString());
            }
            return output;
        }

        public List<string> GetNotAnswered()
        {
            List<string> output = new List<string>();
            foreach (TestingQuestion question in _notAnswered)
            {
                output.Add(question.ToString());
            }
            return output;
        }

    }
}