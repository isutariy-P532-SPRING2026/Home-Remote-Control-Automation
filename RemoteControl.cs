using HomeAutomation.Devices;

namespace HomeAutomation;

/// <summary>
/// Remote with 7 programmable slots.
/// No Command pattern — devices are called directly.
/// Undo is handled by tracking what the last action was per slot.
/// </summary>
public class RemoteControl
{
    public const int SlotCount = 7;

    // The device assigned to each slot (null = empty)
    private readonly IDevice?[] _slots = new IDevice?[SlotCount];

    // Tracks last press per slot: true = ON was last pressed, false = OFF was last pressed
    private readonly bool?[] _lastAction = new bool?[SlotCount];

    public IDevice? GetDevice(int slot) => _slots[slot];

    public void SetDevice(int slot, IDevice? device)
    {
        _slots[slot] = device;
        _lastAction[slot] = null;
    }

    /// <summary>Presses ON for the given slot. Returns a log message.</summary>
    public string PressOn(int slot)
    {
        var device = _slots[slot];
        if (device == null) return $"Slot {slot + 1}: (empty)";

        device.On();
        _lastAction[slot] = true;
        return $"[Slot {slot + 1}] {device.Name} → ON  |  Status: {device.GetStatus()}";
    }

    /// <summary>Presses OFF for the given slot. Returns a log message.</summary>
    public string PressOff(int slot)
    {
        var device = _slots[slot];
        if (device == null) return $"Slot {slot + 1}: (empty)";

        device.Off();
        _lastAction[slot] = false;
        return $"[Slot {slot + 1}] {device.Name} → OFF  |  Status: {device.GetStatus()}";
    }

    /// <summary>
    /// Undoes the last button pressed across all slots.
    /// Iterates slots in reverse order to find the most recent action.
    /// </summary>
    public string PressUndo()
    {
        // Find the last slot that was acted on (simple: we track per slot, undo the last overall)
        // For simplicity without Command pattern: undo last slot that has a recorded action
        for (int i = SlotCount - 1; i >= 0; i--)
        {
            if (_lastAction[i].HasValue)
            {
                var device = _slots[i]!;
                bool lastWasOn = _lastAction[i]!.Value;
                _lastAction[i] = null;

                if (lastWasOn)
                {
                    device.Off();
                    return $"↩ UNDO [Slot {i + 1}] {device.Name} → OFF (undid ON)";
                }
                else
                {
                    device.On();
                    return $"↩ UNDO [Slot {i + 1}] {device.Name} → ON (undid OFF)";
                }
            }
        }
        return "↩ UNDO: nothing to undo";
    }
}
