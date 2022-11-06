using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Manager.ViewModels
{
    public partial class ProcessListItemViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(
            nameof(ProcessName),
            nameof(Id),
            nameof(WorkingSet64MB))]
        private Process _process;

        public string ProcessName => _process.ProcessName;
        public int Id => _process.Id;
        public float WorkingSet64MB => (float)_process.WorkingSet64 / (1024 ^ 2);

        public ProcessListItemViewModel(Process process)
        {
            _process = process;
        }
    }
}
