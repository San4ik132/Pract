using System;
using System.Linq;

namespace Pract8
{
    class MaxQuerySolver
    {
        int[] array;
        public int blockSize;
        public int[] maxInBlock;

        public MaxQuerySolver(int[] inputArray)
        {
            array = inputArray;
            int n = array.Length;
            blockSize = (int)Math.Ceiling(Math.Sqrt(n));
            maxInBlock = new int[blockSize];

            for (int i = 0; i < n; i++)
            {
                int blockIndex = i / blockSize;
                if (i == 0 || array[i] > maxInBlock[blockIndex])
                {
                    maxInBlock[blockIndex] = array[i];
                }
            }
        }

        public void UpdateMaxInBlock(int blockIndex)
        {
            int start = blockIndex * blockSize;
            int end = Math.Min(start + blockSize, array.Length);

            int maxInCurrentBlock = int.MinValue;
            for (int i = start; i < end; i++)
            {
                maxInCurrentBlock = Math.Max(maxInCurrentBlock, array[i]);
            }

            maxInBlock[blockIndex] = maxInCurrentBlock;
        }

        public void UpdateValueAtIndex(int index, int newValue)
        {
            array[index] = newValue;
            int blockIndex = index / blockSize;
            UpdateMaxInBlock(blockIndex);
        }


        public int GetMaxInRangeIndex(int left, int right)
        {
            int max = int.MinValue;

            while (left <= right && left % blockSize != 0 && left != 0)
            {
                max = Math.Max(max, array[left]);
                left++;
            }

            while (left + blockSize <= right)
            {
                max = Math.Max(max, maxInBlock[left / blockSize]);
                left += blockSize;
            }

            while (left <= right)
            {
                max = Math.Max(max, array[left]);
                left++;
            }
            return max;
        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {

            int[] ints = new int[300];

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

            //Array.Sort(ints);
            Console.WriteLine("Отсортированный массив: ");
            int index = 0;
            Array.ForEach(ints, x =>
            {
                Console.WriteLine("[{0}]: {1}", index++, x);
            });


            MaxQuerySolver solver = new MaxQuerySolver(ints);
            var indexMass = solver.GetMaxInRangeIndex(0, ints.Length - 1);
            Console.WriteLine(solver.blockSize);
            Console.WriteLine($"Максимальное значение: {Array.IndexOf(ints, indexMass)}, рассположено на позиции: {indexMass}");
        }
    }
    
}
