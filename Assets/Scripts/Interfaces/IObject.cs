using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObject
{
    Vector3 Position { get; }
    IShape CurrentField { get; set; }
    GameObject ObjectPrefab { get; set; }
}
