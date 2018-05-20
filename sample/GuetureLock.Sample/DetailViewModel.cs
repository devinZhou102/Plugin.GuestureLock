using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GuetureLock.Sample
{
    public class DetailViewModel : INotifyPropertyChanged
    {

        public DetailViewModel()
        {
            CompleteCommand = new Command((arg) => CompleteExcute(arg));
        }

        private string _Result;

        public string Result
        {
            get
            {
                if (_Result == null) _Result = "";
                return _Result;
            }
            set
            {
                SetProperty(ref _Result,value);
            }
        }

        public ICommand CompleteCommand { get; private set; }

        private void CompleteExcute(object checkList)
        {
            if (checkList is List<int>)
            {
                var result = "";
                var datas = checkList as List<int>;
                foreach (var item in datas)
                {
                    result += item + " ";
                }
                Result = result;
            }
        }

        #region

        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T storage, T value,
            [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
