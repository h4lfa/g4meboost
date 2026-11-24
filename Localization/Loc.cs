using System;
using System.Collections.Generic;

namespace G4meBoost.Localization
{
    public static class Loc
    {
        public static event Action? LanguageChanged;

        private static string _lang = "ru";
        public static string Lang
        {
            get => _lang;
            set
            {
                if (_lang == value) return;
                _lang = value;
                LanguageChanged?.Invoke();
            }
        }

        public static void Toggle() => Lang = Lang == "ru" ? "en" : "ru";

        private static readonly Dictionary<string, Dictionary<string, string>> S = new()
        {
            // ── App ──
            ["app_title"]           = new() { ["ru"] = "G4meB∞st",                     ["en"] = "G4meB∞st" },
            ["gaming_mode"]         = new() { ["ru"] = "ИГРОВОЙ РЕЖИМ",                ["en"] = "GAMING MODE" },
            ["mode_on"]             = new() { ["ru"] = "Активен",                      ["en"] = "Active" },
            ["mode_off"]            = new() { ["ru"] = "Неактивен",                    ["en"] = "Inactive" },
            ["optimizations_on"]    = new() { ["ru"] = "оптимизаций применено",        ["en"] = "optimizations applied" },
            ["restore_point"]       = new() { ["ru"] = "Создать точку восстановления", ["en"] = "Create restore point" },
            ["restore_created"]     = new() { ["ru"] = "Точка создана ✓",              ["en"] = "Restore point created ✓" },
            ["restore_failed"]      = new() { ["ru"] = "Ошибка создания точки",        ["en"] = "Failed to create restore point" },
            ["apply_all"]           = new() { ["ru"] = "Применить всё",                ["en"] = "Apply all" },
            ["revert_all"]          = new() { ["ru"] = "Отменить всё",                 ["en"] = "Revert all" },
            ["status_ready"]        = new() { ["ru"] = "Готов к работе",               ["en"] = "Ready" },
            ["status_applying"]     = new() { ["ru"] = "Применение...",                ["en"] = "Applying..." },
            ["status_reverting"]    = new() { ["ru"] = "Восстановление...",            ["en"] = "Reverting..." },
            ["requires_reboot"]     = new() { ["ru"] = "Некоторые изменения вступят в силу после перезагрузки", ["en"] = "Some changes require a reboot" },

            // ── Categories ──
            ["cat_services"]  = new() { ["ru"] = "Службы",      ["en"] = "Services" },
            ["cat_power"]     = new() { ["ru"] = "Питание",     ["en"] = "Power" },
            ["cat_ram"]       = new() { ["ru"] = "RAM",         ["en"] = "RAM" },
            ["cat_network"]   = new() { ["ru"] = "Сеть",        ["en"] = "Network" },
            ["cat_visual"]    = new() { ["ru"] = "Визуал",      ["en"] = "Visual" },
            ["cat_startup"]   = new() { ["ru"] = "Автозагрузка",["en"] = "Startup" },
            ["cat_gamebar"]   = new() { ["ru"] = "Игры",        ["en"] = "Games" },
            ["cat_process"]   = new() { ["ru"] = "Процессы",    ["en"] = "Processes" },

            // ── Services ──
            ["svc_sysmain"]       = new() { ["ru"] = "SysMain (Superfetch)",              ["en"] = "SysMain (Superfetch)" },
            ["svc_sysmain_d"]     = new() { ["ru"] = "Кеширование приложений • тратит RAM", ["en"] = "App caching • uses RAM" },
            ["svc_wsearch"]       = new() { ["ru"] = "Windows Search",                    ["en"] = "Windows Search" },
            ["svc_wsearch_d"]     = new() { ["ru"] = "Индексирование файлов • нагрузка на диск", ["en"] = "File indexing • disk I/O" },
            ["svc_diagtrack"]     = new() { ["ru"] = "Телеметрия (DiagTrack)",            ["en"] = "Telemetry (DiagTrack)" },
            ["svc_diagtrack_d"]   = new() { ["ru"] = "Сбор данных Microsoft • CPU и сеть", ["en"] = "Microsoft data collection • CPU & network" },
            ["svc_dmwappush"]     = new() { ["ru"] = "Push-уведомления WAP",              ["en"] = "WAP Push Messaging" },
            ["svc_dmwappush_d"]   = new() { ["ru"] = "Маршрутизация push-сообщений",     ["en"] = "Push message routing" },
            ["svc_wersvc"]        = new() { ["ru"] = "Отчёты об ошибках",                 ["en"] = "Error Reporting" },
            ["svc_wersvc_d"]      = new() { ["ru"] = "Сбор и отправка отчётов об ошибках", ["en"] = "Collects and sends error reports" },
            ["svc_mapsbroker"]    = new() { ["ru"] = "Менеджер карт",                     ["en"] = "Maps Manager" },
            ["svc_mapsbroker_d"]  = new() { ["ru"] = "Загруженные карты • фоновые обновления", ["en"] = "Downloaded maps • background updates" },
            ["svc_fax"]           = new() { ["ru"] = "Факс",                              ["en"] = "Fax" },
            ["svc_fax_d"]         = new() { ["ru"] = "Служба отправки факсов",            ["en"] = "Fax sending service" },

            // ── Power ──
            ["pwr_high"]       = new() { ["ru"] = "Высокая производительность",           ["en"] = "High Performance" },
            ["pwr_high_d"]     = new() { ["ru"] = "Максимальная частота CPU, без энергосбережения", ["en"] = "Max CPU frequency, no power saving" },
            ["pwr_ultimate"]   = new() { ["ru"] = "Максимальная производительность",      ["en"] = "Ultimate Performance" },
            ["pwr_ultimate_d"] = new() { ["ru"] = "Убирает все задержки управления питанием", ["en"] = "Removes all power management latencies" },

            // ── RAM ──
            ["ram_standby"]    = new() { ["ru"] = "Очистить standby-лист",                ["en"] = "Clear standby list" },
            ["ram_standby_d"]  = new() { ["ru"] = "Освобождает зарезервированную память",  ["en"] = "Frees reserved memory" },
            ["ram_temp"]       = new() { ["ru"] = "Очистить временные файлы",             ["en"] = "Clear temp files" },
            ["ram_temp_d"]     = new() { ["ru"] = "Удаляет файлы из %TEMP% и Windows\\Temp", ["en"] = "Deletes files from %TEMP% and Windows\\Temp" },
            ["ram_dns"]        = new() { ["ru"] = "Очистить кеш DNS",                     ["en"] = "Flush DNS cache" },
            ["ram_dns_d"]      = new() { ["ru"] = "Сбрасывает кеш DNS-запросов",          ["en"] = "Resets DNS query cache" },

            // ── Network ──
            ["net_nagle"]      = new() { ["ru"] = "Отключить Nagle",                      ["en"] = "Disable Nagle's algorithm" },
            ["net_nagle_d"]    = new() { ["ru"] = "Убирает задержку отправки мелких пакетов", ["en"] = "Removes small packet send delay" },
            ["net_throttle"]   = new() { ["ru"] = "Отключить троттлинг сети",             ["en"] = "Disable network throttling" },
            ["net_throttle_d"] = new() { ["ru"] = "Убирает ограничение пропускной способности для мультимедиа", ["en"] = "Removes multimedia bandwidth throttling" },
            ["net_tcpack"]     = new() { ["ru"] = "Быстрое TCP-подтверждение",            ["en"] = "Fast TCP acknowledgement" },
            ["net_tcpack_d"]   = new() { ["ru"] = "Немедленное подтверждение TCP-пакетов", ["en"] = "Immediate TCP packet acknowledgement" },

            // ── Visual ──
            ["vis_animations"]    = new() { ["ru"] = "Отключить анимации",                ["en"] = "Disable animations" },
            ["vis_animations_d"]  = new() { ["ru"] = "Анимации окон, меню, подсказок",    ["en"] = "Window, menu, tooltip animations" },
            ["vis_transparency"]  = new() { ["ru"] = "Отключить прозрачность",            ["en"] = "Disable transparency" },
            ["vis_transparency_d"]= new() { ["ru"] = "Эффект прозрачности панели задач и окон", ["en"] = "Taskbar and window transparency effects" },
            ["vis_shadows"]       = new() { ["ru"] = "Отключить тени окон",               ["en"] = "Disable window shadows" },
            ["vis_shadows_d"]     = new() { ["ru"] = "Тени под окнами рабочего стола",    ["en"] = "Desktop window drop shadows" },

            // ── Game Bar ──
            ["gb_bar"]         = new() { ["ru"] = "Отключить Xbox Game Bar",              ["en"] = "Disable Xbox Game Bar" },
            ["gb_bar_d"]       = new() { ["ru"] = "Оверлей Xbox Game Bar • потребляет ресурсы", ["en"] = "Xbox Game Bar overlay • consumes resources" },
            ["gb_dvr"]         = new() { ["ru"] = "Отключить Game DVR",                   ["en"] = "Disable Game DVR" },
            ["gb_dvr_d"]       = new() { ["ru"] = "Фоновая запись геймплея",              ["en"] = "Background gameplay recording" },
            ["gb_fullscreen"]  = new() { ["ru"] = "Отключить оптимизацию полноэкранных",  ["en"] = "Disable fullscreen optimizations" },
            ["gb_fullscreen_d"]= new() { ["ru"] = "Windows-оптимизации для полноэкранных приложений", ["en"] = "Windows fullscreen app optimizations" },
            ["gb_tips"]        = new() { ["ru"] = "Отключить подсказки Game Bar",         ["en"] = "Disable Game Bar tips" },
            ["gb_tips_d"]      = new() { ["ru"] = "Всплывающие подсказки при запуске игр", ["en"] = "Pop-up tips when launching games" },

            // ── Process ──
            ["proc_priority"]     = new() { ["ru"] = "Игровой приоритет CPU",             ["en"] = "Gaming CPU priority" },
            ["proc_priority_d"]   = new() { ["ru"] = "Повышает приоритет игровых процессов в планировщике", ["en"] = "Raises gaming process priority in scheduler" },
            ["proc_gpu"]          = new() { ["ru"] = "Приоритет GPU",                     ["en"] = "GPU priority" },
            ["proc_gpu_d"]        = new() { ["ru"] = "Максимальный приоритет GPU для игр", ["en"] = "Maximum GPU priority for games" },
            ["proc_timer"]        = new() { ["ru"] = "Высокое разрешение таймера",        ["en"] = "High resolution timer" },
            ["proc_timer_d"]      = new() { ["ru"] = "Повышает точность системного таймера", ["en"] = "Increases system timer precision" },
        };

        public static string Get(string key) =>
            S.TryGetValue(key, out var d) && d.TryGetValue(_lang, out var s) ? s : key;
    }
}
