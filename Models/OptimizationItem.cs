using System;
using System.ComponentModel;

namespace G4meBoost.Models
{
    public class OptimizationItem : INotifyPropertyChanged
    {
        private bool _isEnabled;
        private string _status = "";

        public string NameKey { get; init; } = "";
        public string DescriptionKey { get; init; } = "";
        public string Category { get; init; } = "";
        public bool IsOneShot { get; init; } // true for actions like "clear temp", not toggles

        public Action<bool>? ApplyAction { get; init; }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled == value) return;
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));

                try
                {
                    ApplyAction?.Invoke(value);
                    Status = "";
                }
                catch (Exception ex)
                {
                    Status = ex.Message;
                }
            }
        }

        /// <summary>Sets IsEnabled without triggering ApplyAction (for initialization).</summary>
        public void SetEnabledSilent(bool value)
        {
            _isEnabled = value;
            OnPropertyChanged(nameof(IsEnabled));
        }

        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
