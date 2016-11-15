using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Sensors;
using Windows.UI.Core;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace lightsensor
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private LightSensor _lightSensor;
        private OrientationSensor _orientationsensor;
        
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            _x_tbx_x.Text = "";
            _x_tbx_y.Text = ""; 
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            _lightSensor = LightSensor.GetDefault();
            _orientationsensor = OrientationSensor.GetDefault();
            if (_lightSensor != null)
            {
                // Establish the report interval for all scenarios
                uint minReportInterval = _lightSensor.MinimumReportInterval;
                uint reportInterval = minReportInterval > 16 ? minReportInterval : 16;
                _lightSensor.ReportInterval = reportInterval;

                // Establish the event handler
                _lightSensor.ReadingChanged += new TypedEventHandler<LightSensor, LightSensorReadingChangedEventArgs>(ReadingChanged);
            }
            if (_orientationsensor != null)
            {
                // Establish the report interval for all scenarios
                uint minReportInterval = _orientationsensor.MinimumReportInterval;
                uint reportInterval = minReportInterval > 16 ? minReportInterval : 16;
                _orientationsensor.ReportInterval = reportInterval;

                // Establish the event handler
                _orientationsensor.ReadingChanged += new TypedEventHandler<OrientationSensor, OrientationSensorReadingChangedEventArgs>(ReadingChangedorient);
            }

            

        }

        private async void ReadingChangedorient(OrientationSensor sender, OrientationSensorReadingChangedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                OrientationSensorReading reading = args.Reading;
                //_x_tbx_oriX.Text = String.Format("{0,5:0.00}", reading.Quaternion);
                //_x_tbx_oriY.Text = String.Format("{ 0,5:0.00}", reading.RotationMatrix);
                //_x_tbx_oriZ.Text = String.Format("{ 0,5:0.00}", reading.YawAccuracy);
            });
            
        }

        private async void ReadingChanged(LightSensor sender, LightSensorReadingChangedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                int result = 0;
                float _visibility_reading;
                LightSensorReading reading = args.Reading;
                _visibility_reading = reading.IlluminanceInLux;
                _x_tbx_x.Text = String.Format("{0,5:0.00}", _visibility_reading);
                 
                if (( _visibility_reading <50))
                {
                    _x_tbx_y.Visibility = Visibility.Visible;
                    _x_tbx_y.Text = "Light not sufficient for Study";
                }
                
                else if (Convert.ToInt16(_visibility_reading) > 50 && Convert.ToInt16(_visibility_reading) < 200)
                {
                    _x_tbx_y.Visibility = Visibility.Visible;
                    _x_tbx_y.Text = "Light Safe for study";
                }

                else if (Convert.ToInt16(_visibility_reading) > 200)
                {
                    _x_tbx_y.Visibility = Visibility.Visible;
                    _x_tbx_y.Text = "Light not safe for study";
                }
                else
                {
                    _x_tbx_y.Visibility = Visibility.Collapsed;
                }
                
            });
        }
    }
}
