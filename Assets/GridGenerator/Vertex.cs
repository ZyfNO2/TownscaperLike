using System.Collections.Generic;
using UnityEngine;

namespace GridGenerator
{
    public class Vertex
    {
        
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
        return vertices.GetRange(radius * (radius - 1) * 3 + 1, radius * 6);
    }
    
}
}


