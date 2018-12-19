using System;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace 司控台通信
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Controler2D3 controlerPack = new Controler2D3();
        private SerialPort serial = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);

        public MainWindow()
        {
            InitializeComponent();
            ControllerPanel.DataContext = controlerPack;

            try
            {
                serial.Open();
                CreateSerialThread();
            }
            catch (Exception e)
            {
                LogError("打开串口失败：" + e.Message);
                int port = 5080;
                LogInfo($"使用 Udp 测试，监听端口：{port}。");
                CreateUdpThread(port);
            }
        }
        
        private void CreateUdpThread(int port)
        {
            UdpClient client = new UdpClient(port);
            IPEndPoint sender = new IPEndPoint(0, 0);
            // 测试数据：aa 55 02 00 01 00 01 03 ff 05 0a 13 55 55 55 55 55 55 55 55 55 55 55 55 55 55 55 55 55 55 55

            Task.Run(() =>
            {
                while (true)
                {
                    byte[] buf = client.Receive(ref sender);
                    UpdateControlerPackView(buf);
                }
            });
        }

        private void LogInfo(string message)
        {
            Log(message, Brushes.Black);
        }

        private void CreateSerialThread()
        {
            byte[] buf = new byte[1024];

            Task.Run(() =>
            {
                while (true)
                {
                    int nBytes = serial.Read(buf, 0, buf.Length);
                    UpdateControlerPackView(buf);
                }
            });
        }

        private void UpdateControlerPackView(byte[] buf)
        {
            controlerPack.Unpack(buf);
            Dispatcher.Invoke(() =>
            {
                ControllerPanel.DataContext = null;
                ControllerPanel.DataContext = controlerPack;
            });
        }

        public void LogError(string message)
        {
            Log(message, Brushes.Red);
        }

        private void Log(string message, Brush foreground)
        {
            ListBoxItem item = new ListBoxItem() { Content = message, Foreground = foreground };
            LogListBox.Items.Add(item);
            LogListBox.ScrollIntoView(item);
        }
    }
}
