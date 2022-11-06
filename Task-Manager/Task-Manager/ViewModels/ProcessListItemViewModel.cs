using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Manager.ViewModels
{
    public class ProcessListItemViewModel : ViewModelBase
    {
        public Process Process { get; }

        public string ProcessName => Process.ProcessName;
        public int Id => Process.Id;
        public float WorkingSet64MB => (float)Process.WorkingSet64 / (1024 ^ 2);

        public ProcessListItemViewModel(Process process)
        {
            Process = process;
        }
    }
}
