using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using vJoyInterfaceWrap;
using System.Data.SqlTypes;

namespace vjoyFeeder
{
    //purpose of this class is to handle listening operations
     static class UListen
    {
        static float alpha = 0.5f;
        static float mean =0f;
        static float variance = 0f;

        static float beta() {
            return 1 - alpha;
        }

        static float stdev() { 
        return (float)Math.Sqrt(variance);
        }

        static void updateExpMean(float newValue) {
             float redMean = beta() * mean;
            float meanIncrement = alpha * newValue;
            float newMean=redMean+ meanIncrement;
            float varianceIncrement = alpha * (float)Math.Pow(newValue-mean,2);
            float newVariance = beta() * (variance+varianceIncrement);
            mean=newMean;
            variance=newVariance;
        }

        public static void ListenBrodcast(vJoy joystick, uint id)
        {
            //listens to brodcast messages, replies with IP
            if (true)
            {
                UdpClient listener = new UdpClient(11000);
                IPEndPoint listenToIP = new IPEndPoint(IPAddress.Any, 11000);
               
                try
                {
                    while (true)
                    {
                        //listen to brodcast initially, then it will automatically switch to the sender's ip
                     
                        byte[] buffer = listener.Receive(ref listenToIP);
                        //the sender's ip is stored in listenToIP
                        string message = Encoding.ASCII.GetString(buffer, 0
                            , buffer.Length);
                        //get the message
                        string[] split_message = message.Split(',');
                       // Console.WriteLine(message);
                        if (split_message[0] == "VJOY_DATA") {
                            
                            //1 is WHL
                            //2 is ACL
                            //3 is BRK
                            //obtained x axis, update vjoy
                            int temp_value = int.Parse(split_message[1]);
                            updateExpMean(temp_value);
                            temp_value = (int)Math.Round(mean);

                            joystick.SetAxis(temp_value,id,HID_USAGES.HID_USAGE_X);
                            temp_value = int.Parse(split_message[2]);
                            joystick.SetAxis(temp_value,id,HID_USAGES.HID_USAGE_Y);
                            temp_value = int.Parse(split_message[3]);
                            joystick.SetAxis(temp_value,id,HID_USAGES.HID_USAGE_Z);
                            temp_value = int.Parse(split_message[4]);
                            joystick.SetAxis(temp_value,id,HID_USAGES.HID_USAGE_RX);

                            
                        }
                     

                    }
                }
                //joystick.SetAxis(X, id, HID_USAGES.HID_USAGE_X);
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                finally
                {
                    listener.Close();
                }
            }
            else {
            
            }

        }


    }
}
