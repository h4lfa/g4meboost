# G4meB∞st 🚀
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
```


# RU G4meB∞st 🚀
**Ультра-лёгкий оптимизатор Windows 11 для гейминга и повседневной жизни**

*Разработчик:* **[h4lfa](https://github.com/h4lfa)**

---

## ⚡ О проекте
**G4meB∞st** создан специально для того, чтобы выжать максимум FPS и стабильности фреймтайма в играх, сохранив систему полностью пригодной для повседневной работы, учебы и серфинга.

- 🪶 **Ультра-лёгкий**: нативный C# WPF без тяжелых веб-фреймворков (Electron/Webview).
- 🎨 **Современный UI**: стильная тёмная тема в духе Windows 11 Fluent Design, кастомный заголовок окна, плавные переключатели.
- 🌐 **Двуязычный**: мгновенное переключение между русским и английским языком в один клик.
- 🔄 **Два режима**:
  - **Быстрый режим (One-Click)**: главный тумблер активирует/деактивирует весь комплекс игровых настроек.
  - **Детальная настройка**: вкладки по категориям с независимым контролем каждого параметра.
- 🛡️ **Безопасность**: создание точки восстановления Windows прямо из приложения перед применением твиков.

---

## 🛠️ Категории и оптимизации

### 1. ⚙️ Службы Windows (Services)
- **SysMain (Superfetch)**: освобождает ОЗУ от агрессивного предкеширования приложений.
- **Windows Search**: снижает нагрузку на диск и процессор во время игры за счёт приостановки индексации.
- **Телеметрия (DiagTrack)**: отключение служб фонового сбора диагностических данных Microsoft.
- **WAP Push / WerSvc**: отключение очередей push-сообщений и сбора отчетов об ошибках.
- **MapsBroker / Fax**: отключение неиспользуемых фоновых служб.

### 2. ⚡ План питания (Power Plan)
- **Высокая / Максимальная производительность (Ultimate Performance)**:
  - Фиксация максимальной тактовой частоты ядер CPU.
  - Отключение парковки ядер и задержек энергосбережения.

### 3. 🧠 Очистка RAM и диска (RAM & Storage)
- **Очистка Standby List**: освобождение зарезервированной Windows памяти через системный вызов `NtSetSystemInformation`.
- **Сброс Working Sets**: выгрузка неиспользуемых страниц запущенных фоновых процессов.
- **Очистка временных файлов**: удаление мусора из `%TEMP%` и `Windows\Temp`.
- **Flush DNS**: очистка системного кеша сопоставления доменов.

### 4. 🌐 Сетевые оптимизации (Network & Latency)
- **Отключение алгоритма Nagle (`TcpNoDelay`)**: убирает буферизацию мелких пакетов, снижая сетевой инпут-лаг в онлайн-играх.
- **TcpAckFrequency = 1**: мгновенная отправка подтверждений TCP без ожидания задержки (ACK delay).
- **Снятие ограничения NetworkThrottlingIndex**: отключение троттлинга сетевого адаптера службой Windows Multimedia.

### 5. 👁️ Визуальные эффекты (Visual Effects)
- Отключение ресурсоемких анимаций меню, окон и выпадающих списков.
- Отключение прозрачности (Acrylic/Mica) на слабых GPU.
- Отключение теней окон.

### 6. 🎮 Xbox Game Bar & DVR
- Отключение оверлея Xbox Game Bar (экономит ресурсы GPU/RAM).
- Отключение фоновой видеозаписи Game DVR.
- Настройка оптимизации полноэкранных игр (`GameDVR_FSEBehaviorMode`).

### 7. 🎯 Приоритеты процессов (Multimedia Scheduler)
- Настройка мультимедийного планировщика Windows (`SystemProfile\Tasks\Games`):
  - Приоритет CPU: High (6).
  - Приоритет GPU: Maximum (8).
  - Увеличение чувствительности таймера системы.

---

## 🚀 Запуск

Запустить приложение можно через скомпилированный бинарный файл:
```cmd
G4meBoost.exe
```
*(Приложению требуются права администратора для изменения системных параметров и служб, манифест запрашивает их автоматически).*
