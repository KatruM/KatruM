using BDI3Mobile.CustomRenderer;
using BDI3Mobile.UWP.Renderer;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

//[assembly: ExportRenderer(typeof(CollectionView), typeof(MyCollectionViewRenderer))]
[assembly: ExportRenderer(typeof(MyCollectionView), typeof(ExtendedCollectionViewRenderer))]
namespace BDI3Mobile.UWP.Renderer
{
    public class ExtendedCollectionViewRenderer : CollectionViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<GroupableItemsView> args)
        {
            base.OnElementChanged(args);
            if (Element != null)
            {
                (Element as MyCollectionView).DisposeListView = new Action(() =>
                {
                    this.Dispose(true);
                });
            }
        }
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs changedProperty)
        {
            base.OnElementPropertyChanged(sender, changedProperty);
        }
        protected override void Dispose(bool disposing)
        {
            base.TearDownOldElement(Element);
            base.Dispose(disposing);
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
    public class MyCollectionViewRenderer : CollectionViewRenderer
    {
        protected override void Dispose(bool disposing)
        {
            base.TearDownOldElement(Element);
            base.Dispose(disposing);
            GC.Collect();
            GC.SuppressFinalize(this);
        }
    }
}
