using System;

namespace HomeAutomation.Devices;

public abstract class ApplianceControl : IDevice
{
    protected bool _isOn = false;
    public abstract string Name { get; }

    public virtual void On()
    {
        _isOn = true;
        Console.WriteLine($"{Name} is ON");
    }

    public virtual void Off()
    {
        _isOn = false;
        Console.WriteLine($"{Name} is OFF");
    }

    public virtual string GetStatus() => _isOn ? "ON" : "OFF";
}