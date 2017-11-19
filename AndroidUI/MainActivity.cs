using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using Android.Graphics.Drawables;

namespace AndroidUI
{
    [Activity(Label = "QR Code Diag", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity, View.IOnTouchListener
    {
        private const int c_edgelen = 29; //ToDo no longer needed when proper qrcode is used
        private int[] downPixelCoords;
        ImageView qrCodeImageView;

        public bool OnTouch(View v, MotionEvent e)
        {
            TextView debugTextView = FindViewById<TextView>(Resource.Id.debugTextView);
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    this.downPixelCoords = this.TouchToImageCoordinates(e.GetX(), e.GetY());
                    debugTextView.Text = "Not a click";
                    break;
                case MotionEventActions.Up:
                    var loc = this.TouchToImageCoordinates(e.GetX(), e.GetY());
                    if(loc[0] == downPixelCoords[0] && loc[1] == downPixelCoords[1])
                    {
                        debugTextView.Text = String.Format("({0}|{1})", loc[0], loc[1]);
                    }
                    break;
                default:
                    break;
            }
            return true;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView (Resource.Layout.Main);
            qrCodeImageView = FindViewById<ImageView>(Resource.Id.QRCodeImageViewID);
            qrCodeImageView.SetImageResource(Resource.Drawable.anfang);
            qrCodeImageView.SetOnTouchListener(this);
        }

        private int[] TouchToImageCoordinates(float x, float y)
        {
            if (qrCodeImageView == null)
                throw new InvalidOperationException("qrCodeImageView was not initialized correctly.");
            int[] loc = new int[2];
            //qrCodeImageView.GetLocationOnScreen(loc);   // Get location of imageview
            loc[0] = (int)(x - loc[0]);          // Translation to imageview coordinates
            loc[1] = (int)(y - loc[1]);
            var matVals = new float[9];
            qrCodeImageView.Matrix.GetValues(matVals);
            var imgWidth = qrCodeImageView.Drawable.IntrinsicWidth;
            var imgHeight = qrCodeImageView.Drawable.IntrinsicHeight;
            loc[0] = (int)(c_edgelen * loc[0] / (matVals[Android.Graphics.Matrix.MscaleX] * imgWidth));
            loc[1] = (int)(c_edgelen * loc[1] / (matVals[Android.Graphics.Matrix.MscaleY] * imgHeight));
            return loc;
        }
    }
}

