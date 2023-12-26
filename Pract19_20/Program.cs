using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pract19
{
    internal class Program
    {
        static int V;

        static int MinDistance(int[] distance, bool[] visited)
        {
            int min = int.MaxValue;
            int minIndex = -1;

            for (int v = 0; v < V; v++)
            {
                if (!visited[v] && distance[v] <= min)
                {
                    min = distance[v];
                    minIndex = v;
                }
            }

            return minIndex;
        }

        static void Dijkstra(int[,] graph, int st)
        {
            int[] distance = new int[V];
            bool[] visited = new bool[V];

            for (int i = 0; i < V; i++)
            {
                distance[i] = int.MaxValue;
                visited[i] = false;
            }

            distance[st] = 0;

            for (int count = 0; count < V - 1; count++)
            {
                int u = MinDistance(distance, visited);
                visited[u] = true;

                for (int v = 0; v < V; v++)
                {
                    if (!visited[v] && graph[u, v] != 0 && distance[u] != int.MaxValue && distance[u] + graph[u, v] < distance[v])
                    {
                        distance[v] = distance[u] + graph[u, v];
                    }
                }
            }

            PrintSolution(distance);
        }

        static void PrintSolution(int[] distance)
        {
            Console.WriteLine("Расстояния от начальной вершины:");

            for (int i = 0; i < V; i++)
            {
                Console.WriteLine($"До вершины {i+1} : {distance[i]}");
            }
        }


        static int[,] ReadMatrixFromFile(string filename)
        {
            int[,] matrix;

            using (StreamReader sr = new StreamReader(filename))
            {
                int size = int.Parse(sr.ReadLine());
                V = size;
                matrix = new int[size, size];

                for (int i = 0; i < size; i++)
                {
                    string line = sr.ReadLine();
                    string[] values = line.Split(' ');

                    for (int j = 0; j < size; j++)
                    {
                        matrix[i, j] = int.Parse(values[j]);
                    }
                }
            }

            return matrix;
        }




        static void RunFloydWarshall(int[,] graph)
        {
            int verticesCount = graph.GetLength(0);
            int[,] distance = new int[verticesCount, verticesCount];

            // Инициализация матрицы расстояний
            for (int i = 0; i < verticesCount; i++)
            {
                for (int j = 0; j < verticesCount; j++)
                {
                    distance[i, j] = graph[i, j];
                    if (distance[i, j] == 0 && i != j)
                    {
                        distance[i, j] = int.MaxValue;
                    }
                }
            }

            // Алгоритм Флойда-Уоршелла для обновления расстояний
            for (int k = 0; k < verticesCount; k++)
            {
                for (int i = 0; i < verticesCount; i++)
                {
                    for (int j = 0; j < verticesCount; j++)
                    {
                        if (distance[i, k] != int.MaxValue && distance[k, j] != int.MaxValue &&
                            distance[i, k] + distance[k, j] < distance[i, j])
                        {
                            distance[i, j] = distance[i, k] + distance[k, j];
                        }
                    }
                }
            }

            // Вывод расстояний
            Console.WriteLine("Матрица кратчайших расстояний:");
            for (int i = 0; i < verticesCount; ++i)
            {
                for (int j = 0; j < verticesCount; ++j)
                {
                    if (distance[i, j] == int.MaxValue)
                    {
                        Console.Write("INF   ");
                    }
                    else
                    {
                        Console.Write(distance[i, j] + "\t");
                    }
                }
                Console.WriteLine();
            }
        }



        static void Main()
        {
            int[,] adjacencyMatrix = ReadMatrixFromFile("Input.txt");
            // Проверка, что матрица успешно прочитана
            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < adjacencyMatrix.GetLength(1); j++)
                {
                    Console.Write(adjacencyMatrix[i, j] + "\t");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Алгоритм Дейкстры");
            Dijkstra(adjacencyMatrix, 0);
            Console.WriteLine();

            Console.WriteLine("Алгоритм Флойда-Уолшера");
            RunFloydWarshall(adjacencyMatrix);
        }
    }
}
