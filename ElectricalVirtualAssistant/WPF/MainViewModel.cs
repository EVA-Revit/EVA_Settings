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

        string _paramLoadName;
        public string ParamLoadName
        {
            get { return _paramLoadName; }
            set => SetProperty(ref _paramLoadName, value); //переназначение текстбокса
        }
        string _paramTextName;
        public string ParamTextName
        {
            get { return _paramTextName; }
            set => SetProperty(ref _paramTextName, value); //переназначение текстбокса
        }
        string _paramDoubleName;
        public string ParamDoubleName
        {
            get { return _paramDoubleName; }
            set => SetProperty(ref _paramDoubleName, value); //переназначение текстбокса
        }

        private Window _windowView;
        public Window WindowView
        {
            get { return _windowView; }
            set { _windowView = value; }
        }

        public ParametersNameEntity Ent { get; set; }

        private bool _isLoadSharedParameters;
        public bool IsLoadSharedParameters
        {
            get { return _isLoadSharedParameters; }
            set { _isLoadSharedParameters = value; }
        }

        private bool _isLoadFamelesEVAex;
        public bool IsLoadFamelesEVAex
        {
            get { return _isLoadFamelesEVAex; }
            set { _isLoadFamelesEVAex = value; }
        }

        private bool _isLoadFamelesEVAstreams;
        public bool IsLoadFamelesEVAstreams
        {
            get { return _isLoadFamelesEVAstreams; }
            set { _isLoadFamelesEVAstreams = value; }
        }

        private bool _isLoadFamelesEVAcirc;
        public bool IsLoadFamelesEVAcirc
        {
            get { return _isLoadFamelesEVAcirc; }
            set { _isLoadFamelesEVAcirc = value; }
        }


        //Конструктор
        public MainViewModel(ParametersNameEntity ent)
        {
            Accept = new RelayCommand(o => OkCommand(o)); //проброс команды
            Cancel = new RelayCommand(o => CancelCommand(o));
            DefaultValue = new RelayCommand(o => DefaultValueCommand(o));
            LoadDefaultParameters = new RelayCommand(o => LoadDefaultParametersCommand(o));
            _paramCircName = ent.Param_CircName;
            _paramCircuitsNames = ent.Param_CircuitsNames;
            _paramLoadName = ent.Param_LoadName;
            _paramTextName = ent.Param_TextName;
            _paramDoubleName = ent.Param_DoubleName;
            Ent = ent;
        }

        //Команды к которым привязываеться view
        public ICommand Accept { get; }
        public ICommand Cancel { get; }
        public ICommand DefaultValue { get; }
        public ICommand LoadDefaultParameters { get; }




        //активность кнопки
        //private bool CanCmdExecLoadParameters(object obj)
        //{
        //    if (ParamCircName == "Имя_цепи_EVA" &&
        //    ParamCircuitsNames == "Группа_имен_цепей_EVA")
        //    {
        //        return true;
        //    }
        //    return false;
        //}


        //делигаты команд
        private void OkCommand(object obj)
        {
            Ent.Param_CircName = ParamCircName;
            Ent.Param_CircuitsNames = ParamCircuitsNames;
            Ent.Param_LoadName = ParamLoadName;
            Ent.Param_TextName = ParamTextName;
            Ent.Param_DoubleName = ParamDoubleName;

            _windowView.DialogResult = true;
            _windowView.Close();

        }
        private void CancelCommand(object obj)
        {
            _windowView.Close();
        }
        private void DefaultValueCommand(object obj)
        {
            ParamCircName = "EVA_Имя_цепи";
            ParamCircuitsNames = "EVA_Группа_имен_цепей";
            ParamLoadName = "EVA_Имя_Нагрузки";
            ParamTextName = "EVA_Текст";
            ParamDoubleName = "EVA_Число";

        }
        private void LoadDefaultParametersCommand(object obj)
        {
            _isLoadSharedParameters = true;
        }

    }
}
