using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace DynamicListView
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Costants and Fields

        private bool _isBusy;

        #endregion

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        #endregion

        #region Constructors

        public BaseViewModel()
        {
        }

        #endregion

        #region Event Raising Methods

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName == null)
                return;

            if (propertyName == String.Empty)
                return;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Public Methods

        #endregion

        #region Methods

        protected bool SetProperty<T>(ref T backingStore, T value,
            string chainedPropertyName = "",
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            OnPropertyChanged(chainedPropertyName);
            return true;
        }

        #endregion
    }
}
