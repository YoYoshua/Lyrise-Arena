using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HexGridController : MonoBehaviour, IGrid
{
    public int gridRadius;
    public float hexRadius;

    [HideInInspector]
    public List<IShape> FieldList { get; set; }

    private List<Hex> HighlightedFields;
    private PieceController SelectedPiece;
    private bool IsPieceSelected = false;

    private CommandManager commandManager;

    // Start is called before the first frame update
    void Start()
    {
        commandManager = new CommandManager();

        HighlightedFields = new List<Hex>();
        GenerateGrid();
    }

    #region GenerateGrid()
    /// <summary>
    /// Generates hex grid based on public gridRadius and public hexRadius
    /// </summary>
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
    /// <summary>
    /// Generates Hex object, instantiates Hex Prefab based on Hex object and adds it to global FieldList.
    /// </summary>
    /// <param name="column">Column in grid</param>
    /// <param name="row">Row in grid</param>
    /// <param name="hexRadius">Radius of hex</param>
    /// <param name="hexPrefab">Prefab containing hex sprite and controller</param>
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
    /// <summary>
    /// Removes passed Hex object with hex prefab connected to it.
    /// </summary>
    /// <param name="hex">Hex object to be destroyed</param>
    private void DestroyHex(Hex hex)
    {
        GameObject hexObject = hex.hexObject;
        Destroy(hexObject);
        FieldList.Remove(hex);
    }
    #endregion

    #region OnFieldClick()
    /// <summary>
    /// Executes certain action when clicked on passed field (select, deselect, move) based on current situation.
    /// </summary>
    /// <param name="field">Clicked field</param>
    public void OnFieldClick(Hex field)
    {
        var pieceOnField = IsPieceOnField(field);

        if (!IsPieceSelected && pieceOnField.IsPieceOnField)
        {
            SelectPiece(pieceOnField.Piece);
        }
        else if (IsPieceSelected && !pieceOnField.IsPieceOnField)
        {
            if (CheckIfFieldInRange(field))
            {
                MovePieceToField(SelectedPiece, field);
                DeselectPiece();
            }
            else
            {
                DeselectPiece();
            }
        }
    }
    #endregion

    #region MovePieceToField()
    /// <summary>
    /// Executes move command resulting in moving passed piece to passed field.
    /// </summary>
    /// <param name="selectedPiece">Piece to move</param>
    /// <param name="field">Destination field for piece</param>
    private void MovePieceToField(PieceController selectedPiece, Hex field)
    {
        MoveCommand move = new MoveCommand(selectedPiece, field);
        commandManager.ExecuteCommand(move);
    }
    #endregion

    #region CheckIfFieldInRange()
    /// <summary>
    /// Checks if passed field is in currently highlited fields.
    /// </summary>
    /// <param name="field">Field to check</param>
    /// <returns></returns>
    private bool CheckIfFieldInRange(Hex field)
    {
        return HighlightedFields.Any(h => h.AxialPosition == field.AxialPosition);
    }
    #endregion

    #region SelectPiece()
    /// <summary>
    /// Selects current piece and shows its reach based on its parameters.
    /// </summary>
    /// <param name="piece">Piece to select</param>
    private void SelectPiece(PieceController piece)
    {
        SelectedPiece = piece;
        IsPieceSelected = true;

        HideReach();
        ShowReach(piece, piece.CurrentField);
    }
    #endregion

    #region DelesectPiece()
    /// <summary>
    /// Deselects currently selected piece. If no piece is selected, does nothing.
    /// </summary>
    private void DeselectPiece()
    {
        SelectedPiece = null;
        IsPieceSelected = false;

        HideReach();
    }
    #endregion

    #region IsPieceOnField()
    /// <summary>
    /// Checks whether there is any object with PieceController associated with passed field.
    /// </summary>
    /// <param name="field">Field to check</param>
    /// <returns>IsPieceOnField: bool containing info whether there is piece on current field
    ///          Piece: PieceController object returned when IsPieceOnField is true, containing data about piece on current field</returns>
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
    /// <summary>
    /// Changes sprite of all hexes within piece reach, inserting them into HighlitedFields list.
    /// </summary>
    /// <param name="piece">Piece that contains reach info</param>
    /// <param name="clickedHex">Hex object that is a center for reach calculations</param>
    public void ShowReach(PieceController piece, IShape clickedHex)
    {
        int range = piece.currentActionPoints;

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
    /// <summary>
    /// Hides currently shown reach, clears HighlitedFields list.
    /// </summary>
    public void HideReach()
    {
        foreach (Hex hex in HighlightedFields)
        {
            hex.hexObject.GetComponent<HexController>().SetReach(false);
        }
        HighlightedFields.Clear();
    }
    #endregion

    #region OnUndoButtonClick()
    /// <summary>
    /// Actions executed after clicking Undo button. Undoes last command and deselects piece.
    /// </summary>
    public void OnUndoButtonClick()
    {
        commandManager.UndoCommand();
        DeselectPiece();
    }
    #endregion

    #region OnRedoButtonClick()

    /// <summary>
    /// Actions executed after clicking Redo button. Redoes last command and deselects piece.
    /// </summary>
    public void OnRedoButtonClick()
    {
        commandManager.RedoCommand();
        DeselectPiece();
    }
    #endregion
}
