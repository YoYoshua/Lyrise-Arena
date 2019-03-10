using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexHelperTests
{
    [Test]
    public void CalculateHexPosition_Test()
    {
        // Arrange
        var column = 3;
        var row = 5;
        var radius = 0.7f;

        // Act
        var hexPosition = HexHelper.CalculateHexPosition(new Vector2(column, row), radius);

        // Assert
        Assert.AreEqual(hexPosition.x, 6.66839564f, 0.1f);
        Assert.AreEqual(hexPosition.y, 5.2f, 0.1f);
        Assert.AreEqual(hexPosition.z, 0.0f, 0.1f);
    }

    [Test]
    public void CubeToAxial_Test()
    {
        // Arrange
        var cubeCoords = new Vector3(0, 2, -2);

        // Act
        var axialCoords = HexHelper.CubeToAxial(cubeCoords);

        // Assert
        Assert.AreEqual(axialCoords.x, 0);
        Assert.AreEqual(axialCoords.y, -2);
    }

    [Test]
    public void AxialToCube_Test()
    {
        // Arrange
        var axialCoords = new Vector2(-1, 2);

        // Act
        var cubeCoords = HexHelper.AxialToCube(axialCoords);

        // Assert
        Assert.AreEqual(cubeCoords.x, -1);
        Assert.AreEqual(cubeCoords.y, -1);
        Assert.AreEqual(cubeCoords.z, 2);
    }

    [Test]
    public void CubeRound_Test()
    {
        // Arrange
        var floatCubeCoords = new Vector3(-2.1f, -4.5f, 7.2f);

        // Act
        var roundedCubeCoords = HexHelper.CubeRound(floatCubeCoords);

        // Assert
        Assert.AreEqual(roundedCubeCoords.x, -2);
        Assert.AreEqual(roundedCubeCoords.y, -5);
        Assert.AreEqual(roundedCubeCoords.z, 7);

    }

    [Test]
    public void AxialRound_Test()
    {
        // Arrange
        var floatAxialCoords = new Vector3(-2.1f, 7.2f);

        // Act
        var roundedAxialCoords = HexHelper.AxialRound(floatAxialCoords);

        // Assert
        Assert.AreEqual(roundedAxialCoords.x, -2);
        Assert.AreEqual(roundedAxialCoords.y, 7);
    }

    [Test]
    public void WorldPositionToAxial_Test()
    {
        // Arrange
        var worldPosition = new Vector2(-1.299f, 2.25f);
        var hexSize = 0.5f;

        // Act
        var resultAxial = HexHelper.WorldPositionToAxial(worldPosition, hexSize);

        // Assert
        Assert.AreEqual(resultAxial.x, -3);
        Assert.AreEqual(resultAxial.y, 0);
    }

    [Test]
    public void GetHexOnWorldPosition_NotFound_Test()
    {
        // Arrange
        var worldPosition = new Vector2(-1.299f, 2.25f);
        var hexSize = 0.5f;

        List<Hex> hexList = new List<Hex>();
        hexList.Add(new Hex(0, 2, hexSize));
        hexList.Add(new Hex(0, 3, hexSize));
        hexList.Add(new Hex(2, 3, hexSize));
        hexList.Add(new Hex(-1, 2, hexSize));

        // Act
        var resultHex = HexHelper.GetHexOnWorldPosition(hexList, worldPosition, hexSize);

        // Assert
        Assert.IsNull(resultHex);
    }

    [Test]
    public void GetHexOnWorldPosition_Found_Test()
    {
        // Arrange
        var worldPosition = new Vector2(-1.299f, 2.25f);
        var hexSize = 0.5f;

        List<Hex> hexList = new List<Hex>();
        hexList.Add(new Hex(0, 2, hexSize));
        hexList.Add(new Hex(0, 3, hexSize));
        hexList.Add(new Hex(-3, 0, hexSize));
        hexList.Add(new Hex(-1, 2, hexSize));

        // Act
        var resultHex = HexHelper.GetHexOnWorldPosition(hexList, worldPosition, hexSize);

        // Assert
        Assert.IsNotNull(resultHex);

        Assert.AreEqual(resultHex.Column, -3);
        Assert.AreEqual(resultHex.Row, 0);
    }

    [Test]
    public void AxialOperations_Test()
    {
        // Arrange
        var axialCoords1 = new Vector2(-1, 2);
        var axialCoords2 = new Vector2(3, 1);
        var multiplyConstant = 2;

        // Act
        var resultAdd = HexHelper.AxialAdd(axialCoords1, axialCoords2);
        var resultSubtract = HexHelper.AxialSubtract(axialCoords1, axialCoords2);
        var resultMultiply = HexHelper.AxialMultiply(axialCoords1, multiplyConstant);

        // Assert
        Assert.AreEqual(resultAdd.x, 2);
        Assert.AreEqual(resultAdd.y, 3);

        Assert.AreEqual(resultSubtract.x, -4);
        Assert.AreEqual(resultSubtract.y, 1);

        Assert.AreEqual(resultMultiply.x, -2);
        Assert.AreEqual(resultMultiply.y, 4);
    }

    [Test]
    public void CubeOperations_Test()
    {
        // Arrange
        var cubeCoords1 = new Vector3(-1, 2, -1);
        var cubeCoords2 = new Vector3(3, 1, -4);
        var multiplyConstant = 2;

        // Act
        var resultAdd = HexHelper.CubeAdd(cubeCoords1, cubeCoords2);
        var resultSubtract = HexHelper.CubeSubtract(cubeCoords1, cubeCoords2);
        var resultMultiply = HexHelper.CubeMultiply(cubeCoords1, multiplyConstant);

        // Assert
        Assert.AreEqual(resultAdd.x, 2);
        Assert.AreEqual(resultAdd.y, 3);
        Assert.AreEqual(resultAdd.z, -5);

        Assert.AreEqual(resultSubtract.x, -4);
        Assert.AreEqual(resultSubtract.y, 1);
        Assert.AreEqual(resultSubtract.z, 3);

        Assert.AreEqual(resultMultiply.x, -2);
        Assert.AreEqual(resultMultiply.y, 4);
        Assert.AreEqual(resultMultiply.z, -2);
    }

    [Test]
    public void HexOperations_Test()
    {
        // Arrange
        var radius = 0.5f;
        var hex_1 = new Hex(-1, 2, radius);
        var hex_2 = new Hex(3, 1, radius);
        var multiplyConstant = 2;

        // Act
        var resultAdd = HexHelper.HexAdd(hex_1, hex_2);
        var resultSubtract = HexHelper.HexSubtract(hex_1, hex_2);
        var resultMultiply = HexHelper.HexMultiply(hex_1, multiplyConstant);

        // Assert
        Assert.AreEqual(resultAdd.Column, 2);
        Assert.AreEqual(resultAdd.Row, 3);
        Assert.AreEqual(resultAdd.S, -5);

        Assert.AreEqual(resultSubtract.Column, -4);
        Assert.AreEqual(resultSubtract.Row, 1);
        Assert.AreEqual(resultSubtract.S, 3);

        Assert.AreEqual(resultMultiply.Column, -2);
        Assert.AreEqual(resultMultiply.Row, 4);
        Assert.AreEqual(resultMultiply.S, -2);
    }

    [Test]
    public void GetMovementRange_Test()
    {
        // Arrange
        List<Hex> hexList = new List<Hex>();
        var gridRadius = 3;
        var hexRadius = 0.5f;

        for (int q = -gridRadius; q <= gridRadius; q++)
        {
            int r1 = Mathf.Max(-gridRadius, -q - gridRadius);
            int r2 = Mathf.Min(gridRadius, -q + gridRadius);

            for (int r = r1; r <= r2; r++)
            {
                Hex hex = new Hex(
                    q,
                    r,
                    hexRadius
                    );

                hexList.Add(hex);
            }
        }

        Hex centerHex = new Hex(0, 3, hexRadius);

        var range = 2;

        // Act
        List<Hex> hexInRange = HexHelper.GetMovementRange(centerHex, hexList, range);

        // Assert
        Assert.IsTrue(hexInRange.Count == 9);

        Assert.IsTrue(hexInRange.Any(h => h.Column == 0 && h.Row == 3));
        Assert.IsTrue(hexInRange.Any(h => h.Column == -1 && h.Row == 3));
        Assert.IsTrue(hexInRange.Any(h => h.Column == -2 && h.Row == 3));
        Assert.IsTrue(hexInRange.Any(h => h.Column == -1 && h.Row == 2));
        Assert.IsTrue(hexInRange.Any(h => h.Column == 0 && h.Row == 2));
        Assert.IsTrue(hexInRange.Any(h => h.Column == 1 && h.Row == 2));
        Assert.IsTrue(hexInRange.Any(h => h.Column == 2 && h.Row == 1));
        Assert.IsTrue(hexInRange.Any(h => h.Column == 1 && h.Row == 1));
        Assert.IsTrue(hexInRange.Any(h => h.Column == 0 && h.Row == 1));
    }
}
