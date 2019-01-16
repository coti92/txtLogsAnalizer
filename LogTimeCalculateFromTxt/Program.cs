using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogTimeCalculateFromTxt
{
    class Program
    {

        ////////////////////
        ///////STRUCT///////
        ////////////////////
        
        public struct logClass
        {
            public string logText;
            public DateTime logDate;

            public logClass(DateTime date, string text)
            {
                logText = text;
                logDate = date;
            }
        }


        ////////////////////
        ////////MAIN////////
        ////////////////////
        
        static void Main(string[] args) 
        {
            Console.WriteLine("Start after press a key");
            Console.ReadKey();
            IEnumerable<logClass> logList = ReadTxt();
            WriteTxt(logList);
            Console.WriteLine("Finish the task, press a key");
            Console.ReadKey();
        }


        /////////////////////
        //////FUNCTIONS//////
        /////////////////////

        public static string pathRequest(bool fileExists)
        {
            Console.WriteLine("File Path");
            string path = Console.ReadLine();

            if (fileExists)
            {
                while (!File.Exists(path))
                {
                    Console.WriteLine("Please write the File Path");
                    path = Console.ReadLine();
                }
            }
            else
            {
                Console.WriteLine($"The Path is {path}");
            }

            Console.WriteLine("Valid Path");
            
            return path;
        }

        public static IEnumerable<logClass> ReadTxt()
        {
            Console.WriteLine("Data :");
            string path = pathRequest(true);

            using (var tr = new StreamReader(path))
            {
                //Extraction data

                string file = tr.ReadToEnd();

                IEnumerable<string> dataTimeLines = file.Trim().Split('[');

                List<logClass> logData = new List<logClass>();

                DateTime date;
                string text;

                foreach (var line in dataTimeLines)
                {
                    if (line != "")
                    {
                        IEnumerable<string> data = line.Split(']');
                        date = DateTime.Parse(data.First());
                        text = data.Last().Replace('\r', ' ').Trim();
                        logData.Add(new logClass(date, text));
                    }
                }

                //logData.ForEach(log => Console.WriteLine(log.logDate+" " +log.logText));
                
                tr.Close();

                Console.WriteLine("Extract completed");

                return logData;
            }

        }

        private static void WriteTxt(IEnumerable<logClass> logs)
        {
            //Path donde guardar el CSV
            Console.WriteLine("Results :");
            string path =pathRequest(false);

            List<string> tableNames = new List<string>();

            DateTime a, b;

            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine($"TableName , TimeExecute");
                    foreach (var log in logs)
                    {
                        tableNames.Add(log.logText.Split(' ').Last());
                    }

                    foreach (var table in tableNames)
                    {
                        a = logs.Where(x => x.logText.Contains(table)).Max(x => x.logDate);
                        b = logs.Where(x => x.logText.Contains(table)).Min(x => x.logDate);

                        tw.WriteLine($"{table} , {a-b}");
                    }
                    tw.Close();
                    Console.WriteLine("Work Done");
                }
            }
            else if (File.Exists(path))
            {
                using (var tw = new StreamWriter(path, true))
                {
                    tw.WriteLine($"TableName , TimeExecute");
                    foreach (var log in logs)
                    {
                        tableNames.Add(log.logText.Split(' ').Last());
                    }

                    foreach (var table in tableNames)
                    {
                        a = logs.Where(x => x.logText.Contains(table)).Max(x => x.logDate);
                        b = logs.Where(x => x.logText.Contains(table)).Min(x => x.logDate);

                        tw.WriteLine($"{table} , {a - b}");
                    }
                    tw.Close();
                    Console.WriteLine("Work Done");
                }
            }
        }
    }    
}
