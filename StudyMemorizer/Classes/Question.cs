using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyMemorizer
{
    internal class Question
    {
        // the display name for A
        private List<string> _translationA = new List<string>();
        // alternative vertions of A that will be accepted
        private List<string> _acceptableTranslationsA = new List<string>();
        // the display name for B
        private List<string> _translationB = new List<string>();
        // alternative vertions of B that will be accepted
        private List<string> _acceptableTranslationsB = new List<string>();

        private Question() { }
        public Question(string[] translationA, string[] translationB)
        {
            _translationA.AddRange(translationA[0].Split('/'));
            _translationB.AddRange(translationB[0].Split('/'));
            _acceptableTranslationsA.AddRange(translationA[1..]);
            _acceptableTranslationsB.AddRange(translationB[1..]);
        }

        public bool CheckAnswer(string answer, bool questionIsA)
        {
            List<string> temp = GetAnswers(questionIsA);
            foreach (string s in temp)
            {
                if (RemoveSpecial(s) == RemoveSpecial(answer))
                {
                    return true;
                }
            }
            return false;
        }
        
        private string RemoveSpecial(string value)
        {
            return new string((from c in value where char.IsLetterOrDigit(c) select c).ToArray()).ToLower();
        }

        public void AddAcceptableTranslation(string translation, bool questionIsA)
        {
            if (questionIsA)
            {
                _acceptableTranslationsA.Add(translation);
            }
            else
            {
                _acceptableTranslationsB.Add(translation);
            }
        }

        public string GetData()
        {
            string output = String.Join('/', _translationA);
            if (_acceptableTranslationsA.Count > 0)
            {
                output += $";{String.Join(';', _acceptableTranslationsA)}";
            }
            output += $",{String.Join('/', _translationB)}";
            if (_acceptableTranslationsB.Count > 0)
            {
                output += $";{String.Join(';', _acceptableTranslationsB)}";
            }
            return output;
        }

        public string LabelFormat()
        {
            string output = $"\t{String.Join('/', _translationA)}\n";
            for (int i = 1; i < _acceptableTranslationsA.Count; i++)
            {
                output += $"\t\t{_acceptableTranslationsA[i]}\n";
            }
            output += $"\t{String.Join('/', _translationB)}\n";
            for (int i = 1; i < _acceptableTranslationsB.Count; i++)
            {
                output += $"\t\t{_acceptableTranslationsB[i]}\n";
            }
            return output;
        }

        public string GetQuestion(bool questionIsA)
        {
            Random rand = new Random();
            return questionIsA ? _translationA[rand.Next(_translationA.Count)] : _translationB[rand.Next(_translationB.Count)];
        }

        public List<string> GetAnswers(bool questionIsA)
        {
            return (questionIsA ? _translationB.Concat(_acceptableTranslationsB) : _translationA.Concat(_acceptableTranslationsA)).ToList<string>();
        }
    }
}
