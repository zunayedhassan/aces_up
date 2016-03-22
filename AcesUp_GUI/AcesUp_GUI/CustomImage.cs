using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AcesUp_CoreLibrary;

namespace AcesUp_GUI
{
    class CustomImage : Image
    {
        public string imageName;
        public int pileIndex;

        public CustomImage(Card card)
        {
            // Create Image Element
            this.Width = 72;
            this.Height = 96;

            Initialize("Cards/" + (int)card.CardSuit + card.Rank + ".png", (int) this.Width, (int) this.Height);
        }

        public CustomImage(string imageName, int width, int height)
        {
            // Create Image Element
            this.Width = width;
            this.Height = height;

            Initialize(imageName, (int)this.Width, (int)this.Height);
        }

        private void Initialize(string imageName, int width, int height)
        {
            // Create source
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(imageName, UriKind.RelativeOrAbsolute);
            bitmapImage.DecodePixelWidth = width;
            bitmapImage.DecodePixelHeight = height;
            bitmapImage.EndInit();

            //set image source
            this.Source = bitmapImage;
        }
    }
}
