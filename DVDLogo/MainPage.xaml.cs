using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

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

        private int previousRandom = 0;
        private int[] lastXY = { 0, 0 };
        private uint speed = 1000;

        bool yes;



        private async void translateButton_Pressed(object sender, EventArgs e)
        {
            double realWidth = myGrid.Width;
            double width = realWidth;
            double realHeight = myGrid.Height;
            double height = realHeight + 0;

            int rndWidth = rnd.Next(45, (int)width) + 1;
            //int rndHeight = 365;
            int rndHeight = rnd.Next(45, (int)height) + 1;
            heighttxt.Text = rndHeight.ToString();

            double angle = Math.Atan(realWidth / rndHeight) * 180 / Math.PI;
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
            newLine((int)width, rndHeight);
            await translateButton.TranslateTo(width, rndHeight, speed);
            ImgNumber = rndImage[randomGenerator(0, 5)];

            //run2
            // als blok over helft komt
            if (rndHeight * 2 > height)
            {
                newLine((int)angleWidth, (int)height);
                await translateButton.TranslateTo(angleWidth, height, speed);
                ImgNumber = rndImage[randomGenerator(0, 5)];

                yes = true;
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

                        if (right)
                        {
                            staticWidth = width - tangensWidth2;
                        }
                        else
                        {
                            staticWidth = tangensWidth2;
                        }
                        rndHeight = (int)height;
                    }
                    if (right)
                    {
                        newLine((int)staticWidth, rndHeight * times);
                        await translateButton.TranslateTo(staticWidth, rndHeight * times, speed);
                        ImgNumber = rndImage[randomGenerator(0, 5)];
                        staticWidth = 0;
                        right = false;
                    }
                    else
                    {
                        newLine((int)staticWidth, rndHeight * times);
                        await translateButton.TranslateTo(staticWidth, rndHeight * times, speed);
                        ImgNumber = rndImage[randomGenerator(0, 5)];
                        staticWidth = width;
                        right = true;
                    }
                    times++;
                }
            }


            //run 3
            if (yes)
            {
                double angle2 = 90 - angle;
                double base2 = width - tangensWidth;
                double radians2 = angle2 * (Math.PI / 180);
                double heightFromBottom = Math.Tan(radians2) * base2;
                double x = height - heightFromBottom;

                newLine(0, (int)x);
                await translateButton.TranslateTo(0, x, speed);
            }
            //run4
            //if (heightFromBottom * 2 > height)
            if (yes)
            {
                newLine((int)width, 300);
                await translateButton.TranslateTo(width, 300, speed);
            }
            else
            {
                //double angle3 = 90 - angle;
                //double base2 = width - tangensWidth;
                //double radians2 = angle2 * (Math.PI / 180);
                //double heightFromBottom = Math.Tan(radians2) * base2;
                //double x = height - heightFromBottom;
                //newLine(0, (int)x);
                //await translateButton.TranslateTo(0, x, speed);

                //newLine((int)width, (int)(height - (heightFromBottom * 2)));
                //await translateButton.TranslateTo(width, height - (heightFromBottom * 2), speed);
            }



            newLine(0, 0);
            await translateButton.TranslateTo(0, 0, speed);
            ImgNumber = rndImage[randomGenerator(0, 5)];
            lastXY[0] = 0;
            lastXY[1] = 0;

            yes = false;
        }


        private int randomGenerator(int a, int b)
        {
            int random = rnd.Next(a, b);
            while (previousRandom == random)
            {
                random = rnd.Next(a, b);
            }
            previousRandom = random;
            return random;
        }

        private void newLine(int x2, int y2)
        {
            Line lijn = new Line();
            lijn.X1 = lastXY[0];
            lijn.X2 = x2;
            lijn.Y1 = lastXY[1];
            lijn.Y2 = y2;
            lijn.Stroke = Brush.Red;
            myGrid.Children.Add(lijn);

            //speed berekening
            double a = Math.Abs(lastXY[0] - x2);
            double b = Math.Abs(lastXY[1] - y2);
            speed = (uint)Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)) * 8;


            lastXY[0] = x2;
            lastXY[1] = y2;
        }

        private void removeLines()
        {
            for (int i = 0; i < myGrid.Children.Count; i++)
            {
                if (myGrid.Children[i].GetType() == typeof(Line))
                {
                    myGrid.Children.Remove(myGrid.Children[i]);
                }
            }
        }

        private void Button_Pressed(object sender, EventArgs e)
        {
            removeLines();
        }
    }
}