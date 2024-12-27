using System.Collections.Generic;

namespace GridGenerator
{
    public class Edge
    {
        // 只读属性，表示这条边连接的六边形顶点集合
        public readonly HashSet<Vertex_hex> hexes;
        
        public readonly Vertex_mid mid;

        // 构造函数，用于初始化边的两个顶点，并将其添加到边的集合中
        public Edge(Vertex_hex a, Vertex_hex b, List<Vertex_mid> mids,List<Edge> edges)
        {
            // 创建一个包含两个端点的HashSet
            hexes = new HashSet<Vertex_hex> { a, b };

            // 将新创建的边添加到边的列表中
            edges.Add(this);

            // 创建边的中点，并将其添加到中点的列表中
            mid = new Vertex_mid(this, mids);
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