using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

class TwoWayNaturalMergeSort
{

    public static string tmp1Path = "tmp1.txt", tmp2Path = "tmp2.txt";
    public static void Sort(string inputFilePath)
    {
        bool isSorted = false;
        while (true)
        {
            using (StreamReader inputFile = new StreamReader(inputFilePath))
            {
                bool isFirst = true;
                string line1 = inputFile.ReadLine();
                string line2 = "";
                while (true)
                {
                    if (line1 == "") break;
                    line2 = inputFile.ReadLine();
                    if (!int.TryParse(line2, out int n))
                    {
                        if (isFirst)
                        {
                            AddToTmp(line1, true);
                        }
                        else
                        {
                            AddToTmp(line1, false);
                        }
                        break;
                    }
                    else if (int.Parse(line1) <= int.Parse(line2))
                    {
                        if (isFirst)
                        {
                            AddToTmp(line1, true);
                        }
                        else
                        {
                            AddToTmp(line1, false);
                        }
                    }
                    else
                    {
                        if (isFirst)
                        {
                            AddToTmp(line1, true);
                            isFirst = false;
                        }
                        else
                        {
                            AddToTmp(line1, false);
                            isFirst = true;
                        }
                    }
                    line1 = line2;
                }
            }
            using (StreamWriter file = new StreamWriter(inputFilePath))
            using (StreamReader tmp1 = new StreamReader(tmp1Path))
            using (StreamReader tmp2 = new StreamReader(tmp2Path))
            {
                string line1 = tmp1.ReadLine();
                string line2 = tmp2.ReadLine();
                while (true)
                {
                    if (!int.TryParse(line1, out int n2) && !int.TryParse(line2, out int n3))
                    {
                        break;
                    }
                    else if (!int.TryParse(line1, out int n))
                    {
                        file.WriteLine(line2);
                        line2 = tmp2.ReadLine();
                        continue;
                    }
                    else if (!int.TryParse(line2, out int n1))
                    {
                        file.WriteLine(line1);
                        line1 = tmp1.ReadLine();
                        continue;
                    }
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
            File.Delete(tmp1Path);
            File.Delete(tmp2Path);
            isSorted = true;
            using (StreamReader file = new StreamReader(inputFilePath)) 
            {
                string line1 = file.ReadLine();
                string line2 = "";
                while (true) 
                {
                    line2 = file.ReadLine();
                    if (!int.TryParse(line2, out int n)) break;
                    if (int.Parse(line1) <= int.Parse(line2))
                    {
                        line1 = line2;
                        continue;
                    }
                    else 
                    {
                        isSorted = false;
                        break;
                    }
                }
            }
            if (isSorted) break;
        }
    }

    public static void AddToTmp(string line, bool isFirst)
    {
        if (isFirst)
        {
            using (StreamWriter sw = File.AppendText(tmp1Path))
            {
                sw.WriteLine(line);
            }
        }
        else
        {
            using (StreamWriter sw = File.AppendText(tmp2Path))
            {
                sw.WriteLine(line);
            }
        }
    }
}

class Program
{
    static void Main()
    {
        string inputFilePath = "input.txt";

        TwoWayNaturalMergeSort.Sort(inputFilePath);

        Console.WriteLine("Файл успешно отсортирован.");
    }
}
