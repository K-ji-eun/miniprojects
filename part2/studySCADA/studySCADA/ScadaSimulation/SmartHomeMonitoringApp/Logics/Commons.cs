using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;

namespace SmartHomeMonitoringApp.Logics
{
    public class Commons
    {
        // 화면마다 공유할 MQTT 브로커 IP
        public static string BROKERHOST = "127.0.0.1";

        public static string MQTTTOPIC = "SmartHome/IoTData/";

        public static string MYSQL_CONNSTRING = "Sever=localhost;" +
                                                "Port=3306;" +
                                                "Database=miniproject;" +
                                                "Uid=root;" +
                                                "Pwd=12345;";

        // MQTT 클라이언트 공용 객체
        public static MqttClient MQTT_CLIENT {  get; set; }
    }
}
