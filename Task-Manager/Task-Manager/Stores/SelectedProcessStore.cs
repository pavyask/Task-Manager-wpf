using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Manager.Stores
{
    public class SelectedProcessStore
    {
        private Process _selectedProcess;

        public Process SelectedProcess
        {
            get => _selectedProcess;
            set
            {
                _selectedProcess = value;
                SelectedProcessChanged?.Invoke();
            }
        }

        public event Action SelectedProcessChanged;
    }
}
