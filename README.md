# Home Automation Remote Control
## Avalonia C# Desktop App — VS Code Setup

### Prerequisites
- .NET 8 SDK → https://dotnet.microsoft.com/download
- VS Code + C# Dev Kit extension

---

### Run Locally (VS Code)

```bash
cd HomeAutomation
dotnet restore
dotnet run
```

---

### Run with Docker

```bash
# Build image
docker build -t home-remote .

# Run (Linux — needs X11 forwarding or VNC)
docker run -e DISPLAY=$DISPLAY -v /tmp/.X11-unix:/tmp/.X11-unix home-remote
```

---

### Project Structure

```
HomeAutomation/
├── Program.cs               # Entry point
├── App.axaml / .cs          # Avalonia application
├── MainWindow.axaml         # Remote control UI (7 slots + UNDO)
├── MainWindow.axaml.cs      # UI logic — no Command pattern
├── RemoteControl.cs         # 7-slot remote, direct device calls, undo tracking
└── Devices/
    ├── IDevice.cs           # Interface all devices implement
    ├── ApplianceControl.cs  # Abstract base (on/off/status)
    └── Devices.cs           # All 13 devices from UML diagram:
                             #   Light, CeilingLight, OutdoorLight,
                             #   GardenLight, TV, Stereo, CeilingFan,
                             #   GarageDoor, Hottub, FaucetControl,
                             #   Thermostat, SecurityControl, Sprinkler
```

---

### How It Works

- **7 slots** — use the ComboBox in each row to assign any device
- **ON / OFF** buttons call the device's `On()` / `Off()` directly (no Command pattern)
- **UNDO** reverses the most recent button press by calling the opposite method
- The **Activity Log** (right panel) shows every action with timestamp

---

### Dockerfile (optional)

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["./HomeAutomation"]
```
