using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
        private ProcessListItemViewModel? _selectedProcessListItemViewModel;

        [ObservableProperty]
        private ProcessPriorityClass _selectedPriorityClass;

        private Process? SelectedProcess => SelectedProcessListItemViewModel?.Process;

        public string? ProcessName
        {
            get { try { return SelectedProcess?.ProcessName; } catch (Exception) { return ""; } }
        }
        public int? Id
        {
            get { try { return SelectedProcess?.Id; } catch (Exception) { return null; } }
        }
        public string? SessionId
        {
            get { try { return SelectedProcess?.SessionId.ToString(); } catch (Exception) { return ""; } }
        }
        public string? BasePriority
        {
            get { try { return SelectedProcess?.BasePriority.ToString(); } catch (Exception) { return ""; } }
        }
        public string? ProcessPriorityClass
        {
            get { try { return SelectedProcess?.PriorityClass.ToString(); } catch (Exception) { return ""; } }
        }
        public string? TotalProcessorTime
        {
            get { try { return SelectedProcess?.TotalProcessorTime.ToString(); } catch (Exception) { return ""; } }
        }
        public string? WorkingSet64
        {
            get { try { return SelectedProcess?.WorkingSet64.ToString(); } catch (Exception) { return ""; } }
        }
        public string? ThreadsCount
        {
            get { try { return SelectedProcess?.Threads.Count.ToString(); } catch (Exception) { return ""; } }
        }
        public ProcessThreadCollection? Threads
        {
            get { try { return SelectedProcess?.Threads; } catch (Exception) { return null; } }
        }

        public bool IsRefreshing => _timer.IsEnabled;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ProcessListItemViewModels))]
        private string? _nameFilter;

        [ObservableProperty]
        private string? _refreshRate;

        public ObservableCollection<ProcessListItemViewModel> ProcessListItemViewModels { get; private set; }

        private DispatcherTimer _timer;


        public MainWindowViewModel(double refreshInterval = 1)
        {
            ProcessListItemViewModels = new ObservableCollection<ProcessListItemViewModel>();
            RefreshProcessList();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(refreshInterval) };
            _timer.Tick += OnTimerTick;
            _timer.Start();
        }


        private void RefreshProcessList()
        {
            var newProcesses = Process.GetProcesses();
            var oldProcessIds = ProcessListItemViewModels.Select(p => p.Id).ToList();

            //REMOVED PROCESSES
            foreach (var oldProcessId in oldProcessIds)
            {
                if (!newProcesses.Any(newProcess => newProcess.Id == oldProcessId))
                {
                    var processItem = ProcessListItemViewModels.First(item => item.Id == oldProcessId);
                    ProcessListItemViewModels.Remove(processItem);
                }
            }

            //UPDATED PROCESSES
            foreach (var oldProcessId in oldProcessIds)
            {
                if (newProcesses.Any(newProcess => newProcess.Id == oldProcessId))
                {
                    var processItem = ProcessListItemViewModels.First(item => item.Id == oldProcessId);
                    processItem.Process = newProcesses.First(newProcess => newProcess.Id == processItem.Id);
                }
            }

            //NEW PROCESSES
            foreach (var newProcess in newProcesses)
            {
                if (!ProcessListItemViewModels.Any(processItem => processItem.Id == newProcess.Id))
                    ProcessListItemViewModels.Add(new ProcessListItemViewModel(newProcess));
            }

            //NEW FILTER
            if (NameFilter != null)
            {
                foreach (var newProcess in newProcesses)
                {
                    if (!newProcess.ProcessName.StartsWith(NameFilter, StringComparison.OrdinalIgnoreCase))
                    {
                        var item = ProcessListItemViewModels.First(item => item.Id == newProcess.Id);
                        ProcessListItemViewModels.Remove(item);
                    }
                }
            }


            OnPropertyChanged(nameof(ProcessListItemViewModels));
            OnPropertyChanged(nameof(SelectedProcess));
            OnPropertyChanged(nameof(ProcessName));
            OnPropertyChanged(nameof(Id));
            OnPropertyChanged(nameof(SessionId));
            OnPropertyChanged(nameof(BasePriority));
            OnPropertyChanged(nameof(ProcessPriorityClass));
            OnPropertyChanged(nameof(TotalProcessorTime));
            OnPropertyChanged(nameof(WorkingSet64));
            OnPropertyChanged(nameof(ThreadsCount));
            OnPropertyChanged(nameof(Threads));
        }

        private void OnTimerTick(object? sender, EventArgs? e)
        {
            RefreshProcessList();
        }

        [RelayCommand]
        private void Refresh()
        {
            if (!_timer.IsEnabled)
            {
                _timer.Start();
                OnPropertyChanged("IsRefreshing");
            }
            RefreshProcessList();
            if (RefreshRate != null)
                _timer.Interval = TimeSpan.FromSeconds(int.Parse(RefreshRate));
            Console.WriteLine();
        }

        [RelayCommand]
        private void StopRefreshing()
        {
            if (_timer.IsEnabled)
            {
                _timer.Stop();
                OnPropertyChanged("IsRefreshing");
            }
        }

        [RelayCommand]
        private void Kill()
        {
            if (SelectedProcess != null && _selectedProcessListItemViewModel != null)
            {
                SelectedProcess.Kill();
                ProcessListItemViewModels.Remove(_selectedProcessListItemViewModel);
            }
        }

        [RelayCommand]
        private void ChangePriority()
        {
            try
            {
                if (SelectedProcess != null)
                    SelectedProcess.PriorityClass = SelectedPriorityClass;
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
