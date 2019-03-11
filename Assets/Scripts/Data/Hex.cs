using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : IShape
{
    public int Column { get; private set; }
    public int Row { get; private set; }
    public int S { get; private set; }

    public Vector3 Position { get; private set; }
    public float Radius { get; }

    public Vector3 CubePosition
    {
        get
        {
            return new Vector3(Column, S, Row);
        }
    }

    public Vector3 AxialPosition
    {
        get
        {
            return HexHelper.CubeToAxial(CubePosition);
        }
    }

    public GameObject hexObject { get; set; }

    public Hex()
    {
    }

    public Hex(int column, int row, float radius)
    {
        Radius = radius;
        SetPosition(new Vector2(column, row));
    }

    public void SetPosition(Vector2 coords)
    {
        Column = (int)coords.x;
        Row = (int)coords.y;
        S = -((int)coords.x + (int)coords.y);

        Position = HexHelper.CalculateHexWorldPosition(CubePosition, Radius);
    }
}
