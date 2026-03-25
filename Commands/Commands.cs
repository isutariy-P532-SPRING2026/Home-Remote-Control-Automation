using HomeAutomation.Devices;

namespace HomeAutomation.Commands;

// ════════════════════════════════════════════════════════
//  Light
// ════════════════════════════════════════════════════════
public class LightOnCommand : ICommand
{
    private readonly Light _light;
    public string Description => $"{_light.Name} ON";
    public LightOnCommand(Light light) => _light = light;
    public void Execute() => _light.On();
    public void Undo()    => _light.Off();
}

public class LightOffCommand : ICommand
{
    private readonly Light _light;
    public string Description => $"{_light.Name} OFF";
    public LightOffCommand(Light light) => _light = light;
    public void Execute() => _light.Off();
    public void Undo()    => _light.On();
}

// ════════════════════════════════════════════════════════
//  CeilingLight
// ════════════════════════════════════════════════════════
public class CeilingLightOnCommand : ICommand
{
    private readonly CeilingLight _light;
    public string Description => $"{_light.Name} ON";
    public CeilingLightOnCommand(CeilingLight light) => _light = light;
    public void Execute() => _light.On();
    public void Undo()    => _light.Off();
}

public class CeilingLightOffCommand : ICommand
{
    private readonly CeilingLight _light;
    public string Description => $"{_light.Name} OFF";
    public CeilingLightOffCommand(CeilingLight light) => _light = light;
    public void Execute() => _light.Off();
    public void Undo()    => _light.On();
}

// ════════════════════════════════════════════════════════
//  OutdoorLight
// ════════════════════════════════════════════════════════
public class OutdoorLightOnCommand : ICommand
{
    private readonly OutdoorLight _light;
    public string Description => $"{_light.Name} ON";
    public OutdoorLightOnCommand(OutdoorLight light) => _light = light;
    public void Execute() => _light.On();
    public void Undo()    => _light.Off();
}

public class OutdoorLightOffCommand : ICommand
{
    private readonly OutdoorLight _light;
    public string Description => $"{_light.Name} OFF";
    public OutdoorLightOffCommand(OutdoorLight light) => _light = light;
    public void Execute() => _light.Off();
    public void Undo()    => _light.On();
}

// ════════════════════════════════════════════════════════
//  GardenLight
// ════════════════════════════════════════════════════════
public class GardenLightOnCommand : ICommand
{
    private readonly GardenLight _light;
    public string Description => $"{_light.Name} ON";
    public GardenLightOnCommand(GardenLight light) => _light = light;
    public void Execute() => _light.ManualOn();
    public void Undo()    => _light.ManualOff();
}

public class GardenLightOffCommand : ICommand
{
    private readonly GardenLight _light;
    public string Description => $"{_light.Name} OFF";
    public GardenLightOffCommand(GardenLight light) => _light = light;
    public void Execute() => _light.ManualOff();
    public void Undo()    => _light.ManualOn();
}

// ════════════════════════════════════════════════════════
//  TV
// ════════════════════════════════════════════════════════
public class TVOnCommand : ICommand
{
    private readonly TV _tv;
    public string Description => $"{_tv.Name} ON";
    public TVOnCommand(TV tv) => _tv = tv;
    public void Execute() => _tv.On();
    public void Undo()    => _tv.Off();
}

public class TVOffCommand : ICommand
{
    private readonly TV _tv;
    public string Description => $"{_tv.Name} OFF";
    public TVOffCommand(TV tv) => _tv = tv;
    public void Execute() => _tv.Off();
    public void Undo()    => _tv.On();
}

// ════════════════════════════════════════════════════════
//  Stereo
// ════════════════════════════════════════════════════════
public class StereoOnCommand : ICommand
{
    private readonly Stereo _stereo;
    public string Description => $"{_stereo.Name} ON";
    public StereoOnCommand(Stereo stereo) => _stereo = stereo;
    public void Execute() => _stereo.On();
    public void Undo()    => _stereo.Off();
}

public class StereoOffCommand : ICommand
{
    private readonly Stereo _stereo;
    public string Description => $"{_stereo.Name} OFF";
    public StereoOffCommand(Stereo stereo) => _stereo = stereo;
    public void Execute() => _stereo.Off();
    public void Undo()    => _stereo.On();
}

// ════════════════════════════════════════════════════════
//  CeilingFan  (ON = High, UNDO restores previous speed)
// ════════════════════════════════════════════════════════
public class CeilingFanOnCommand : ICommand
{
    private readonly CeilingFan _fan;
    private string _prevSpeed = "off";
    public string Description => $"{_fan.Name} HIGH";
    public CeilingFanOnCommand(CeilingFan fan) => _fan = fan;
    public void Execute() { _prevSpeed = _fan.GetSpeed(); _fan.High(); }
    public void Undo()
    {
        switch (_prevSpeed)
        {
            case "HIGH":   _fan.High();   break;
            case "MEDIUM": _fan.Medium(); break;
            case "LOW":    _fan.Low();    break;
            default:       _fan.Off();    break;
        }
    }
}

