namespace HomeAutomation.Devices;

public interface IDevice
{
    string Name { get; }
    void On();
    void Off();
    string GetStatus();
}
