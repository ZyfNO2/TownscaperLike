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
        
        
        private Grid grid;

        private void Awake()
        {
            grid = new Grid(radius,cellSize);
            Debug.Log(grid.hexes.Count);
            
        }

        private void OnDrawGizmos()
        {
            if (grid != null)
            {
                foreach (Vertex_hex vertex in grid.hexes)
                {
                    // 绘制一个半径为0.3f的球体在每个顶点的位置
                    Gizmos.DrawSphere(vertex.coord.worldPosition, 0.3f);
                }
            }
        }
    }
    
    
    
    
    
}