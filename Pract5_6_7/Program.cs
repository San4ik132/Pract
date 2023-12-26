using System;
using System.Linq;

namespace Pract567
{
    internal class Program
    {

        private static void Main()
        {
            int[] ints = new int[51];

            Random rnd = new Random();

            for (var j = 0; j < ints.Length; j++)
            {
                int newValue;
                do
                {
                    newValue = rnd.Next(0, 1000);
                } while (ints.Contains(newValue));

                ints[j] = newValue; 
            }

            Array.Sort(ints);
            Console.WriteLine("Отсортированный массив: ");
            int index = 0;
            Array.ForEach(ints, x =>
            {
                Console.WriteLine("[{0}]: {1}", index++, x);
            });





            Console.Write("\nНайти: ");
            int key = int.Parse(Console.ReadLine());
            int i = BinarySearch(ints, key, 0, ints.Length - 1); 
            if (i < 0 || i > ints.Length )
                Console.WriteLine("Элемент не найден");
            else
                Console.WriteLine("Индекс искомого элемента Бинарный поиск: {0}", i);

            int u = FibonacciSearch(ints, ints.Length, key);
            if (u < 0 || u > ints.Length)
                Console.WriteLine("Элемент не найден");
            else
                Console.WriteLine("Индекс искомого элемента Фибоначчиев поиск: {0}", u);

            int p = InterpolatingSearch(ints, ints.Length, key);
            if (p < 0 || p > ints.Length)
                Console.WriteLine("Элемент не найден");
            else
                Console.WriteLine("Индекс искомого элемента Интерполяционный поиск: {0}", p);
            Console.ReadKey(true);





        }
        static int BinarySearch(int[] d, int key, int left, int right) //бинарный поиск
        {
            if (left <= right)
            {
                int mid = left + (right - left) / 2;

                if (d[mid] == key)               
                    return mid;                
                else if (d[mid] > key)                
                    return BinarySearch(d, key, left, mid - 1);                
                else                
                    return BinarySearch(d, key, mid + 1, right);                
            }
            return -(left + 1); 
        }


        static int FibonacciSearch(int[] arr, int n, int x)
        {
            int fibMMm2 = 0; // (m-2)-ое число Фибоначчи
            int fibMMm1 = 1; // (m-1)-ое число Фибоначчи
            int fibM = fibMMm2 + fibMMm1; // m-ое число Фибоначчи

            while (fibM < n)
            {
                fibMMm2 = fibMMm1;
                fibMMm1 = fibM;
                fibM = fibMMm2 + fibMMm1;
            }

            int offset = -1;

            while (fibM > 1)
            {
                int i = Math.Min(offset + fibMMm2, n - 1);

                if (arr[i] < x)
                {
                    fibM = fibMMm1;
                    fibMMm1 = fibMMm2;
                    fibMMm2 = fibM - fibMMm1;
                    offset = i;
                }
                else if (arr[i] > x)
                {
                    fibM = fibMMm2;
                    fibMMm1 = fibMMm1 - fibMMm2;
                    fibMMm2 = fibM - fibMMm1;
                }
                else
                {
                    return i;
                }
            }

            if (fibMMm1 > 0 && offset + 1 < n && arr[offset + 1] == x)
            {
                return offset + 1;
            }

            return -1;
        }


        public static int InterpolatingSearch(int[] a, int n, int key)
        {
            int lo = 0;
            int hi = n - 1;
            int mid = -1;
            int index = -1;
            while (lo <= hi)
            {
                mid = lo + (int)(((double)(hi - lo) / (a[hi] - a[lo])) * (key - a[lo]));
                if (a[mid] == key)
                {
                    index = mid;
                    break;
                }
                else
                {
                    if (a[mid] < key)
                    {
                        lo = mid + 1;
                    }
                    else
                    {
                        hi = mid - 1;
                    }
                }
            }
            return index;
        }
    }
}

    