public class CeilingFanOffCommand : ICommand
{
    private readonly CeilingFan _fan;
    private string _prevSpeed = "off";
    public string Description => $"{_fan.Name} OFF";
    public CeilingFanOffCommand(CeilingFan fan) => _fan = fan;
    public void Execute() { _prevSpeed = _fan.GetSpeed(); _fan.Off(); }
    public void Undo()
    {
        switch (_prevSpeed)
        {
            case "HIGH":   _fan.High();   break;
            case "MEDIUM": _fan.Medium(); break;
            case "LOW":    _fan.Low();    break;
            default:       _fan.Off();    break;
        }
    }
}

// ════════════════════════════════════════════════════════
//  GarageDoor
// ════════════════════════════════════════════════════════
public class GarageDoorOpenCommand : ICommand
{
    private readonly GarageDoor _door;
    public string Description => $"{_door.Name} OPEN";
    public GarageDoorOpenCommand(GarageDoor door) => _door = door;
    public void Execute() => _door.Up();
    public void Undo()    => _door.Down();
}

public class GarageDoorCloseCommand : ICommand
{
    private readonly GarageDoor _door;
    public string Description => $"{_door.Name} CLOSE";
    public GarageDoorCloseCommand(GarageDoor door) => _door = door;
    public void Execute() => _door.Down();
    public void Undo()    => _door.Up();
}

// ════════════════════════════════════════════════════════
//  Hottub
// ════════════════════════════════════════════════════════
public class HottubOnCommand : ICommand
{
    private readonly Hottub _tub;
    public string Description => $"{_tub.Name} ON";
    public HottubOnCommand(Hottub tub) => _tub = tub;
    public void Execute() => _tub.On();
    public void Undo()    => _tub.Off();
}

public class HottubOffCommand : ICommand
{
    private readonly Hottub _tub;
    public string Description => $"{_tub.Name} OFF";
    public HottubOffCommand(Hottub tub) => _tub = tub;
    public void Execute() => _tub.Off();
    public void Undo()    => _tub.On();
}

// ════════════════════════════════════════════════════════
//  FaucetControl
// ════════════════════════════════════════════════════════
public class FaucetOnCommand : ICommand
{
    private readonly FaucetControl _faucet;
    public string Description => $"{_faucet.Name} OPEN";
    public FaucetOnCommand(FaucetControl faucet) => _faucet = faucet;
    public void Execute() => _faucet.OpenValve();
    public void Undo()    => _faucet.CloseValve();
}

public class FaucetOffCommand : ICommand
{
    private readonly FaucetControl _faucet;
    public string Description => $"{_faucet.Name} CLOSE";
    public FaucetOffCommand(FaucetControl faucet) => _faucet = faucet;
    public void Execute() => _faucet.CloseValve();
    public void Undo()    => _faucet.OpenValve();
}

// ════════════════════════════════════════════════════════
//  Thermostat
// ════════════════════════════════════════════════════════
public class ThermostatOnCommand : ICommand
{
    private readonly Thermostat _t;
    public string Description => $"{_t.Name} ON";
    public ThermostatOnCommand(Thermostat t) => _t = t;
    public void Execute() => _t.On();
    public void Undo()    => _t.Off();
}

public class ThermostatOffCommand : ICommand
{
    private readonly Thermostat _t;
    public string Description => $"{_t.Name} OFF";
    public ThermostatOffCommand(Thermostat t) => _t = t;
    public void Execute() => _t.Off();
    public void Undo()    => _t.On();
}

// ════════════════════════════════════════════════════════
//  SecurityControl
// ════════════════════════════════════════════════════════
public class SecurityArmCommand : ICommand
{
    private readonly SecurityControl _s;
    public string Description => $"{_s.Name} ARM";
    public SecurityArmCommand(SecurityControl s) => _s = s;
    public void Execute() => _s.Arm();
    public void Undo()    => _s.Disarm();
}

public class SecurityDisarmCommand : ICommand
{
    private readonly SecurityControl _s;
    public string Description => $"{_s.Name} DISARM";
    public SecurityDisarmCommand(SecurityControl s) => _s = s;
    public void Execute() => _s.Disarm();
    public void Undo()    => _s.Arm();
}

// ════════════════════════════════════════════════════════
//  Sprinkler
// ════════════════════════════════════════════════════════
public class SprinklerOnCommand : ICommand
{
    private readonly Sprinkler _sp;
    public string Description => $"{_sp.Name} ON";
    public SprinklerOnCommand(Sprinkler sp) => _sp = sp;
    public void Execute() => _sp.WaterOn();
    public void Undo()    => _sp.WaterOff();
}

public class SprinklerOffCommand : ICommand
{
    private readonly Sprinkler _sp;
    public string Description => $"{_sp.Name} OFF";
    public SprinklerOffCommand(Sprinkler sp) => _sp = sp;
    public void Execute() => _sp.WaterOff();
    public void Undo()    => _sp.WaterOn();
}
