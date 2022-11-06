using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Manager.Stores;

namespace Task_Manager.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ProcessListViewModel ProcessListViewModel { get; }
        public ProcessDetailsViewModel ProcessDetailsViewModel { get; }

        public MainWindowViewModel(SelectedProcessStore selectedProcessStore)
        {
            ProcessListViewModel = new ProcessListViewModel(selectedProcessStore);
            ProcessDetailsViewModel = new ProcessDetailsViewModel(selectedProcessStore);
        }

    }
}
