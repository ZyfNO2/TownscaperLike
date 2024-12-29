using System;
using UnityEngine;

namespace GridGenerator
{
    public class GridGenerator : MonoBehaviour
    {
        [SerializeField]
        private int radius;

        [SerializeField]
        private int cellSize;
        
        [SerializeField]
        private int relaxTimes;
        
        private Grid grid;

        private void Awake()
        {
            grid = new Grid(radius,cellSize,relaxTimes);
            //Debug.Log(grid.hexes.Count);
        }


        private void Update()
        {
            if(relaxTimes > 0)
            {
                foreach(SubQuad subQuad in grid.subQuads)
                {
                    subQuad.CalculateRelaxOffset();
                }
                foreach(Vertex vertex in grid.vertices) // 这里vertices应该是一个已经定义好的Vertex对象的集合
                {
                    vertex.Relax();
                }
                relaxTimes--;
            }
        }

        private void OnDrawGizmos()
        {
            if (grid != null)
            {
                foreach (Vertex_hex vertex in grid.hexes)
                {
                    Gizmos.DrawSphere(vertex.currentPosition, 0.1f);
                }
                // Gizmos.color = Color.yellow;
                // foreach (Triangle triangle in grid.triangles)
                // {
                //     Gizmos.DrawLine(triangle.a.currentPosition, triangle.b.currentPosition);
                //     Gizmos.DrawLine(triangle.b.currentPosition, triangle.c.currentPosition);
                //     Gizmos.DrawLine(triangle.c.currentPosition, triangle.a.currentPosition);
                //     //Gizmos.DrawSphere((triangle.a.currentPosition + triangle.b.coord.worldPosition + triangle.c.coord.worldPosition) / 3, 0.05f);
                // }
                // Gizmos.color = Color.green;
                // foreach (Quad quad in grid.quads)
                // {
                //     Gizmos.DrawLine(quad.a.currentPosition, quad.b.currentPosition);
                //     Gizmos.DrawLine(quad.b.currentPosition, quad.c.currentPosition);
                //     Gizmos.DrawLine(quad.c.currentPosition, quad.d.currentPosition);
                //     Gizmos.DrawLine(quad.d.currentPosition, quad.a.currentPosition);
                // }
                
                Gizmos.color = Color.red;
                foreach (Vertex_mid mid in grid.mids)
                {
                    Gizmos.DrawSphere(mid.currentPosition, 0.2f);
                }

                Gizmos.color = Color.cyan; 
                foreach (Vertex_center center in grid.centers)
                {
                    Gizmos.DrawSphere(center.currentPosition, 0.2f);
                }
                Gizmos.color = Color.white;
                foreach (SubQuad subQuad in grid.subQuads)
                {
                    Gizmos.DrawLine(subQuad.a.currentPosition, subQuad.b.currentPosition);
                    Gizmos.DrawLine(subQuad.b.currentPosition, subQuad.c.currentPosition);
                    Gizmos.DrawLine(subQuad.c.currentPosition, subQuad.d.currentPosition);
                    Gizmos.DrawLine(subQuad.d.currentPosition, subQuad.a.currentPosition);
                }
            }
        }
    }
    
    
    
    
    
}