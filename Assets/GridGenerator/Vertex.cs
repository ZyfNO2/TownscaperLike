using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GridGenerator
{
    public class Vertex
    {
        public Vector3 initialPosition;
        public Vector3 currentPosition;
        public Vector3 offset = Vector3.zero;

        public void Relax()
        {
            currentPosition = initialPosition + offset;
        }
    }



public class Coord
{
    public readonly int q;
    public readonly int r;
    public readonly int s;
    public readonly Vector3 worldPosition;

    public Coord(int q, int r, int s)
    {
        this.q = q;
        this.r = r;
        this.s = s;
        worldPosition = WorldPosition();
        
    }

    // 计算并返回该坐标在世界中的位置
    public Vector3 WorldPosition()
    {
        
        
        return new Vector3(q * Mathf.Sqrt(3) / 2, 0, -(float)r - ((float)q / 2) )* 2 * Grid.cellSize; 
    }
    
    // 定义六个方向的坐标偏移量
    static public Coord[] directions = new Coord[]
    {
        new Coord(0,1,-1),
        new Coord(-1,1,0),
        new Coord(-1,0,1),
        new Coord(0,-1,1),
        new Coord(1,-1,0),
        new Coord(1,0,-1)
    };

    static public Coord Direction(int direction)
    {
        return Coord.directions[direction];
    }
    
    // 
    public Coord Add(Coord coord)
    {
        return new Coord(q + coord.q, r + coord.r, s + coord.s);
    }

    public Coord Scale(int k)
    {
        return new Coord(q * k, r * k, s * k);
    }
    
    //返回当前坐标在指定方向上的邻居坐标
    public Coord Neighbor(int direction)
    {
        return Add(Direction(direction));
    }
    
    //生成一个半径为radius的环形坐标列表
    public static List<Coord> Coord_Ring(int radius)
    {
        List<Coord> result = new List<Coord>();
        if (radius == 0)
        {
            result.Add(new Coord(0, 0, 0));
        }
        else
        {
            Coord coord = Coord.Direction(4).Scale(radius);
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < radius; j++)
                {
                    result.Add(coord);
                    coord = coord.Neighbor(i);
                }
            }
        }
        return result;
    }
    //生成一个半径为radius的六边形坐标列表
    public static List<Coord> Coord_Hex(int radius)
    {
        List<Coord> result = new List<Coord>();
        for (int i = 0; i <= radius; i++)
        {
            result.AddRange(Coord_Ring(i));
        }
        return result;
    }

  
    
}

public class Vertex_hex : Vertex
{
    public readonly Coord coord;
    
    public Vertex_hex(Coord coord)
    {   
        this.coord = coord;
        initialPosition = coord.worldPosition;
        currentPosition = initialPosition;
    }

    public static void Hex(List<Vertex_hex> vertices, int radius)
    {
        foreach (Coord coord in Coord.Coord_Hex(radius))
        {
            vertices.Add(new Vertex_hex(coord));
            
        }
    }
    
    
    public static List<Vertex_hex> GrabRing(int radius, List<Vertex_hex> vertices)
    {
        if (radius == 0)
        {
            return vertices.GetRange(0, 1);
        }
        //GetRange 返回一个从指定索引开始，并具有指定长度的指定类型子数组。 起始点为那一圈的第一个顶点
        return vertices.GetRange(radius * (radius - 1) * 3 + 1, radius * 6);
    }
    
}
public class Vertex_mid : Vertex
{
    // 构造函数，用于初始化边的中点
    public Vertex_mid(Edge edge, List<Vertex_mid> mids)
    {
        // 获取边的两个端点
        Vertex_hex a = edge.hexes.ToArray()[0];
        Vertex_hex b = edge.hexes.ToArray()[1];

        // 将当前中点对象添加到中点列表中
        mids.Add(this);

        // 计算中点的初始位置，即两个端点位置的平均值
        initialPosition = (a.initialPosition + b.initialPosition) / 2;
        currentPosition = initialPosition;
    }
}

// 基类 Vertex_center 继承自 Vertex，用于表示中心点
public class Vertex_center : Vertex
{
    
}

// Vertex_triangleCenter 类继承自 Vertex_center，用于表示三角形的中心点
public class Vertex_triangleCenter : Vertex_center
{
    public Vertex_triangleCenter(Triangle triangle)
    {
        // 计算三角形三个顶点初始位置的平均值，作为三角形中心点的初始位置
        initialPosition = (triangle.a.initialPosition + triangle.b.initialPosition + triangle.c.initialPosition) / 3;
        currentPosition = initialPosition;
    }
}

// Vertex_quadCenter 类继承自 Vertex_center，用于表示四边形的中心点
public class Vertex_quadCenter : Vertex_center
{
    public Vertex_quadCenter(Quad quad)
    {
        // 计算四边形四个顶点初始位置的平均值，作为四边形中心点的初始位置
        // 注意：这里应该是 quad.d.initialPosition 而不是 quad.c.initialPosition 重复
        initialPosition = (quad.a.initialPosition + quad.b.initialPosition + quad.c.initialPosition + quad.d.initialPosition) / 4;
        currentPosition = initialPosition;
    }
}







}


