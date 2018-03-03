using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using AudioSwitcher.AudioApi.CoreAudio;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;


namespace Volume
{
    public static class Program
    {
        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);
        private const int MOUSEEVENTF_MOVE = 0x0001;
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const int MOUSEEVENTF_RIGHTUP = 0x0010;
        private const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const int MOUSEEVENTF_ABSOLUTE = 0x8000;
        public static void LeftClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
        }

        public static void SU()
        {
            mouse_event(MOUSEEVENTF_MIDDLEUP, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
        }

        public static void SD()
        {
            mouse_event(MOUSEEVENTF_MIDDLEDOWN, System.Windows.Forms.Control.MousePosition.X, System.Windows.Forms.Control.MousePosition.Y, 0, 0);
        }
        public static void TurnVolumeDown(ref CoreAudioDevice myDevice)
        {
            if(myDevice.Volume>=2)
            {
                myDevice.Volume -= 2;

            }
        }

        public static void TurnVolumeUp(ref CoreAudioDevice myDevice)
        {
            if(myDevice.Volume<=98)
            {
                myDevice.Volume += 2;
            }
        }

        public static void MCL()
        {
            Point curPos = Cursor.Position;
            if(curPos.X!=1)
            {
                curPos.X-=20;
                Cursor.Position = curPos;
            }
        }

        public static void MCR()
        {
            Point curPos = Cursor.Position;
            if (curPos.X != 1365)
            {
                curPos.X += 20;
                Cursor.Position = curPos;
            }
        }

        public static void MCU()
        {
            Point curPos = Cursor.Position;
            if (curPos.Y != 1)
            {
                curPos.Y -= 20;
                Cursor.Position = curPos;
            }
        }

        public static void MCD()
        {
            Point curPos = Cursor.Position;
            if (curPos.Y != 768)
            {
                curPos.Y += 20;
                Cursor.Position = curPos;
            }
        }

        static void Main(string[] args)
        {
            SerialPort arduino = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
            CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            Console.WriteLine("Current Volume: {0}", defaultPlaybackDevice.Volume);
            defaultPlaybackDevice.Volume = 80;

            arduino.Open();
            string arduinoCode;
            //int volumeNumber;
            if (arduino.IsOpen)
            {
                while (true)
                {
                    arduinoCode = arduino.ReadLine();

                    if (Int32.TryParse(arduinoCode, out int arduinoNumber))
                    {
                        switch (arduinoNumber)
                        {
                            case 851: TurnVolumeDown(ref defaultPlaybackDevice); break;
                            case 51: TurnVolumeDown(ref defaultPlaybackDevice); break;
                            case 850: TurnVolumeUp(ref defaultPlaybackDevice); break;
                            case 50: TurnVolumeUp(ref defaultPlaybackDevice); break;
                            case 871: SendKeys.SendWait("k"); Console.WriteLine("Pause/Play"); break;
                            case 71: SendKeys.SendWait("k"); Console.WriteLine("Pause/Play"); break;
                            case 855: MCL(); break;
                            case 55: MCL(); break;
                            case 856: MCR(); break;
                            case 56: MCR(); break;
                            case 854: MCU(); break;
                            case 54: MCU(); break;
                            case 853: MCD(); break;
                            case 53: MCD(); break;
                            case 875: LeftClick();break;
                            case 75: LeftClick(); break;
                        }

                        Console.WriteLine("Current Volume: {0}", defaultPlaybackDevice.Volume);
                        Console.WriteLine("");
                    }
                    else
                    {
                        
                        if (arduinoCode.Trim().Equals("85C") || arduinoCode.Trim().Equals("5C"))
                        {
                            Console.WriteLine("Next");
                            SendKeys.SendWait("+n");
                            Console.WriteLine("NEXT");
                        }
                        if (arduinoCode.Trim().Equals("85B") || arduinoCode.Trim().Equals("5B"))
                        {
                            SendKeys.SendWait("%{LEFT}");
                            Console.WriteLine("PREV");
                        }
                        Console.WriteLine(arduinoCode);
                    }
                }
            }
        }
    }
}
