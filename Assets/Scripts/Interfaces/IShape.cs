using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShape
{
    int Column { get; }
    int Row { get; }

    Vector3 Position { get; set; }
}
