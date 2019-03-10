using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    [HideInInspector]
    public IShape CurrentField;

    // Start is called before the first frame update
    void Start()
    {
        IShape startField = 
            HexGrid.FieldList.Where(h => h.Position == new Vector3(0, 0)).FirstOrDefault()
            ?? HexGrid.FieldList[0];

        CurrentField = startField;
        this.transform.position = CurrentField.Position;
    }

    void OnMouseDrag()
    {
        this.transform.position = MouseHelper.GetMouseWorldPoint(Camera.main);
    }

    void OnMouseUp()
    {
        if(CurrentField is Hex)
        {
            Vector3 hexPosition = HexHelper.WorldPositionToAxial(this.transform.position, HexGrid.hexRadius);
            Debug.Log(hexPosition);
            IShape newField = HexGrid.FieldList.Where(f => f.Column == hexPosition.x && f.Row == hexPosition.y).FirstOrDefault();

            if(newField != null)
            {
                CurrentField = newField;
                Debug.Log(CurrentField);
                this.transform.position = CurrentField.Position;
            }

        }
    }

    private HexGrid HexGrid
    {
        get
        {
            return GetComponentInParent<HexGrid>();
        }
    }
}
