using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class HexHelper
{
    public static float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;

    public static Vector3 CubeAdd(Vector3 cube_1, Vector3 cube_2)
    {
        return cube_1 + cube_2;
    }

    public static Vector3 CubeSubtract(Vector3 cube_1, Vector3 cube_2)
    {
        return cube_1 - cube_2;
    }

    public static Vector3 CubeMultiply(Vector3 cube_1, int k)
    {
        return new Vector3(
            cube_1.x * k,
            cube_1.y * k,
            cube_1.z * k
            );
    }

    public static Vector2 AxialAdd(Vector2 axial_1, Vector2 axial_2)
    {
        return CubeToAxial(CubeAdd(AxialToCube(axial_1), AxialToCube(axial_2)));
    }

    public static Vector2 AxialSubtract(Vector2 axial_1, Vector2 axial_2)
    {
        return CubeToAxial(CubeSubtract(AxialToCube(axial_1), AxialToCube(axial_2)));
    }

    public static Vector2 AxialMultiply(Vector2 axial_1, int k)
    {
        return CubeToAxial(CubeMultiply(AxialToCube(axial_1), k));
    }

    public static Hex HexAdd(Hex hex_1, Hex hex_2)
    {
        if(hex_1.Radius == hex_2.Radius)
        {
            return new Hex(hex_1.Column + hex_2.Column, hex_1.Row + hex_2.Row, hex_1.Radius);
        }
        else
        {
            Debug.Log("Hexes have different radiuses");
            return new Hex();
        }
    }

    public static Hex HexSubtract(Hex hex_1, Hex hex_2)
    {
        if (hex_1.Radius == hex_2.Radius)
        {
            return new Hex(hex_1.Column - hex_2.Column, hex_1.Row - hex_2.Row, hex_1.Radius);
        }
        else
        {
            Debug.Log("Hexes have different radiuses");
            return new Hex();
        }
    }

    public static Hex HexMultiply(Hex hex_1, int k)
    {
        return new Hex(hex_1.Column * k, hex_1.Row * k, hex_1.Radius);
    }

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

    public static List<Hex> GetMovementRange(Hex centerHex, List<Hex> hexList, int range)
    {
        List<Hex> results = new List<Hex>();
        Vector2 axialCoords = new Vector2();
        for (int q = -range; q <= range; q++)
        {
            int r1 = Mathf.Max(-range, -q - range);
            int r2 = Mathf.Min(range, -q + range);

            for (int r = r1; r <= r2; r++)
            {
                axialCoords.x = q;
                axialCoords.y = r;

                axialCoords = HexHelper.AxialAdd(centerHex.AxialPosition, axialCoords);

                Hex hexInRange = hexList.Where(h => h.Column == axialCoords.x && h.Row == axialCoords.y).FirstOrDefault();

                if(hexInRange != null)
                {
                    results.Add(hexInRange);
                }
            }
        }

        return results;
    }

    public static int CubeDistance(Vector3 cube_1, Vector3 cube_2)
    {
        return (int)((Mathf.Abs(cube_1.x - cube_2.x) + Mathf.Abs(cube_1.y - cube_2.y) + Mathf.Abs(cube_1.z - cube_2.z)) / 2);
    }

    public static int AxialDistance(Vector2 axial_1, Vector2 axial_2)
    {
        return CubeDistance(AxialToCube(axial_1), AxialToCube(axial_2));
    }
}
