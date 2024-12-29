using System.Collections.Generic;


namespace GridGenerator
{
    public class Grid
    {
        public static int radius;
        public static float cellSize ;
        public readonly List<Vertex_hex> hexes = new List<Vertex_hex>();
        public readonly List<Triangle> triangles= new List<Triangle>();
        public readonly List<Vertex_mid> mids= new List<Vertex_mid>();
        public readonly List<Vertex_center> centers= new List<Vertex_center>();
        public readonly List<Vertex> vertices = new List<Vertex>();
        public readonly List<Edge> edges= new List<Edge>();
        public readonly List<Quad> quads= new List<Quad>();
        public readonly List<SubQuad> subQuads= new List<SubQuad>();

        public Grid(int radius, float cellSize, int relaxTimes)
        {
            Grid.radius = radius;
            Grid.cellSize = cellSize;
            Vertex_hex.Hex(hexes, radius);
            Triangle.Triangles_Hex(hexes, mids, centers, edges, triangles);
            while (Triangle.HasNeighborTriangles(triangles))
            {
                Triangle.RandomlyMergeTriangles(mids, centers, edges, triangles, quads);
            }
            vertices.AddRange(hexes);
            vertices.AddRange(mids);
            vertices.AddRange(centers);
            foreach (Triangle triangle in triangles)
            {
                triangle.Subdivide(subQuads);
            }

            foreach (Quad quad in quads)
            {
                quad.Subdivide(subQuads);
            }
            
            // for (int i = 0; i < relaxTimes; i++)
            // {
            //     foreach(SubQuad subQuad in subQuads)
            //     {
            //         subQuad.CalculateRelaxOffset();
            //     }
            //     foreach(Vertex vertex in vertices) // 这里vertices应该是一个已经定义好的Vertex对象的集合
            //     {
            //         vertex.Relax();
            //     }
            // }
            


        }
    }
}
