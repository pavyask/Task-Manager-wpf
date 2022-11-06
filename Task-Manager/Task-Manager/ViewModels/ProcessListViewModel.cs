using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Task_Manager.Stores;

namespace Task_Manager.ViewModels
{
    public class ProcessListViewModel : ViewModelBase
    {
        private readonly SelectedProcessStore _selectedProcessStore;

        //private readonly ObservableCollection<ProcessListItemViewModel> _processListItemViewModels;
        public ObservableCollection<ProcessListItemViewModel> ProcessListItemViewModels { get; private set; }

        private ProcessListItemViewModel _selectedProcessListItemViewModel;
        public ProcessListItemViewModel SelectedProcessListItemViewModel
        {
            get => _selectedProcessListItemViewModel;
            set
            {
                _selectedProcessListItemViewModel = value;
                OnPropertyChanged(nameof(SelectedProcessListItemViewModel));
                _selectedProcessStore.SelectedProcess = _selectedProcessListItemViewModel.Process;
            }
        }

        public ICommand RefreshCommand { get; }

        public ICommand StopRefreshingCommand { get; }

        public ProcessListViewModel(SelectedProcessStore selectedProcessStore)
        {
            _selectedProcessStore = selectedProcessStore;
            ProcessListItemViewModels = new ObservableCollection<ProcessListItemViewModel>();
            RefreshProcessList();
        }

        private void RefreshProcessList()
        {
            ProcessListItemViewModels.Clear();
            foreach (var process in Process.GetProcesses())
            {
                ProcessListItemViewModels.Add(new ProcessListItemViewModel(process));
            }
        }
    }
}
