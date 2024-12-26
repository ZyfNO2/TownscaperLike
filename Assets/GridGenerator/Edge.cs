using System.Collections.Generic;

namespace GridGenerator
{
    public class Edge
    {
        // 只读属性，表示这条边连接的六边形顶点集合
        public readonly HashSet<Vertex_hex> hexes;

        // 构造函数，用于初始化边的两个顶点，并将其添加到边的集合中
        public Edge(Vertex_hex a, Vertex_hex b, List<Edge> edges)
        {
            hexes = new HashSet<Vertex_hex> { a, b };
            edges.Add(this);
        }
        
        public static Edge FindEdge(Vertex_hex a, Vertex_hex b, List<Edge> edges)
        {
            foreach (Edge edge in edges)
            {
                if (edge.hexes.Contains(a) && edge.hexes.Contains(b))
                {
                    return edge;
                }
            }
            return null;
        }
        
        
        
        
    }
    
    
    
    
    
    
}