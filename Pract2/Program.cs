using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pract2
{
    internal class Program
    {
        static void BucketSort(int[] sarray, int array_size)
        {
             int max = array_size;
            int[][] bucket = new int[10][];
            for (int i = 0; i < 10; i++)
            {
                bucket[i] = new int[max + 1];
            }

            // инициализация массива bucket 
            for (int x = 0; x < 10; x++)
            {
                bucket[x][max] = 0;
            }

            // основной цикл для каждого разряда
            for (int digit = 1; digit <= 1000000000; digit *= 10)
            {
                // запись в массив bucket
                for (int i = 0; i < max; i++)
                {
                    // получение цифр 0-9
                    int dig = (sarray[i] / digit) % 10;
                    // добавить число в массив bucket и увеличить счетчик
                    bucket[dig][bucket[dig][max]++] = sarray[i];
                }
                // запись карманов в массив
                int idx = 0;
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < bucket[x][max]; y++)
                    {
                        sarray[idx++] = bucket[x][y];
                    }
                    // обнуление массива bucket 
                    bucket[x][max] = 0;
                }
            }

            // высвобождение памяти
            for (int i = 0; i < 10; i++)
            {
                Array.Resize(ref bucket[i], 0);
            }
        }

        static void Main()
        {
            int[] arr = new int [50];
            Random random = new Random();

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = random.Next(0, 499999);
            }


            BucketSort(arr, arr.Length);

            Console.WriteLine("Отсартированный массив:");
            foreach (var item in arr)
            {
                Console.Write(item + " ");
            }
        }
    }
}
