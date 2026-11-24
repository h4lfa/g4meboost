# EN G4meB∞st 🚀
**Ultra-lightweight Windows 11 optimizer for gaming and everyday use**

---

## ⚡ About the Project
**G4meB∞st** is specifically designed to squeeze out maximum FPS and frametime stability in games while keeping your system fully usable for everyday work, study, and web browsing.

- 🪶 **Ultra-lightweight**: native C# WPF with no heavy web frameworks (Electron/Webview).
- 🎨 **Modern UI**: sleek dark theme inspired by Windows 11 Fluent Design, custom window title bar, and smooth toggles.
- 🌐 **Bilingual**: instant one-click switching between Russian and English.
- 🔄 **Two Modes**:
  - **Quick Mode (One-Click)**: a master toggle that enables/disables the entire suite of gaming optimizations.
  - **Detailed Configuration**: categorized tabs with independent control over every setting.
- 🛡️ **Safety**: create a Windows Restore Point directly from the app before applying any tweaks.

---

## 🛠️ Categories & Optimizations

### 1. ⚙️ Windows Services
- **SysMain (Superfetch)**: frees up RAM from aggressive application pre-caching.
- **Windows Search**: reduces disk and CPU load during gaming by pausing indexing.
- **Telemetry (DiagTrack)**: disables Microsoft's background diagnostic data collection services.
- **WAP Push / WerSvc**: disables push message routing queues and error reporting.
- **MapsBroker / Fax**: disables unused background services.

### 2. ⚡ Power Plan
- **High / Ultimate Performance**:
  - Locks CPU cores to their maximum clock speed.
  - Disables core parking and power-saving delays.

### 3. 🧠 RAM & Storage Cleanup
- **Clear Standby List**: frees Windows-reserved memory via the `NtSetSystemInformation` system call.
- **Empty Working Sets**: unloads unused memory pages from running background processes.
- **Temporary Files Cleanup**: removes junk files from `%TEMP%` and `Windows\Temp`.
- **Flush DNS**: clears the system DNS resolver cache.

### 4. 🌐 Network & Latency Optimizations
- **Disable Nagle's Algorithm (`TcpNoDelay`)**: removes buffering of small packets, reducing network input lag in online games.
- **TcpAckFrequency = 1**: sends TCP acknowledgments immediately without waiting for the ACK delay.
- **Remove NetworkThrottlingIndex Limit**: disables network adapter throttling by the Windows Multimedia service.

### 5. 👁️ Visual Effects
- Disables resource-intensive animations for menus, windows, and dropdown lists.
- Disables transparency effects (Acrylic/Mica) on low-end GPUs.
- Disables window shadows.

### 6. 🎮 Xbox Game Bar & DVR
- Disables the Xbox Game Bar overlay (saves GPU/RAM resources).
- Disables background Game DVR video recording.
- Configures fullscreen optimizations (`GameDVR_FSEBehaviorMode`).

### 7. 🎯 Process Priorities (Multimedia Scheduler)
- Configures the Windows Multimedia Class Scheduler (`SystemProfile\Tasks\Games`):
  - CPU Priority: High (6).
  - GPU Priority: Maximum (8).
  - Increases system timer responsiveness.

---

## 🚀 Getting Started

You can launch the application using the compiled binary:
```cmd
G4meBoost.exe
