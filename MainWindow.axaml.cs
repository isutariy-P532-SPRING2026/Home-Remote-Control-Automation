using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using HomeAutomation.Devices;

namespace HomeAutomation;

public partial class MainWindow : Window
{
    // ── All available devices ─────────────────────────────────────
    private readonly List<IDevice> _allDevices;

    // ── The remote ───────────────────────────────────────────────
    private readonly RemoteControl _remote = new();

    // ── The 7 ComboBoxes (one per slot) ──────────────────────────
    private ComboBox[] _slotBoxes = null!;

    public MainWindow()
    {
        InitializeComponent();

        // Build the device catalogue
        _allDevices = new List<IDevice>
        {
            new Light(),
            new CeilingLight(),
            new OutdoorLight(),
            new GardenLight(),
            new TV(),
            new Stereo(),
            new CeilingFan(),
            new GarageDoor(),
            new Hottub(),
            new FaucetControl(),
            new Thermostat(),
            new SecurityControl(),
            new Sprinkler()
        };

        // Grab all slot ComboBoxes
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

        // Populate each ComboBox with "(empty)" + all devices
        foreach (var cb in _slotBoxes)
        {
            var items = new List<string> { "— empty —" };
            foreach (var d in _allDevices) items.Add(d.Name);
            cb.ItemsSource = items;
            cb.SelectedIndex = 0;
        }

        // Pre-assign sensible defaults to first 7 slots
        SetDefaultDevices();

        Log("Remote ready — assign devices and press ON / OFF.", "#7FB3D3");
    }

    // ── Pre-assign the first 7 devices to the 7 slots ─────────────
    private void SetDefaultDevices()
    {
        for (int i = 0; i < RemoteControl.SlotCount && i < _allDevices.Count; i++)
        {
            _slotBoxes[i].SelectedIndex = i + 1;          // skip "— empty —"
            _remote.SetDevice(i, _allDevices[i]);
        }
    }

    // ── Slot ComboBox changed ─────────────────────────────────────
    public void SlotChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is not ComboBox cb) return;
        int slot = int.Parse(cb.Tag?.ToString() ?? "0");
        int idx  = cb.SelectedIndex - 1;                  // -1 = "empty"

        IDevice? device = (idx >= 0 && idx < _allDevices.Count) ? _allDevices[idx] : null;
        _remote.SetDevice(slot, device);

        var msg = device == null
            ? $"Slot {slot + 1}: cleared"
            : $"Slot {slot + 1}: assigned → {device.Name}";
        Log(msg, "#BDC3C7");
    }

    // ── ON button ─────────────────────────────────────────────────
    public void OnPressed(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
        int slot = int.Parse(btn.Tag?.ToString() ?? "0");
        string result = _remote.PressOn(slot);
        Log(result, "#2ECC71");
    }

    // ── OFF button ────────────────────────────────────────────────
    public void OffPressed(object? sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
        int slot = int.Parse(btn.Tag?.ToString() ?? "0");
        string result = _remote.PressOff(slot);
        Log(result, "#E74C3C");
    }

    // ── UNDO button ───────────────────────────────────────────────
    public void UndoPressed(object? sender, RoutedEventArgs e)
    {
        string result = _remote.PressUndo();
        Log(result, "#9B59B6");
    }

    // ── Append a line to the activity log ─────────────────────────
    private void Log(string message, string hex = "#A8D8EA")
    {
        var panel  = this.FindControl<StackPanel>("LogPanel")!;
        var scroll = this.FindControl<ScrollViewer>("LogScroll")!;

        string timestamp = DateTime.Now.ToString("HH:mm:ss");

        var tb = new TextBlock
        {
            Text       = $"[{timestamp}]  {message}",
            Foreground = new SolidColorBrush(Color.Parse(hex)),
            FontFamily = new FontFamily("Consolas,Courier New,Monospace"),
            FontSize   = 12,
            Margin     = new Avalonia.Thickness(0, 2),
            TextWrapping = Avalonia.Media.TextWrapping.Wrap
        };

        panel.Children.Insert(0, tb);   // newest at top

        // Keep log from growing forever
        while (panel.Children.Count > 200)
            panel.Children.RemoveAt(panel.Children.Count - 1);
    }
}
