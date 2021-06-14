using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using My_Task_Manager.View.ViewModel;
using System.Windows;
using Microsoft.Win32;

namespace My_Task_Manager.ModelView
{
    public  class ViewModel  : INotifyPropertyChanged
    {
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; // ивент обновления
        public void OnPropertyChanged([CallerMemberName] string prop = "")
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion
        public ObservableCollection<Process> process { get; set; }
        public ViewModel()
        {
            process = new ObservableCollection<Process>();
            InitializeComponen();
        }
        public void InitializeComponen()
        {
            try
            {
                if (process.Count != 0)
                    process.Clear();
                foreach (var item in Process.GetProcesses().ToList())
                {
                    process.Add(item);
                  
                }
               
            }
            catch (Exception)
            { return; }


        }
        private Process selected_item;
        public Process Selected_Item
        {
            get { return selected_item; }
            set { selected_item = value; OnPropertyChanged("Selected_Item"); }
        }

        private RelayCommand add;
        public RelayCommand Add
        {
            get
            {
                return add ?? (add = new RelayCommand(act =>
                {
                    if (MessageBox.Show("Добавить в автозагрузку ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        return;
                    if (Selected_Item != null)
                    {                    
                        var autostart = Registry.CurrentUser.CreateSubKey("Software").
                        CreateSubKey("Microsoft").
                        CreateSubKey("Windows").
                        CreateSubKey("CurrentVersion").
                        CreateSubKey("Run");
                        autostart.SetValue(Selected_Item.ProcessName, Selected_Item.MainModule.FileName);

                        InitializeComponen();
                    }
                   
                }));
            }
        }
        private RelayCommand dell;
        public RelayCommand Dell
        {
            get
            {
                return dell ?? (dell = new RelayCommand(
              act =>
              {
                  if (MessageBox.Show("Удалить Процесс?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                      return;              
                     Selected_Item?.Kill();                                         
                      InitializeComponen();      
              }));
            }
        } 
        private RelayCommand update;
        public RelayCommand UpDate
        {
            get
            {
                return update ?? (update = new RelayCommand( act =>{InitializeComponen();}));
            }
        }
    }
}
