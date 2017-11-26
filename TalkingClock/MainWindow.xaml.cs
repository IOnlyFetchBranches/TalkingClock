using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TalkingClock.Audio;

namespace TalkingClock
{
    /// <summary>
    /// There exists a bug if you happen to run the program right when the time changes :(
    /// </summary>
    public partial class MainWindow : Window
    {
        //This is our Update Engine!
        private static Updater Announcer;

        //Global Variables
        private static FontFamily GLOBAL_FONT=new FontFamily("sans-seriff");

        public MainWindow()
        {
            InitializeComponent();


            //Bind to updater, we use refs to create bingins to the updater
            Announcer = new Updater(ref HourLabel, ref MinLabel, ref AMPMLabel);
            //Same principle applies to audio controls! Announcer.bindControls(<Controls>)
            //When i add buttons we will do that


            //DESIGN BOI
            HourLabel.FontFamily = GLOBAL_FONT;
            MinLabel.FontFamily = GLOBAL_FONT;
            ColonLabel.FontFamily = GLOBAL_FONT;
            AMPMLabel.FontFamily = GLOBAL_FONT;

           

            Thread driverThread = new Thread((x) =>
            {

                ColonLabel.Dispatcher.Invoke(() =>
                {
                    ColonLabel.Opacity = 0;
                });
                string oldsecs = DateTime.Now.ToString("ss");
                string newsecs = DateTime.Now.ToString("ss");
                //sync it up so that it doesnt run until the next second.
                while (oldsecs == newsecs)
                {
                    newsecs = DateTime.Now.ToString("ss");
                }

                while (true)
                {
                    ColonLabel.Dispatcher.Invoke(() =>
                    {
                        ColonLabel.Opacity = (1.0);
                        Announcer.UpdateTime();
                        ;
                    });
                    
                    Announcer.UpdateTime();
                    Thread.Sleep(500);
                    ColonLabel.Dispatcher.Invoke(() =>
                    {
                        ColonLabel.Opacity = (0.0);
                        Announcer.UpdateTime();
                        ;
                    });
                    Thread.Sleep(500);
                }


            });
            //Start
            driverThread.IsBackground = true;
            //driverThread.Priority = ThreadPriority.Highest; //needs to be in sync
            driverThread.Start();

            




        }
    }

    


}
