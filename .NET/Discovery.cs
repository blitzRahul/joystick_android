using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace vjoyFeeder
{
    internal class Discovery
    {

        public static void brodcast() {

       


            UdpClient sender = new UdpClient(11001);
            sender.EnableBroadcast = true;
            IPEndPoint sendToIP = new IPEndPoint(IPAddress.Parse("192.168.137.255"), 11001);
           
            
            while (true)
            {
                //send brodcast packets every second
                foreach (string i in GetBrodcastAddresses()) {
                    IPEndPoint temp = new IPEndPoint(IPAddress.Parse(i),11001);
                    sender.Send(Encoding.ASCII.GetBytes("VJOY_FEEDER_AVAILABLE"),temp);
                    Console.WriteLine("brodcast sent over: "+i);
                }
                Thread.Sleep(1000);
            }



            
            //IPAddress temp = IPAddress.Parse("127.0.0.1");

            //rahul

            //brodcast to all local ips
        }

        private static IPAddress GetBroadcastAddress(IPAddress address, IPAddress subnetMask)
        {
            byte[] ipAdressBytes = address.GetAddressBytes();
            byte[] subnetMaskBytes = subnetMask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress);
        }

            //static void listenTo

            private static List<string> GetBrodcastAddresses()
        {
            // Get the host name of the local machine
            
            string hostName = Dns.GetHostName();

            // Get the IP addresses of the local machine
            IPAddress[] addresses = Dns.GetHostAddresses(hostName);
                                   
            // Print the IP addresse
            //
            // store all the ips here
            List<string> ips = new List<string>();

          
            foreach (IPAddress address in addresses)
            {
                string temp = address.ToString().Split('.')[0];
                if (temp == "192" || temp=="172")
                {
                    ips.Add(GetBroadcastAddress(address,GetSubnetMask(address)).ToString());
                }

            }

            return ips;
            
        }
        private static IPAddress GetSubnetMask(IPAddress addr) {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (unicastIPAddressInformation.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        if (addr.Equals(unicastIPAddressInformation.Address))
                        {
                            return unicastIPAddressInformation.IPv4Mask;
                        }
                    }
                }
            }
            return IPAddress.None;
        }
    }
}
