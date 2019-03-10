using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceController : MonoBehaviour
{
    public Creature creature;
    public int CurrentActionPoints;
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

        creature = new Creature
        {
            HealthPoints = 100,
            ActionPoints = 2,
            OffensivePower = 12,
            DefensivePower = 20,
            ElementalAffinity = new Dictionary<Aspect, float>
            {
                { Aspect.Fire, 0.8f },
                { Aspect.Earth, 0.5f },
                { Aspect.Light, 0.3f },
                { Aspect.Wind, 0.2f },
                { Aspect.Life, 0.2f },
                { Aspect.Shadow, 0.2f },
                { Aspect.Void, 0.2f },
                { Aspect.Water, 0.2f }
            }
        };

        CurrentActionPoints = creature.ActionPoints;
    }

    void OnMouseDrag()
    {
        this.transform.position = MouseHelper.GetMouseWorldPoint(Camera.main);
    }

    internal void MovePiece(Hex destination)
    {
        Hex currentHex = (Hex)CurrentField;
        int distance = HexHelper.AxialDistance(currentHex.AxialPosition, destination.AxialPosition);
        CurrentField = destination;
        this.transform.position = destination.Position;
        CurrentActionPoints -= distance;
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
