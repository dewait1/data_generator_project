using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator
{
    class FileController
    {
        private string currentDir;

        public FileController()
        {
            currentDir = Environment.CurrentDirectory;
        }

        public void OpenFile(string fileName)
        {
            Process.Start(currentDir + @"\DataSets\" + fileName + ".txt");          
        }

        public string[] ReadFile(string fileName)
        {
            string[] lines = System.IO.File.ReadAllLines(currentDir + @"\DataSets\" + fileName);
            return lines;
        }

        async public void SaveToFile(string text)
        {
            using (StreamWriter sw = new StreamWriter(currentDir + @"\log.txt", false, System.Text.Encoding.Default))
            {
                await sw.WriteLineAsync(text);
            }
        }
    }
}
