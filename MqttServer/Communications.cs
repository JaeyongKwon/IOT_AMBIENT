using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MQTTnet;
using MQTTnet.Client;

namespace MqttServer
{
    public class Communications
    {
        private IMqttClient mqttClient = null;
        private bool isReconnect = true;
        public string ReceiveMsg = "";
        public string FlagMsg = "";
        //public string username = "host002";
        //public string password = "psw002";
        //public string clientId = "client002";

        public string username = "";
        public string password = "";
        public string clientId = "ESP8266Client";

        public static string strPayloadMsg = "";

        private async Task Publish()
        {
            string topic = "host/datetime";

            if (string.IsNullOrEmpty(topic))
            {
                throw new Exception("topic Empty!");
            }

            string inputString = DateTime.UtcNow.ToLongTimeString();
            //2.4.Version
            //var appMsg = new MqttApplicationMessage(topic, Encoding.UTF8.GetBytes(inputString), MqttQualityOfServiceLevel.AtMostOnce, false);
            //mqttClient.PublishAsync(appMsg);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(inputString)
                .WithAtMostOnceQoS()
                .WithRetainFlag(true)
                .Build();

            await mqttClient.PublishAsync(message);
        }

        private async Task Subscribe()
        {
            string topic = "slave/datetime";

            if (string.IsNullOrEmpty(topic))
            {
                throw new Exception("Subscribe Empty！");
            }

            if (!mqttClient.IsConnected)
            {
                throw new Exception("MQTT Client Not Connected！");
            }

            // Subscribe to a topic
            await mqttClient.SubscribeAsync(new TopicFilterBuilder()
                .WithTopic(topic)
                .WithAtMostOnceQoS()
                .Build()
                );

            FlagMsg += ($"Topic[{topic}] Environment {Environment.NewLine}");
        }

        private async Task ConnectMqttServerAsync()
        {
            // Create a new MQTT client.
            if (mqttClient == null)
            {
                var factory = new MqttFactory();
                mqttClient = factory.CreateMqttClient();

                mqttClient.ApplicationMessageReceived += MqttClient_ApplicationMessageReceived;
                mqttClient.Connected += MqttClient_Connected;
                mqttClient.Disconnected += MqttClient_Disconnected;
            }

            //
            try
            {
                //Create TCP based options using the builder.
                var options = new MqttClientOptionsBuilder()
                    .WithClientId(clientId)
                    .WithTcpServer("127.0.0.1", 8222)
                    .WithCredentials(username, password)
                    //.WithTls()
                    .WithCleanSession()
                    .Build();

                //// For .NET Framwork & netstandard apps:
                //MqttTcpChannel.CustomCertificateValidationCallback = (x509Certificate, x509Chain, sslPolicyErrors, mqttClientTcpOptions) =>
                //{
                //    if (mqttClientTcpOptions.Server == "server_with_revoked_cert")
                //    {
                //        return true;
                //    }

                //    return false;
                //};

                await mqttClient.ConnectAsync(options);
            }
            catch (Exception ex)
            {
                FlagMsg += ($"MQTT Server not Connected！" + Environment.NewLine + ex.Message + Environment.NewLine);
            }
        }

        private void MqttClient_Connected(object sender, EventArgs e)
        {
            FlagMsg = ("MQTT Server Connected！" + Environment.NewLine);
        }

        private async void MqttClient_Disconnected(object sender, EventArgs e)
        {
            DateTime curTime = new DateTime();
            curTime = DateTime.UtcNow;
            FlagMsg = ($">> [{curTime.ToLongTimeString()}]");
            FlagMsg += ("MQTT Server disconnected！" + Environment.NewLine);

            //Reconnecting
            if (isReconnect)
            {
                FlagMsg += ("Reconnecting" + Environment.NewLine);

                var options = new MqttClientOptionsBuilder()
                    .WithClientId(clientId)
                    .WithTcpServer("127.0.0.1", 8222)
                    .WithCredentials(username, password)
                    //.WithTls()
                    .WithCleanSession()
                    .Build();
                await Task.Delay(TimeSpan.FromSeconds(5));
                try
                {
                    await mqttClient.ConnectAsync(options);
                }
                catch
                {
                    FlagMsg += ("### RECONNECTING FAILED ###" + Environment.NewLine);
                }
            }
            else
            {
                FlagMsg += ("OFFLine！" + Environment.NewLine);
            }
        }

        private void MqttClient_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            ReceiveMsg = ($">> {"### RECEIVED APPLICATION MESSAGE ###"}{Environment.NewLine}");
            ReceiveMsg += ($">> Topic = {e.ApplicationMessage.Topic}{Environment.NewLine}");
            ReceiveMsg += ($">> Payload = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}{Environment.NewLine}");
            ReceiveMsg += ($">> QoS = {e.ApplicationMessage.QualityOfServiceLevel}{Environment.NewLine}");
            ReceiveMsg += ($">> Retain = {e.ApplicationMessage.Retain}{Environment.NewLine}");
        }
    }
}
