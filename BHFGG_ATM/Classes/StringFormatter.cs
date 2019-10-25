using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BHFGG_ATM.EventArgClasses;
using BHFGG_ATM.Interfaces;
using TransponderReceiver;
using ITransponderReceiver = TransponderReceiver.ITransponderReceiver;

namespace BHFGG_ATM.Classes
{
    public class StringFormatter : IStringFormatter
    {
        #region Event receiver

        public List<string> CurrentTransponderData { get; set; }

        public StringFormatter(ITransponderReceiver transponderReceiver)
        {
            transponderReceiver.TransponderDataReady += HandleTransponderDataEvent;
        }

        private void HandleTransponderDataEvent(object sender, RawTransponderDataEventArgs e)
        {
            CurrentTransponderData = e.TransponderData;
            FormatData(CurrentTransponderData);
        }


        #endregion

        #region Event source

        public event EventHandler<DataFormattedEventArgs> DataFormattedEvent;
        

        public void FormatData(List<string> stringToFormat)
        {
            List<Track> newListOfTrack = new List<Track>();

            foreach (var dataString in stringToFormat)
            {
                string[] sArray = dataString.Split(';');
                Track track = new Track();
                
                track.Tag = sArray[0];
                track.PositionX = Convert.ToDouble(sArray[1]);
                track.PositionY = Convert.ToDouble(sArray[2]);
                track.Altitude = Convert.ToDouble(sArray[3]);
                track.Timestamp = sArray[4];

                //Skal have lavet noget med track 1 og track 2 her..
                //track.CompassCourse = track.CalculateCompassCourse()();
                //track.HorizontalVelocity = track.CalculateCurrentVelocity();

                newListOfTrack.Add(track);
            }

            OnDataFormattedEvent(new DataFormattedEventArgs { DataFormatted = newListOfTrack});
        }

        protected virtual void OnDataFormattedEvent(DataFormattedEventArgs e)
        {
            DataFormattedEvent?.Invoke(this, e);
        }


        #endregion


    }
}
