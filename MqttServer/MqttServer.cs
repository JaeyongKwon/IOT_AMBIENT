using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using MQTTnet;
using MQTTnet.Diagnostics;
using MQTTnet.Protocol;
using MQTTnet.Server;
using Newtonsoft.Json;

using System.Security.Cryptography.X509Certificates;

namespace MqttServer
{
    public class MqttServer
    {
        public static Boolean bMqttServerWored;
        public static string strMQTTServerPort = "1883";
        public static Boolean bMqttServerReceived = false;
        public static string[] strRcvWord;

        public static string txtReceiveMsg = "";

        public static Dictionary<string, string> dictLocation = new Dictionary<string, string>();
        public static List<string> Location_list = new List<string>();

        private static IMqttServer mqttServer = null;
        private static List<string> connectedClientId = new List<string>();

       // private static System.Timers.Timer TimerMqttRcvData = new System.Timers.Timer();

        public static void MqttStop()
        {
            Task.Run(async () => { await EndMqttServer_2_7_5(); });
			bMqttServerWored = false;
            bMqttServerReceived = false;
        //    TimerMqttRcvData.Dispose();
        //    TimerMqttRcvData.Elapsed -= TimerMqttRcvData_Elapsed;


            Console.WriteLine("MQTT Server Timer Stop！");
        }

        static void TimerMqttRcvData_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (mqttServer == null)
            {
                Console.WriteLine("Please await mqttServer.StartAsync()");
                Thread.Sleep(1000);
            }

            //var inputString = Console.ReadLine().ToLower().Trim();
            var inputString = "";

            if (inputString == "exit")
            {
                Task.Run(async () => { await EndMqttServer_2_7_5(); });
                Console.WriteLine("MQTT Server Task Stop！");
            }
            else if (inputString == "clients")
            {
                var connectedClients = mqttServer.GetConnectedClientsAsync();

                Console.WriteLine($"Cliend ID：");
                //2.4.0
                //foreach (var item in mqttServer.GetConnectedClients())
                //{
                //    Console.WriteLine($"Clientid：{item.ClientId}，Version：{item.ProtocolVersion}");
                //}
            }
            else if (inputString.StartsWith("hello:"))
            { 
                string msg = inputString.Substring(6);
                Topic_Hello(msg);
            }
            else if (inputString.StartsWith("control:"))
            {
                string msg = inputString.Substring(8);
                Topic_Host_Control(msg);
            }
            else if (inputString.StartsWith("serialize:"))
            {
                AllData data = new AllData();
                data.m_data = new EquipmentDataJson();
                data.m_data.str_test = "host";
                data.m_data.str_arr_test = new string[3] { "h1", "h2", "h3" };
                data.m_data.int_test = 5;
                data.m_data.int_arr_test = new int[5] { 2, 4, 5, 8, 10 };
                string msg = JsonConvert.SerializeObject(data.m_data);
                Topic_Serialize(msg);
            }
            else if (inputString.StartsWith("subscribe:"))
            {
                string msg = inputString.Substring(10);
                Subscribe(msg);
            }
            else
            {
               Console.WriteLine($"Command[{inputString}] is not defined！");
            }
            Thread.Sleep(100);
        }

        public static void MqttStart()
        {
            Task.Run(async () => { await StartMqttServer_2_7_5(); });
            // Write all trace messages to the console window.
            MqttNetGlobalLogger.LogMessagePublished += MqttNetTrace_TraceMessagePublished;

            //2.4.0版本
            //MqttNetTrace.TraceMessagePublished += MqttNetTrace_TraceMessagePublished;
            //new Thread(StartMqttServer).Start();

            bMqttServerWored = true;

#if MQTTSERVER_TIMER
            while (true)
            {
                if (mqttServer == null)
                {
                    Console.WriteLine("Please await mqttServer.StartAsync()");
                    Thread.Sleep(1000);
                    continue;
                }

                var inputString = Console.ReadLine().ToLower().Trim();

                if (inputString == "exit")
                {
                    Task.Run(async () => { await EndMqttServer_2_7_5(); });
                    Console.WriteLine("MQTT Server Task Stop！");
                    break;
                }
                else if (inputString == "clients")
                {
                    var connectedClients = mqttServer.GetConnectedClientsAsync();

                    Console.WriteLine($"Cliend ID：");
                    //2.4.0
                    //foreach (var item in mqttServer.GetConnectedClients())
                    //{
                    //    Console.WriteLine($"Clientid：{item.ClientId}，Version：{item.ProtocolVersion}");
                    //}
                }
                else if (inputString.StartsWith("hello:"))
                {
                    string msg = inputString.Substring(6);
                    Topic_Hello(msg);
                }
                else if (inputString.StartsWith("control:"))
                {
                    string msg = inputString.Substring(8);
                    Topic_Host_Control(msg);
                }
                else if (inputString.StartsWith("serialize:"))
                {
                    AllData data = new AllData();
                    data.m_data = new EquipmentDataJson();
                    data.m_data.str_test = "host";
                    data.m_data.str_arr_test = new string[3] { "h1", "h2", "h3" };
                    data.m_data.int_test = 5;
                    data.m_data.int_arr_test = new int[5] { 2, 4, 5, 8, 10 };
                    string msg = JsonConvert.SerializeObject(data.m_data);
                    Topic_Serialize(msg);
                }
                else if (inputString.StartsWith("subscribe:"))
                {
                    string msg = inputString.Substring(10);
                    Subscribe(msg);
                }
                else
                {
                    Console.WriteLine($"Command[{inputString}] is not defined！");
                }
                Thread.Sleep(100);
            }
#else
        //    TimerMqttRcvData.Interval = 1000;
       //     TimerMqttRcvData.Elapsed += new ElapsedEventHandler(TimerMqttRcvData_Elapsed);
        //    TimerMqttRcvData.Start();
#endif
        }

