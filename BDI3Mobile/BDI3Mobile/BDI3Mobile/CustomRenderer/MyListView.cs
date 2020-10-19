using System;
using Xamarin.Forms;

namespace BDI3Mobile.CustomRenderer
{
    public class MyListView : ListView
    {
        public Action DisposeListView { get; set; }
    }

    public class MyCollectionView : CollectionView
    {
        public Action DisposeListView { get; set; }
    }
}
