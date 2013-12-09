using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            // instantiate the sensor instance
            KinectSensor sensor = KinectSensor.KinectSensors[0];
            // initialize the cameras
            sensor.DepthStream.Enable();
            sensor.DepthFrameReady += sensor_DepthFrameReady;          
            // make it look like The Matrix
            Console.ForegroundColor = ConsoleColor.Green;
            // start the data streaming
            sensor.Start();
            while (Console.ReadKey().Key != ConsoleKey.Spacebar) { }
        }

        static void sensor_DepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
        {
            using (var depthFrame = e.OpenDepthImageFrame())
            {
                if (depthFrame == null)
                    return;
                short[] bits = new short[depthFrame.PixelDataLength];
                depthFrame.CopyPixelDataTo(bits);
                foreach (var bit in bits)
                    Console.Write(bit);
            }
        }
    }

    public partial class MainWindow : Window
    {
        #region Member Variables
        private KinectSensor _Kinect;
        #endregion Member Variables
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += (s, e) => { DiscoverKinectSensor(); };
            this.Unloaded += (s, e) => { this.Kinect = null; };
        }
        #endregion Constructor

        #region Methods
        private void DiscoverKinectSensor()
        {
            KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;
            this.Kinect = KinectSensor.KinectSensors
                .FirstOrDefault(x => x.Status == KinectStatus.Connected);
        }
        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case KinectStatus.Connected:
                    if (this.Kinect == null)
                    {
                        this.Kinect = e.Sensor;
                    }
                    break;
                case KinectStatus.Disconnected:
                    if (this.Kinect == e.Sensor)
                    {
                        this.Kinect = null;
                        this.Kinect = KinectSensor.KinectSensors
                            .FirstOrDefault(x => x.Status == KinectStatus.Connected);
                        if (this.Kinect == null)
                        {
                            //Notify the user that the sensor is disconnected
                        }
                    }
                    break;
                //Handle all other statuses according to needs
            }
        }
        #region Properties
        public KinectSensor Kinect
        {
            get { return this._Kinect; }
            set
            {
                if (this._Kinect != value)
                {
                    if (this._Kinect != null)
                    {
                        //Uninitialize
                        this._Kinect = null;
                    }
                    if (value != null && value.Status == KinectStatus.Connected)
                    {
                        this._Kinect = value;
                        //Initialize
                    }
                }
            }
        }
        #endregion Properties
    }
}