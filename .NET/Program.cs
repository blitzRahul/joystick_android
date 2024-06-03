

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using vjoyFeeder;
// Don't forget to add this 
using vJoyInterfaceWrap;
using System.Runtime.InteropServices;

namespace FeederDemoCS
{
    class Program
    {
        static public vJoy joystick;
        static public uint id=1;
         static void Main(string[] args) {
            joystick = new vJoy();
            //do the setup procedure

            if (setupVjoy())
            {
                //if the setup worked we are ready to write..
                long maxval=0;
                long minval = 0;
                joystick.GetVJDAxisMax(id,HID_USAGES.HID_USAGE_X,ref maxval);
                Console.WriteLine(maxval);
                joystick.GetVJDAxisMin(id, HID_USAGES.HID_USAGE_X, ref minval);
                Console.WriteLine(minval);
                Thread BrodcastListenerThread = new Thread(() => { UListen.ListenBrodcast(joystick,id); });

                Thread DiscoveryThread = new Thread(Discovery.brodcast);

                DiscoveryThread.Start();
                BrodcastListenerThread.Start();
                BrodcastListenerThread.Join();
                DiscoveryThread.Join();
                
                

              //  BrodcastListenerThread.Start();

               // BrodcastListenerThread.Join();
            }
            else return;


           
        }


        static bool setupVjoy() {
            // Get the driver attributes (Vendor ID, Product ID, Version Number)
            if (!joystick.vJoyEnabled())
            {
                Console.WriteLine("vJoy driver not enabled: Failed Getting vJoy attributes.\n");
                return false;
            }
            else
                Console.WriteLine("Vendor: {0}\nProduct :{1}\nVersion Number:{2}\n", joystick.GetvJoyManufacturerString(), joystick.GetvJoyProductString(), joystick.GetvJoySerialNumberString());

            // Get the state of the requested device
            VjdStat status = joystick.GetVJDStatus(id);
            switch (status)
            {
                case VjdStat.VJD_STAT_OWN:
                    Console.WriteLine("vJoy Device {0} is already owned by this feeder\n", id);
                    break;
                case VjdStat.VJD_STAT_FREE:
                    Console.WriteLine("vJoy Device {0} is free\n", id);
                    break;
                case VjdStat.VJD_STAT_BUSY:
                    Console.WriteLine("vJoy Device {0} is already owned by another feeder\nCannot continue\n", id);
                    return false;
                case VjdStat.VJD_STAT_MISS:
                    Console.WriteLine("vJoy Device {0} is not installed or disabled\nCannot continue\n", id);
                    return false;
                default:
                    Console.WriteLine("vJoy Device {0} general error\nCannot continue\n", id);
                    return false;
            };

            // Check which axes are supported
            bool AxisX = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_X);
            bool AxisY = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_Y);
            bool AxisZ = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_Z);
            bool AxisRX = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RX);
            bool AxisRZ = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RZ);
            // Get the number of buttons and POV Hat switchessupported by this vJoy device
            int nButtons = joystick.GetVJDButtonNumber(id);
            int ContPovNumber = joystick.GetVJDContPovNumber(id);
            int DiscPovNumber = joystick.GetVJDDiscPovNumber(id);

            // Print results
            Console.WriteLine("\nvJoy Device {0} capabilities:\n", id);
            Console.WriteLine("Numner of buttons\t\t{0}\n", nButtons);
            Console.WriteLine("Numner of Continuous POVs\t{0}\n", ContPovNumber);
            Console.WriteLine("Numner of Descrete POVs\t\t{0}\n", DiscPovNumber);
            Console.WriteLine("Axis X\t\t{0}\n", AxisX ? "Yes" : "No");
            Console.WriteLine("Axis Y\t\t{0}\n", AxisX ? "Yes" : "No");
            Console.WriteLine("Axis Z\t\t{0}\n", AxisX ? "Yes" : "No");
            Console.WriteLine("Axis Rx\t\t{0}\n", AxisRX ? "Yes" : "No");
            Console.WriteLine("Axis Rz\t\t{0}\n", AxisRZ ? "Yes" : "No");

            // Test if DLL matches the driver
            UInt32 DllVer = 0, DrvVer = 0;
            bool match = joystick.DriverMatch(ref DllVer, ref DrvVer);
            if (match)
                Console.WriteLine("Version of Driver Matches DLL Version ({0:X})\n", DllVer);
            else
                Console.WriteLine("Version of Driver ({0:X}) does NOT match DLL Version ({1:X})\n", DrvVer, DllVer);


            // Acquire the target
            if ((status == VjdStat.VJD_STAT_OWN) || ((status == VjdStat.VJD_STAT_FREE) && (!joystick.AcquireVJD(id))))
            {
                Console.WriteLine("Failed to acquire vJoy device number {0}.\n", id);
                return false;
            }
            else
            {
                Console.WriteLine("Acquired: vJoy device number {0}.\n", id);
                return true;
            }

        }
    } // class Program
} // namespace FeederDemoCS
