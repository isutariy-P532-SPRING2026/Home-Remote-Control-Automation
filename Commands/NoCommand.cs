namespace HomeAutomation.Commands;

/// <summary>
/// Null Object — assigned to empty slots so we never need null checks.
/// </summary>
public class NoCommand : ICommand
{
    public string Description => "(empty)";
    public void Execute() { }
    public void Undo()    { }
}
