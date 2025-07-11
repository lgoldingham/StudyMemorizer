using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyMemorizer.Classes
{
    internal class DataHandler
    {
        private static DataHandler _instance = null;
        private List<Dataset> _dataSet = new List<Dataset>();
        private DataHandler() { }

        public static DataHandler GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DataHandler();
                _instance.LoadData();
            }
            return _instance;
        }

        public int LoadData()
        {
            string path = Path.Combine(FileSystem.Current.AppDataDirectory, "Datasets");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Debug.WriteLine(path);
                return 0;
            }

            string[] files = Directory.GetFiles(path);
            Array.Sort(files);
            int count = 0;
            _dataSet.Clear();
            foreach (string item in files)
            {
                count++;
                _dataSet.Add(new Dataset(item));
                Debug.WriteLine($"Loaded: {item}");
            }
            return count;
        }

        public List<string> GetNames()
        {
            List<string> names = new List<string>();
            foreach (Dataset dataset in _dataSet)
            {
                names.Add(dataset.GetName());
            }
            return names;
        }

        public Dataset FindDatasetByName(string name)
        {
            foreach (Dataset dataset in _dataSet)
            {
                if (dataset.GetName() == name)
                {
                    return dataset;
                }
            }
            return null;
        }

        public List<Dataset> GetDatasets()
        {
            return _dataSet;
        }
        public void Delete(Dataset dataset)
        {
            _dataSet.Remove(dataset);
            string path = Path.Combine(FileSystem.Current.AppDataDirectory, "Datasets", dataset.GetName());
            if (Path.Exists(path))
            {
                File.Delete(path);
            }
        }

        public List<string> ExportData()
        {
            List<string> output = new List<string>();
            for (int i = 0; i < _dataSet.Count; i++)
            {
                output.Add(_dataSet[i].GetName());
                output.Add(_dataSet[i].GetData());
            }
            return output;
        }

        public void Add(Dataset dataset)
        {
            _dataSet.Add(dataset);
        }

        public void ImportData(string input)
        {
            string[] values = input.Trim().Split('\n', '\r');
            Dataset dataset = null;
            List<string> datasetValues = new List<string>();
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] != "")
                {
                    if (!values[i].Contains(','))
                    {
                        if (dataset != null)
                        {
                            _dataSet.Add(dataset);
                            dataset.Save();
                            dataset = null;
                        }
                        if (FindDatasetByName(values[i]) == null)
                        {
                            dataset = new Dataset();
                            dataset.SetName(values[i]);
                        }
                    }
                    else if (dataset != null)
                    {
                        string[] line = values[i].Split(',');
                        dataset.Add(new Question(line[0].Split(';'), line[1].Split(';')));
                    }
                }
            }
            
        }


    }
}
