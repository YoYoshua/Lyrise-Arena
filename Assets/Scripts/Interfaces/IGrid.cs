using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrid
{
    List<IShape> FieldList { get; set; }

    void GenerateGrid();
    void ShowReach(PieceController piece, IShape field);
    void HideReach();
}
