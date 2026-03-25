using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using HomeAutomation.Commands;
using HomeAutomation.Devices;

namespace HomeAutomation;

public partial class MainWindow : Window
{
    // ── One instance of every device ─────────────────────────────
    private readonly Light           _light           = new();
    private readonly CeilingLight    _ceilingLight    = new();
    private readonly OutdoorLight    _outdoorLight    = new();
    private readonly GardenLight     _gardenLight     = new();
    private readonly TV              _tv              = new();
    private readonly Stereo          _stereo          = new();
    private readonly CeilingFan      _ceilingFan      = new();
    private readonly GarageDoor      _garageDoor      = new();
    private readonly Hottub          _hottub          = new();
    private readonly FaucetControl   _faucet          = new();
    private readonly Thermostat      _thermostat      = new();
    private readonly SecurityControl _security        = new();
    private readonly Sprinkler       _sprinkler       = new();

    // ── Device display names (must match index order below) ──────
    private readonly List<string> _deviceNames;

    // ── ON / OFF command pairs per device (index matches name list)
    private readonly List<(ICommand On, ICommand Off)> _commandPairs;

    // ── The remote ───────────────────────────────────────────────
    private readonly RemoteControl _remote = new();

    // ── The 7 ComboBoxes ─────────────────────────────────────────
    private ComboBox[] _slotBoxes = null!;

    public MainWindow()
    {
        InitializeComponent();

        // Build command pairs — exactly mirrors the Java sample style
        _deviceNames = new List<string>
        {
            "Light",
            "Ceiling Light",
            "Outdoor Light",
            "Garden Light",
            "TV",
            "Stereo",
            "Ceiling Fan",
            "Garage Door",
            "Hot Tub",
            "Faucet",
            "Thermostat",
            "Security",
            "Sprinkler"
        };

        _commandPairs = new List<(ICommand, ICommand)>
        {
            (new LightOnCommand(_light),              new LightOffCommand(_light)),
            (new CeilingLightOnCommand(_ceilingLight),new CeilingLightOffCommand(_ceilingLight)),
            (new OutdoorLightOnCommand(_outdoorLight),new OutdoorLightOffCommand(_outdoorLight)),
            (new GardenLightOnCommand(_gardenLight),  new GardenLightOffCommand(_gardenLight)),
            (new TVOnCommand(_tv),                    new TVOffCommand(_tv)),
            (new StereoOnCommand(_stereo),            new StereoOffCommand(_stereo)),
            (new CeilingFanOnCommand(_ceilingFan),    new CeilingFanOffCommand(_ceilingFan)),
            (new GarageDoorOpenCommand(_garageDoor),  new GarageDoorCloseCommand(_garageDoor)),
            (new HottubOnCommand(_hottub),            new HottubOffCommand(_hottub)),
            (new FaucetOnCommand(_faucet),            new FaucetOffCommand(_faucet)),
            (new ThermostatOnCommand(_thermostat),    new ThermostatOffCommand(_thermostat)),
            (new SecurityArmCommand(_security),       new SecurityDisarmCommand(_security)),
            (new SprinklerOnCommand(_sprinkler),      new SprinklerOffCommand(_sprinkler))
        };

        // Grab ComboBoxes
        _slotBoxes = new[]
        {
            this.FindControl<ComboBox>("Slot0")!,
            this.FindControl<ComboBox>("Slot1")!,
            this.FindControl<ComboBox>("Slot2")!,
            this.FindControl<ComboBox>("Slot3")!,
            this.FindControl<ComboBox>("Slot4")!,
            this.FindControl<ComboBox>("Slot5")!,
            this.FindControl<ComboBox>("Slot6")!
        };

        // Populate ComboBoxes
        var items = new List<string> { "— empty —" };
        items.AddRange(_deviceNames);
        foreach (var cb in _slotBoxes)
        {
            cb.ItemsSource = items;
            cb.SelectedIndex = 0;
        }

        SetDefaultDevices();
        Log("Remote ready — Command pattern active.", "#7FB3D3");
    }

    // ── Pre-assign first 7 devices to slots ──────────────────────
    private void SetDefaultDevices()
    {
        for (int i = 0; i < RemoteControl.SlotCount && i < _commandPairs.Count; i++)
        {
            _slotBoxes[i].SelectedIndex = i + 1;
            var (on, off) = _commandPairs[i];
            _remote.SetCommand(i, on, off);
            Log($"Slot {i + 1}: assigned → {_deviceNames[i]}", "#BDC3C7");
        }
    }

    // ── Slot ComboBox changed ─────────────────────────────────────
    public void SlotChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is not ComboBox cb) return;
        int slot = int.Parse(cb.Tag?.ToString() ?? "0");
        int idx  = cb.SelectedIndex - 1;   // -1 = "empty"

        if (idx < 0 || idx >= _commandPairs.Count)
        {
            _remote.ClearSlot(slot);
            Log($"Slot {slot + 1}: cleared", "#BDC3C7");
        }
        else
        {
            var (on, off) = _commandPairs[idx];
            _remote.SetCommand(slot, on, off);
            Log($"Slot {slot + 1}: assigned → {_deviceNames[idx]}", "#BDC3C7");
        }
    }

    // ── ON button ─────────────────────────────────────────────────
    public void OnPressed(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
        int slot   = int.Parse(btn.Tag?.ToString() ?? "0");
        string msg = _remote.PressOn(slot);
        Log(msg, "#2ECC71");
    }

    // ── OFF button ────────────────────────────────────────────────
    public void OffPressed(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
        int slot   = int.Parse(btn.Tag?.ToString() ?? "0");
        string msg = _remote.PressOff(slot);
        Log(msg, "#E74C3C");
    }

    // ── UNDO button ───────────────────────────────────────────────
    public void UndoPressed(object? sender, RoutedEventArgs e)
    {
        string msg = _remote.PressUndo();
        Log(msg, "#9B59B6");
    }

    // ── Activity log ──────────────────────────────────────────────
    private void Log(string message, string hex = "#A8D8EA")
    {
        var panel  = this.FindControl<StackPanel>("LogPanel")!;
        var tb = new TextBlock
        {
            Text         = $"[{DateTime.Now:HH:mm:ss}]  {message}",
            Foreground   = new SolidColorBrush(Color.Parse(hex)),
            FontFamily   = new FontFamily("Consolas,Courier New,Monospace"),
            FontSize     = 12,
            Margin       = new Avalonia.Thickness(0, 2),
            TextWrapping = Avalonia.Media.TextWrapping.Wrap
        };
        panel.Children.Insert(0, tb);
        while (panel.Children.Count > 200)
            panel.Children.RemoveAt(panel.Children.Count - 1);
    }
}
