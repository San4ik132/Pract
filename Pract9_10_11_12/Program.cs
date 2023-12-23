using System;
using System.IO;

namespace Pract9_10_11_12
{
    internal class Program
    {
        public class Find
        {
            static int[] GetPrefix(string s)
            {
                int[] result = new int[s.Length];
                result[0] = 0;

                for (int i = 1; i < s.Length; i++)
                {
                    int k = result[i - 1];
                    while (s[k] != s[i] && k > 0) k = result[k - 1];

                    if (s[k] == s[i]) result[i] = k + 1;
                    else result[i] = 0;
                }
                return result;
            }

           public static int FindSubstring(string pattern, string text)
            {
                int[] pf = GetPrefix(pattern);
                int index = 0;

                for (int i = 0; i < text.Length; i++)
                {
                    while (index > 0 && pattern[index] != text[i]) index = pf[index - 1];

                    if (pattern[index] == text[i]) index++;
                    if (index == pattern.Length) return i - index + 1;
                }

                return -1;
            }


            public static int Boyer(string text, string pattern)
            {
                int N = text.Length;
                int M = pattern.Length;
                int[] D = new int[2048];

                for (int j = 0; j < D.Length; j++)
                    D[j] = M;
                for (int j = 0; j < M - 1; j++)
                    D[pattern[j]] = M - j - 1;

                // поиск
                int i = M - 1;

                while (i < N)
                {
                    int j = M - 1;
                    int k = i;
                    while (j >= 0 && pattern[j] == text[k])
                    {
                        k--;
                        j--;
                    }
                    if (j < 0)
                    {
                        return k + 1; // совпадение найдено
                    }

                    i += D[text[i]]; // сдвиг
                }

                return -1; // совпадение не найдено
            }
        }
        static int SearchWordInFile(string filePath, string searchWord)
        {
            if (File.Exists(filePath))
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    int lineNumber = 1;

                    while ((line = sr.ReadLine()) != null)
                    {
                        int index = line.IndexOf(searchWord);
                        if (index != -1)
                        {
                            return index;
                        }

                        lineNumber++;
                    }
                }
            }
            else
            {
                Console.WriteLine("Файл не найден.");
            }

            return -1;
        }

        static int GetSubstringRK(string text, string pattern)
        {
            const int alphabetSize = 256;
            const int mod = 9973;

            int patternHash = pattern[0] % mod;
            int textHash = text[0] % mod;

            int firstIndexHash = 1;

            for (int i = 1; i < pattern.Length; i++)
            {
                patternHash = (patternHash * alphabetSize + pattern[i]) % mod;
                textHash = (textHash * alphabetSize + text[i]) % mod;
                firstIndexHash = (firstIndexHash * alphabetSize) % mod;
            }

            for (int i = 0; i <= text.Length - pattern.Length; i++)
            {
                if (patternHash == textHash && CompareText(text, i, pattern))
                {
                    return i;  
                }

                if (i == text.Length - pattern.Length) break;

                textHash = (textHash - (text[i] * firstIndexHash) % mod + mod) % mod;
                textHash = (textHash * alphabetSize + text[i + pattern.Length]) % mod;
            }

            return -1; 
        }

        static bool CompareText(string text, int index, string pattern)
        {
            for (int i = 0; i < pattern.Length; i++)
            {
                if (pattern[i] != text[index + i])
                {
                    return false;
                }
            }

            return true;
        }


        static void Main()
        {
            Console.WriteLine("Введите слово для поиска:");
            string searchWord = Console.ReadLine();
            string filePath = "Input.txt"; // Замените на путь к вашему файлу

            int index0 = SearchWordInFile(filePath, searchWord);
            if (index0 != -1) Console.WriteLine($"Первое вхождение слова ({searchWord}) найдено на позиции {index0}");
            else Console.WriteLine($"Слово ({searchWord}) не найдено в тексте");
            Console.WriteLine();

            string text = File.ReadAllText("Input.txt");
            int index1 = Find.FindSubstring(searchWord, text);
            if (index1 != -1) Console.WriteLine($"Первое вхождение слова ({searchWord}) найдено на позиции {index1}");
            else Console.WriteLine($"Слово ({searchWord}) не найдено в тексте");
            Console.WriteLine();

            int index2 = Find.Boyer(text, searchWord);
            if (index2 != -1) Console.WriteLine($"Первое вхождение слова ({searchWord}) найдено на позиции {index2}");
            else Console.WriteLine($"Слово ({searchWord}) не найдено в тексте");            
            Console.WriteLine();

            int index3 = GetSubstringRK(text, searchWord);
            if (index3 != -1) Console.WriteLine($"Первое вхождение слова ({searchWord}) найдено на позиции {index3}");
            else Console.WriteLine($"Слово ({searchWord}) не найдено в тексте");
        }
    }
}
