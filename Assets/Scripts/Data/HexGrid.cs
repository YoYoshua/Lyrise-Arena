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

    private List<Hex> HighlightedFields;
    private PieceController SelectedPiece;
    private bool IsPieceSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        HighlightedFields = new List<Hex>();
        GenerateGrid();
    }

    #region CheckField()
    public void CheckField(Hex field)
    {
        var pieceOnField = IsPieceOnField(field);
        if (!IsPieceSelected && pieceOnField.IsPieceOnField)
        {
            SelectPiece(pieceOnField.Piece);
        }
        else if (IsPieceSelected && !pieceOnField.IsPieceOnField)
        {
            if(CheckIfFieldInRange(field))
            {
                MovePieceToField(SelectedPiece, field);
                DeselectPiece();
            }
            else
            {
                DeselectPiece();
            }
        }
        Debug.Log(String.Format("IsPieceSelected: {0}", IsPieceSelected));
    }

    private void MovePieceToField(PieceController selectedPiece, Hex field)
    {
        selectedPiece.MovePiece(field);
    }

    private bool CheckIfFieldInRange(Hex field)
    {
        return HighlightedFields.Any(h => h.AxialPosition == field.AxialPosition);
    }

    private void DeselectPiece()
    {
        SelectedPiece = null;
        IsPieceSelected = false;

        HideReach();
    }

    private void SelectPiece(PieceController piece)
    {
        SelectedPiece = piece;
        IsPieceSelected = true;

        HideReach();
        ShowReach(piece, piece.CurrentField);
    }
    #endregion

    #region GenerateGrid()
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
    #endregion

    #region GenerateHex()
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
    #endregion

    #region DestroyHex()
    private void DestroyHex(Hex hex)
    {
        GameObject hexObject = hex.hexObject;
        Destroy(hexObject);
        FieldList.Remove(hex);
    }
    #endregion

    #region IsPieceOnField()
    public (bool IsPieceOnField, PieceController Piece) IsPieceOnField(Hex field)
    {
        List<PieceController> piecesOnBoard = gameObject.GetComponentsInChildren<PieceController>().ToList();
        List<Hex> hexList = FieldList.ConvertAll(h => (Hex)h);

        foreach (var piece in piecesOnBoard)
        {
            if (piece.CurrentField == field)
            {
                return (true, piece);
            }
        }

        return (false, null);
    }
    #endregion

    #region ShowReach()
    public void ShowReach(PieceController piece, IShape clickedHex)
    {
        int range = piece.CurrentActionPoints;

        List<Hex> reachableHexes = new List<Hex>();
        reachableHexes = HexHelper.GetMovementRange((Hex)piece.CurrentField, FieldList.Cast<Hex>().ToList(), range);

        foreach (Hex hex in reachableHexes)
        {
            hex.hexObject.GetComponent<HexController>().SetReach(true);
        }

        HighlightedFields = reachableHexes;
    }
    #endregion

    #region HideReach()
    public void HideReach()
    {
        foreach (Hex hex in HighlightedFields)
        {
            hex.hexObject.GetComponent<HexController>().SetReach(false);
        }
        HighlightedFields.Clear();
    }
    #endregion
}
