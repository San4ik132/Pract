using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pract15
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GraphCreationClass Graph = new GraphCreationClass();

            Graph.ReadAndFillIn("Input.txt");
            Console.WriteLine("Список смежности");
            Graph.PrintGraph();
            Console.WriteLine();

            Console.WriteLine("Матрица инциндентности");
            PrintMatrix(Graph.BuildIncidenceMatrix());
            Console.WriteLine();

            Console.WriteLine("Матрица смежности");
            PrintMatrix(Graph.BuildAdjacencyMatrix());
            Console.WriteLine();

            Console.WriteLine("Перечень ребер (дуг) или списки ребер (дуг).");
            PrintMatrix(Graph.GetEdgeArray());
            Console.WriteLine();

            Console.WriteLine("Алгоритм BFS");
            var list = Graph.ShortestPathBFS(1, 5, Graph.BuildAdjacencyMatrix());
            foreach (var item in list) Console.Write($"{item} ");
            Console.WriteLine();

            Console.WriteLine("Алгоритм DFS");
            Graph.Matrix(Graph.BuildAdjacencyMatrix());
            var list1 = Graph.FindShortestPath(1, 5);
            foreach (var item in list1) Console.Write($"{item} ");
            Console.WriteLine();



            int[,] adjacencyMatrix;

            using (StreamReader sr = new StreamReader("1.txt"))
            {
                string[] lines = sr.ReadToEnd().Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                int n = lines.Length; // Размер матрицы (n x n)
                adjacencyMatrix = new int[n, n];

                for (int i = 0; i < n; i++)
                {
                    string[] values = lines[i].Split(' ');
                    for (int j = 0; j < n; j++)
                    {
                        adjacencyMatrix[i, j] = int.Parse(values[j].Trim());
                    }
                }

            }



            List<int> result = TopologicalSort.TopologicalSortUsingDFS(adjacencyMatrix);

            if (TopologicalSort.HasCycle(adjacencyMatrix))
            {
                Console.WriteLine("Граф содержит циклы.");
            }
            else
            {
                TopologicalSort.PrintTopologicalOrder(result);
                TopologicalSort.PrintAdjacencyMatrix(adjacencyMatrix, result);
            }

        }

        static void PrintMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(matrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
        }
    }


    class TopologicalSort
    {
        private static void DFS(int[,] graph, int vertex, bool[] visited, Stack<int> stack)
        {
            visited[vertex] = true;

            for (int i = 0; i < graph.GetLength(0); i++)
            {
                if (graph[vertex, i] == 1 && !visited[i])
                {
                    DFS(graph, i, visited, stack);
                }
            }

            stack.Push(vertex);
        }

        public static List<int> TopologicalSortUsingDFS(int[,] graph)
        {
            int numVertices = graph.GetLength(0);
            bool[] visited = new bool[numVertices];
            Stack<int> stack = new Stack<int>();

            for (int i = 0; i < numVertices; i++)
            {
                if (!visited[i])
                {
                    DFS(graph, i, visited, stack);
                }
            }

            return new List<int>(stack);
        }


        public static void PrintTopologicalOrder(List<int> result)
        {
            Console.WriteLine("Топологически отсортированный порядок:");
            foreach (int vertex in result)
            {
                Console.Write(vertex + " ");
            }
        }

        public static bool HasCycle(int[,] graph)
        {
            int numVertices = graph.GetLength(0);
            bool[] visited = new bool[numVertices];
            bool[] recursionStack = new bool[numVertices];

            for (int i = 0; i < numVertices; i++)
            {
                if (HasCycleUtil(graph, i, visited, recursionStack))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasCycleUtil(int[,] graph, int vertex, bool[] visited, bool[] recursionStack)
        {
            if (!visited[vertex])
            {
                visited[vertex] = true;
                recursionStack[vertex] = true;

                for (int i = 0; i < graph.GetLength(0); i++)
                {
                    if (graph[vertex, i] == 1)
                    {
                        if (!visited[i] && HasCycleUtil(graph, i, visited, recursionStack))
                        {
                            return true;
                        }
                        else if (recursionStack[i])
                        {
                            return true;
                        }
                    }
                }
            }

            recursionStack[vertex] = false;
            return false;
        }


        public static void PrintAdjacencyMatrix(int[,] graph, List<int> result)
        {
            Console.WriteLine("\nОтсортированная список смежности:");
            foreach (int vertex in result)
            {
                Console.Write(vertex + ": ");
                for (int i = 0; i < graph.GetLength(1); i++)
                {
                    if (graph[vertex, i] == 1)
                    {
                        Console.Write(i + " ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
