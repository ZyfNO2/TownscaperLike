using System.Collections.Generic;


namespace GridGenerator
{
    public class Grid
    {
        public static int radius;
        public static float cellSize ;
        public readonly List<Vertex_hex> hexes = new List<Vertex_hex>();
        public readonly List<Triangle> triangles= new List<Triangle>();
        public readonly List<Edge> edges= new List<Edge>();
        public readonly List<Quad> quads= new List<Quad>();
        

        public Grid(int radius,float cellSize)
        {
            Grid.radius = radius;
            Grid.cellSize = cellSize;
            Vertex_hex.Hex(hexes, radius);
            Triangle.Triangles_Hex(hexes, edges,triangles);
            while (Triangle.HasNeighborTriangles(triangles))
            {
                Triangle.RandomlyMergeTriangles(edges,triangles,quads);
            }
            
        }
    }
}
