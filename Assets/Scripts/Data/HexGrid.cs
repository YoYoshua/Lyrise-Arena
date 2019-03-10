using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGrid : MonoBehaviour, IGrid
{
    public int gridRadius;
    public float hexRadius;

    [HideInInspector]
    public List<IShape> FieldList { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    void Update()
    {
        if(Input.GetMouseButton(0))
        {
            CheckPieceOnField(MouseHelper.GetMouseWorldPoint(Camera.main));
        }
    }

    public void GenerateGrid()
    {
        FieldList = new List<IShape>();

        for (int q = -gridRadius; q <= gridRadius; q++)
        {
            int r1 = Mathf.Max(-gridRadius, -q - gridRadius);
            int r2 = Mathf.Min(gridRadius, -q + gridRadius);

            for (int r = r1; r <= r2; r++)
            {
                GenerateHex(q, r, hexRadius);
            }
        }
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

            //set new hex object as a child
            hexObject.transform.parent = gameObject.transform;

            //pass Hex object to HexController
            hexObject.GetComponent<HexController>().HexObject = hex;

            //scale hexObject
            float scale = hexRadius / 0.5f;
            hexObject.transform.localScale = new Vector3(
                hexObject.transform.localScale.x * scale,
                hexObject.transform.localScale.y * scale,
                hexObject.transform.localScale.z * scale);

            //assign gameObject to Hex object
            hex.hexObject = hexObject;

            FieldList.Add(hex);
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
        FieldList.Remove(hex);
    }

    public void CheckPieceOnField(Vector2 position)
    {
        List<PieceController> piecesOnBoard = gameObject.GetComponentsInChildren<PieceController>().ToList();

        Vector3 clickedHexCoords = HexHelper.WorldPositionToAxial(MouseHelper.GetMouseWorldPoint(Camera.main), hexRadius);
        List<Hex> hexList = FieldList.ConvertAll(h => (Hex)h);

        Hex clickedHex = hexList.Where(h => h.CubePosition == clickedHexCoords).FirstOrDefault();

        foreach(var piece in piecesOnBoard)
        {
            if(piece.CurrentField == clickedHex)
            {
                Debug.Log("Piece clicked!");
            }
        }
    }
}