        private static void MqttServer_ClientConnected(object sender, MqttClientConnectedEventArgs e)
        {
            Console.WriteLine($"Client [{e.Client.ClientId}], Version：{e.Client.ProtocolVersion}");
            connectedClientId.Add(e.Client.ClientId);
        }

        private static void MqttServer_ClientDisconnected(object sender, MqttClientDisconnectedEventArgs e)
        {
            Console.WriteLine($"Client [{e.Client.ClientId}] disconnected！");
            connectedClientId.Remove(e.Client.ClientId);
        }

        private static void MqttServer_ApplicationMessageReceived(object sender, MqttApplicationMessageReceivedEventArgs e)
        {
            string recv = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
            Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
            Console.WriteLine($"Client ID [{e.ClientId}]>>");
            Console.WriteLine($"+ Topic = {e.ApplicationMessage.Topic}");
            Console.WriteLine($"+ Payload = {recv}");
            Console.WriteLine($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
            Console.WriteLine($"+ Retain = {e.ApplicationMessage.Retain}");
            Console.WriteLine();

            if (CheckedWatchForm() == true)
            {
                txtReceiveMsg = ("### RECEIVED APPLICATION MESSAGE ###");
                txtReceiveMsg += ($"Client ID [{e.ClientId}]>>");
                txtReceiveMsg += ($"+ Topic = {e.ApplicationMessage.Topic}");
                txtReceiveMsg += ($"+ Payload = {recv}");
                txtReceiveMsg += ($"+ QoS = {e.ApplicationMessage.QualityOfServiceLevel}");
                txtReceiveMsg += ($"+ Retain = {e.ApplicationMessage.Retain}");

                SendDataWatch(txtReceiveMsg);
            }

            if (e.ApplicationMessage.Topic == "slave/json")
            {
                JsonData(recv);
            }
            else if (e.ApplicationMessage.Topic == "AudioLevel")
            {
                bMqttServerReceived = true;
                PayloadData(recv);
            }
        }

        public static void SendDataWatch(string _recvStr)
        {
            try
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form.GetType() == typeof(Message))
                    {
                        Message.GetInstance.ControlChange(_recvStr);
                    }
                }
            }
            catch (Exception e1)
            {
                Console.WriteLine("Utils Watch.ControlChange Error >> " + e1.ToString());
            }
        }

        public static bool CheckedWatchForm()
        {
            return Message.GetInstance.Visible;
        }

        private static void MqttNetTrace_TraceMessagePublished(object sender, MqttNetLogMessagePublishedEventArgs e)
        {
            var trace = $">> [{e.TraceMessage.Timestamp:O}] [{e.TraceMessage.ThreadId}] [{e.TraceMessage.Source}] [{e.TraceMessage.Level}]: {e.TraceMessage.Message}";
            if (e.TraceMessage.Exception != null)
            {
                trace += Environment.NewLine + e.TraceMessage.Exception.ToString();
            }

            Console.WriteLine(trace);
        }

