using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{
    private IMovable Target;
    private IShape PreviousField;
    private IShape DestinationField;

    public MoveCommand(IMovable target, IShape destination)
    {
        this.Target = target;
        this.DestinationField = destination;
    }

    /// <summary>
    /// Executes command.
    /// </summary>
    public void Execute()
    {
        PreviousField = Target.CurrentField;
        Target.Move(DestinationField);
    }

    /// <summary>
    /// Undoes command.
    /// </summary>
    public void Undo()
    {
        Target.Move(PreviousField, true);
    }
}
