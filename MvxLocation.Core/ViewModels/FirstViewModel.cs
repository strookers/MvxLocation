using System;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.Location;

namespace MvxLocation.Core.ViewModels
{
    public class FirstViewModel 
        : MvxViewModel
    {
        private readonly IMvxLocationWatcher _locationWatcher;

        private bool _hasGpsLocation;
        private bool _started;

        public FirstViewModel(IMvxLocationWatcher locationWatcher)
        {
            _locationWatcher = locationWatcher;
        }

        private MvxCommand _toggleGpsCommand;
        public ICommand ToggleGpsCommand
        {
            get
            {
                _toggleGpsCommand = _toggleGpsCommand ?? new MvxCommand(() =>
                {
                    if (!Started)
                        _locationWatcher.Start(
                            new MvxLocationOptions { Accuracy = MvxLocationAccuracy.Fine }, OnLocation, OnError);
                    else
                        _locationWatcher.Stop();
                    Started = !Started;
                });
                return _toggleGpsCommand;
            }
        }

        public void OnError(MvxLocationError obj)
        {
            Mvx.Trace(MvxTraceLevel.Error, "Failed to get location: {0}", obj.Code);
        }

        private void OnLocation(MvxGeoLocation location)
        {
            Lat = location.Coordinates.Latitude;
            Lng = location.Coordinates.Longitude;
            HasGpsLocation = true;
        }

        public bool HasGpsLocation
        {
            get { return _hasGpsLocation; }
            set
            {
                _hasGpsLocation = value;
                RaisePropertyChanged(() => HasGpsLocation);
            }
        }

        public bool Started
        {
            get { return _started; }
            set
            {
                _started = value;
                RaisePropertyChanged(() => Started);
            }
        }

        private double _lat;
        public double Lat
        {
            get { return _lat; }
            set { if (_lat == value) return; _lat = value; RaisePropertyChanged(() => Lat); }
        }


        private double _lng;
        public double Lng
        {
            get { return _lng; }
            set { if (_lng == value) return; _lng = value; RaisePropertyChanged(() => Lng); }
        }
    }
}
