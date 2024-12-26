using System.Collections.Generic;

namespace GridGenerator
{
    public class Quad
    {
        // 只读属性，表示四边形的四个顶点
        public readonly Vertex_hex a;
        public readonly Vertex_hex b;
        public readonly Vertex_hex c;
        public readonly Vertex_hex d;

        // 只读属性，表示四边形的四条边
        public readonly Edge ab;
        public readonly Edge bc;
        public readonly Edge cd;
        public readonly Edge ad;

        // 构造函数，用于初始化四边形的四个顶点和四条边
        public Quad(Vertex_hex a, Vertex_hex b, Vertex_hex c, Vertex_hex d, List<Edge> edges,List<Quad> quads)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            ab = Edge.FindEdge(a, b, edges);
            bc = Edge.FindEdge(b, c, edges);
            cd = Edge.FindEdge(c, d, edges);
            ad = Edge.FindEdge(a, d, edges);
            quads.Add(this);
        }
    }
}