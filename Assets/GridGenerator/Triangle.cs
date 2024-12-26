using System;
using System.Collections.Generic;
using System.Linq;

namespace GridGenerator
{
    public class Triangle
    {
        // 只读属性，代表三角形的三个顶点
        public readonly Vertex_hex a;
        public readonly Vertex_hex b;
        public readonly Vertex_hex c;
        
        public readonly Vertex_hex[] vertices;
        
        
        
        public readonly Edge ab;
        public readonly Edge bc;
        public readonly Edge ac;
        
        public readonly Edge[] edges;
        

        public Triangle(Vertex_hex a, Vertex_hex b, Vertex_hex c,List<Edge> edges, List<Triangle> triangles)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            vertices = new Vertex_hex[] { a, b, c };
            
            // 创建边线
            ab = Edge.FindEdge(a, b, edges);
            bc = Edge.FindEdge(b, c, edges);
            ac = Edge.FindEdge(a, c, edges);

            if (ab == null)
            {
                ab = new Edge(a, b, edges);
            }
            if (bc == null)
            {
                bc = new Edge(b, c, edges);
            }
            if (ac == null)
            {
                ac = new Edge(a, c, edges);
            }

            this.edges = new Edge[] { ab, bc, ac };
            triangles.Add(this);
            
            
        }

        /// <summary>
        /// 一个三角形的三个顶点中有两个相对与另一个是在不同的圈层上的（ring）。相对的，半径大的就是外圈，小的就是内圈
        /// </summary>
        /// <param name="radius"></param>
        /// <param name="vertices"></param>
        /// <param name="triangles"></param>
        public static void Triangles_Ring(int radius, List<Vertex_hex> vertices,List<Edge> edges, List<Triangle> triangles)
        {
            // 获取内圈的顶点列表
            List<Vertex_hex> inner = Vertex_hex.GrabRing(radius - 1, vertices);
            // 获取外圈的顶点列表
            List<Vertex_hex> outer = Vertex_hex.GrabRing(radius, vertices);
            // 遍历六边形的六个方向
            // 第一圈有6个，第二圈有12个，以此类推
            //所以说六个方向中，每一个方向radius个
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    // 创建两个顶点在外圈，一个顶点在内圈的三角形
                    Vertex_hex a = outer[i * radius + j];
                    //取余：转一圈回到0
                    Vertex_hex b = outer[(i * radius + j + 1) % outer.Count];
                    Vertex_hex c = inner[(i * (radius - 1) + j) % inner.Count];
                    // 将新创建的三角形添加到三角形列表中
                    new Triangle(a, b, c, edges,triangles);

                    // 创建一个顶点在外圈，两个顶点在内圈的三角形
                    if (j > 0)
                    {
                        Vertex_hex d = inner[i * (radius - 1) + j - 1];
                        // 将新创建的三角形添加到三角形列表中
                        new Triangle(a, c, d,edges, triangles);
                    }
                }
            }
        }
        
        public static void Triangles_Hex(List<Vertex_hex> vertices, List<Edge> edges,List<Triangle> triangles)
        {
            //i = 0 就没有顶点
            for (int i = 1; i <= Grid.radius; i++)
            {
                Triangles_Ring(i, vertices,edges ,triangles);
            }
        }
        
        // 判断当前三角形是否与目标三角形相邻(取边的交集，交集为1，相邻)
        public bool isNeighbor(Triangle target)
        {
            HashSet<Edge> intersection = new HashSet<Edge>(this.edges);
            intersection.IntersectWith(target.edges);
            return intersection.Count == 1;
        }

        // 查找并返回所有与当前三角形相邻的三角形
        public List<Triangle> FindAllNeighborTriangles(List<Triangle> triangles)
        {
            List<Triangle> result = new List<Triangle>();
            foreach (Triangle triangle in triangles)
            {
                if (this.isNeighbor(triangle))
                {
                    result.Add(triangle);
                }
            }
            return result;
        }
        //找到相邻三角形相邻边的函数
        public Edge NeighborEdge(Triangle target)
        {
            HashSet<Edge> intersection = new HashSet<Edge>(edges);
            intersection.IntersectWith(target.edges);
            return intersection.Single();
        }
        //查找当前三角形中与给定邻居三角形 neighbor 不共享的孤立顶点
        public Vertex_hex IsolatedVertex_Self(Triangle neighbor)
        {
            HashSet<Vertex_hex> exception = new HashSet<Vertex_hex>(vertices);
            exception.ExceptWith(NeighborEdge(neighbor).hexes);
            return exception.Single();
        }
        //用于查找给定邻居三角形 neighbor 中与当前三角形不共享的孤立顶点。
        public Vertex_hex IsolatedVertex_Neighbor(Triangle neighbor)
        {
            HashSet<Vertex_hex> exception = new HashSet<Vertex_hex>(neighbor.vertices);
            exception.ExceptWith(NeighborEdge(neighbor).hexes);
            return exception.Single();
        }
        
        public void MergeNeighborTriangles(Triangle neighbor, List<Edge> edges, List<Triangle> triangles, List<Quad> quads)
        {
            Vertex_hex a = IsolatedVertex_Self(neighbor);
            Vertex_hex b = vertices[(Array.IndexOf(vertices, a) + 1) % 3];
            Vertex_hex c = IsolatedVertex_Neighbor(neighbor);
            Vertex_hex d = neighbor.vertices[(Array.IndexOf(neighbor.vertices, c) + 1) % 3];

            Quad quad = new Quad(a, b, c, d, edges, quads);
            quads.Add(quad);
            edges.Remove(NeighborEdge(neighbor));
            triangles.Remove(this);
            triangles.Remove(neighbor);
        }
        
        public static bool HasNeighborTriangles(List<Triangle> triangles)
        {
            foreach (Triangle a in triangles)
            {
                foreach (Triangle b in triangles)
                {
                    if (a.isNeighbor(b))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        
        
        
        public static void RandomlyMergeTriangles(List<Edge> edges, List<Triangle> triangles, List<Quad> quads)
        {
            int randomIndex = UnityEngine.Random.Range(0, triangles.Count);
            List<Triangle> neighbors = triangles[randomIndex].FindAllNeighborTriangles(triangles);
            if (neighbors.Count != 0)
            {
                int randomNeighborIndex = UnityEngine.Random.Range(0, neighbors.Count);
                triangles[randomIndex].MergeNeighborTriangles(neighbors[randomNeighborIndex], edges, triangles, quads);
            }
        }
        
    }
}