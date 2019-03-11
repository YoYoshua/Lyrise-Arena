﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PieceController : MonoBehaviour, IMovable
{
    public Creature Creature { get; private set; }
    public int currentActionPoints;

    [HideInInspector]
    public IShape CurrentField { get; private set; }

    private HexGridController HexGrid
    {
        get
        {
            return GetComponentInParent<HexGridController>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        IShape startField = 
            HexGrid.FieldList.Where(h => h.Position == new Vector3(0, 0)).FirstOrDefault()
            ?? HexGrid.FieldList[0];

        CurrentField = startField;
        this.transform.position = CurrentField.Position;

        Creature = new Creature
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

        currentActionPoints = Creature.ActionPoints;
    }

    #region Move()
    /// <summary>
    /// Moves piece to passed destination shape. When isUndoing is true, APs are added instead of subtracted.
    /// </summary>
    /// <param name="destination">Destination field</param>
    /// <param name="isUndoing">Whether the action is being done or undone</param>
    public void Move(IShape destination, bool isUndoing = false)
    {
        Hex currentHex = (Hex)CurrentField;
        Hex destinationHex = (Hex)destination;

        int distance = HexHelper.AxialDistance(currentHex.AxialPosition, destinationHex.AxialPosition);
        CurrentField = destination;
        this.transform.position = destination.Position;

        if (!isUndoing)
        {
            currentActionPoints -= distance;
        }
        else
        {
            currentActionPoints += distance;
        }
    } 
    #endregion
}
