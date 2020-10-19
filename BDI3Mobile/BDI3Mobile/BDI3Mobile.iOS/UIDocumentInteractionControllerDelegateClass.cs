using System;
using UIKit;

namespace BDI3Mobile.iOS
{
    public class UIDocumentInteractionControllerDelegateClass : UIDocumentInteractionControllerDelegate
    {

        UIViewController ownerVC;

        public UIDocumentInteractionControllerDelegateClass(UIViewController vc)
        {
            ownerVC = vc;
        }

        public override UIViewController ViewControllerForPreview(UIDocumentInteractionController controller)
        {
            return ownerVC;
        }

        public override UIView ViewForPreview(UIDocumentInteractionController controller)
        {
            return ownerVC.View;
        }
    }
}