#region 2.7.5

        private static async Task StartMqttServer_2_7_5()
        {
            if (mqttServer == null)
            {
                // Configure MQTT server.
                var optionsBuilder = new MqttServerOptionsBuilder()
                    .WithConnectionBacklog(100)
                    .WithDefaultEndpointPort(Convert.ToUInt16(strMQTTServerPort))
                    //.WithConnectionValidator(ValidatingMqttClients()) // Ignore ClientID, Username, Password 
                    ;

                // Start a MQTT server.
                mqttServer = new MqttFactory().CreateMqttServer();
                mqttServer.ApplicationMessageReceived += MqttServer_ApplicationMessageReceived;
                mqttServer.ClientConnected += MqttServer_ClientConnected;
                mqttServer.ClientDisconnected += MqttServer_ClientDisconnected;

                Task.Run(async () => { await mqttServer.StartAsync(optionsBuilder.Build()); });
                //mqttServer.StartAsync(optionsBuilder.Build());
                Console.WriteLine("MQTT Server Start Successfully！");
            }
            else
            {
                Console.WriteLine("MQTT Server is not NULL！");
            }
        }

        private static async Task EndMqttServer_2_7_5()
        {
            if (mqttServer != null)
            {
                await mqttServer.StopAsync();

                mqttServer = null;
            }
            else
            {
                Console.WriteLine("mqttserver=null");
            }
        }

        private static Action<MqttConnectionValidatorContext> ValidatingMqttClients()
        {
            // Setup client validator.    
            var options = new MqttServerOptions();
            options.ConnectionValidator = c =>
            {
                Dictionary<string, string> c_u = new Dictionary<string, string>();
                c_u.Add("client001", "username001");
                c_u.Add("client002", "username002");
                Dictionary<string, string> u_psw = new Dictionary<string, string>();
                u_psw.Add("username001", "psw001");
                u_psw.Add("username002", "psw002");

#if SIMPLE
                c.ReturnCode = MqttConnectReturnCode.ConnectionAccepted;
#else
                if (c_u.ContainsKey(c.ClientId) && c_u[c.ClientId] == c.Username)
                {
                    if (u_psw.ContainsKey(c.Username) && u_psw[c.Username] == c.Password)
                    {
                        c.ReturnCode = MqttConnectReturnCode.ConnectionAccepted;
                    }
                    else
                    {
                        c.ReturnCode = MqttConnectReturnCode.ConnectionRefusedBadUsernameOrPassword;
                    }
                }
                else
                {
                    c.ReturnCode = MqttConnectReturnCode.ConnectionRefusedIdentifierRejected;
                }
#endif
            };


            return options.ConnectionValidator;
        }

        private static void Usingcertificate(ref MqttServerOptions options)
        {
            var certificate = new X509Certificate(@"C:\certs\test\test.cer", "");
            options.TlsEndpointOptions.Certificate = certificate.Export(X509ContentType.Cert);
            var aes = new System.Security.Cryptography.AesManaged();

        }

#endregion

#region Topic

        private static async void Topic_Hello(string msg)
        {
            string topic = "topic/hello";

            //2.4.0 MQTTNet Version
            //var appMsg = new MqttApplicationMessage(topic, Encoding.UTF8.GetBytes(inputString), MqttQualityOfServiceLevel.AtMostOnce, false);
            //mqttClient.PublishAsync(appMsg);

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(msg)
                .WithAtMostOnceQoS()
                .WithRetainFlag()
                .Build();
            await mqttServer.PublishAsync(message);
        }

        private static async void Topic_Host_Control(string msg)
        {
            string topic = "topic/host/control";

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(msg)
                .WithAtMostOnceQoS()
                .WithRetainFlag(false)
                .Build();
            await mqttServer.PublishAsync(message);
        }

        private static async void Topic_Serialize(string msg)
        {
            string topic = "topic/serialize";

            var message = new MqttApplicationMessageBuilder()
                .WithTopic(topic)
                .WithPayload(msg)
                .WithAtMostOnceQoS()
                .WithRetainFlag(false)
                .Build();
            await mqttServer.PublishAsync(message);
        }

        /// </summary>
        /// <param name="topic"></param>
        private static void Subscribe(string topic)
        {
            List<TopicFilter> topicFilter = new List<TopicFilter>();
            topicFilter.Add(new TopicFilterBuilder()
                .WithTopic(topic)
                .WithAtMostOnceQoS()
                .Build());

            mqttServer.SubscribeAsync("client001", topicFilter);
            Console.WriteLine($"Subscribe:[{"client001"}]，Topic：{topic}");
        }

#endregion

#region JsonSerialize

        private static void JsonData(string recvPayload)
        {
            AllData data = new AllData();
            data.m_data = (EquipmentDataJson)JsonConvert.DeserializeObject(recvPayload, typeof(EquipmentDataJson));
            Console.Write($"recv: str_test={data.m_data.str_test}, str_arr_test={data.m_data.str_arr_test}");
            Console.Write($"recv: int_test={data.m_data.int_test}, int_arr_test={data.m_data.int_arr_test}");
            Console.WriteLine("");
        }

        #endregion

        private static void PayloadData(string recvPayload)
        {
            int index = recvPayload.IndexOf(',');

            //Console.WriteLine("recvPayload : {0}", recvPayload.Length);
            //Console.WriteLine("index : {0}", index);

            strRcvWord = recvPayload.Split(',');

            Console.WriteLine("strWords[0] : {0}", strRcvWord[0]);
            Console.WriteLine("strWords[1] : {0}", strRcvWord[1].Trim());

            string[] strLocation = strRcvWord[0].Split('=');
            string[] strValue = strRcvWord[1].Split('=');

            if(Location_list.Contains(strLocation[1]))
            {
                dictLocation[strLocation[1]] = strValue[1];
            }
            else
            {
                Location_list.Add(strLocation[1]);
                dictLocation.Add(strLocation[1], strValue[1]);
            }
        }
    }
}

