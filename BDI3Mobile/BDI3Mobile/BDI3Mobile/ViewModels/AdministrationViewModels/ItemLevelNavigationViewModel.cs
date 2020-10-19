using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Xamarin.Forms;

namespace BDI3Mobile.ViewModels.AdministrationViewModels
{
    #region ItemGroup
    [Serializable]
    public class ItemGroup
    {
        public List<ItemGroup> Children { get; set; } = new List<ItemGroup>();
        public List<Item> Items { get; set; } = new List<Item>();
        public List<DetailedItemView> detailedItemViews { get; set; } = new List<DetailedItemView>();
        public string TreeViewName { get; set; }
        public string Name { get; set; }
        public int TreeViewId { get; set; }
        public List<string> Parent = new List<string>();
        public bool IsEnabled { get; set; }
    }

    [Serializable]
    public class Item
    {
        public string Key { get; set; }
    }

    public class DetailedItemView
    {
        public string Key { get; set; }
    }
    #endregion

    #region ItemModelProvider
    public partial class ItemModelProvider : List<ItemModel>
    {
        #region Fields
        private Timer _Timer;
        #endregion

        #region Events
        public event EventHandler ItemsLoaded;
        #endregion

        #region Constructor
        public ItemModelProvider()
        {
            _Timer = new Timer(TimerCallback, null, 3000, 0);
        }
        #endregion

        #region Private Methods
        private void TimerCallback(object state)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                AddItems();
            });
        }

        private void AddItems()
        {
            Add(new ItemModel { Name = 1, Description = "First" });
            Add(new ItemModel { Name = 2, Description = "Second" });
            Add(new ItemModel { Name = 3, Description = "Third" });
            ItemsLoaded?.Invoke(this, new EventArgs());
        }
        #endregion
    }
    #endregion

    #region ItemModel
    public class ItemModel : INotifyPropertyChanged
    {
        #region Fields
        private int _Name;
        private string _Description;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Public Properties
        public int Name
        {
            get
            {
                return _Name;
            }

            set
            {
                _Name = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }
        }
        #endregion

        #region Public Methods
        public override bool Equals(object obj)
        {
            var itemModel = obj as ItemModel;
            if (itemModel == null)
            {
                return false;
            }

            var returnValue = Name.Equals(itemModel.Name);

            Debug.WriteLine($"An {nameof(ItemModel)} was tested for equality. Equal: {returnValue}");

            return returnValue;
        }

        public override int GetHashCode()
        {
            Debug.WriteLine($"{nameof(GetHashCode)} was called on an {nameof(ItemModel)}");
            return Name;
        }

        #endregion
    }
    #endregion
}
