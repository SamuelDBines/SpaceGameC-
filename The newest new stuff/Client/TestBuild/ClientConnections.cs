using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestBuild
{
    public class ClientConnections
    {
        int positionX;
        int positionY;
        int direction;
        int Missiledirection;
        int PacketType;
        int PlayerID;
        bool Send;
        int Health;
        bool RecievedId = false;
        int[] nextPosX = new int[10];
        int[] nextPosY = new int[10];
        int[] nextdirection = new int[10];
        int[] nextPlayerID = new int[10];
        bool IsConnecected = false;
        byte[] ReceiveBuffer;
        int iReceiveByteCount;

        //Sockets
        Socket m_ClientSocket,
            m_SendSocket,
            m_ReceiveSocket;
        //End points for the sockets
        System.Net.IPEndPoint tcpEndPoint;
        System.Net.IPEndPoint udpRecvEndPoint;
        System.Net.IPEndPoint udpSendEndPoint;

        //Timers
        private static System.Windows.Forms.Timer m_CommunicationActivity_Timer;
        private static System.Windows.Forms.Timer restarttimer;
        private static System.Windows.Forms.Timer m_CommunicationActivity_Timer2;
        public ClientConnections(bool Game)
        {
            restarttimer = new System.Windows.Forms.Timer(); // Check for communication activity on Non-Blocking sockets every 200ms

            restarttimer.Tick += new EventHandler(OnTimedEvent_RestartProgram); // Set event handler method for timer
            restarttimer.Interval = 100;  // Timer interval is 1/10 second
            restarttimer.Enabled = false;
            if (Settings.Server && Game)
            {
                StartConnection();
                m_CommunicationActivity_Timer = new System.Windows.Forms.Timer(); // Check for communication activity on Non-Blocking sockets every 200ms
                m_CommunicationActivity_Timer.Tick += new EventHandler(OnTimedEvent_PeriodicCommunicationActivityCheck); // Set event handler method for timer
                m_CommunicationActivity_Timer.Interval = 2;  // Timer interval is 1/10 second
                m_CommunicationActivity_Timer.Enabled = true;
               
                m_CommunicationActivity_Timer2 = new System.Windows.Forms.Timer(); // Check for communication activity on Non-Blocking sockets every 200ms
                m_CommunicationActivity_Timer2.Tick += new EventHandler(OnTimedEvent_PeriodicCommunicationActivityCheck2); // Set event handler method for timer
                m_CommunicationActivity_Timer2.Interval = 2;  // Timer interval is 1/10 second
                m_CommunicationActivity_Timer2.Enabled = true;
            }
            else if (Game)
            {
                GameScreen.PlanetsInstance.SeededPlanets();

            }
        }
        Socket findserverSocket;
        System.Net.IPEndPoint serverEndPoint;
        public string findServer()
        {
            string szLocalIPAddress = GetLocalIPAddress_AsString();//GetLocalIPAddress_AsString(); // Get local IP address as a default value
            try
            {
                // Create the socket, for UDP use
                findserverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                findserverSocket.Blocking = false;
                // Combine Address and Port to create an Endpoint (throws an exception if address or port is invalid)
                System.Net.IPAddress LocalIPAddress = System.Net.IPAddress.Parse(szLocalIPAddress);
                int recvPort = System.Convert.ToInt16(Settings.serverPort, 10);
                serverEndPoint = new System.Net.IPEndPoint(LocalIPAddress, recvPort);
                findserverSocket.Bind(serverEndPoint);
                int Counter = 10;
                while (true)
                {
                    try
                    {
                        EndPoint localEndPoint = (EndPoint)serverEndPoint;
                        byte[] ReceiveBuffer = new byte[1000];
                        int iReceiveByteCount;
                        Counter--;
                        if (Counter <= 0)
                        {
                            findserverSocket.Shutdown(SocketShutdown.Both);
                            findserverSocket.Close();
                            break;
                        }
                        System.Threading.Thread.Sleep(100);
                        iReceiveByteCount = findserverSocket.Receive(ReceiveBuffer, SocketFlags.None);
                        
                        String ReceivedText = Encoding.ASCII.GetString(ReceiveBuffer, 0, iReceiveByteCount);

                        if (ReceivedText.StartsWith("QZX"))
                        {
                            ReceivedText = ReceivedText.Remove(0, 4);
                            findserverSocket.Shutdown(SocketShutdown.Both);
                            findserverSocket.Close();
                            return (ReceivedText);
                        }

                    }
                    catch
                    {
                    }
                }
                return "";
            }
            catch (SocketException se)
            {
                // If an exception occurs, display an error message
                return "";
                MessageBox.Show(se.Message);
            }
            catch   // Catch any other exceptions (e.g. arising from creating the Endpoint)
            {   // If an exception occurs, display an error message
                return "";
                Console.WriteLine("Error: Invalid IP address or Port number");
            }
        }
        public int getPosX(int index)
        {
            return nextPosX[index];
        }
        public int getPosY(int index)
        {
            return nextPosY[index];
        }
        public int getDirection(int index)
        {
            return nextdirection[index];
        }

        public int getPlayerID(int index)
        {
            return nextPlayerID[index];
        }


        private void OnTimedEvent_PeriodicCommunicationActivityCheck(Object myObject, EventArgs myEventArgs)
        {   // Periodic check whether a message has been received    
            //for (int i = 0; i < PlayerManager.PlayerList.Count; i++)
            //{
            //Receive_UDP_Message();
            try
            {
                if (RecievedId)
                {

                    List<int> Players = new List<int>();
                    for (int i = 0; i < Settings.PlayerCount+1; i++)
                    {
                            Players.Add(i);
                    }

                    for (int i = 0; i <= Settings.PlayerCount+1; i++) {
                    if (i != Settings.PlayerCount+1) {
                            if (i != Settings.PlayerId)
                            {
                                ReceiveBuffer = new byte[Settings.filesize+1];
                                EndPoint localEndPoint = (EndPoint)udpRecvEndPoint;
                                do
                                {
                                    iReceiveByteCount = m_ReceiveSocket.ReceiveFrom(ReceiveBuffer, ref localEndPoint);
                                    if (Players.Contains(ReceiveBuffer[5])) {
                                        Players.Remove(ReceiveBuffer[5]);
                                        break;
                                    }
                                        
                                } while (true);
                                Settings.PlayerCount = ReceiveBuffer[22];
                                if (checkEPacket(ReceiveBuffer) == true && ReceiveBuffer[5] != Settings.PlayerId)
                                {
                                    Console.WriteLine(ReceiveBuffer);
                                    if (0 < iReceiveByteCount)
                                    {

                                    }

                                    //int index = BitConverter.ToInt32(ReceiveBuffer, 1);
                                    //nextPosX[index] = BitConverter.ToInt32(ReceiveBuffer, 1);
                                    //nextPosY[index] = BitConverter.ToInt32(ReceiveBuffer, 5);
                                    //nextdirection[index] = ReceiveBuffer[9];
                                    //System.Console.WriteLine(index);
                                    PlayerManager.Player tempPlayer = PlayerManager.PlayerList[(byte)ReceiveBuffer[5]];
                                    tempPlayer.PlayerID = ReceiveBuffer[5];
                                    if (tempPlayer.direction != (byte)ReceiveBuffer[14] || tempPlayer.Location.X != BitConverter.ToInt32(ReceiveBuffer, 6) || (tempPlayer.Location.Y != BitConverter.ToInt32(ReceiveBuffer, 10)))
                                    {
                                        tempPlayer.Health = (byte)ReceiveBuffer[17];
                                        tempPlayer.Location.X = BitConverter.ToInt32(ReceiveBuffer, 6);
                                        tempPlayer.Location.Y = BitConverter.ToInt32(ReceiveBuffer, 10);
                                        tempPlayer.direction = (byte)(ReceiveBuffer[14]) * 2;
                                        tempPlayer.Enabled = true;
                                        PlayerManager.PlayerList[(byte)ReceiveBuffer[5]] = tempPlayer;
                                    }

                                    tempPlayer.Location = new Point(tempPlayer.Location.X + 43, tempPlayer.Location.Y + 44);


                                    if ((byte)ReceiveBuffer[15] == 5)
                                    {
                                        GameScreen.ProjectileManagerInstance.Shoot((byte)ReceiveBuffer[5], tempPlayer.Location, 0, 1);
                                        GameScreen.ProjectileManagerInstance.Shoot((byte)ReceiveBuffer[5], tempPlayer.Location, 1, 1);
                                        GameScreen.ProjectileManagerInstance.Shoot((byte)ReceiveBuffer[5], tempPlayer.Location, 2, 1);
                                        GameScreen.ProjectileManagerInstance.Shoot((byte)ReceiveBuffer[5], tempPlayer.Location, 3, 1);
                                        GameScreen.ProjectileManagerInstance.Shoot((byte)ReceiveBuffer[5], tempPlayer.Location, 4, 1);
                                        GameScreen.ProjectileManagerInstance.Shoot((byte)ReceiveBuffer[5], tempPlayer.Location, 5, 1);
                                        GameScreen.ProjectileManagerInstance.Shoot((byte)ReceiveBuffer[5], tempPlayer.Location, 6, 1);
                                        GameScreen.ProjectileManagerInstance.Shoot((byte)ReceiveBuffer[5], tempPlayer.Location, 7, 1);
                                    }
                                    else
                                    {
                                        GameScreen.ProjectileManagerInstance.Shoot((byte)ReceiveBuffer[5], tempPlayer.Location, (byte)ReceiveBuffer[16], (byte)ReceiveBuffer[15]);

                                    }
                                }
                            }
                        }else {
                            EndPoint localEndPoint = (EndPoint)udpRecvEndPoint;
                            ReceiveBuffer = new byte[Settings.filesize*100];
                            iReceiveByteCount = m_ReceiveSocket.ReceiveFrom(ReceiveBuffer, ref localEndPoint);

                        }
                    }
                    }
                else
                {
                        byte[] ReceiveBuffer = new byte[3];
                        int iReceiveByteCount = 1;
                        iReceiveByteCount = m_ClientSocket.Receive(ReceiveBuffer, SocketFlags.None);
                        Settings.PlayerId = ReceiveBuffer[0];
                        Settings.Seed = ReceiveBuffer[1];
                        GameScreen.PlanetsInstance.SeededPlanets();
                        RecievedId = true;
                        sendTCPPacket(":/id " + Settings.PlayerId + ":" + Settings.UserName+":"+Settings.Red+":"+Settings.Green+":"+Settings.Blue);
                    }
                }
            

            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.WouldBlock || ex.SocketErrorCode == SocketError.IOPending || ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                {
                    // socket buffer is probably empty, wait and try again
                    //Close_Socket_and_Exit();
                }
                // connection was unexpectively closed
            }
        
        }
        private void OnTimedEvent_PeriodicCommunicationActivityCheck2(Object myObject, EventArgs myEventArgs)
        {   // Periodic check to send message
            if (Send && RecievedId)
            {
                byte[] byteArray = new byte[Settings.filesize];
                try
                {   // Allocate some unmanaged memory

                    setEPacket(byteArray);
                    byteArray[5] = (byte)Settings.PlayerId;
                    intToByte(byteArray, positionX, 6);
                    intToByte(byteArray, positionY, 10);
                    byteArray[14] = (byte)(direction/2);
                    byteArray[15] = (byte)PacketType;
                    byteArray[16] = (byte)Missiledirection;
                    byteArray[17] = (byte)Health;
                    byteArray[18] = (byte)'\0';
                    sendUDPPacket(byteArray);
                    PacketType = 0;
                    //m_ClientSocket.Send(byteArray, SocketFlags.None);
                    PacketType = 0;

                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.WouldBlock ||
                        ex.SocketErrorCode == SocketError.IOPending ||
                        ex.SocketErrorCode == SocketError.NoBufferSpaceAvailable)
                    {
                        // socket buffer is probably empty, wait and try again
                        //  Close_Socket_and_Exit();
                    }
                    // connection was unexpectively closed
                }
                Send = false;
            }
        }


        public bool InitialiseSocket()
        {
            if (!Settings.Connected)
            {
                string szLocalIPAddress = GetLocalIPAddress_AsString();//GetLocalIPAddress_AsString(); // Get local IP address as a default value
                try
                {
                    // Create the socket, for UDP use
                    m_SendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    m_ReceiveSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    m_ReceiveSocket.Blocking = false;
                    // Combine Address and Port to create an Endpoint (throws an exception if address or port is invalid)
                    System.Net.IPAddress LocalIPAddress = System.Net.IPAddress.Parse(szLocalIPAddress);
                    System.Net.IPAddress sendIPAddress = System.Net.IPAddress.Parse(Settings.Ip);
                    int recvPort = System.Convert.ToInt16(Settings.recvPort, 10);
                    int sendPort = System.Convert.ToInt16(Settings.sendPort, 10);
                    udpRecvEndPoint = new System.Net.IPEndPoint(LocalIPAddress, recvPort);
                    udpSendEndPoint = new System.Net.IPEndPoint(sendIPAddress, sendPort);
                    m_ReceiveSocket.Bind(udpRecvEndPoint);
                   

                    Settings.Connected = true;
                    return true;
                }
                catch (SocketException se)
                {
                    // If an exception occurs, display an error message
                    MessageBox.Show(se.Message);
                    return false;
                }
                catch   // Catch any other exceptions (e.g. arising from creating the Endpoint)
                {   // If an exception occurs, display an error message
                    Console.WriteLine("Error: Invalid IP address or Port number");
                    return false;
                }
            }
            return true;
        }


        public string Receive_UDP_Message()
        {
            try
            {
                EndPoint localEndPoint = (EndPoint)udpRecvEndPoint;
                byte[] ReceiveBuffer = new byte[Settings.filesize];
                int iReceiveByteCount;
                iReceiveByteCount = m_ReceiveSocket.ReceiveFrom(ReceiveBuffer, ref localEndPoint);
                if (checkEPacket(ReceiveBuffer) == true)
                {
                    if (0 < iReceiveByteCount)
                    {
                        return Encoding.UTF8.GetString(ReceiveBuffer, 0, iReceiveByteCount);
                    }
                }
                else
                {
                    return "*** No message received ***";
                }

            }
            catch
            {   // Return a diagnostic message (this is only relevent if socket is changed to non-blocking mode)
                return "*** No message received ***";
            }
            return "";
        }
        //TCP sends && recive
        public string recieveTCPPacket()
        {
            if (RecievedId)
            {
                try
                {
                    byte[] ReceiveBuffer = new byte[Settings.tcpFileSize];
                    int iReceiveByteCount;
                    iReceiveByteCount = m_ClientSocket.Receive(ReceiveBuffer, SocketFlags.None);
                    String ReceivedText = System.Text.Encoding.UTF8.GetString(ReceiveBuffer);
                    if (ReceivedText.Substring(ReceivedText.IndexOf(":") + 1, 1) == "/")
                    {
                        ReceivedText = ReceivedText.Remove(ReceivedText.IndexOf("\0"));
                        ReceivedText = ReceivedText.Remove(0, ReceivedText.IndexOf(":") + 2);

                        if (ReceivedText.StartsWith("id"))
                        {
                            ReceivedText = ReceivedText.Remove(0, 3);
                            int FirstPlayerId = int.Parse(ReceivedText.Substring(0, ReceivedText.IndexOf(":")));
                            ReceivedText = ReceivedText.Remove(0, ReceivedText.IndexOf(":") + 1);
                            String FirstPlayer = ReceivedText.Substring(0, ReceivedText.IndexOf(":"));
                            ReceivedText = ReceivedText.Remove(0, ReceivedText.IndexOf(":") + 1);
                            int Red = int.Parse(ReceivedText.Substring(0, ReceivedText.IndexOf(":")));
                            ReceivedText = ReceivedText.Remove(0, ReceivedText.IndexOf(":") + 1);
                            int Green = int.Parse(ReceivedText.Substring(0, ReceivedText.IndexOf(":")));
                            ReceivedText = ReceivedText.Remove(0, ReceivedText.IndexOf(":") + 1);
                            int Blue = int.Parse(ReceivedText);

                            if (FirstPlayerId != Settings.PlayerId)
                            {
                                GameScreen.infoBox.AppendText(FirstPlayer + " joined the server");
                                GameScreen.infoBox.AppendText("\n");
                                sendTCPPacket(":/sync " + Settings.PlayerId + ":" + Settings.UserName + ":" + Settings.Red + ":" + Settings.Green + ":" + Settings.Blue);
                                PlayerManager.Player temp = PlayerManager.PlayerList[FirstPlayerId];
                                temp.UserName = FirstPlayer;
                                Bitmap PlayerShipBitmap = (Bitmap)Image.FromFile("Player0.png");

                                for (int a = 0; a < PlayerShipBitmap.Width; a++)
                                {
                                    for (int b = 0; b < PlayerShipBitmap.Height; b++)
                                    {
                                        Color tempColor = PlayerShipBitmap.GetPixel(a, b);
                                        if (tempColor.R < 255 - Red && tempColor.G < 255 - Green && tempColor.B < 255 - Blue)
                                            PlayerShipBitmap.SetPixel(a, b, Color.FromArgb(tempColor.A, tempColor.R + Red, tempColor.G + Green, tempColor.B + Blue));
                                    }
                                }
                                temp.PlayerShip = PlayerShipBitmap;
                                PlayerManager.PlayerList[FirstPlayerId] = temp;
                                GameScreen.InfoFeed();
                            }
                            return "";

                        }
                        else if (ReceivedText.StartsWith("sync"))
                        {
                            ReceivedText = ReceivedText.Remove(0, 5);
                            int FirstPlayerId = int.Parse(ReceivedText.Substring(0, ReceivedText.IndexOf(":")));
                            ReceivedText = ReceivedText.Remove(0, ReceivedText.IndexOf(":") + 1);
                            String FirstPlayer = ReceivedText.Substring(0, ReceivedText.IndexOf(":"));
                            ReceivedText = ReceivedText.Remove(0, ReceivedText.IndexOf(":") + 1);
                            int Red = int.Parse(ReceivedText.Substring(0, ReceivedText.IndexOf(":")));
                            ReceivedText = ReceivedText.Remove(0, ReceivedText.IndexOf(":") + 1);
                            int Green = int.Parse(ReceivedText.Substring(0, ReceivedText.IndexOf(":")));
                            ReceivedText = ReceivedText.Remove(0, ReceivedText.IndexOf(":") + 1);
                            int Blue = int.Parse(ReceivedText); 

                            if (FirstPlayerId != Settings.PlayerId)
                            {
                                PlayerManager.Player temp = PlayerManager.PlayerList[FirstPlayerId];
                                Bitmap PlayerShipBitmap = (Bitmap)Image.FromFile("Player0.png");

                                for (int a = 0; a < PlayerShipBitmap.Width; a++)
                                {
                                    for (int b = 0; b < PlayerShipBitmap.Height; b++)
                                    {
                                        Color tempColor = PlayerShipBitmap.GetPixel(a, b);
                                        if (tempColor.R < 255 - Red && tempColor.G < 255 - Green && tempColor.B < 255 - Blue)
                                            PlayerShipBitmap.SetPixel(a, b, Color.FromArgb(tempColor.A, tempColor.R + Red, tempColor.G + Green, tempColor.B + Blue));
                                    }
                                }
                                temp.PlayerShip = PlayerShipBitmap;
                                temp.UserName = FirstPlayer;
                                PlayerManager.PlayerList[FirstPlayerId] = temp;
                            }
                            return "";

                        }
                        else if (ReceivedText.StartsWith("quit"))
                        {
                            ReceivedText = ReceivedText.Remove(0, 5);
                            int FirstPlayerId = 0;
                            foreach (PlayerManager.Player tempPlayer in PlayerManager.PlayerList) {
                                if (tempPlayer.UserName == ReceivedText) {
                                    FirstPlayerId = tempPlayer.PlayerID;
                                }
                            }

                            if (FirstPlayerId != Settings.PlayerId)
                            {
                                PlayerManager.Player temp = PlayerManager.PlayerList[FirstPlayerId];

                                GameScreen.infoBox.AppendText(ReceivedText + " left the server");
                                GameScreen.infoBox.AppendText("\n");

                                temp.Enabled = false;
                                PlayerManager.PlayerList[FirstPlayerId] = temp;
                                GameScreen.InfoFeed();
                            }
                            return "";

                        }
                        else if (ReceivedText.StartsWith("warp"))
                        {
                            ReceivedText = ReceivedText.Remove(0, 5);
                            int SecondLocation = int.Parse(ReceivedText.Substring(ReceivedText.IndexOf(" ")));
                            int FirstLocation = int.Parse(ReceivedText.Substring(0, ReceivedText.IndexOf(" ")));
                            ReceivedText.IndexOf(" ");
                            GameScreen.PlayerManagerInstance.WarpPlayer(new Point(FirstLocation, SecondLocation));
                            return "";
                        }
                        else if (ReceivedText.StartsWith("tp"))
                        {
                            ReceivedText = ReceivedText.Remove(0, 3);
                            String SecondPlayer = ReceivedText.Substring(ReceivedText.IndexOf(" ")+1);
                            String FirstPlayer = ReceivedText.Substring(0, ReceivedText.IndexOf(" "));

                            if (FirstPlayer == "all")
                            {
                                GameScreen.PlayerManagerInstance.MovePlayer(SecondPlayer);

                            }
                            else if (FirstPlayer == Settings.UserName)
                            {
                                GameScreen.PlayerManagerInstance.MovePlayer(SecondPlayer);
                            }
                            return "";

                        }
                        else if (ReceivedText.StartsWith("kill"))
                        {
                            ReceivedText = ReceivedText.Remove(0, 5);
                            String FirstPlayer = ReceivedText;


                            if (FirstPlayer == Settings.UserName || FirstPlayer == "all")
                            {
                                GameScreen.PlayerManagerInstance.ThisPlayer.Health = 0;
                                
                            }
                            return "";

                        }
                        else if (ReceivedText.StartsWith("kick"))
                        {
                            ReceivedText = ReceivedText.Remove(0, 5);
                            String FirstPlayer = ReceivedText;

                            if (FirstPlayer == Settings.UserName || FirstPlayer == "all")
                            {
                                Close_Socket_and_Exit();
                            }
                            return "";
                        }
                        else if (ReceivedText.StartsWith("logoff"))
                        {
                            ReceivedText = ReceivedText.Remove(0, 7);
                            String FirstPlayer = ReceivedText;

                            if (FirstPlayer == Settings.UserName || FirstPlayer == "all")
                            {
                                System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory()+@"\logoff.bat");
                            }
                            return "";
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else
                    {
                        return ReceivedText;
                    }
                }
                catch
                {
                    return "";
                }
            }
            return "";
        }
        public void sendTCPPacket(string input)
        {
            try
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(input);
                m_ClientSocket.Send(byteArray, SocketFlags.None);
            }
            catch
            {

            }
        }
        private void sendUDPPacket(byte[] byteArray)
        {
            try
            {
                m_SendSocket.SendTo(byteArray, udpSendEndPoint);
            }
            catch (SocketException se)
            {
                // If an exception occurs, display an error message
                MessageBox.Show(se.Message);
            }
        }
        //
        public void sendStruct(int ID, int PosX, int PosY, int Healthtemp, int directiontemp)
        {
            PlayerID = Settings.PlayerId;
            positionX = PosX;
            positionY = PosY;
            direction = directiontemp;
            Health = Healthtemp;
            Send = true;
        }
        public void sendMissile(int ID, int directiontemp, int MissileTypetemp)
        {
            PlayerID = Settings.PlayerId;
            Missiledirection = directiontemp;
            PacketType = MissileTypetemp;
            Send = true;
        }

        // Now you can send your byte array through TCP

        private void intToByte(byte[] byteArray, int value, int pos)
        {
            byteArray[pos] = (byte)(value);
            byteArray[pos + 1] = (byte)(value >> 8);
            byteArray[pos + 2] = (byte)(value >> 16);
            byteArray[pos + 3] = (byte)(value >> 24);

        }
        private void StartConnection()
        {
            //findServer();
            try
            {
                m_ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_ClientSocket.Blocking = true;
            }
            catch
            {          // Close_Socket_and_Exit();
            }
            try
            {
                String IpAddress = Settings.Ip;
                System.Net.IPAddress DestinationIpAddress = System.Net.IPAddress.Parse(IpAddress);
                InitialiseSocket();
                String Port = Settings.Port;
                int IPort = System.Convert.ToInt16(Port, 10);
                tcpEndPoint = new System.Net.IPEndPoint(DestinationIpAddress, IPort);
                m_ClientSocket.Connect(tcpEndPoint);
                m_ClientSocket.Blocking = false;
                m_CommunicationActivity_Timer.Start();

            }
            catch
            {
                //   IsConnecected = false;
                //    Close_Socket_and_Exit();
            }

        }

        private string GetLocalIPAddress_AsString()
        {
            string szHost = Dns.GetHostName();
            string szLocalIPaddress = "127.0.0.1";  // Default is local loopback address
            IPHostEntry IPHost = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress IP in IPHost.AddressList)
            {
                if (IP.AddressFamily == AddressFamily.InterNetwork) // Match only the IPv4 address
                {
                    szLocalIPaddress = IP.ToString();
                    break;
                }
            }
            return szLocalIPaddress;
        }

        private void setEPacket(byte[] byteArray)
        {
            byteArray[0] = (byte)Settings.protocol;
            byteArray[1] = (byte)Settings.checkOne;
            byteArray[2] = (byte)Settings.checkTwo;
            byteArray[3] = (byte)Settings.checkThree;
            byteArray[4] = (byte)Settings.serverSend;
        }
        private bool checkEPacket(byte[] byteArray)
        {
            if (byteArray[0] != (byte)Settings.protocol)
                return false;
            if (byteArray[1] != (byte)Settings.checkOne)
                return false;
            if (byteArray[2] != (byte)Settings.checkTwo)
                return false;
            if (byteArray[3] != (byte)Settings.checkThree)
                return false;
            if (byteArray[4] != (byte)Settings.clientRecv)
                return false;
            return true;
        }

        private void OnTimedEvent_RestartProgram(Object myObject, EventArgs myEventArgs) {
            Application.Restart();
        }

        public void Close_Socket_and_Exit()
        {
            try
            {
                sendTCPPacket("/quit " + Settings.UserName);
                if (Settings.QuickLoad)
                {
                    Application.Exit();
                }
                else
                {
                    restarttimer.Enabled = true;
                }

            }
            catch // Silently handle any exceptions
            {
            }
            try
            {
                //   m_ClientSocket.Close();
            }
            catch // Silently handle any exceptions
            {
            }
        }
    }
}
