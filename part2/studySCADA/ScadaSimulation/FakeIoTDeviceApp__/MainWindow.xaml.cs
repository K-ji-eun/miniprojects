using Bogus;
using FakeIoTDeviceApp.Models;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using uPLibrary.Networking.M2Mqtt;

namespace FakerIoTDeviceApp
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        Faker<SensorInfo> FakeHomeSensor = null; // 가짜 스마트홈 센서값 변수

        MqttClient client;
        Thread MqttThread { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            InitFakeData();
        }

        private void InitFakeData()
        {
            var Rooms = new[] { "Bed", "Bath", "Living", "Dining" };

            FakeHomeSensor = new Faker<SensorInfo>()
                .RuleFor(s => s.Home_Id, "D101H703") // 임의로 픽스된 홈아이디 101동 703호
                .RuleFor(s => s.Room_Name, f => f.PickRandom(Rooms)) // 실행할때마다 방이름이 계속변경
                .RuleFor(s => s.Sensing_DateTime, f => f.Date.Past(0)) // 현재시각이 생성
                .RuleFor(s => s.Temp, f => f.Random.Float(20.0f, 30.0f)) // 20~ 30도 사이의 실수값 생성
                .RuleFor(s => s.Humid, f => f.Random.Float(40.0f, 64.0f)); // 40~ 64% 사이의 습도값
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtMqttBrokerIp.Text))
            {
                //await this.ShowMessageAsync("오류", "브로커 아이피를 입력하세요");
                return;
            }

            // 브로커 아이피로 접속
            ConnectMqttBroker();

            // 하위의 로직을 무한반복 
            StartPublish();

        }

        private void StartPublish()
        {

            MqttThread = new Thread(() =>
            {
                while (true)
                {
                    // 가짜 스마트홈 센서값 생성
                    SensorInfo info = FakeHomeSensor.Generate();
                    Debug.WriteLine($"{info.Home_Id} / {info.Room_Name} / {info.Sensing_DateTime} / {info.Temp}");

                    // 센서값 MQTT 브로커에 전송(Publish)

                    // RtbLog에 출력

                    // 1초 동안 대기
                    Thread.Sleep(1000);
                }
            });
            MqttThread.Start();
        }

        private void ConnectMqttBroker()
        {

            client = new MqttClient(TxtMqttBrokerIp.Text);
            client.Connect("SmartHomeDev"); // publish client Id를 지정 
        }
    }
}