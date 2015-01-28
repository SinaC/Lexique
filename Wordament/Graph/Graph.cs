using System;
using System.Collections.Generic;
using System.Linq;

namespace Wordament.Graph
{
    public class Graph
    {
        private static readonly Tuple<int, int>[] Adjacents =
            {
                new Tuple<int, int>(-1, -1),
                new Tuple<int, int>(-1, 0),
                new Tuple<int, int>(-1, 1),

                new Tuple<int, int>(0, -1),
                //new Tuple<int, int>(0, 0),
                //new Tuple<int, int>(0, 1),

                //new Tuple<int, int>(1, -1),
                //new Tuple<int, int>(1, 0),
                //new Tuple<int, int>(1, 1)
            };

        public class Vertex
        {
            public enum Types
            {
                Normal,
                Or,
                Begin,
                End
            }

            public List<int> Edges { get; set; }
            public string Content { get; set; }

            public Vertex()
            {
                Edges = new List<int>();
            }
        }

        public List<Vertex> Vertices { get; set; }

        //public Graph(string[,] table)
        //{
        //    Vertices = new List<Vertex>();
        //    int[,] indices = new int[table.GetLength(0), table.GetLength(1)];
        //    for(int x = 0; x < table.GetLength(0); x++)
        //        for(int y = 0; y < table.GetLength(1); y++)
        //        {
        //            // Vertice
        //            string content = table[x, y];
        //            if (String.IsNullOrWhiteSpace(content))
        //                continue;
        //            indices[x, y] = Vertices.Count;
        //            Vertices.Add(new Vertex
        //                {
        //                    Content = content
        //                });

        //            // Edges
        //            foreach (Tuple<int, int> adjacent in Adjacents)
        //            {
        //                int adjacentX = x + adjacent.Item1;
        //                int adjacentY = y + adjacent.Item2;
        //                if (adjacentX >= 0 && adjacentY >= 0 && adjacentX < table.GetLength(0) && adjacentY < table.GetLength(1))
        //                {
        //                    int index = indices[adjacentX, adjacentY];
        //                    Vertices[index].Edges.Add(Vertices.Count - 1);
        //                    Vertices[Vertices.Count - 1].Edges.Add(index);
        //                }
        //            }
        //        }
        //}

        public Graph(string[,] table)
        {
            Vertices = new List<Vertex>();
            List<int>[,] indices = new List<int>[table.GetLength(0), table.GetLength(1)];
            for (int x = 0; x < table.GetLength(0); x++)
            {
                for (int y = 0; y < table.GetLength(1); y++)
                {
                    string content = table[x, y];
                    if (String.IsNullOrWhiteSpace(content))
                        continue;
                    //
                    indices[x, y] = new List<int>();
                    // Handle double cell such as Q/Z
                    bool isDoubleCell = false;
                    if (content.Length == 3 && (content[1] == '/' || content[1] == '|'))
                    {
                        indices[x,y].Add(Vertices.Count);
                        Vertices.Add(new Vertex
                            {
                                Content = content.Substring(2) // Extract 2nd letter
                            });
                        isDoubleCell = true;
                        content = content.Substring(0, 1); // Keep 1st letter
                    }
                    //
                    indices[x, y].Add(Vertices.Count);
                    Vertices.Add(new Vertex
                        {
                            Content = content
                        });
                    //
                    foreach (Tuple<int, int> adjacent in Adjacents)
                    {
                        int adjacentX = x + adjacent.Item1;
                        int adjacentY = y + adjacent.Item2;
                        if (adjacentX >= 0 && adjacentY >= 0 && adjacentX < table.GetLength(0) && adjacentY < table.GetLength(1))
                            foreach (int index in indices[adjacentX, adjacentY])
                            {
                                if (isDoubleCell)
                                {
                                    Vertices[index].Edges.Add(Vertices.Count - 2);
                                    Vertices[Vertices.Count - 2].Edges.Add(index);
                                }
                                Vertices[index].Edges.Add(Vertices.Count - 1);
                                Vertices[Vertices.Count - 1].Edges.Add(index);
                            }
                    }
                }
            }
        }
    }
}
