using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ATEVersions_Management.Models.HelperModels
{
    public class MQTTConnector
    {
        static private string MQTTServerIPaddress 
        { 
            get 
            {
                return "10.220.99.252";
            }
            set { }
        }
        static private int MQTTPort
        {
            get
            {
                return 1883;
            }
            set { }
        }
        static private string MQTTUser
        {
            get
            {
                return "ubeete";
            }
            set { }
        }
        static private string MQTTPassword
        {
            get
            {
                return "foxconn168!!";
            }
            set { }
        }
        static private string[] MQTTSubcribeTopic
        {
            get
            {
                return new string[] { "SaveEnergy/WOL/0", "SaveEnergy/AIR/0" };
            }
        }
        static private string[] MQTTPublishTopic
        {
            get
            {
                return new string[] { "SaveEnergy/WOL/0", "SaveEnergy/AIR/0" };
            }
        }
        static private byte[] MQTTQoSLevels
        {
            get
            {
                return new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE };
            }            
        } 
        static private MqttClient _MqttClient
        {
            get
            {
                return new MqttClient(MQTTServerIPaddress, MQTTPort, false, MqttSslProtocols.None, null, null);
            }
        }
        static public bool MQTTPublishMessage(string topic, string message, string errorLogDirPath)
        {
            try
            {
                MqttClient MQTTClient = _MqttClient;
                MQTTClient.Connect(Guid.NewGuid().ToString(), MQTTUser, MQTTPassword);

                string publishTopic = MQTTPublishTopic[0];
                if(topic == "AIR")
                {
                    publishTopic = MQTTPublishTopic[1];

                }

                ushort publishAction = MQTTClient.Publish(publishTopic, Encoding.UTF8.GetBytes(message));
                //Task.Delay(611);
                //MQTTClient.Publish(MQTTPublishTopic[0], Encoding.UTF8.GetBytes(mqttMessageContent));
                //LogRecord.WriteErrorLogRecord(errorLogDirPath, DateTime.Now, "MQTTConnection", "MQTTPublishMessage", publishAction.ToString());
                //MQTTClient.Disconnect();
                return true;
                
            }
            catch (Exception ex)
            {
                LogRecord.WriteErrorLogRecord(errorLogDirPath, DateTime.Now, "MQTTConnector", "MQTTPublishMessage", ex.Message);
                return false;
            }
            
        }
        static public bool MQTT_MessageSystem_SendNoticeMessage(string mqttPublishTopic, string mqttMessageContent, string errorLogDirPath)
        {

            try
            {
                MqttClient MQTTClient = _MqttClient;
                MQTTClient.Connect(Guid.NewGuid().ToString(), MQTTUser, MQTTPassword);               
                MQTTClient.Publish(mqttPublishTopic, Encoding.UTF8.GetBytes(mqttMessageContent));
                return true;

            }
            catch (Exception ex)
            {
                LogRecord.WriteErrorLogRecord(errorLogDirPath, DateTime.Now, "MQTTConnector", "MQTT_MessageSystem_SendNoticeMessage", ex.Message);
                return false;
            }

        }
    }
}