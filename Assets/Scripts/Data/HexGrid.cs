using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGrid : MonoBehaviour, IGrid
{
    public int gridRadius;
    public float hexRadius;

    private List<Hex> hexTable;

    public void GenerateGrid()
    {
        hexTable = new List<Hex>();

        for (int q = -gridRadius; q <= gridRadius; q++)
        {
            int r1 = Mathf.Max(-gridRadius, -q - gridRadius);
            int r2 = Mathf.Min(gridRadius, -q + gridRadius);

            for(int r = r1; r <= r2; r++)
            {
                GenerateHex(q, r, hexRadius);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    private void GenerateHex(
        int column, 
        int row, 
        float hexRadius, 
        GameObject hexPrefab = null)
    {
        try
        {
            //instantiate hex and add to hexTable
            Hex hex = new Hex(
                column,
                row,
                hexRadius
                );

            var hexObject = Instantiate(
                hexPrefab ?? PrefabManager.HexPrefab,
                hex.Position,
                Quaternion.identity
                );

            //scale hexObject
            float scale = hexRadius / 0.5f;
            hexObject.transform.localScale = new Vector3(
                hexObject.transform.localScale.x * scale,
                hexObject.transform.localScale.y * scale,
                hexObject.transform.localScale.z * scale);

            //assign gameObject to Hex object
            hex.hexObject = hexObject;

            hexTable.Add(hex);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private void DestroyHex(Hex hex)
    {
        GameObject hexObject = hex.hexObject;
        Destroy(hexObject);
        hexTable.Remove(hex);
    }


    public Hex GetHoverHex(Vector2 point)
    {
        var q = (2.0f / 3.0f * point.x) / hexRadius;
        var r = (-1.0f / 3.0f * point.x + Mathf.Sqrt(3) / 3 * point.y) / hexRadius;

        try
        {
            Vector2 hexCoords = HexHelper.HexRound(point);
            return hexTable.Where(h => h.Column == hexCoords.x && h.Row == hexCoords.y).FirstOrDefault();
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
            return new Hex();
        }
    }
}
