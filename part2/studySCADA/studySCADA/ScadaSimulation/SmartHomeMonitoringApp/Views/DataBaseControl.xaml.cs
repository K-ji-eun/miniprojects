using MahApps.Metro.Controls;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using SmartHomeMonitoringApp.Logics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SmartHomeMonitoringApp.Views
{
    /// <summary>
    /// DataBaseControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DataBaseControl : UserControl
    {
        public bool IsConnected { get; set; }

        public DataBaseControl()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            TxbBrokerUrl.Text = Commons.BROKERHOST;
            TxbMqttTopic.Text = Commons.MQTTTOPIC;
            TxtConnString.Text = Commons.MYSQL_CONNSTRING;

            IsConnected = false;    // 아직 접속이 안되었음
            BtnConnDb.IsChecked = false;

        }

        // 토글버튼 클릭이벤트 핸들러
        private void BtnConnDb_Click(object sender, RoutedEventArgs e)
        {
            if (IsConnected == false)
            {
                BtnConnDb.IsChecked = true;
                IsConnected = true;

                // Mqtt 브로커에 생성
                Commons.MQTT_CLIENT = new uPLibrary.Networking.M2Mqtt.MqttClient(Commons.BROKERHOST);

                try
                {
                    // Mqtt subscribe(구독할) 로직
                    if (Commons.MQTT_CLIENT.IsConnected == false)
                    {
                        // Mqtt 접속
                        Commons.MQTT_CLIENT.MqttMsgPublishReceived += MQTT_CLIENT_MqttMsgPublishReceived;
                        Commons.MQTT_CLIENT.Connect("MONITOR");  // clientId = 모니터
                        Commons.MQTT_CLIENT.Subscribe(new string[] { Commons.MQTTTOPIC },
                            new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE}); // 
                        UpdateLog(">>> MQTT Broker Connected");

                        BtnConnDb.IsChecked = true;
                        IsConnected = true; 
                    } 
                }
                catch 
                {
                    // Pass.
                }

            }
            else 
            {
                BtnConnDb.IsChecked= false;
                IsConnected = false; // 예외발생하면 
            }
        }

        // Subscribe가 발생할 때 이벤트핸들러
        private void MQTT_CLIENT_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var msg = Encoding.UTF8.GetString(e.Message);
            UpdateLog(msg);
            SetToDataBase(msg, e.Topic); // 실제 DB에 저장처리
        }

        private void SetToDataBase(string msg, string topic)
        {
            var currValue = JsonConvert.DeserializeObject<Dictionary<string, string>>(msg);
            if (currValue != null)
                //Debug.WriteLine(currValue["Home_Id"]);
                //Debug.WriteLine(currValue["Room_Name"]);
                //Debug.WriteLine(currValue["Sensing_DateTime"]);
                //Debug.WriteLine(currValue["Temp"]);
                //Debug.WriteLine(currValue["Humid"]);
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(Commons.MYSQL_CONNSTRING))
                    {
                        if (conn.State == System.Data.ConnectionState.Closed) conn.Open();
                        string insQuery = "INSERT INTO smarthomesensor ...";

                        MySqlCommand cmd = new MySqlCommand(insQuery, conn);
                        cmd.Parameters.Add("@Home_Id", currValue["Home_Id"]);
                        // ... 파라미터 다섯개
                        if (cmd.EndExecuteNonQuery() == 1)
                        {
                            UpdateLog(">>> DB Insert succed.");
                        }
                        else
                        {
                            UpdateLog(">>> DB Insert failed."); // 일어날 일이 거의 없음
                        }
                    }
                }
                catch(Exception ex)
                {
                    UpdateLog($"!!! Erorr 발생 : {ex.Message}");
                }
        }

        private void UpdateLog(string msg) 
        {
            // 예외처리 필요!
            this.Invoke(() =>
            {
                TxtLog.Text += $"{msg}\n";
                TxtLog.ScrollToEnd();
            });
        }
    }
}
