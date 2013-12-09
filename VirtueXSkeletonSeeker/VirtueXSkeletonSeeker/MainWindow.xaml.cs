using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
      
        #region Member Variables
        private KinectSensor _KinectDevice;
        private readonly System.Windows.Media.Brush[] _SkeletonBrushes;
        private Skeleton[] _FrameSkeletons;
        int i=39; //start number of the image , TODO should read from the file the last number and increment
        String str;
       
        
        #endregion Member Variables
        
        
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
             Process currentProcess = Process.GetCurrentProcess();
             str = currentProcess.ProcessName;

            this._SkeletonBrushes = new[] { System.Windows.Media.Brushes.Black, System.Windows.Media.Brushes.Crimson, System.Windows.Media.Brushes.Indigo,
                System.Windows.Media.Brushes.DodgerBlue, System.Windows.Media.Brushes.Purple, System.Windows.Media.Brushes.Pink };

            KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;
            this.KinectDevice = KinectSensor.KinectSensors
                .FirstOrDefault(x => x.Status == KinectStatus.Connected);
        }
        #endregion Constructor

        #region Methods
        public void StartTimer(object o, RoutedEventArgs sender)
        {
            System.Windows.Threading.DispatcherTimer myDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            myDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 500); // 100 Milliseconds 
            myDispatcherTimer.Tick += new EventHandler(Each_Tick);
            myDispatcherTimer.Start();
        }
        public void Each_Tick(object o, EventArgs sender)
        {
                myTextBlock.Text = "Count up: " + i++.ToString();
                CaptureApplication(str);
        }
        private void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case KinectStatus.Initializing:
                case KinectStatus.Connected:
                    this.KinectDevice = e.Sensor;
                    break;
                case KinectStatus.Disconnected:
                    //TODO: Give the user feedback to plug-in a Kinect device.
                    this.KinectDevice = null;
                    break;
                default:
                    //TODO: Show an error state
                    break;
            }
        }

        private void KinectDevice_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            using(SkeletonFrame frame = e.OpenSkeletonFrame())
            {
                if(frame != null)
                {
                    Polyline figure;
                    System.Windows.Media.Brush userBrush;
                    Skeleton skeleton;

                    LayoutRoot.Children.Clear();
                    frame.CopySkeletonDataTo(this._FrameSkeletons);

                    for(int i = 0; i < this._FrameSkeletons.Length; i++)
                    {
                        skeleton = this._FrameSkeletons[i];

                        if(skeleton.TrackingState == SkeletonTrackingState.Tracked)
                        {
                            userBrush = this._SkeletonBrushes[i % this._SkeletonBrushes.Length];

                            //Draws the skeleton’s head and torso
                            joints = new [] { JointType.Head, JointType.ShoulderCenter,
                                JointType.ShoulderLeft, JointType.Spine,
                                JointType.ShoulderRight, JointType.ShoulderCenter,
                                JointType.HipCenter, JointType.HipLeft,
                                JointType.Spine, JointType.HipRight,
                                JointType.HipCenter };
                            LayoutRoot.Children.Add(CreateFigure1(skeleton, userBrush, joints));
                            
                            //Draws the skeleton’s left leg
                            joints = new [] { JointType.HipLeft, JointType.KneeLeft,
                                JointType.AnkleLeft, JointType.FootLeft };
                            LayoutRoot.Children.Add(CreateFigure1(skeleton, userBrush, joints));

                            //Draws the skeleton’s right leg
                            joints = new [] { JointType.HipRight, JointType.KneeRight,
                                JointType.AnkleRight, JointType.FootRight };
                            LayoutRoot.Children.Add(CreateFigure1(skeleton, userBrush, joints));
                            
                            //Draws the skeleton’s left arm
                            joints = new [] { JointType.ShoulderLeft, JointType.ElbowLeft,
                                JointType.WristLeft, JointType.HandLeft };
                            LayoutRoot.Children.Add(CreateFigure1(skeleton, userBrush, joints));
                            
                            //Draws the skeleton’s right arm
                            joints = new [] { JointType.ShoulderRight, JointType.ElbowRight,
                                JointType.WristRight, JointType.HandRight };
                            LayoutRoot.Children.Add(CreateFigure1(skeleton, userBrush, joints));
                        }
                    }

                    

                    //SaveFile();
                }
            }
           
        }

        private Polyline CreateFigure1(Skeleton skeleton, System.Windows.Media.Brush brush, JointType[] joints)
        {
            System.Windows.Shapes.Polyline figure = new System.Windows.Shapes.Polyline();
            figure.StrokeThickness = 8;
            figure.Stroke = brush;
            for (int i = 0; i < joints.Length; i++)
            {
                figure.Points.Add(GetJointPoint(skeleton.Joints[joints[i]]));
            }
            return figure;
        }
        private System.Windows.Point GetJointPoint(Joint joint)
        {
            DepthImagePoint point = this.KinectDevice.MapSkeletonPointToDepth(joint.Position,
            this.KinectDevice.DepthStream.Format);
            point.X *= (int)this.LayoutRoot.ActualWidth / this.KinectDevice.DepthStream.FrameWidth;
            point.Y *= (int)this.LayoutRoot.ActualHeight / this.KinectDevice.DepthStream.FrameHeight;
            return new System.Windows.Point(point.X, point.Y);
        }

        #endregion Methods


        #region Properties
        public KinectSensor KinectDevice
        {
            get { return this._KinectDevice; }
            set
            {
                if (this._KinectDevice != value)
                {
                    //Uninitialize
                    if (this._KinectDevice != null)
                    {
                        this._KinectDevice.Stop();
                        this._KinectDevice.SkeletonFrameReady -= KinectDevice_SkeletonFrameReady;
                        this._KinectDevice.SkeletonStream.Disable();
                        this._FrameSkeletons = null;
                    }
                    this._KinectDevice = value;
                    //Initialize
                    if (this._KinectDevice != null)
                    {
                        if (this._KinectDevice.Status == KinectStatus.Connected)
                        {
                            this._KinectDevice.SkeletonStream.Enable();
                            this._FrameSkeletons = new Skeleton[this._KinectDevice.SkeletonStream.FrameSkeletonArrayLength];
                            this.KinectDevice.SkeletonFrameReady += KinectDevice_SkeletonFrameReady;
                            this._KinectDevice.Start();
                        }
                    }
                }
            }
        }

        private void TakePictureButton_Click(object sender, RoutedEventArgs e)
        {
            string fileName = "snapshotSkeleton.jpg";
            for (int i = 1; File.Exists(fileName); i++)
            {
                String file = i.ToString();
                fileName = "snapshotSkeleton" + file + ".jpg";
            }
            using (FileStream savedSnapshot = new FileStream(fileName, FileMode.CreateNew))
            {
                BitmapSource image = (BitmapSource)VideoStreamElement.Source;
                JpegBitmapEncoder jpgEncoder = new JpegBitmapEncoder();
                jpgEncoder.QualityLevel = 70;
                jpgEncoder.Frames.Add(BitmapFrame.Create(image));
                jpgEncoder.Save(savedSnapshot);

                savedSnapshot.Flush();
                savedSnapshot.Close();
                savedSnapshot.Dispose();
            }
        }
        public void CaptureApplication(string procName)
        {
            var proc = Process.GetProcessesByName(procName)[0];
            var rect = new User32.Rect();
            User32.GetWindowRect(proc.MainWindowHandle, ref rect);

            int width = rect.right - rect.left;
            int height = rect.bottom - rect.top;

            var bmp = new System.Drawing.Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            System.Drawing.Graphics graphics = System.Drawing.Graphics.FromImage(bmp);
            graphics.CopyFromScreen(rect.left, rect.top, 0, 0, new System.Drawing.Size(width, height), System.Drawing.CopyPixelOperation.SourceCopy);
            bmp.Save("C:\\Users\\Rikesh\\Documents\\Kinect\\VirtueX\\TestersDatabase\\Skeleton\\Skeleton_files\\Skeleton"+i.ToString()+".jpg", ImageFormat.Jpeg);
        }

        private class User32
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct Rect
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [DllImport("user32.dll")]
            public static extern IntPtr GetWindowRect(IntPtr hWnd, ref Rect rect);
        }
        private void SaveFile(String strProcessName)
        {
            
          //  Process currentProcess = Process.GetCurrentProcess();
          //  str = currentProcess.ProcessName;
            CaptureApplication(strProcessName);
        }

        #endregion Properties

        public JointType[] joints { get; set; }

        private void ColorImageElement_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }
    }
}
