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
            nameof(WorkingSet64MB),
            nameof(TotalProcessorTime))]
        private Process _process;

        public string? ProcessName
        {
            get { try { return _process.ProcessName; } catch (Exception) { return ""; } }
        }
        public int? Id
        {
            get { try { return _process.Id; } catch (Exception) { return null; } }
        }
        public string? WorkingSet64MB
        {
            get { try { return ((float)_process.WorkingSet64 / (1024 ^ 2)).ToString(); } catch (Exception) { return ""; } }
        }

        public string? TotalProcessorTime
        {
            get { try { return _process?.TotalProcessorTime.ToString(); } catch (Exception) { return ""; } }
        }

        public ProcessListItemViewModel(Process process)
        {
            _process = process;
        }
    }
}
