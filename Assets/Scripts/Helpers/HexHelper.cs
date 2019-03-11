using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class HexHelper
{
    private static readonly float WIDTH_MULTIPLIER = Mathf.Sqrt(3) / 2;

    #region CubeOperations
    /// <summary>
    /// Adds two cube-based coordinates.
    /// </summary>
    /// <param name="cube_1"></param>
    /// <param name="cube_2"></param>
    /// <returns></returns>
    public static Vector3 CubeAdd(Vector3 cube_1, Vector3 cube_2)
    {
        return cube_1 + cube_2;
    } 

    /// <summary>
    /// Subtracts two cube-based coordinates.
    /// </summary>
    /// <param name="cube_1"></param>
    /// <param name="cube_2"></param>
    /// <returns></returns>
    public static Vector3 CubeSubtract(Vector3 cube_1, Vector3 cube_2)
    {
        return cube_1 - cube_2;
    }

    /// <summary>
    /// Multiply cube-based coordinate by value.
    /// </summary>
    /// <param name="cube_1"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public static Vector3 CubeMultiply(Vector3 cube_1, int k)
    {
        return new Vector3(
            cube_1.x * k,
            cube_1.y * k,
            cube_1.z * k
            );
    }
    #endregion

    #region AxialOperations
    /// <summary>
    /// Adds two axial-based coordinates.
    /// </summary>
    /// <param name="axial_1"></param>
    /// <param name="axial_2"></param>
    /// <returns></returns>
    public static Vector2 AxialAdd(Vector2 axial_1, Vector2 axial_2)
    {
        return CubeToAxial(CubeAdd(AxialToCube(axial_1), AxialToCube(axial_2)));
    }

    /// <summary>
    /// Subtracts two axial-based coordinates.
    /// </summary>
    /// <param name="axial_1"></param>
    /// <param name="axial_2"></param>
    /// <returns></returns>
    public static Vector2 AxialSubtract(Vector2 axial_1, Vector2 axial_2)
    {
        return CubeToAxial(CubeSubtract(AxialToCube(axial_1), AxialToCube(axial_2)));
    }

    /// <summary>
    /// Multiply axial-based coordinate by value.
    /// </summary>
    /// <param name="axial_1"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    public static Vector2 AxialMultiply(Vector2 axial_1, int k)
    {
        return CubeToAxial(CubeMultiply(AxialToCube(axial_1), k));
    }
    #endregion

    #region HexOperations
    /// <summary>
    /// Adds two Hex objects coordinates.
    /// </summary>
    /// <param name="hex_1"></param>
    /// <param name="hex_2"></param>
    /// <returns>Hex object with resulting coordinates</returns>
    public static Hex HexAdd(Hex hex_1, Hex hex_2)
    {
        if (hex_1.Radius == hex_2.Radius)
        {
            return new Hex(hex_1.Column + hex_2.Column, hex_1.Row + hex_2.Row, hex_1.Radius);
        }
        else
        {
            Debug.Log("Hexes have different radiuses");
            return new Hex();
        }
    }

    /// <summary>
    /// Subtracts two Hex objects coordinates.
    /// </summary>
    /// <param name="hex_1"></param>
    /// <param name="hex_2"></param>
    /// <returns>Hex object with resulting coordinates</returns>
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

    /// <summary>
    /// Multiply Hex object's coordinates by value.
    /// </summary>
    /// <param name="hex_1"></param>
    /// <param name="k"></param>
    /// <returns>Hex object with resulting coordinates</returns>
    public static Hex HexMultiply(Hex hex_1, int k)
    {
        return new Hex(hex_1.Column * k, hex_1.Row * k, hex_1.Radius);
    }
    #endregion

    #region CalculateHexWorldPosition()
    /// <summary>
    /// Calculates Hex position in world coordinates.
    /// </summary>
    /// <param name="axial">Axial coordinates of hex</param>
    /// <param name="radius">Radius of hex</param>
    /// <returns>Vector containing x and y parameters of Hex in world coordinates</returns>
    public static Vector3 CalculateHexWorldPosition(Vector2 axial, float radius)
    {
        float height = radius * 2;
        float width = WIDTH_MULTIPLIER * height;

        float vertical = height * 0.75f;
        float horizontal = width;

        return new Vector3(horizontal * (axial.x + axial.y / 2f), vertical * axial.y);
    }
    #endregion

    #region Conversions Axial <-> Cube
    /// <summary>
    /// Converts cube-based coordinates to axial-based coordinates
    /// </summary>
    /// <param name="cube"></param>
    /// <returns></returns>
    public static Vector2 CubeToAxial(Vector3 cube)
    {
        var q = cube.x;
        var r = cube.z;
        return new Vector2(q, r);
    }

    /// <summary>
    /// Converts axial-based coordinates to cube-based coordinates
    /// </summary>
    /// <param name="axial"></param>
    /// <returns></returns>
    public static Vector3 AxialToCube(Vector2 axial)
    {
        var x = axial.x;
        var z = axial.y;
        var y = -x - z;
        return new Vector3(x, y, z);
    }
    #endregion

    #region CubeRound()
    /// <summary>
    /// Rounds cube-based coordinates with float values to int values
    /// </summary>
    /// <param name="cube"></param>
    /// <returns></returns>
    public static Vector3 CubeRound(Vector3 cube)
    {
        var rx = Mathf.Round(cube.x);
        var ry = Mathf.Round(cube.y);
        var rz = Mathf.Round(cube.z);

        var xDiff = Mathf.Abs(rx - cube.x);
        var yDiff = Mathf.Abs(ry - cube.y);
        var zDiff = Mathf.Abs(rz - cube.z);

        if (xDiff > yDiff && xDiff > zDiff)
        {
            rx = -ry - rz;
        }
        else if (yDiff > zDiff)
        {
            ry = -rx - rz;
        }
        else
        {
            rz = -rx - ry;
        }

        return new Vector3(rx, ry, rz);
    }
    #endregion

    #region AxialRound()
    /// <summary>
    /// Rounds axial-based coordinates with float values to int values
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static Vector2 AxialRound(Vector2 hex)
    {
        return CubeToAxial(CubeRound(AxialToCube(hex)));
    }
    #endregion

    #region WorldPositionToAxial()
    /// <summary>
    /// Converts world position coordinates to axial-based hex coordinates. Hex coordinates are rounded before returning value.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="hexSize"></param>
    /// <returns></returns>
    public static Vector2 WorldPositionToAxial(Vector2 position, float hexSize)
    {
        var q = (Mathf.Sqrt(3) / 3.0f * position.x - 1.0f / 3.0f * position.y) / hexSize;
        var z = (2.0f / 3.0f * position.y) / hexSize;
        var r = -q - z;
        return AxialRound(new Vector2(q, r));
    }
    #endregion

    #region GetHexByWorldPosition()
    /// <summary>
    /// Returns Hex object containing world position coordinates from list of hexes.
    /// </summary>
    /// <param name="hexList">List of hexes</param>
    /// <param name="position">World position coordinates</param>
    /// <param name="hexRadius">Hex radius</param>
    /// <returns></returns>
    public static Hex GetHexByWorldPosition(List<Hex> hexList, Vector3 position, float hexRadius)
    {
        if (hexList.Any())
        {
            position = WorldPositionToAxial(position, hexRadius);
            return hexList.Where(h => h.AxialPosition == position).FirstOrDefault();
        }

        return null;
    }
    #endregion

    #region GetRange()
    /// <summary>
    /// Returns all hexes inside range of center. Center hex is returned as well.
    /// </summary>
    /// <param name="centerHex">Center hex</param>
    /// <param name="hexList">List of hexes</param>
    /// <param name="range">Range of center hex</param>
    /// <returns>List of Hex objects that are in range of center hex</returns>
    public static List<Hex> GetRange(Hex centerHex, List<Hex> hexList, int range)
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

                if (hexInRange != null)
                {
                    results.Add(hexInRange);
                }
            }
        }

        return results;
    }
    #endregion

    #region Distances
    /// <summary>
    /// Distance between two hexes in cube-based coordinate system
    /// </summary>
    /// <param name="cube_1"></param>
    /// <param name="cube_2"></param>
    /// <returns></returns>
    public static int CubeDistance(Vector3 cube_1, Vector3 cube_2)
    {
        return (int)((Mathf.Abs(cube_1.x - cube_2.x) + Mathf.Abs(cube_1.y - cube_2.y) + Mathf.Abs(cube_1.z - cube_2.z)) / 2);
    }

    /// <summary>
    /// Distance between two hexes in axial-based coordinate system
    /// </summary>
    /// <param name="axial_1"></param>
    /// <param name="axial_2"></param>
    /// <returns></returns>
    public static int AxialDistance(Vector2 axial_1, Vector2 axial_2)
    {
        return CubeDistance(AxialToCube(axial_1), AxialToCube(axial_2));
    } 
    #endregion
}
