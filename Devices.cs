using System;

namespace HomeAutomation.Devices;

// ── Light ─────────────────────────────────────────────
public class Light : ApplianceControl
{
    public override string Name => "Light";
}

// ── CeilingLight ──────────────────────────────────────
public class CeilingLight : ApplianceControl
{
    public override string Name => "Ceiling Light";
    private int _brightness = 100;

    public void Dim(int level)
    {
        _brightness = Math.Clamp(level, 0, 100);
        Console.WriteLine($"{Name} dimmed to {_brightness}%");
    }

    public override string GetStatus() => _isOn ? $"ON ({_brightness}%)" : "OFF";
}

// ── OutdoorLight ──────────────────────────────────────
public class OutdoorLight : ApplianceControl
{
    public override string Name => "Outdoor Light";
}

// ── GardenLight ───────────────────────────────────────
public class GardenLight : IDevice
{
    public string Name => "Garden Light";
    private bool _isOn = false;
    private TimeSpan _duskTime = new TimeSpan(19, 0, 0);
    private TimeSpan _dawnTime = new TimeSpan(6, 0, 0);

    public void SetDuskTime(TimeSpan t)  { _duskTime = t;  Console.WriteLine($"{Name} dusk set to {t}"); }
    public void SetDawnTime(TimeSpan t)  { _dawnTime = t;  Console.WriteLine($"{Name} dawn set to {t}"); }
    public void ManualOn()  { _isOn = true;  Console.WriteLine($"{Name} manually ON"); }
    public void ManualOff() { _isOn = false; Console.WriteLine($"{Name} manually OFF"); }

    public void On()  => ManualOn();
    public void Off() => ManualOff();
    public string GetStatus() => _isOn ? $"ON (dusk:{_duskTime:hh\\:mm} dawn:{_dawnTime:hh\\:mm})" : "OFF";
}

// ── TV ────────────────────────────────────────────────
public class TV : ApplianceControl
{
    public override string Name => "TV";
    private int _channel = 1;
    private int _volume  = 20;

    public void SetInputChannel(int ch) { _channel = ch; Console.WriteLine($"{Name} → channel {ch}"); }
    public void SetVolume(int vol)      { _volume  = Math.Clamp(vol, 0, 100); Console.WriteLine($"{Name} volume {_volume}"); }

    public override string GetStatus() => _isOn ? $"ON ch:{_channel} vol:{_volume}" : "OFF";
}

// ── Stereo ────────────────────────────────────────────
public class Stereo : ApplianceControl
{
    public override string Name => "Stereo";
    private string _mode = "Radio";
    private int _volume  = 20;

    public void SetCd()    { _mode = "CD";    Console.WriteLine($"{Name} → CD");    }
    public void SetDvd()   { _mode = "DVD";   Console.WriteLine($"{Name} → DVD");   }
    public void SetRadio() { _mode = "Radio"; Console.WriteLine($"{Name} → Radio"); }
    public void SetVolume(int vol) { _volume = Math.Clamp(vol, 0, 100); }

    public override string GetStatus() => _isOn ? $"ON [{_mode}] vol:{_volume}" : "OFF";
}

// ── CeilingFan ────────────────────────────────────────
public class CeilingFan : IDevice
{
    public string Name => "Ceiling Fan";
    private string _speed = "off";

    public void High()   { _speed = "HIGH";   Console.WriteLine($"{Name} → HIGH");   }
    public void Medium() { _speed = "MEDIUM"; Console.WriteLine($"{Name} → MEDIUM"); }
    public void Low()    { _speed = "LOW";    Console.WriteLine($"{Name} → LOW");    }
    public void On()     => High();
    public void Off()    { _speed = "off";    Console.WriteLine($"{Name} OFF"); }

    public string GetSpeed()    => _speed;
    public string GetStatus()   => _speed == "off" ? "OFF" : $"ON [{_speed}]";
}

// ── GarageDoor ────────────────────────────────────────
public class GarageDoor : IDevice
{
    public string Name => "Garage Door";
    private string _state = "closed";

    public void Up()       { _state = "open";   Console.WriteLine($"{Name} OPEN");  }
    public void Down()     { _state = "closed"; Console.WriteLine($"{Name} CLOSED");}
    public void Stop()     { _state = "stopped";Console.WriteLine($"{Name} STOPPED");}
    public void LightOn()  => Console.WriteLine($"{Name} light ON");
    public void LightOff() => Console.WriteLine($"{Name} light OFF");

    public void On()  => Up();
    public void Off() => Down();
    public string GetStatus() => _state.ToUpper();
}

// ── Hottub ────────────────────────────────────────────
public class Hottub : IDevice
{
    public string Name => "Hot Tub";
    private bool  _jetsOn    = false;
    private bool  _circulate;
    private int   _temp      = 100;

    public void Circulate()         { _circulate = true;  Console.WriteLine($"{Name} circulating"); }
    public void JetsOn()            { _jetsOn = true;     Console.WriteLine($"{Name} jets ON");     }
    public void JetsOff()           { _jetsOn = false;    Console.WriteLine($"{Name} jets OFF");    }
    public void SetTemperature(int t){ _temp = t;         Console.WriteLine($"{Name} temp → {t}°"); }

    public void On()  { Circulate(); JetsOn(); }
    public void Off() { _jetsOn = false; _circulate = false; Console.WriteLine($"{Name} OFF"); }
    public string GetStatus() => $"Temp:{_temp}° Jets:{(_jetsOn?"ON":"OFF")}";
}

// ── FaucetControl ─────────────────────────────────────
public class FaucetControl : IDevice
{
    public string Name => "Faucet";
    private bool _open = false;

    public void OpenValve()  { _open = true;  Console.WriteLine($"{Name} valve OPEN");  }
    public void CloseValve() { _open = false; Console.WriteLine($"{Name} valve CLOSED");}

    public void On()  => OpenValve();
    public void Off() => CloseValve();
    public string GetStatus() => _open ? "OPEN" : "CLOSED";
}

// ── Thermostat ────────────────────────────────────────
public class Thermostat : IDevice
{
    public string Name => "Thermostat";
    private int  _setTemp  = 70;
    private bool _heating  = false;

    public void SetTemperature(int t) { _setTemp = t; Console.WriteLine($"{Name} set to {t}°"); }
    public void On()  { _heating = true;  Console.WriteLine($"{Name} ON → {_setTemp}°"); }
    public void Off() { _heating = false; Console.WriteLine($"{Name} OFF"); }
    public string GetStatus() => _heating ? $"ON ({_setTemp}°)" : "OFF";
}

// ── SecurityControl ───────────────────────────────────
public class SecurityControl : IDevice
{
    public string Name => "Security";
    private bool _armed = false;

    public void Arm()   { _armed = true;  Console.WriteLine($"{Name} ARMED");   }
    public void Disarm(){ _armed = false; Console.WriteLine($"{Name} DISARMED");}

    public void On()  => Arm();
    public void Off() => Disarm();
    public string GetStatus() => _armed ? "ARMED 🔒" : "DISARMED 🔓";
}

// ── Sprinkler ─────────────────────────────────────────
public class Sprinkler : IDevice
{
    public string Name => "Sprinkler";
    private bool _running = false;

    public void WaterOn()  { _running = true;  Console.WriteLine($"{Name} ON");  }
    public void WaterOff() { _running = false; Console.WriteLine($"{Name} OFF"); }

    public void On()  => WaterOn();
    public void Off() => WaterOff();
    public string GetStatus() => _running ? "WATERING 💧" : "OFF";
}