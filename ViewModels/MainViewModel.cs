using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using G4meBoost.Localization;
using G4meBoost.Models;
using G4meBoost.Services;

namespace G4meBoost.ViewModels
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        { _execute = execute; _canExecute = canExecute; }
        public RelayCommand(Action execute) : this(_ => execute()) { }
        public bool CanExecute(object? p) => _canExecute?.Invoke(p) ?? true;
        public void Execute(object? p) => _execute(p);
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

    public class CategoryInfo : INotifyPropertyChanged
    {
        private bool _isSelected;
        public string Key { get; init; } = "";
        public string DisplayName => Loc.Get(Key);
        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; PropertyChanged?.Invoke(this, new(nameof(IsSelected))); }
        }
        public void RefreshName() => PropertyChanged?.Invoke(this, new(nameof(DisplayName)));
        public event PropertyChangedEventHandler? PropertyChanged;
    }

    public class MainViewModel : INotifyPropertyChanged
    {
        private bool _isGamingMode;
        private string _statusText = "";
        private readonly List<OptimizationItem> _allItems;

        public ObservableCollection<CategoryInfo> Categories { get; } = new();
        public ObservableCollection<OptimizationItem> CurrentItems { get; } = new();

        public ICommand ToggleLangCommand { get; }
        public ICommand SelectCategoryCommand { get; }
        public ICommand CreateRestorePointCommand { get; }

        public MainViewModel()
        {
            // Initialize categories
            var catKeys = new[] { "cat_services", "cat_power", "cat_ram", "cat_network",
                                  "cat_visual", "cat_gamebar", "cat_process" };
            foreach (var key in catKeys)
                Categories.Add(new CategoryInfo { Key = key });
            Categories[0].IsSelected = true;

            // Initialize all optimization items
            _allItems = CreateItems();

            // Read current system state
            InitializeStates();

            // Update gaming mode based on current state
            _isGamingMode = _allItems.Count(i => i.IsEnabled && !i.IsOneShot) > _allItems.Count(i => !i.IsOneShot) / 2;

            // Filter to first category
            RefreshCurrentItems();

            // Commands
            ToggleLangCommand = new RelayCommand(() =>
            {
                Loc.Toggle();
                RefreshAllText();
            });

            SelectCategoryCommand = new RelayCommand(param =>
            {
                if (param is CategoryInfo cat)
                {
                    foreach (var c in Categories) c.IsSelected = false;
                    cat.IsSelected = true;
                    RefreshCurrentItems();
                }
            });

            CreateRestorePointCommand = new RelayCommand(CreateRestorePoint);

            // Listen for language changes
            Loc.LanguageChanged += RefreshAllText;

            StatusText = Loc.Get("status_ready");
        }

        // ── Properties ──

        public bool IsGamingMode
        {
            get => _isGamingMode;
            set
            {
                if (_isGamingMode == value) return;
                _isGamingMode = value;
                OnPropertyChanged(nameof(IsGamingMode));
                OnPropertyChanged(nameof(ModeTitle));
                OnPropertyChanged(nameof(ModeStatus));
                ApplyGamingMode(value);
            }
        }

        public string ModeTitle => Loc.Get("gaming_mode");

        public string ModeStatus
        {
            get
            {
                var count = _allItems.Count(i => i.IsEnabled && !i.IsOneShot);
                return _isGamingMode
                    ? $"{Loc.Get("mode_on")} • {count} {Loc.Get("optimizations_on")}"
                    : Loc.Get("mode_off");
            }
        }

        public string StatusText
        {
            get => _statusText;
            set { _statusText = value; OnPropertyChanged(nameof(StatusText)); }
        }

        public string LangButtonText => Loc.Lang == "ru" ? "EN" : "RU";
        public string RestorePointText => Loc.Get("restore_point");
        public string RequiresRebootText => Loc.Get("requires_reboot");

        // ── Methods ──

        private List<OptimizationItem> CreateItems()
        {
            return new List<OptimizationItem>
            {
                // ── Services ──
                new() { NameKey = "svc_sysmain", DescriptionKey = "svc_sysmain_d", Category = "cat_services",
                    ApplyAction = on => { if (on) ServiceOptimizer.DisableService("SysMain"); else ServiceOptimizer.EnableService("SysMain"); } },
                new() { NameKey = "svc_wsearch", DescriptionKey = "svc_wsearch_d", Category = "cat_services",
                    ApplyAction = on => { if (on) ServiceOptimizer.DisableService("WSearch"); else ServiceOptimizer.EnableService("WSearch"); } },
                new() { NameKey = "svc_diagtrack", DescriptionKey = "svc_diagtrack_d", Category = "cat_services",
                    ApplyAction = on => { if (on) ServiceOptimizer.DisableService("DiagTrack"); else ServiceOptimizer.EnableService("DiagTrack"); } },
                new() { NameKey = "svc_dmwappush", DescriptionKey = "svc_dmwappush_d", Category = "cat_services",
                    ApplyAction = on => { if (on) ServiceOptimizer.DisableService("dmwappushservice"); else ServiceOptimizer.EnableService("dmwappushservice"); } },
                new() { NameKey = "svc_wersvc", DescriptionKey = "svc_wersvc_d", Category = "cat_services",
                    ApplyAction = on => { if (on) ServiceOptimizer.DisableService("WerSvc"); else ServiceOptimizer.EnableService("WerSvc"); } },
                new() { NameKey = "svc_mapsbroker", DescriptionKey = "svc_mapsbroker_d", Category = "cat_services",
                    ApplyAction = on => { if (on) ServiceOptimizer.DisableService("MapsBroker"); else ServiceOptimizer.EnableService("MapsBroker"); } },
                new() { NameKey = "svc_fax", DescriptionKey = "svc_fax_d", Category = "cat_services",
                    ApplyAction = on => { if (on) ServiceOptimizer.DisableService("Fax"); else ServiceOptimizer.EnableService("Fax"); } },

                // ── Power ──
                new() { NameKey = "pwr_high", DescriptionKey = "pwr_high_d", Category = "cat_power",
                    ApplyAction = on => { if (on) PowerPlanOptimizer.SetHighPerformance(); else PowerPlanOptimizer.SetBalanced(); } },
                new() { NameKey = "pwr_ultimate", DescriptionKey = "pwr_ultimate_d", Category = "cat_power",
                    ApplyAction = on => { if (on) PowerPlanOptimizer.SetUltimatePerformance(); else PowerPlanOptimizer.SetBalanced(); } },

                // ── RAM (one-shot actions) ──
                new() { NameKey = "ram_standby", DescriptionKey = "ram_standby_d", Category = "cat_ram", IsOneShot = true,
                    ApplyAction = on => { if (on) { RamCleaner.ClearStandbyList(); RamCleaner.ClearWorkingSets(); } } },
                new() { NameKey = "ram_temp", DescriptionKey = "ram_temp_d", Category = "cat_ram", IsOneShot = true,
                    ApplyAction = on => { if (on) RamCleaner.ClearTempFiles(); } },
                new() { NameKey = "ram_dns", DescriptionKey = "ram_dns_d", Category = "cat_ram", IsOneShot = true,
                    ApplyAction = on => { if (on) RamCleaner.FlushDns(); } },

                // ── Network ──
                new() { NameKey = "net_nagle", DescriptionKey = "net_nagle_d", Category = "cat_network",
                    ApplyAction = on => { if (on) NetworkOptimizer.DisableNagle(); else NetworkOptimizer.EnableNagle(); } },
                new() { NameKey = "net_throttle", DescriptionKey = "net_throttle_d", Category = "cat_network",
                    ApplyAction = on => { if (on) NetworkOptimizer.DisableThrottling(); else NetworkOptimizer.EnableThrottling(); } },
                new() { NameKey = "net_tcpack", DescriptionKey = "net_tcpack_d", Category = "cat_network",
                    ApplyAction = on => { if (on) NetworkOptimizer.EnableFastAck(); else NetworkOptimizer.DisableFastAck(); } },

                // ── Visual ──
                new() { NameKey = "vis_animations", DescriptionKey = "vis_animations_d", Category = "cat_visual",
                    ApplyAction = on => { if (on) VisualEffectsOptimizer.DisableAnimations(); else VisualEffectsOptimizer.EnableAnimations(); } },
                new() { NameKey = "vis_transparency", DescriptionKey = "vis_transparency_d", Category = "cat_visual",
                    ApplyAction = on => { if (on) VisualEffectsOptimizer.DisableTransparency(); else VisualEffectsOptimizer.EnableTransparency(); } },
                new() { NameKey = "vis_shadows", DescriptionKey = "vis_shadows_d", Category = "cat_visual",
                    ApplyAction = on => { if (on) VisualEffectsOptimizer.DisableShadows(); else VisualEffectsOptimizer.EnableShadows(); } },

                // ── Game Bar ──
                new() { NameKey = "gb_bar", DescriptionKey = "gb_bar_d", Category = "cat_gamebar",
                    ApplyAction = on => { if (on) GameBarOptimizer.DisableGameBar(); else GameBarOptimizer.EnableGameBar(); } },
                new() { NameKey = "gb_dvr", DescriptionKey = "gb_dvr_d", Category = "cat_gamebar",
                    ApplyAction = on => { if (on) GameBarOptimizer.DisableGameDvr(); else GameBarOptimizer.EnableGameDvr(); } },
                new() { NameKey = "gb_fullscreen", DescriptionKey = "gb_fullscreen_d", Category = "cat_gamebar",
                    ApplyAction = on => { if (on) GameBarOptimizer.DisableFullscreenOptimizations(); else GameBarOptimizer.EnableFullscreenOptimizations(); } },
                new() { NameKey = "gb_tips", DescriptionKey = "gb_tips_d", Category = "cat_gamebar",
                    ApplyAction = on => { if (on) GameBarOptimizer.DisableTips(); else GameBarOptimizer.EnableTips(); } },

                // ── Process Priority ──
                new() { NameKey = "proc_priority", DescriptionKey = "proc_priority_d", Category = "cat_process",
                    ApplyAction = on => { if (on) ProcessPriorityManager.SetGamingPriority(); else ProcessPriorityManager.ResetPriority(); } },
                new() { NameKey = "proc_gpu", DescriptionKey = "proc_gpu_d", Category = "cat_process",
                    ApplyAction = on => { if (on) ProcessPriorityManager.SetGpuPriority(); else ProcessPriorityManager.ResetGpuPriority(); } },
                new() { NameKey = "proc_timer", DescriptionKey = "proc_timer_d", Category = "cat_process",
                    ApplyAction = on => { if (on) ProcessPriorityManager.EnableHighResTimer(); else ProcessPriorityManager.DisableHighResTimer(); } },
            };
        }

        private void InitializeStates()
        {
            foreach (var item in _allItems)
            {
                try
                {
                    bool state = item.NameKey switch
                    {
                        // Services
                        "svc_sysmain"   => ServiceOptimizer.IsServiceDisabled("SysMain"),
                        "svc_wsearch"   => ServiceOptimizer.IsServiceDisabled("WSearch"),
                        "svc_diagtrack" => ServiceOptimizer.IsServiceDisabled("DiagTrack"),
                        "svc_dmwappush" => ServiceOptimizer.IsServiceDisabled("dmwappushservice"),
                        "svc_wersvc"    => ServiceOptimizer.IsServiceDisabled("WerSvc"),
                        "svc_mapsbroker"=> ServiceOptimizer.IsServiceDisabled("MapsBroker"),
                        "svc_fax"       => ServiceOptimizer.IsServiceDisabled("Fax"),
                        // Power
                        "pwr_high"      => PowerPlanOptimizer.IsHighPerformance(),
                        "pwr_ultimate"  => PowerPlanOptimizer.IsHighPerformance(),
                        // Network
                        "net_nagle"     => NetworkOptimizer.IsNagleDisabled(),
                        "net_throttle"  => NetworkOptimizer.IsThrottlingDisabled(),
                        "net_tcpack"    => NetworkOptimizer.IsFastAckEnabled(),
                        // Visual
                        "vis_animations"   => VisualEffectsOptimizer.AreAnimationsDisabled(),
                        "vis_transparency" => VisualEffectsOptimizer.IsTransparencyDisabled(),
                        "vis_shadows"      => VisualEffectsOptimizer.AreShadowsDisabled(),
                        // Game Bar
                        "gb_bar"        => GameBarOptimizer.IsGameBarDisabled(),
                        "gb_dvr"        => GameBarOptimizer.IsGameDvrDisabled(),
                        "gb_fullscreen" => GameBarOptimizer.AreFullscreenOptimizationsDisabled(),
                        "gb_tips"       => GameBarOptimizer.AreTipsDisabled(),
                        // Process
                        "proc_priority" => ProcessPriorityManager.IsGamingPrioritySet(),
                        "proc_gpu"      => ProcessPriorityManager.IsGpuPriorityMax(),
                        "proc_timer"    => ProcessPriorityManager.IsHighResTimerEnabled(),
                        _ => false
                    };
                    item.SetEnabledSilent(state);
                }
                catch
                {
                    item.SetEnabledSilent(false);
                }
            }
        }

        private void RefreshCurrentItems()
        {
            var selectedCat = Categories.FirstOrDefault(c => c.IsSelected);
            if (selectedCat == null) return;

            CurrentItems.Clear();
            foreach (var item in _allItems.Where(i => i.Category == selectedCat.Key))
                CurrentItems.Add(item);
        }

        private void ApplyGamingMode(bool enable)
        {
            StatusText = enable ? Loc.Get("status_applying") : Loc.Get("status_reverting");

            foreach (var item in _allItems.Where(i => !i.IsOneShot))
            {
                try
                {
                    item.IsEnabled = enable;
                }
                catch { /* continue with other items */ }
            }

            // Also run one-shot cleanups when enabling
            if (enable)
            {
                try { RamCleaner.CleanAll(); } catch { }
            }

            StatusText = Loc.Get("status_ready");
            OnPropertyChanged(nameof(ModeStatus));
        }

        private void CreateRestorePoint()
        {
            StatusText = Loc.Get("restore_point") + "...";
            try
            {
                Helpers.CmdHelper.RunPowerShell(
                    "Checkpoint-Computer -Description 'G4meBoost Backup' -RestorePointType 'MODIFY_SETTINGS'");
                StatusText = Loc.Get("restore_created");
            }
            catch
            {
                StatusText = Loc.Get("restore_failed");
            }
        }

        public void RefreshAllText()
        {
            OnPropertyChanged(nameof(ModeTitle));
            OnPropertyChanged(nameof(ModeStatus));
            OnPropertyChanged(nameof(LangButtonText));
            OnPropertyChanged(nameof(RestorePointText));
            OnPropertyChanged(nameof(RequiresRebootText));
            OnPropertyChanged(nameof(StatusText));

            foreach (var cat in Categories)
                cat.RefreshName();

            // Refresh items (they bind NameKey/DescriptionKey via converter)
            RefreshCurrentItems();
        }

        public int CountEnabled() => _allItems.Count(i => i.IsEnabled && !i.IsOneShot);

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
