using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager
{
    private List<ICommand> CommandList;
    private ICommand CurrentCommand;

    public CommandManager()
    {
        CommandList = new List<ICommand>();
        CurrentCommand = null;
    }

    #region ExecuteCommand()
    /// <summary>
    /// Executes command and adds it to internal command list.
    /// </summary>
    /// <param name="command">Command to be executed.</param>
    public void ExecuteCommand(ICommand command)
    {
        try
        {
            command.Execute();
            int commandIndex = -1;

            if (CommandList.Count != 0)
            {
                commandIndex = CommandList.IndexOf(CurrentCommand);
            }

            if (commandIndex == -1 || (commandIndex + 1 == CommandList.Count))
            {
                CommandList.Add(command);
            }
            else
            {
                CommandList[commandIndex + 1] = command;
            }

            CurrentCommand = command;
        }
        catch (NullReferenceException ex)
        {
            Debug.Log(ex.Message);
        }
    }
    #endregion

    #region UndoCommand()
    /// <summary>
    /// Undoes last executed command/
    /// </summary>
    public void UndoCommand()
    {
        if (CurrentCommand != null)
        {
            CurrentCommand.Undo();
            int commandIndex = CommandList.IndexOf(CurrentCommand);

            if (commandIndex != 0)
            {
                CurrentCommand = CommandList[commandIndex - 1];
            }
            else
            {
                CurrentCommand = null;
            }
        }
    }
    #endregion

    #region RedoCommand()
    /// <summary>
    /// Redoes last undoed command.
    /// </summary>
    public void RedoCommand()
    {
        if (CurrentCommand == null && CommandList.Count > 0)
        {
            CurrentCommand = CommandList[0];
            CurrentCommand.Execute();
        }
        else if (CurrentCommand != null && CommandList.Count > CommandList.IndexOf(CurrentCommand) + 1)
        {
            CurrentCommand = CommandList[CommandList.IndexOf(CurrentCommand) + 1];
            CurrentCommand.Execute();
        }
    }
    #endregion
}
