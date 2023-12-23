using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pract1
{
    internal class Program
    {

        public void sort(int[] arr)
        {
            int n = arr.Length; 
            
            for (int i = n / 2 - 1; i >= 0; i--) 
                heapify(arr, n, i);

            for (int i = n - 1; i >= 0; i--)
            {
                (arr[i], arr[0]) = (arr[0], arr[i]);
                heapify(arr, i, 0);
            }
        }

        void heapify(int[] arr, int n, int i)
        {
            int largest = i;

            if (2 * i + 1 < n && arr[2 * i + 1] > arr[largest])
                largest = 2 * i + 1;

            if (2 * i + 2 < n && arr[2 * i + 2] > arr[largest])
                largest = 2 * i + 2;

            if (largest != i)
            {
                (arr[largest], arr[i]) = (arr[i], arr[largest]);
                heapify(arr, n, largest);
            }
        }

        static void printArray(int[] arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n; ++i)
                Console.Write(arr[i] + " ");
            Console.Read();
        }


        public static void Main()
        {
            int[] arr = new int[300];
            Random random = new Random();

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = random.Next(1, 250); 
            }

            Program ob = new Program();
            ob.sort(arr);

            Array.Reverse(arr);

            Console.WriteLine("Отсортированный массив по убыванию:");
            printArray(arr);
        }
    }
}
