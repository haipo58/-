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
        // 司控台 -> 3D 的数据包
        private Controler2D3 controlerPack = new Controler2D3();

        // 串口
        private SerialPort serial = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);

        /// <summary>
        /// 构造
        /// </summary>
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
        
        /// <summary>
        /// 创建 UDP 测试通信
        /// </summary>
        /// <param name="port"></param>
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

        /// <summary>
        /// 输出普通信息
        /// </summary>
        /// <param name="message"></param>
        private void LogInfo(string message)
        {
            Log(message, Brushes.Black);
        }

        /// <summary>
        /// 读取串口数据线程
        /// </summary>
        private void CreateSerialThread()
        {
            byte[] buf = new byte[1024];

            Task.Run(() =>
            {
                while (true)
                {
                    int nBytes = serial.Read(buf, 0, buf.Length);
                    if (nBytes > 1)
                    {
                        byte[] newBuf = new byte[nBytes + 1];
                        Array.Copy(buf, 0, newBuf, 1, nBytes);
                        newBuf[0] = 0xAA;
                        UpdateControlerPackView(buf);
                    }
                }
            });
        }

        /// <summary>
        /// 解包并更新显示
        /// </summary>
        /// <param name="buf">收到的数据</param>
        private void UpdateControlerPackView(byte[] buf)
        {
            controlerPack.Unpack(buf);
            Dispatcher.Invoke(() =>
            {
                ControllerPanel.DataContext = null;
                ControllerPanel.DataContext = controlerPack;
            });
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="message">错误信息</param>
        public void LogError(string message)
        {
            Log(message, Brushes.Red);
        }

        /// <summary>
        /// 输出信息
        /// </summary>
        /// <param name="message">信息</param>
        /// <param name="foreground">文字颜色</param>
        private void Log(string message, Brush foreground)
        {
            ListBoxItem item = new ListBoxItem() { Content = message, Foreground = foreground };
            LogListBox.Items.Add(item);
            LogListBox.ScrollIntoView(item);
        }
    }
}
