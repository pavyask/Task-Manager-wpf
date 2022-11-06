using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Manager.Stores;

namespace Task_Manager.ViewModels
{
    public class ProcessDetailsViewModel : ViewModelBase
    {
        private readonly SelectedProcessStore _selectedProcessStore;
        private Process SelectedProcess => _selectedProcessStore.SelectedProcess;

        public bool HasSelectedProecss => SelectedProcess != null;

        public string ProcessName => SelectedProcess.ProcessName;
        public int Id => SelectedProcess.Id;
        public int SessionId => SelectedProcess.SessionId;
        public int BasePriority => SelectedProcess.BasePriority;
        public ProcessPriorityClass ProcessPriorityClass => SelectedProcess.PriorityClass;

        public TimeSpan TotalProcessorTime => SelectedProcess.TotalProcessorTime;
        public long WorkingSet64 => SelectedProcess.WorkingSet64;

        public int ThreadsCount => SelectedProcess.Threads.Count;
        public ProcessThreadCollection Threads => SelectedProcess.Threads;

        public ProcessDetailsViewModel(SelectedProcessStore selectedProcessStore)
        {
            _selectedProcessStore = selectedProcessStore;
            _selectedProcessStore.SelectedProcessChanged += SelectedProcessStore_SelectedProcessChanged;
        }

        private void SelectedProcessStore_SelectedProcessChanged()
        {
            OnPropertyChanged(nameof(HasSelectedProecss));
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
    }
}
