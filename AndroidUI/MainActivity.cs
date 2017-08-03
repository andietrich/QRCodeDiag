using Android.App;
using Android.Widget;
using Android.OS;

namespace AndroidUI
{
    [Activity(Label = "QR Code Diag", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            
            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            ImageView qrCodeImageView = FindViewById<ImageView>(Resource.Id.QRCodeImageViewID);
            qrCodeImageView.SetImageResource(Resource.Drawable.anfang);
        }
    }
}

