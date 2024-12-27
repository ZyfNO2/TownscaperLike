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
        public readonly List<Edge> edges= new List<Edge>();
        public readonly List<Quad> quads= new List<Quad>();
        public readonly List<SubQuad> subQuads= new List<SubQuad>();

        public Grid(int radius,float cellSize)
        {
            Grid.radius = radius;
            Grid.cellSize = cellSize;
            Vertex_hex.Hex(hexes, radius);
            Triangle.Triangles_Hex(hexes, mids,centers,edges,triangles);
            while (Triangle.HasNeighborTriangles(triangles))
            {
                Triangle.RandomlyMergeTriangles(mids,centers,edges,triangles,quads);
            }

            foreach (Triangle triangle in triangles)
            {
                triangle.Subdivide(subQuads);
            }

            foreach (Quad quad in quads)
            {
                quad.Subdivide(subQuads);
            }
            
            
        }
    }
}
