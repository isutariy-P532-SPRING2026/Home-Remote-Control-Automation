using HomeAutomation.Commands;

namespace HomeAutomation;

/// <summary>
/// Remote with 7 programmable slots.
/// Each slot holds an ON command and an OFF command.
/// Undo calls Undo() on whichever command was last executed.
/// </summary>
public class RemoteControl
{
    public const int SlotCount = 7;

    private readonly ICommand[] _onCommands  = new ICommand[SlotCount];
    private readonly ICommand[] _offCommands = new ICommand[SlotCount];

    // Tracks the last command executed (for UNDO)
    private ICommand _lastCommand;

    public RemoteControl()
    {
        var noOp = new NoCommand();
        for (int i = 0; i < SlotCount; i++)
        {
            _onCommands[i]  = noOp;
            _offCommands[i] = noOp;
        }
        _lastCommand = noOp;
    }

    /// <summary>Assign a command pair to a slot.</summary>
    public void SetCommand(int slot, ICommand onCommand, ICommand offCommand)
    {
        _onCommands[slot]  = onCommand;
        _offCommands[slot] = offCommand;
    }

    /// <summary>Clear a slot back to NoCommand.</summary>
    public void ClearSlot(int slot)
    {
        var noOp = new NoCommand();
        _onCommands[slot]  = noOp;
        _offCommands[slot] = noOp;
    }

    public ICommand GetOnCommand(int slot)  => _onCommands[slot];
    public ICommand GetOffCommand(int slot) => _offCommands[slot];

    /// <summary>Press the ON button for a slot.</summary>
    public string PressOn(int slot)
    {
        _onCommands[slot].Execute();
        _lastCommand = _onCommands[slot];
        return $"[Slot {slot + 1}] {_onCommands[slot].Description}";
    }

    /// <summary>Press the OFF button for a slot.</summary>
    public string PressOff(int slot)
    {
        _offCommands[slot].Execute();
        _lastCommand = _offCommands[slot];
        return $"[Slot {slot + 1}] {_offCommands[slot].Description}";
    }

    /// <summary>Undo the last command executed.</summary>
    public string PressUndo()
    {
        if (_lastCommand is NoCommand)
            return "↩ UNDO: nothing to undo";

        _lastCommand.Undo();
        string desc = _lastCommand.Description;
        _lastCommand = new NoCommand();
        return $"↩ UNDO → reversed: {desc}";
    }
}
