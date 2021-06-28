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

        private double prevAngle;

        bool yes;
        bool right;

        double rndHeightSom = 0;
        int times = 1;
        double x;
        double y;
        double tangHeight = 0;
        bool toBottom = true;
        double myHeight = 0;
        bool firstFromBottom = false;
        bool overHelft = false;
        int rondeBottom = 0;
        double eersteGrond = 0;


        private async void dvd_Pressed(object sender, EventArgs e)
        {
            double width = myGrid.Width;
            double height = myGrid.Height;

            int rndWidth = rnd.Next(45, (int)width) + 1;
            //int rndHeight = 400;
            int rndHeight = rnd.Next(45, (int)height) + 1;
            heighttxt.Text = rndHeight.ToString();

            tangHeight = rndHeight;
            rndHeightSom += rndHeight;





            //fix overflows
            //double angleWidth = width - tangensWidth;
            //if (angleWidth < 0)
            //{
            //    angleWidth = 0;
            //}
            //if (angleWidth > width)
            //{
            //    angleWidth = width;
            //}



            //run 1
            newLine((int)width, rndHeight);
            await dvd.TranslateTo(width, rndHeight, speed);
            ImgNumber = rndImage[randomGenerator(0, 5)];

            //runs
            for (int i = 0; i >= 0; i++)
            {

                if (toBottom)
                {
                    rndHeightSom += rndHeight;
                    //naar links
                    if (!right)
                    {
                        //als tegen zijkant komt
                        if (rndHeightSom < height)
                        {
                            times++;
                            x = 0;
                            y = rndHeight * times;
                        }
                        //tegen onderkant scherm
                        else
                        {
                            double angle = Math.Atan(width / tangHeight) * 180 / Math.PI;
                            double tangensWidth = Math.Tan(angle * (Math.PI / 180)) * (height - rndHeightSom + rndHeight);
                            prevAngle = angle;

                            x = width - tangensWidth;
                            y = height;
                            rndHeightSom = 0;
                            toBottom = false;

                            //tangHeight aanpassen
                        }
                        right = true;
                    }
                    //naar rechts
                    else
                    {
                        //als tegen zijkant komt
                        if (rndHeightSom < height)
                        {
                            times++;
                            x = width;
                            y = rndHeight * times;
                        }
                        //tegen onderkant scherm
                        else
                        {
                            double angle = Math.Atan(width / tangHeight) * 180 / Math.PI;
                            double radians = angle * (Math.PI / 180);
                            double tangensWidth = Math.Tan(radians) * (height - rndHeightSom + rndHeight);
                            prevAngle = angle;

                            x = tangensWidth;
                            y = height;
                            rndHeightSom = 0;
                            toBottom = false;

                            //tangHeight aanpassen
                        }
                        right = false;
                    }
                }
                // to top
                else
                {
                    rndHeightSom += myHeight;
                    times = 0;
                    rondeBottom++;

                    double revAngle = 90 - prevAngle;
                    double radians = revAngle * (Math.PI / 180);
                    double heightFromBottom = Math.Tan(radians) * lastXY[0];

                    double heightFromBottom2 = Math.Tan(radians) * width;

                    if (rndHeightSom == 0)
                    {
                        eersteGrond += heightFromBottom;
                        rndHeightSom += heightFromBottom;
                        rndHeightSom += heightFromBottom2;
                        firstFromBottom = true;
                    }
                    //als 2e keer over de helft komt
                    if ((eersteGrond + heightFromBottom2) > (height / 2) && rondeBottom == 3)
                    {
                        overHelft = true;
                    }

                    // naar rechts
                    if (!right)
                    {
                        //==================================
                        //als tegen zijkant komt
                        if (rndHeightSom < height && !overHelft)
                        {
                            times++;
                            x = width;
                            if (firstFromBottom)
                            {
                                y = height - heightFromBottom;
                                firstFromBottom = false;
                            }
                            else
                            {
                                rndHeightSom += heightFromBottom2;
                                y = height - (heightFromBottom2 * times) - (height - lastXY[1]);
                            }
                        }
                        //tegen onderkant scherm
                        else
                        {
                            //double angle = Math.Atan(width / tangHeight) * 180 / Math.PI;
                            //double tangensWidth = Math.Tan(angle * (Math.PI / 180)) * (height - rndHeightSom + rndHeight);

                            //x = width - tangensWidth;
                            //y = height;
                            //toBottom = false;
                            await DisplayAlert("j", "DONE", "OK");

                            //tangHeight aanpassen
                        }
                        right = true;
                    }
                    // naar links
                    else
                    {
                        //==================================
                        //als tegen zijkant komt
                        if (rndHeightSom < height && !overHelft)
                        {
                            times++;
                            x = 0;
                            if (firstFromBottom)
                            {
                                y = height - heightFromBottom;
                                firstFromBottom = false;
                            }
                            else
                            {
                                rndHeightSom += heightFromBottom2;
                                y = height - (heightFromBottom2 * times) - (height - lastXY[1]);
                            }
                        }
                        //tegen onderkant scherm
                        else
                        {
                            //double angle = Math.Atan(width / tangHeight) * 180 / Math.PI;
                            //double tangensWidth = Math.Tan(angle * (Math.PI / 180)) * (height - rndHeightSom + rndHeight);

                            //x = width - tangensWidth;
                            //y = height;
                            //toBottom = false;
                            await DisplayAlert("j", "DONE", "OK");

                            //tangHeight aanpassen
                        }
                        right = false;
                    }
                }




                newLine((int)x, (int)y);
                await dvd.TranslateTo(x, y, speed);
                ImgNumber = rndImage[randomGenerator(0, 5)];
            }

            newLine(0, 0);
            await dvd.TranslateTo(0, 0, speed);
            ImgNumber = rndImage[randomGenerator(0, 5)];
            lastXY[0] = 0;
            lastXY[1] = 0;
            tangHeight = 0;
            rndHeightSom = 0;
            right = false;
            times = 1;
            toBottom = true;



            //run2
            // als blok over helft komt
            //if (rndHeight * 2 > height)
            //{
            //    newLine((int)angleWidth, (int)height);
            //    await dvd.TranslateTo(angleWidth, height, speed);
            //    ImgNumber = rndImage[randomGenerator(0, 5)];

            //    yes = true;
            //}
            //else
            //{
            //    bool right = false;
            //    int times = 2;
            //    double staticWidth = 0;
            //    rndHeightSom += rndHeight;
            //    // zolang grond nog niet geraakt is
            //    while (rndHeightSom < height)
            //    {
            //        rndHeightSom += rndHeight;

            //        // als laatste tik voor dat blok de grond raakt
            //        if (rndHeightSom > height)
            //        {
            //            double tangensWidth2 = Math.Tan(Math.Atan(width / rndHeight) * 180 / Math.PI * (Math.PI / 180)) * ((rndHeight * times) - height);
            //            double angleWidth2 = width - tangensWidth2;
            //            times = 1;

            //            if (right)
            //            {
            //                staticWidth = width - tangensWidth2;
            //            }
            //            else
            //            {
            //                staticWidth = tangensWidth2;
            //            }
            //            rndHeight = (int)height;
            //        }
            //        if (right)
            //        {
            //            newLine((int)staticWidth, rndHeight * times);
            //            await dvd.TranslateTo(staticWidth, rndHeight * times, speed);
            //            ImgNumber = rndImage[randomGenerator(0, 5)];
            //            staticWidth = 0;
            //            right = false;
            //        }
            //        else
            //        {
            //            newLine((int)staticWidth, rndHeight * times);
            //            await dvd.TranslateTo(staticWidth, rndHeight * times, speed);
            //            ImgNumber = rndImage[randomGenerator(0, 5)];
            //            staticWidth = width;
            //            right = true;
            //        }
            //        times++;
            //    }
            //}


            ////run 3
            //if (yes)
            //{
            //    double angle2 = 90 - angle;
            //    double base2 = width - tangensWidth;
            //    double radians2 = angle2 * (Math.PI / 180);
            //    double heightFromBottom = Math.Tan(radians2) * base2;
            //    double x = height - heightFromBottom;

            //    newLine(0, (int)x);
            //    await dvd.TranslateTo(0, x, speed);
            //}
            ////run4
            ////if (heightFromBottom * 2 > height)
            //if (yes)
            //{
            //    newLine((int)width, 300);
            //    await dvd.TranslateTo(width, 300, speed);
            //}
            //else
            //{
            //    //double angle3 = 90 - angle;
            //    //double base2 = width - tangensWidth;
            //    //double radians2 = angle2 * (Math.PI / 180);
            //    //double heightFromBottom = Math.Tan(radians2) * base2;
            //    //double x = height - heightFromBottom;
            //    //newLine(0, (int)x);
            //    //await dvd.TranslateTo(0, x, speed);

            //    //newLine((int)width, (int)(height - (heightFromBottom * 2)));
            //    //await dvd.TranslateTo(width, height - (heightFromBottom * 2), speed);
            //}



            //newLine(0, 0);
            //await dvd.TranslateTo(0, 0, speed);
            //ImgNumber = rndImage[randomGenerator(0, 5)];
            //lastXY[0] = 0;
            //lastXY[1] = 0;

            //yes = false;
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
            speed = (uint)Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2)) * 4;

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