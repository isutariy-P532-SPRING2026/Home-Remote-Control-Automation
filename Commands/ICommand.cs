namespace HomeAutomation.Commands;

/// <summary>
/// Command pattern interface — every action is wrapped in a Command.
/// Execute() performs the action; Undo() reverses it.
/// </summary>
public interface ICommand
{
    void Execute();
    void Undo();
    string Description { get; }
}
