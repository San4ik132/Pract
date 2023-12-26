using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pract15
{
    internal class GraphCreationClass
    {
        public Dictionary<string, List<(string, int)>> MyGraph;

        public int edgeCount;
        public int vertexCount;


        // Конструктор по умолчанию, создающий пустой граф
        public GraphCreationClass()
        {

            MyGraph = new Dictionary<string, List<(string, int)>>();
        }

        // Конструктор для создания графа из файла.
        public GraphCreationClass(string fileName)
        {
            ReadAndFillIn(fileName);
        }

        // Конструктор копирования
        public GraphCreationClass(GraphCreationClass other)
        {
            MyGraph = new Dictionary<string, List<(string, int)>>(other.MyGraph);
        }


        public void ReadAndFillIn(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            // Проверка, что в файле есть данные для обработки
            if (lines.Length > 0)
            {
                string[] info = lines[0].Split(' ');
                vertexCount = int.Parse(info[0]);
                edgeCount = int.Parse(info[1]);

                // Обработка оставшихся строк для заполнения графа
                for (int i = 1; i <= vertexCount; i++)
                {
                    string[] items = lines[i].Split(' '); // Разделение данных о вершинах и ребрах
                    string node = items[0];

                    List<(string, int)> adjacentNodes = new List<(string, int)>();
                    for (int j = 1; j < items.Length; j += 2)
                    {
                        string adjacentNode = items[j];
                        int weight = int.Parse(items[j + 1]);
                        adjacentNodes.Add((adjacentNode, weight));
                    }

                    MyGraph[node] = adjacentNodes;
                }
            }
        }

        public int[,] BuildIncidenceMatrix()
        {
            var uniqueEdges = new HashSet<string>();
            var edgeIndexes = new Dictionary<string, int>();

            int numVertices = vertexCount;
            int numEdges = edgeCount;

            int[,] incidenceMatrix = new int[numVertices, numEdges];

            int edgeIndex = 0;
            foreach (var vertex in MyGraph)
            {
                string startVertex = vertex.Key;
                var edges = vertex.Value;

                foreach (var edge in edges)
                {
                    string endVertex = edge.Item1;
                    int weight = edge.Item2;

                    string edgeKey = $"{startVertex}-{endVertex}";

                    if (!uniqueEdges.Contains(edgeKey) && !uniqueEdges.Contains($"{endVertex}-{startVertex}"))
                    {
                        uniqueEdges.Add(edgeKey);
                        uniqueEdges.Add($"{endVertex}-{startVertex}");

                        edgeIndexes.Add(edgeKey, edgeIndex);

                        incidenceMatrix[int.Parse(startVertex) - 1, edgeIndex] = weight;
                        incidenceMatrix[int.Parse(endVertex) - 1, edgeIndex] = weight;

                        edgeIndex++;
                    }
                    else
                    {
                        int columnIndex;
                        if (edgeIndexes.TryGetValue(edgeKey, out columnIndex) ||
                            edgeIndexes.TryGetValue($"{endVertex}-{startVertex}", out columnIndex))
                        {
                            incidenceMatrix[int.Parse(startVertex) - 1, columnIndex] = weight;
                            incidenceMatrix[int.Parse(endVertex) - 1, columnIndex] = weight;
                        }
                    }
                }
            }

            return incidenceMatrix;
        }


        public int[,] BuildAdjacencyMatrix()
        {
            int numVertices = MyGraph.Count;
            int[,] adjacencyMatrix = new int[numVertices, numVertices];

            // Заполним матрицу смежности на основе переданного графа
            foreach (var startPoint in MyGraph.Keys)
            {
                var edges = MyGraph[startPoint];
                foreach (var endPointTuple in edges)
                {
                    string endPoint = endPointTuple.Item1;
                    int weight = endPointTuple.Item2;
                    int endPointIndex = int.Parse(endPoint) - 1;

                    adjacencyMatrix[int.Parse(startPoint) - 1, endPointIndex] = weight;
                }
            }

            return adjacencyMatrix;
        }

        public int[,] GetEdgeArray()
        {
            int[,] edgeArray = new int[3, edgeCount];
            HashSet<string> addedEdges = new HashSet<string>(); // Хэш-множество для отслеживания добавленных рёбер

            int column = 0;
            foreach (var vertexEdges in MyGraph)
            {
                foreach (var edge in vertexEdges.Value)
                {
                    string edgeRepresentation = $"{vertexEdges.Key}-{edge.Item1}";
                    string reverseEdgeRepresentation = $"{edge.Item1}-{vertexEdges.Key}";

                    if (!addedEdges.Contains(reverseEdgeRepresentation))
                    {
                        // Добавляем ребро в массив и отмечаем его как добавленное
                        edgeArray[0, column] = int.Parse(vertexEdges.Key); // Начальная вершина
                        edgeArray[1, column] = int.Parse(edge.Item1); // Конечная вершина
                        edgeArray[2, column] = edge.Item2; // Вес дуги
                        addedEdges.Add(edgeRepresentation);
                        column++;
                    }
                }
            }

            return edgeArray;
        }


        public List<int> ShortestPathBFS(int start, int end, int[,] adjacencyMatrix)
        {
            var queue = new Queue<int>();
            var visited = new bool[adjacencyMatrix.GetLength(0)];
            var previous = new int[adjacencyMatrix.GetLength(0)];

            queue.Enqueue(start - 1);
            visited[start - 1] = true;
            previous[start - 1] = -1;

            while (queue.Count > 0)
            {
                int current = queue.Dequeue();

                if (current == end - 1)
                {
                    // Построение пути начиная с конечной точки
                    List<int> path = new List<int>();
                    int at = end - 1;
                    while (at != -1)
                    {
                        path.Insert(0, at + 1);
                        at = previous[at];
                    }
                    return path;
                }

                for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
                {
                    if (adjacencyMatrix[current, i] > 0 && !visited[i])
                    {
                        queue.Enqueue(i);
                        visited[i] = true;
                        previous[i] = current;
                    }
                }
            }

            return new List<int>(); // Если путь не найден
        }

        private int[,] adjacencyMatrix;
        private List<int> shortestPath;


        public void Matrix(int[,] matrix)
        {
            adjacencyMatrix = matrix;
        }

        public List<int> FindShortestPath(int start, int end)
        {
            start--; 
            end--;  

            shortestPath = new List<int>();
            bool[] visited = new bool[adjacencyMatrix.GetLength(0)];

            DFS(start, end, visited, new List<int>());

            for (int i = 0; i < shortestPath.Count; i++)
            {
                shortestPath[i]++;
            }

            return shortestPath;
        }

        private void DFS(int current, int end, bool[] visited, List<int> currentPath)
        {
            visited[current] = true;
            currentPath.Add(current); // Увеличиваем номер вершины на 1

            if (current == end && (shortestPath.Count == 0 || currentPath.Count < shortestPath.Count))
            {
                shortestPath = new List<int>(currentPath);
            }

            for (int i = 0; i < adjacencyMatrix.GetLength(0); i++)
            {
                if (adjacencyMatrix[current, i] > 0 && !visited[i])
                {
                    DFS(i, end, visited, currentPath);
                }
            }

            visited[current] = false;
            currentPath.Remove(current); 
        }


       



        public void PrintGraph()
        {
            foreach (var node in MyGraph)
            {
                Console.Write(node.Key + ": ");
                foreach (var edge in node.Value)
                {
                    Console.Write($"({edge.Item1}, {edge.Item2}) ");
                }
                Console.WriteLine();
            }
        }

    }

   
   
}
