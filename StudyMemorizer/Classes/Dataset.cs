using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace StudyMemorizer.Classes
{
    internal class Dataset
    {
        // holds the name of the dataset, used for display
        private string _name = "";
        // used to hold the questions
        private List<Question> _questionData = new List<Question>();
        // used to hold the location of the dataset
        private string _path = "";

        public Dataset()
        {
            _name = "Untitled Dataset";
            _path = Path.Combine(FileSystem.Current.AppDataDirectory, "Datasets", _name);
            int i = 0;
            while (Path.Exists(_path))
            {
                _name = $"Untitled Dataset {i}";
                _path = Path.Combine(FileSystem.Current.AppDataDirectory, "Datasets", _name);
            }
        }
        public Dataset(string path)
        {
            _name = Path.GetFileName(path);
            _path = path;
            LoadData();
        }

        public void Add(Question question)
        {
            _questionData.Add(question);
        }

        public void SetName(string name)
        {
            string newPath = Path.Combine(FileSystem.Current.AppDataDirectory, "Datasets", name);
            if (!Path.Exists(newPath))
            {
                if (!Path.Exists(_path))
                {
                    _name = name;
                    _path = newPath;
                }
                else if (_path != newPath)
                {
                    _name = name;
                    File.Move(_path, newPath);
                    _path = newPath;
                }
            }

        }

        public void LoadData()
        {
            StreamReader sr = new StreamReader(_path);
            while (!sr.EndOfStream)
            {
                var line = sr.ReadLine().Trim().Split(',');
                if (line.Length == 2)
                {
                    Question q = new Question(line[0].Split(';'), line[1].Split(';'));
                    _questionData.Add(q);
                }

            }
        }

        public void Save(string newName, string newData)
        {
            SetName(newName);
            if (_name == newName || !File.Exists(Path.Combine(FileSystem.Current.AppDataDirectory, "Datasets", newName)))
            {
                _name = newName;
                _questionData.Clear();
                string[] lines = newData.Split('\n', '\r');
                foreach (string line in lines)
                {
                    string[] translations = line.Split(',');
                    if (translations.Length == 2)
                    {
                        _questionData.Add(new Question(translations[0].Split(';'), translations[1].Split(';')));
                    }
                }
                Save();
            }
            
        }

        public void Save()
        {
            if (Path.Exists(_path))
            {
                File.Delete(_path);
            }
            StreamWriter sw = new StreamWriter(_path);
            foreach (Question q in _questionData)
            {
                sw.WriteLine(q.GetData());
            }
            sw.Close();
        }

        public List<Question> GetQuestions()
        {
            return _questionData;
        }

        public int NumQuestions()
        {
            return _questionData.Count;
        }

        public override string ToString()
        {
            return $"{_name}({NumQuestions()})";
        }

        public string LabelFormat()
        {
            string output = "";
            for (int i = 0; i < NumQuestions(); i++)
            {
                output += $"Question {i + 1}:\n{_questionData[i].LabelFormat()}";
            }
            return output;
        }

        public string GetName()
        {
            return _name;
        }

        public string GetData()
        {
            string output = "";
            foreach (Question question in _questionData)
            {
                output += $"{question.GetData()}{Environment.NewLine}";
            }
            return output;
        }
    }
}
