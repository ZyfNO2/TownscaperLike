namespace GridGenerator
{
    public class SubQuad
    {
        // 只读属性，表示子四边形的四个顶点
        public readonly Vertex_hex a;
        public readonly Vertex_mid b;
        public readonly Vertex_center c;
        public readonly Vertex_mid d;

        // 构造函数，用于初始化子四边形的四个顶点
        public SubQuad(Vertex_hex a, Vertex_mid b, Vertex_center c, Vertex_mid d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }
    }
}