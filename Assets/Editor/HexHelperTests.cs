using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
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
}
