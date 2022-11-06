using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using Task_Manager.ViewModels;

namespace Task_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowViewModel();
            InitializeComponent();
            cmbPriorities.ItemsSource = Enum.GetValues(typeof(ProcessPriorityClass));
            cmbPriorities.SelectedIndex = 0;
        }
    }
}
