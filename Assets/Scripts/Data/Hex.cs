using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : IShape
{
    public int Column { get; }
    public int Row { get; }
    public int S { get; }
    public Vector3 Position { get; set; }
    public GameObject hexObject { get; set; }

    public static float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;

    public Hex()
    {
    }

    public Hex(int column, int row, float radius)
    {
        Column = column;
        Row = row;
        S = -(column + row);

        CalculatePosition(radius);
    }

    private void CalculatePosition(float radius)
    {
        float height = radius * 2;
        float width = WIDTH_MULTIPLIER * height;

        float vertical = height * 0.75f;
        float horizontal = width;

        Position = new Vector3(horizontal * (Column + Row / 2f), vertical * Row);
    }

    public Vector2 GetAxialPositon()
    {
        return HexHelper.CubeToAxial(Position);
    }
}
