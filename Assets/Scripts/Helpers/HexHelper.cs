using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class HexHelper
{
    public static float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;

    public static Vector3 CalculateHexPosition(Vector2 axial, float radius)
    {
        float height = radius * 2;
        float width = WIDTH_MULTIPLIER * height;

        float vertical = height * 0.75f;
        float horizontal = width;

        return new Vector3(horizontal * (axial.x + axial.y / 2f), vertical * axial.y);
    }

    public static Vector2 CubeToAxial(Vector3 cube)
    {
        var q = cube.x;
        var r = cube.z;
        return new Vector2(q, r);
    }

    public static Vector3 AxialToCube(Vector2 axial)
    {
        var x = axial.x;
        var z = axial.y;
        var y = -x - z;
        return new Vector3(x, y, z);
    }

    public static Vector3 CubeRound(Vector3 cube)
    {
        var rx = Mathf.Round(cube.x);
        var ry = Mathf.Round(cube.y);
        var rz = Mathf.Round(cube.z);

        var xDiff = Mathf.Abs(rx - cube.x);
        var yDiff = Mathf.Abs(ry - cube.y);
        var zDiff = Mathf.Abs(rz - cube.z);

        if(xDiff > yDiff && xDiff > zDiff)
        {
            rx = -ry - rz;
        }
        else if(yDiff > zDiff)
        {
            ry = -rx - rz;
        }
        else
        {
            rz = -rx - ry;
        }

        return new Vector3(rx, ry, rz);
    }

    public static Vector2 AxialRound(Vector2 hex)
    {
        return CubeToAxial(CubeRound(AxialToCube(hex)));
    }

    public static Vector2 WorldPositionToAxial(Vector2 position, float hexSize)
    {
        var q = (Mathf.Sqrt(3) / 3.0f * position.x - 1.0f / 3.0f * position.y) / hexSize;
        var z = (2.0f / 3.0f * position.y) / hexSize;
        var r = -q - z;
        return AxialRound(new Vector2(q, r));
    }

    public static Hex GetHexOnWorldPosition(List<Hex> hexList, Vector3 position, float hexSize)
    {
        if(hexList.Any())
        {
            position = WorldPositionToAxial(position, hexSize);
            return hexList.Where(h => h.AxialPosition == position).FirstOrDefault();
        }

        return null;
    }
}
