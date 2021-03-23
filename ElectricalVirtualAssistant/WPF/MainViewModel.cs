using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System;

namespace EVA_S.WPF
{
    class MainViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged realise
        public event PropertyChangedEventHandler PropertyChanged;
        public int GetPropertyChangedSubscribledLenght()
        {
            return PropertyChanged?.GetInvocationList()?.Length ?? 0;
        }
        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value)) return false;

            storage = value;
            RaisePropertyChanged(propertyName);

            return true;
        }
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }
        #endregion

        string _paramCircName;
        public string ParamCircName
        {
            get { return _paramCircName; }
            set => SetProperty(ref _paramCircName, value); //переназначение текстбокса
        }

        string _paramCircuitsNames;
        public string ParamCircuitsNames
        {
            get { return _paramCircuitsNames; }
            set => SetProperty(ref _paramCircuitsNames, value); //переназначение текстбокса

        }


        private Window _windowView;
        public Window WindowView
        {
            get { return _windowView; }
            set { _windowView = value; }
        }

        public ParametersNameEntity Ent { get; set; }

        //Конструктор
        public MainViewModel(ParametersNameEntity ent)
        {
            Accept = new RelayCommand(o => OkCommand(o)); //проброс команды
            Cancel = new RelayCommand(o => CancelCommand(o));
            DefaultValue = new RelayCommand(o => DefaultValueCommand(o));
            _paramCircName = ent.Param_CircName;
            _paramCircuitsNames = ent.Param_CircuitsNames;
           Ent = ent;
        }

        //Команды к которым привязываеться view
        public ICommand Accept { get; }
        public ICommand Cancel { get; }
        public ICommand DefaultValue { get; }

        //делигаты команд
        private void OkCommand(object obj)
        {
            Ent.Param_CircName = ParamCircName;
            Ent.Param_CircuitsNames = ParamCircuitsNames;
            _windowView.Close();

        }
        private void CancelCommand(object obj)
        {
            _windowView.Close();
        }
        private void DefaultValueCommand(object obj)
        {
            ParamCircName = "Имя_цепи_EVA";
            ParamCircuitsNames = "Комментарии";
            
        }

        //ParamCircName
    }
}
