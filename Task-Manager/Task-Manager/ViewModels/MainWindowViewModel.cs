using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace Task_Manager.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(
            nameof(SelectedProcess),
            nameof(ProcessName),
            nameof(Id),
            nameof(SessionId),
            nameof(BasePriority),
            nameof(ProcessPriorityClass),
            nameof(TotalProcessorTime),
            nameof(WorkingSet64),
            nameof(ThreadsCount),
            nameof(Threads))]
        private ProcessListItemViewModel _selectedProcessListItemViewModel;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SelectedProcess))]
        private ProcessPriorityClass _selectedPriorityClass;

        private Process? SelectedProcess => SelectedProcessListItemViewModel?.Process;

        public string? ProcessName =>SelectedProcess?.ProcessName;
        public int? Id => SelectedProcess?.Id;
        public int? SessionId => SelectedProcess?.SessionId;
        public int? BasePriority => SelectedProcess?.BasePriority;
        public ProcessPriorityClass? ProcessPriorityClass => SelectedProcess?.PriorityClass;
        public TimeSpan? TotalProcessorTime => SelectedProcess?.TotalProcessorTime;
        public long? WorkingSet64 => SelectedProcess?.WorkingSet64;
        public int? ThreadsCount => SelectedProcess?.Threads.Count;
        public ProcessThreadCollection? Threads => SelectedProcess?.Threads;

        public bool IsRefreshing => _timer.IsEnabled;

        public ObservableCollection<ProcessListItemViewModel> ProcessListItemViewModels { get; private set; }

        private DispatcherTimer _timer;


        public MainWindowViewModel(double refreshInterval=1)
        {
            ProcessListItemViewModels = new ObservableCollection<ProcessListItemViewModel>();
            RefreshProcessList();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(refreshInterval) };
            _timer.Tick += RefreshProcessList;
            _timer.Start();
        }

        private void RefreshProcessList()
        {
            ProcessListItemViewModels.Clear();
            foreach (var process in Process.GetProcesses())
            {
                ProcessListItemViewModels.Add(new ProcessListItemViewModel(process));
            }
        }

        private void RefreshProcessList(object sender, EventArgs e)
        {
            ProcessListItemViewModels.Clear();
            foreach (var process in Process.GetProcesses())
            {
                ProcessListItemViewModels.Add(new ProcessListItemViewModel(process));
            }
        }

        [RelayCommand]
        private void Refresh()
        {
            if (!_timer.IsEnabled)
                _timer.Start();
            RefreshProcessList();
            OnPropertyChanged("IsRefreshing");
        }

        [RelayCommand]
        private void StopRefreshing()
        {
            if (_timer.IsEnabled)
                _timer.Stop();
            OnPropertyChanged("IsRefreshing");
        }

        [RelayCommand]
        private void Kill()
        {
            if (SelectedProcess != null)
            {
                SelectedProcess.Kill();
                ProcessListItemViewModels.Remove(_selectedProcessListItemViewModel);
            }
        }

        [RelayCommand]
        private void ChangePriority()
        {
            if (SelectedProcess != null)
                MessageBox.Show($"ChangePriority: {SelectedPriorityClass}");
            SelectedProcess.PriorityClass = SelectedPriorityClass;
            //OnPropertyChanged("SelectedProcess");
        }
    }
}
