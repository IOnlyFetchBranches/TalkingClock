using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace TalkingClock.Audio
{
    class Updater
    {
        //Public Properties

        //Define references to WPF controls.
        public Label HourLabel { get; set; }
        public Label MinLabel { get; set; }
        public Label AMPMLabel { get; set; }

        //private components

            //Our TTS Engine.
        private static SpeechSynthesizer announcer = new SpeechSynthesizer();





        public Updater(ref Label hourLabel, ref Label minLabel, ref Label ampmLabel)
        {

            HourLabel = hourLabel;
            MinLabel = minLabel;
            AMPMLabel = ampmLabel;

            //Init the defaults for the voice
            announcer.Rate = 1;
            announcer.SelectVoice(announcer.GetInstalledVoices().First().VoiceInfo.Name);



            while (HourLabel == null)
            {
                //Pause until the system catches up.
            }
            //Update the UI with the current times
            InitialTimes();

            //test audio
            AnnounceNew();


        }
        

        private static string oldMin;
        private static string oldHour;

        //Init currnt times on load;
        private void InitialTimes()
        {
            //Init Values;
            HourLabel.Content = DateTime.Now.ToString("hh");
            MinLabel.Content = DateTime.Now.ToString("mm");

            if (DateTime.Now.Hour > 11 && DateTime.Now.Hour < 23)
            {
                AMPMLabel.Content = "PM";
            }
            else
            {
                AMPMLabel.Content = "AM";
            }
        }

      
        public void UpdateTime()
        {
            //Here we run the time announce/GUI Update so we dont throw off the main thread with computation

            if (oldMin == null)
            {
                oldMin = DateTime.Now.ToString("mm");
                oldHour = DateTime.Now.ToString("hh");
                UpdateTime(); //recall the function
            }
            else
            {


                if (DateTime.Now.ToString("mm") != oldMin)
                {
                    //Update
                    oldMin = DateTime.Now.ToString("mm");

                    if (DateTime.Now.Hour.ToString("hh") != oldHour)
                    {
                        oldHour = DateTime.Now.Hour.ToString("hh");
                        //Update Hour if needed
                        HourLabel.Dispatcher.Invoke(() =>
                        {
                            HourLabel.Content = DateTime.Now.ToString("hh");
                        });
                        //Check AM/PM
                        if (DateTime.Now.Hour > 11 && DateTime.Now.Hour < 23)
                        {
                            AMPMLabel.Dispatcher.Invoke(() =>
                            {
                                AMPMLabel.Content = "PM";
                            });

                        }
                        else
                        {
                            AMPMLabel.Dispatcher.Invoke(() =>
                            {
                                AMPMLabel.Content = "AM";
                            });
                        }

                    }

                    //Update Minute Regardless;

                    MinLabel.Dispatcher.Invoke(() =>
                    {
                        MinLabel.Content = DateTime.Now.ToString("mm");
                    });

                   //Announce
                    AnnounceNew();
                    







                }

            }
            //End UpdateTime()
        }
        
        

        public static void AnnounceNew()
        {
            string currentPeriod =
                (Int32.Parse(DateTime.Now.ToString("HH")) > 0 && Int32.Parse(DateTime.Now.ToString("HH")) < 12) ? "AM" : "PM";


            //announcer.Speak("Tong, Where are you??");
            

            if (DateTime.Now.ToString("hh").StartsWith("0"))
                announcer.Speak("The Time is now " + DateTime.Now.ToString("hh").ToCharArray()[1] + " " +
                                DateTime.Now.ToString("mm") + "," + currentPeriod);
            else if (DateTime.Now.ToString("mm") == "00") 
            {
                announcer.Speak("The Time is now " + DateTime.Now.ToString("hh").ToCharArray()[1] + " " +
                                "O'Clock");
            }
            else
            {
                announcer.Speak("The Time is now " + DateTime.Now.ToString("hh") + " " +
                                DateTime.Now.ToString("mm")+","+ currentPeriod);
            }
            
        }

        //Volumee related methods/Variables
        private static int oldVolume;
        private static bool isMuted = false;

        public static void Mute()
        {
            if (!isMuted)
            {
                oldVolume = announcer.Volume;
                announcer.Volume = 0;
                isMuted = true;
            }
            else
            {
                isMuted = false;
                announcer.Volume = oldVolume;
            }
        }

        public static void SetVolume(int volume)
        {
            announcer.Volume = volume;
            oldVolume = announcer.Volume;
            
        }

    }
}
