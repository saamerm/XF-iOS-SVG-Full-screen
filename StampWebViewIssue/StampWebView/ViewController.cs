using CoreGraphics;
using Foundation;
using System;
using System.IO;
using System.Reflection;
using UIKit;

namespace StampWebView
{
    public partial class ViewController : UIViewController
    {
        UIWebView webView;
        nfloat zoom = 1;
        UIButton button = new UIButton();
        UIButton button1 = new UIButton();
        CGRect originalFrame;
        string html;
        public ViewController (IntPtr handle) : base (handle)
        {
            webView = new UIWebView();
            webView.Opaque = false;
            webView.BackgroundColor = UIColor.Yellow;
            webView.LoadFinished += WebView_LoadFinished;
            webView.ScrollView.UserInteractionEnabled = false;

            button.SetTitle("Zoom In", UIControlState.Normal);
            button.TouchUpInside += Button_TouchUpInside;
            button.BackgroundColor = UIColor.Black;

            button1.SetTitle("Zoom Out", UIControlState.Normal);
            button1.TouchUpInside += Button_TouchUpInside1;
            button1.BackgroundColor = UIColor.Black;

            Stream stream = typeof(ViewController).GetTypeInfo().Assembly.GetManifestResourceStream("StampWebView.Approved.html");
            using (StreamReader reader = new StreamReader(stream))
            {
                html = reader.ReadToEnd();
            }
        }

        private void Button_TouchUpInside1(object sender, EventArgs e)
        {
            zoom -= 0.5f;
            webView.Frame = new CoreGraphics.CGRect(originalFrame.X, originalFrame.Y, originalFrame.Width * zoom, originalFrame.Height * zoom);
        }

        private void WebView_LoadFinished(object sender, EventArgs e)
        {
            webView.ScrollView.MinimumZoomScale = 1;
            webView.ScrollView.MaximumZoomScale = 10;
        }

        private void Button_TouchUpInside(object sender, EventArgs e)
        {
            zoom += 0.5f;
            for (int i=1; i>0;i++)
            {
                webView.Frame = new CoreGraphics.CGRect(originalFrame.X, originalFrame.Y, originalFrame.Width * zoom, originalFrame.Height * zoom);
                zoom += 0.1f;
                if (zoom > 7)
                    break;
            }
        }
        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            webView.LoadHtmlString(html, null);
            View.AddSubview(webView);
            View.BackgroundColor = UIColor.LightGray;
            webView.Transform = CGAffineTransform.MakeRotation(1.57f);
            webView.Frame = new CoreGraphics.CGRect(0, 50, 42, 171);
            originalFrame = webView.Frame; 
            View.AddSubview(button);
            View.AddSubview(button1);
            button.Frame = new CoreGraphics.CGRect(View.Frame.Width / 2, 0, View.Frame.Width / 2, 50);
            button1.Frame = new CGRect(0, 0, View.Frame.Width / 2, 50);
        }
    }
}