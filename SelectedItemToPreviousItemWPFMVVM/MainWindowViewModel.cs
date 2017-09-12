using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace SelectedItemToPreviousItemWPFMVVM
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Constructor
        private bool _isControlLoaded { get; set; }
        public MainWindowViewModel()
        {
            InitializeCommands();

            Numbers = new ObservableCollection<SelectedItemToPreviousItemWPFMVVM.Numbers>();
            Numbers.Add(new SelectedItemToPreviousItemWPFMVVM.Numbers { Name = "One", Value = 1 });
            Numbers.Add(new SelectedItemToPreviousItemWPFMVVM.Numbers { Name = "Two", Value = 2 });
            Numbers.Add(new SelectedItemToPreviousItemWPFMVVM.Numbers { Name = "Three", Value = 3 });
            Numbers.Add(new SelectedItemToPreviousItemWPFMVVM.Numbers { Name = "Four", Value = 4 });
            Numbers.Add(new SelectedItemToPreviousItemWPFMVVM.Numbers { Name = "Five", Value = 5 });
        }
        #endregion

        #region ICommand
        public ICommand LoadedCommand { get; set; }
        private void InitializeCommands()
        {
            LoadedCommand = new CustomCommand(OnLoaded, CanExecuteOnLoaded);
        }

        private bool CanExecuteOnLoaded(object obj) => true;

        private void OnLoaded(object obj)
        {
            _isControlLoaded = true;
        }
        #endregion

        #region Properties
        private ObservableCollection<Numbers> _numbers;

        public ObservableCollection<Numbers> Numbers
        {
            get { return _numbers; }
            set { _numbers = value; RaisePropertyChanged(nameof(Numbers)); }
        }

        private Numbers _selectedNumber;

        public Numbers SelectedNumber
        {
            get { return _selectedNumber; }
            set
            {
                if (_isControlLoaded)
                {
                    var previousValue = _selectedNumber;

                    if (value == _selectedNumber)
                        return;

                    _selectedNumber = value;

                    if (MessageBox.Show("Do you want to change to the Selected Item?", "Resetting ComboBox", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                    {
                        Application.Current.Dispatcher.BeginInvoke(
                        new Action(() =>
                        {
                            _selectedNumber = previousValue;
                            RaisePropertyChanged(nameof(SelectedNumber));
                        }),
                        DispatcherPriority.Normal, null);

                        return;
                    }

                    RaisePropertyChanged(nameof(SelectedNumber));
                }
                else
                {
                    if (value == _selectedNumber)
                        return;

                    _selectedNumber = value;
                    RaisePropertyChanged(nameof(SelectedNumber));
                }
            }
        }


        #endregion

        #region Methods

        #endregion


        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class Numbers
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
