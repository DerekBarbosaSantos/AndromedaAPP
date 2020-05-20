using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AndromedaAPP.ViewModels
{
    abstract class BaseViewModel : INotifyPropertyChanged
    {
        bool isBusy = false;
        string title = string.Empty;

        public Page pageParent { get; set; }
        public BaseViewModel(Page parent)
        {
            pageParent = parent;
        }

        public bool IsBusy
        {
            get 
            { 
                return pageParent.IsBusy; 
            }
            set 
            {
                isBusy = pageParent.IsBusy = value;
                SetProperty(ref isBusy, value); 
            }
        }

        public string Title
        {
            get 
            { 
                return pageParent.Title; 
            }
            set 
            {
                title = pageParent.Title = value;
                SetProperty(ref title, value); 
            }
        }

        public Application Current { get => Application.Current; }

        protected async Task DisplayAlert(string title, string message, string cancel)
        {
            await pageParent.DisplayAlert(title, message, cancel);
        }

        protected async Task<bool> DisplayAlert(string title, string message, string accepted, string cancel)
        {
            return await pageParent.DisplayAlert(title, message, accepted, cancel);
        }

        protected async Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            return await pageParent.DisplayActionSheet(title, cancel, destruction, buttons);
        }

        protected async Task<string> DisplayAlert(string title, string message, string accept, string cancel, string placeholder, 
                                                    int maxLength, Keyboard keyboard)
        {
            return await pageParent.DisplayPromptAsync(title, message, accept, cancel, placeholder, maxLength, keyboard);
        }

        public INavigation Navigation { get => pageParent.Navigation; }

        protected async Task SetSecureStorageValue(string key, string value)
        {
            await Xamarin.Essentials.SecureStorage.SetAsync(key, value);
        }

        protected async Task<string> GetSecureStorageValue(string key)
        {
            return await Xamarin.Essentials.SecureStorage.GetAsync(key);
        }

        protected void SetStorageValue(string key, string value)
        {
            if(!Current.Properties.ContainsKey(key))
            {
                Current.Properties.Add(key, value);
            }
            else
            {
                Current.Properties[key] = value;
            }
        }

        protected string GetStorageValue(string key)
        {
            if (Current.Properties.ContainsKey(key))
            {
               return (string)Current.Properties[key];
            }

            return default;
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName]string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
