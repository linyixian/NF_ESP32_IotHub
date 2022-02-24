using System;
using System.Diagnostics;
using System.Threading;

//追加
using System.Device.WiFi;
using nanoFramework.Networking;
using nanoFramework.Azure.Devices.Client;


namespace NF_ESP32_IotHub
{
    public class Program
    {
        const string ssid = "YOUR SSID";
        const string password = "PASSWORD";
        const string deviceId = "DEVICEID";
        const string saskey = "SASKEY";
        const string iothubAddress = "YOUR IOTHUB ADDRESS";

        public static void Main()
        {
            if (!ConnectWifi())
            {
                Debug.WriteLine("Connection fail...");
                return;
            }
            else
            {
                Debug.WriteLine("Connected...");
            }

            //クライアント
            DeviceClient client = new DeviceClient(iothubAddress, deviceId, saskey);

            var isOpen=client.Open();
            Debug.WriteLine($"Connection is open: {isOpen}");

            for (int i = 0; i < 10; i++)
            {
                client.SendMessage("Hello IoTHub!");
                Debug.WriteLine("Send Message...");
                Thread.Sleep(10000);
            }
            Thread.Sleep(Timeout.Infinite);
        }

        private static bool ConnectWifi()
        {
            Debug.WriteLine("Connecting WiFi");

            var success = WiFiNetworkHelper.ConnectDhcp(ssid, password, reconnectionKind: WiFiReconnectionKind.Automatic, requiresDateTime: true, token: new CancellationTokenSource(60000).Token);

            if (!success)
            {
                Debug.WriteLine($"Can't connect to the network, error: {WiFiNetworkHelper.Status}");
                if (WiFiNetworkHelper.HelperException != null)
                {
                    Debug.WriteLine($"ex: {WiFiNetworkHelper.HelperException}");
                }
            }

            Debug.WriteLine($"Date and time is now {DateTime.UtcNow}");

            return success;
        }
    }
}
