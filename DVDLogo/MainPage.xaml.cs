using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace DVDLogo
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        Random rnd = new Random();
        string[] rndImage = { "dvdb", "dvdr", "dvdrd", "dvdg", "dvdy" };

        private string _imgNumber = "dvdb";
        public string ImgNumber
        {
            get { return _imgNumber; }
            set { _imgNumber = value; OnPropertyChanged("dvdImage"); }
        }


        public ImageSource dvdImage
        {
            get { return ImageSource.FromResource($"DVDLogo.Images.{_imgNumber}.png"); }
        }

        //s10  779 412
        //s20  765 360
        private async void translateButton_Pressed(object sender, EventArgs e)
        {

            //lb1.Text = myGrid.Width.ToString();
            //lb2.Text = myGrid.Height.ToString();

            double realWidth = myGrid.Width;
            double width = realWidth;
            double realHeight = myGrid.Height;
            double height = realHeight + 0; //s20 +6 (margin top in xaml aanpassen)

            int rndWidth = rnd.Next(45, (int)width) + 1;
            int rndHeight = rnd.Next(45, (int)height) + 1;

            double tangensWidth = Math.Tan(Math.Atan(realWidth / rndHeight) * 180 / Math.PI * (Math.PI / 180)) * (realHeight - rndHeight);

            double rndHeightSom = 0;

            //fix overflows
            double angleWidth = realWidth - tangensWidth;
            if (angleWidth < 0)
            {
                angleWidth = 0;
            }
            if (angleWidth > width)
            {
                angleWidth = width;
            }


            //run 1
            uint speed1 = (uint)(3000 - ((int)width + rndHeight));
            await translateButton.TranslateTo(width, rndHeight, speed1);
            ImgNumber = rndImage[rnd.Next(0, 5)];

            //run2
            // als blok over helft komt
            if (rndHeight * 2 > height)
            {
                await translateButton.TranslateTo(angleWidth, height, 1000);
            }
            else
            {
                bool right = false;
                int times = 2;
                double staticWidth = 0;
                rndHeightSom += rndHeight;
                // zolang grond nog niet geraakt is
                while (rndHeightSom < height)
                {
                    rndHeightSom += rndHeight;

                    // als laatste tik voor dat blok de grond raakt
                    if (rndHeightSom > height)
                    {
                        double tangensWidth2 = Math.Tan(Math.Atan(realWidth / rndHeight) * 180 / Math.PI * (Math.PI / 180)) * ((rndHeight * times) - realHeight);
                        double angleWidth2 = realWidth - tangensWidth2;
                        times = 1;

                        //hier
                        if (right)
                        {
                            if (angleWidth2 < 0)
                            {
                                staticWidth = (angleWidth2 * -1);
                            }
                            else
                            {
                                staticWidth = width - (tangensWidth2 * -1);
                            }
                        }
                        else
                        {
                            if (tangensWidth2 < 0)
                            {
                                staticWidth = (tangensWidth2 * -1); //fixed
                            }
                            else
                            {
                                staticWidth = tangensWidth2;
                            }
                        }
                        //hier ook
                        if (staticWidth > width)
                        {
                            staticWidth = width - rnd.Next(1, 100);
                        }

                        rndHeight = (int)height;
                    }


                    uint speed2 = (uint)(2000 - ((int)staticWidth + rndHeight));
                    if (right)
                    {
                        await translateButton.TranslateTo(staticWidth, rndHeight * times, speed2);
                        staticWidth = 0;
                        right = false;
                        ImgNumber = rndImage[rnd.Next(0, 5)];
                    }
                    else
                    {
                        await translateButton.TranslateTo(staticWidth, rndHeight * times, speed2);
                        staticWidth = width;
                        right = true;
                        ImgNumber = rndImage[rnd.Next(0, 5)];
                    }
                    times++;
                }
            }
            ImgNumber = rndImage[rnd.Next(0, 5)];
            await translateButton.TranslateTo(0, 0, 1000);
            ImgNumber = rndImage[rnd.Next(0, 5)];

        }
    }
}