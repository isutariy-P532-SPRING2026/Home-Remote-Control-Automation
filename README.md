# 🏠 Home Automation Remote Control
### Avalonia C# Desktop App — Command Pattern

---

## 📋 Project Overview

A desktop remote control simulation built with **Avalonia UI** and **C# (.NET 8)**.  
The remote has **7 programmable slots**, each with an **ON** and **OFF** button, plus a global **UNDO** button.  
Device actions are encapsulated using the **Command design pattern**.

---

## 🗂️ Project Structure

```
HomeAutomation/
│
├── Program.cs                   # Entry point
├── App.axaml / App.axaml.cs     # Avalonia application bootstrap
├── MainWindow.axaml             # Remote control UI (7 slots + UNDO button)
├── MainWindow.axaml.cs          # UI logic — wires commands to buttons
├── RemoteControl.cs             # 7-slot remote — stores ICommand pairs, handles UNDO
│
├── Commands/
│   ├── ICommand.cs              # Interface: Execute(), Undo(), Description
│   ├── NoCommand.cs             # Null Object — used for empty slots (no null checks needed)
│   └── Commands.cs              # 26 concrete command classes (one per device × action)
│
└── Devices/
    ├── IDevice.cs               # Interface: On(), Off(), GetStatus()
    ├── ApplianceControl.cs      # Abstract base class with default On/Off behaviour
    └── Devices.cs               # 13 device classes from UML diagram
```

---

## 🔌 Devices Implemented (from UML diagram)

| Device | ON Command | OFF Command |
|---|---|---|
| Light | LightOnCommand | LightOffCommand |
| CeilingLight | CeilingLightOnCommand | CeilingLightOffCommand |
| OutdoorLight | OutdoorLightOnCommand | OutdoorLightOffCommand |
| GardenLight | GardenLightOnCommand | GardenLightOffCommand |
| TV | TVOnCommand | TVOffCommand |
| Stereo | StereoOnCommand | StereoOffCommand |
| CeilingFan | CeilingFanOnCommand (→ High) | CeilingFanOffCommand |
| GarageDoor | GarageDoorOpenCommand | GarageDoorCloseCommand |
| Hot Tub | HottubOnCommand | HottubOffCommand |
| Faucet | FaucetOnCommand (openValve) | FaucetOffCommand (closeValve) |
| Thermostat | ThermostatOnCommand | ThermostatOffCommand |
| Security | SecurityArmCommand | SecurityDisarmCommand |
| Sprinkler | SprinklerOnCommand (waterOn) | SprinklerOffCommand (waterOff) |

---

## 🎨 Command Pattern — How It Works

```
                 ┌─────────────────┐
                 │  <<interface>>  │
                 │    ICommand     │
                 │─────────────────│
                 │ + Execute()     │
                 │ + Undo()        │
                 │ + Description   │
                 └────────┬────────┘
                          │ implements
          ┌───────────────┼───────────────┐
          │               │               │
  ┌───────────────┐ ┌───────────┐ ┌────────────┐
  │LightOnCommand │ │TVOnCommand│ │ NoCommand  │
  │───────────────│ │───────────│ │────────────│
  │Execute()→On() │ │Execute()  │ │Execute() {}│  ← Null Object
  │Undo()  →Off() │ │Undo()     │ │Undo()    {}│    (empty slots)
  └───────────────┘ └───────────┘ └────────────┘

  ┌──────────────────────────────────────────────────┐
  │               RemoteControl                      │
  │──────────────────────────────────────────────────│
  │  _onCommands[7]   : ICommand[]                   │
  │  _offCommands[7]  : ICommand[]                   │
  │  _lastCommand     : ICommand                     │
  │──────────────────────────────────────────────────│
  │  SetCommand(slot, onCmd, offCmd)                 │
  │  PressOn(slot)  → onCmd.Execute()                │
  │  PressOff(slot) → offCmd.Execute()               │
  │  PressUndo()    → _lastCommand.Undo()            │
  └──────────────────────────────────────────────────┘
```

**Flow for a button press:**
```
User clicks ON (Slot 1)
  → MainWindow.OnPressed()
    → remote.PressOn(0)
      → _onCommands[0].Execute()    // LightOnCommand.Execute()
        → light.On()                // actual device call
  → _lastCommand = LightOnCommand

User clicks UNDO
  → remote.PressUndo()
    → _lastCommand.Undo()           // LightOnCommand.Undo()
      → light.Off()                 // reverses the action
```

---

## ▶️ Running the App

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- VS Code + **C# Dev Kit** extension

### Steps

```bash
# 1. Navigate to the project folder
cd HomeAutomation

# 2. Restore NuGet packages
dotnet restore

# 3. Run the app
dotnet run
```

---

## 🖥️ UI Guide

| Element | Description |
|---|---|
| **ComboBox (each row)** | Pick which device to assign to that slot |
| **ON button (green)** | Calls `Execute()` on the slot's ON command |
| **OFF button (red)** | Calls `Execute()` on the slot's OFF command |
| **UNDO LAST (purple)** | Calls `Undo()` on the most recently executed command |
| **Activity Log** | Shows every action with timestamp, colour-coded by type |

---

## 📦 NuGet Dependencies

| Package | Version |
|---|---|
| Avalonia | 11.2.3 |
| Avalonia.Desktop | 11.2.3 |
| Avalonia.Themes.Fluent | 11.2.3 |
| Avalonia.Fonts.Inter | 11.2.3 |

---