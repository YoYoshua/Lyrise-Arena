using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{
    IShape CurrentField { get; }
    void Move(IShape destination, bool isUndoing = false);
}
