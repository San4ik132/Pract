using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pract21_22
{
    internal class Program
    {

        class KruskalAlgorithm
        {
            private struct Edge
            {
                public int n1, n2; // Вершины
                public int w; // Вес ребра
            }

            private Edge[] edges;
            private int[] nodes;
            private int last_n;
            private int[,] adjacencyMatrix;

            public KruskalAlgorithm(int NV, int NE, int[,] matrix)
            {
                edges = new Edge[NE];
                nodes = new int[NV];
                adjacencyMatrix = matrix;

                for (int i = 0; i < NV; i++)
                {
                    nodes[i] = -1; // Инициализация цветов вершин
                }

                InitializeEdgesFromMatrix();

                // Вызываем метод сортировки после инициализации ребер
                SortEdgesByWeight();
            }

            private void InitializeEdgesFromMatrix()
            {
                int edgeCount = 0;
                for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
                {
                    for (int j = i; j < adjacencyMatrix.GetLength(1); j++)
                    {
                        if (adjacencyMatrix[i, j] != 0)
                        {
                            edges[edgeCount++] = new Edge { n1 = i, n2 = j, w = adjacencyMatrix[i, j] };
                        }
                    }
                }
            }

            private void SortEdgesByWeight()
            {
                Array.Sort(edges, (x, y) => x.w.CompareTo(y.w));
            }

            private int GetColor(int n)
            {
                if (nodes[n] < 0)
                {
                    last_n = n;
                    return n;
                }
                int color = GetColor(nodes[n]);
                nodes[n] = last_n;
                return color;
            }

            public void ExecuteKruskal()
            {
                for (int i = 0; i < edges.Length; i++)
                {
                    int c2 = GetColor(edges[i].n2);
                    if (GetColor(edges[i].n1) != c2)
                    {
                        nodes[last_n] = edges[i].n2;
                        Console.WriteLine($"{edges[i].n1} {edges[i].n2} {edges[i].w}");
                    }
                }
            }

            public void PrintSortedEdges()
            {
                Console.WriteLine("Sorted Edges:");
                foreach (var edge in edges)
                {
                    Console.WriteLine($"{edge.n1} {edge.n2} {edge.w}");
                }
            }
        }














        static int[,] ReadMatrixFromFile(string filename, int rows, int columns)
        {
            int[,] matrix = new int[rows, columns];

            if (!File.Exists(filename))
            {
                Console.WriteLine("Файл не найден: " + filename);
                Environment.Exit(1);
            }

            string[] lines = File.ReadAllLines(filename);
            if (lines.Length != rows)
            {
                Console.WriteLine("Неправильное количество строк в файле.");
                Environment.Exit(1);
            }

            for (int i = 0; i < rows; i++)
            {
                string[] values = lines[i].Split(' ');

                if (values.Length != columns)
                {
                    Console.WriteLine("Ошибка чтения значения в строке " + i);
                    Environment.Exit(1);
                }

                for (int j = 0; j < columns; j++)
                {
                    int value;
                    bool parseResult = int.TryParse(values[j], out value);

                    switch (parseResult)
                    {
                        case true:
                            matrix[i, j] = value;
                            break;
                        case false:
                            Console.WriteLine("Ошибка парсинга целочисленного значения в файле.");
                            Environment.Exit(1);
                            break;
                    }
                }
            }

            return matrix;
        }
        static void Main()
        {
            int[,] graph = ReadMatrixFromFile("Input.txt", 6, 6);

            // Выводим матрицу в консоль для проверки
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    Console.Write(graph[i, j] + " ");
                }
                Console.WriteLine();
            }

            int[,] matrix = new int[,]
      {
            { 0, 4, 0, 0, 0, 0 },
            { 4, 0, 8, 0, 0, 0 },
            { 0, 8, 0, 7, 0, 4 },
            { 0, 0, 7, 0, 9, 14 },
            { 0, 0, 0, 9, 0, 10 },
            { 0, 0, 4, 14, 10, 0 }
      };

            KruskalAlgorithm kruskal = new KruskalAlgorithm(6, 9, matrix);
            kruskal.ExecuteKruskal();


            //KruskalAlgorithm kruskal = new KruskalAlgorithm(graph);
            //var result = kruskal.GetMinimumSpanningTree();
            //List<Edge> minimumSpanningTree = result.minimumSpanningTree;
            //int totalWeight = result.totalWeight;

            //Console.WriteLine("Алгорит Краскала");
            //Console.WriteLine("Минимальный каркас графа:");
            //foreach (var edge in minimumSpanningTree)
            //{
            //    Console.WriteLine($"{edge.Source + 1} - {edge.Destination + 1} : {edge.Weight}");
            //}
            //Console.WriteLine($"Общий вес: {totalWeight}");



            //PrimsAlgorithm prims = new PrimsAlgorithm();
            //List<Edge2> minimumSpanningTree1 = prims.FindMinimumSpanningTree1(graph);
            //int totalWeight1 = 0;
            //foreach (var edge in minimumSpanningTree)
            //{
            //    totalWeight1 += edge.Weight;
            //}
            //Console.WriteLine("Алгорит Прима");
            //Console.WriteLine("Минимальный каркас графа:");
            //foreach (var edge in minimumSpanningTree1)
            //{
            //    Console.WriteLine($"{edge.Source+1} - {edge.Destination+1} : {edge.Weight}");
            //}
            //Console.WriteLine($"Общий вес: {totalWeight1}");
        }





        public class PrimsAlgorithm
        {
            public List<Edge2> FindMinimumSpanningTree1(int[,] graph)
            {
                int verticesCount = graph.GetLength(0);
                bool[] inTree = new bool[verticesCount];
                int[] distance = new int[verticesCount];
                int[] parent = new int[verticesCount];
                List<Edge2> minimumSpanningTree = new List<Edge2>();

                for (int i = 0; i < verticesCount; i++)
                {
                    distance[i] = int.MaxValue;
                }

                distance[0] = 0;
                parent[0] = -1;

                for (int i = 0; i < verticesCount - 1; i++)
                {
                    int u = MinimumDistanceVertex(distance, inTree);
                    inTree[u] = true;

                    for (int v = 0; v < verticesCount; v++)
                    {
                        if (graph[u, v] != 0 && !inTree[v] && graph[u, v] < distance[v])
                        {
                            parent[v] = u;
                            distance[v] = graph[u, v];
                        }
                    }
                }

                for (int i = 1; i < verticesCount; i++)
                {
                    minimumSpanningTree.Add(new Edge2 { Source = parent[i], Destination = i, Weight = graph[i, parent[i]] });
                }

                return minimumSpanningTree;
            }

            private int MinimumDistanceVertex(int[] distance, bool[] inTree)
            {
                int min = int.MaxValue;
                int minIndex = -1;

                for (int v = 0; v < distance.Length; v++)
                {
                    if (!inTree[v] && distance[v] < min)
                    {
                        min = distance[v];
                        minIndex = v;
                    }
                }

                return minIndex;
            }
        }

        public class Edge2
        {
            public int Source { get; set; }
            public int Destination { get; set; }
            public int Weight { get; set; }
        }
    }
}
