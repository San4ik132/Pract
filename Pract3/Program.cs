using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

class Program
{

    class TwoWayNaturalMergeSort
    {
        public static string File1 = "F1.txt", File2 = "F2.txt";

        // Метод для сортировки
        public static void Sort(string inputFilePath)
        {
            Stopwatch stopwatch = new Stopwatch();
            bool isSorted = false;
            while (true)
            {
                stopwatch.Start();
                SplitFile(inputFilePath);
                isSorted = CheckIfSorted(inputFilePath);
                stopwatch.Stop();
                if (isSorted)
                    break;
            }

            Console.WriteLine($"Время, затраченное на эту итерацию: {stopwatch.Elapsed.TotalSeconds} секунд");
        }

        //Метод для разделения файла
        public static void SplitFile(string inputFilePath)
        {
            using (StreamReader inputFile = new StreamReader(inputFilePath))
            {
                bool isFirst = true;
                string line1 = inputFile.ReadLine();
                string line2 = inputFile.ReadLine();

                while (!string.IsNullOrEmpty(line1))
                {
                    if (string.IsNullOrEmpty(line2))
                    {
                        AddToTmp(line1, isFirst ? File1 : File2);
                        break;
                    }

                    if (!int.TryParse(line2, out _))
                    {
                        AddToTmp(line1, isFirst ? File1 : File2);
                        break;
                    }

                    if (int.Parse(line1) <= int.Parse(line2))
                    {
                        AddToTmp(line1, isFirst ? File1 : File2);
                    }
                    else
                    {
                        if (isFirst)
                        {
                            AddToTmp(line1, File1);
                            isFirst = false;
                        }
                        else
                        {
                            AddToTmp(line1, File2);
                            isFirst = true;
                        }
                    }

                    line1 = line2;
                    line2 = inputFile.ReadLine();
                }
            }

            MergeTmpFiles(inputFilePath);
        }

        // Метод для добавления строки во временный файл
        public static void AddToTmp(string line, string tmpPath)
        {
            using (StreamWriter sw = File.AppendText(tmpPath))
            {
                sw.WriteLine(line);
            }
        }

        // Метод для слияния временных файлов
        public static void MergeTmpFiles(string inputFilePath)
        {
            using (StreamWriter file = new StreamWriter(inputFilePath))
            using (StreamReader tmp1 = new StreamReader(File1))
            using (StreamReader tmp2 = new StreamReader(File2))
            {
                string line1 = tmp1.ReadLine();
                string line2 = tmp2.ReadLine();

                while (!string.IsNullOrEmpty(line1) || !string.IsNullOrEmpty(line2))
                {
                    if (!int.TryParse(line1, out _) && !int.TryParse(line2, out _))
                    {
                        break;
                    }
                    else if (!int.TryParse(line1, out _))
                    {
                        file.WriteLine(line2);
                        line2 = tmp2.ReadLine();
                    }
                    else if (!int.TryParse(line2, out _))
                    {
                        file.WriteLine(line1);
                        line1 = tmp1.ReadLine();
                    }
                    else
                    {
                        if (int.Parse(line1) <= int.Parse(line2))
                        {
                            file.WriteLine(line1);
                            line1 = tmp1.ReadLine();
                        }
                        else
                        {
                            file.WriteLine(line2);
                            line2 = tmp2.ReadLine();
                        }
                    }
                }
            }

            File.Delete(File1);
            File.Delete(File2);
        }

        // Метод для проверки отсортированности файла
        public static bool CheckIfSorted(string inputFilePath)
        {
            bool isSorted = true;

            using (StreamReader file = new StreamReader(inputFilePath))
            {
                string line1 = file.ReadLine();
                string line2 = file.ReadLine();

                while (!string.IsNullOrEmpty(line2))
                {
                    if (!int.TryParse(line2, out _))
                    {
                        isSorted = false;
                        break;
                    }

                    if (int.Parse(line1) > int.Parse(line2))
                    {
                        isSorted = false;
                        break;
                    }

                    line1 = line2;
                    line2 = file.ReadLine();
                }
            }

            return isSorted;
        }
    }
    static void Main()
    {

        using (StreamWriter f = new StreamWriter("Output.txt"))
        {
            Random rnd = new Random();
            for (int i = 0; i < 500; i++)
            {
                int a = rnd.Next(0, 100);
                f.WriteLine(a);
            }
        }

        File.Copy("Output.txt", "Input.txt", true);

        TwoWayNaturalMergeSort.Sort("Output.txt");

        Console.WriteLine("Файл успешно отсортирован.");

        string filePath = "Output.txt"; // Путь к файлу

        int lineCount = File.ReadLines(filePath).Count(); // Подсчет количества строк в файле

        Console.WriteLine("Количество строк в файле выхода: " + lineCount);


        string filePath1 = "Input.txt"; // Путь к файлу

        int lineCount1 = File.ReadLines(filePath1).Count(); // Подсчет количества строк в файле

        Console.WriteLine("Количество строк в файле входа: " + lineCount1);
    }

}

