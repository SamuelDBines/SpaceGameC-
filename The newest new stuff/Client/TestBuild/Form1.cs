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

namespace Client_Login
{
    public partial class clientform : Form
    {
        struct Message_Position
        {
            public int positionX;
            public int positionY;
        }
        struct packetType
        {
            public int packetID;
        }
        struct Message_Player
        {
            

        }
        Socket m_ClientSocket;
        System.Net.IPEndPoint m_remoteEndPoint;
        private static System.Windows.Forms.Timer m_CommunicationActivity_Timer;
        enum Packet
        {
            P_ChatMessage,
            P_DataUnit,
            P_Datatower,
            P_postion,
            P_UserInformation
        };
        public clientform()
        {
            InitializeComponent();
            m_CommunicationActivity_Timer = new System.Windows.Forms.Timer(); // Check for communication activity on Non-Blocking sockets every 200ms
            m_CommunicationActivity_Timer.Tick += new EventHandler(OnTimedEvent_PeriodicCommunicationActivityCheck); // Set event handler method for timer
            m_CommunicationActivity_Timer.Interval = 100;  // Timer interval is 1/10 second
            m_CommunicationActivity_Timer.Enabled = false;
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Create the socket, for TCP use
                m_ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_ClientSocket.Blocking = true; // Socket operates in Blocking mode initially
            }
            catch // Handle any exceptions
            {
                Close_Socket_and_Exit();
            }
            try
            {
                // Get the IP address from the appropriate text box
                String szIPAddress = "127.0.0.1";
                System.Net.IPAddress DestinationIPAddress = System.Net.IPAddress.Parse(szIPAddress);

                // Get the Port number from the appropriate text box
                String szPort = "8001";
                int iPort = System.Convert.ToInt16(szPort, 10);

                // Combine Address and Port to create an Endpoint
                m_remoteEndPoint = new System.Net.IPEndPoint(DestinationIPAddress, iPort);

                m_ClientSocket.Connect(m_remoteEndPoint);
                m_ClientSocket.Blocking = false;    // Socket is now switched to Non-Blocking mode for send/ receive activities
                                                    /*  Connect_button.Text = "Connected";
                                                      Connect_button.Enabled = false;
                                                      CloseConnection_button.Enabled = true;
                                                      CloseConnection_button.Text = "Close Connection";
                                                      Send_button.Enabled = true;*/
                MessageBox.Show(szPort);
                m_CommunicationActivity_Timer.Start();  // Start the timer to perform periodic checking for received messages   
            }
            catch // Catch all exceptions
            {   // If an exception occurs, display an error message
               // Connect_button.Text = "(Connect attempt failed)\nRetry Connect";
            }
        }
        private void OnTimedEvent_PeriodicCommunicationActivityCheck(Object myObject, EventArgs myEventArgs)
        {   // Periodic check whether a message has been received    
            try
            {
                EndPoint RemoteEndPoint = (EndPoint)m_remoteEndPoint;
                byte[] ReceiveBuffer = new byte[1024];
                int iReceiveByteCount;
                iReceiveByteCount = m_ClientSocket.ReceiveFrom(ReceiveBuffer, ref RemoteEndPoint);

                string szReceivedMessage;
                if (0 < iReceiveByteCount)
                {   // Copy the number of bytes received, from the message buffer to the text control
                    szReceivedMessage = Encoding.ASCII.GetString(ReceiveBuffer, 0, iReceiveByteCount);
                    richTextBox1.Text = szReceivedMessage;
                }
            }
            catch // Silently handle any exceptions
            {
            }
        }
    
    
       
        public void sendStruct()
        {
            Message_Position data = new Message_Position();
            data.positionX = 5;
            data.positionY = 8;
            int structureSize = Marshal.SizeOf(data);
            byte[] byteArray = new byte[structureSize];
            IntPtr memPtr = IntPtr.Zero;
            try
            {
                // Allocate some unmanaged memory
                memPtr = Marshal.AllocHGlobal(structureSize);

                // Copy struct to unmanaged memory
                Marshal.StructureToPtr(data, memPtr, true);

                // Copies to byte array
                Marshal.Copy(memPtr, byteArray, 0, structureSize);

                m_ClientSocket.Send(byteArray, SocketFlags.None);
            }
            finally
            {
                if (memPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(memPtr);
                }
            }

            // Now you can send your byte array through TCP
            
        }
        public void sendPackettype()
        {
            packetType data = new packetType();
            data.packetID = 1;            
            int structureSize = Marshal.SizeOf(data);
            byte[] byteArray = new byte[structureSize];
            IntPtr memPtr = IntPtr.Zero;
            try
            {
                // Allocate some unmanaged memory
                memPtr = Marshal.AllocHGlobal(structureSize);
                // Copy struct to unmanaged memory
                Marshal.StructureToPtr(data, memPtr, true);
                // Copies to byte array
                Marshal.Copy(memPtr, byteArray, 0, structureSize);

                m_ClientSocket.Send(byteArray, SocketFlags.None);
            }
            finally
            {
                if (memPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(memPtr);
                }
            }
            sendStruct();
            // Now you can send your byte array through TCP

        }
        private void Quit_Click(object sender, EventArgs e)
        {
            this.Close();
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

        //Close socket 
        private void Close_Socket_and_Exit()
        {
            try
            {
                m_ClientSocket.Shutdown(SocketShutdown.Both);
            }
            catch // Silently handle any exceptions
            {
            }
            try
            {
                m_ClientSocket.Close();
            }
            catch // Silently handle any exceptions
            {
            }
            this.Close();
        }

        private void oneVsoneServer_Click(object sender, EventArgs e)
        {
            try
            {
                String szData = usernameTextField.Text + "\0";
                if (szData.Equals(""))
                {
                    szData = "Default message";
                }
                byte[] byData = System.Text.Encoding.ASCII.GetBytes(szData);
                m_ClientSocket.Send(byData, SocketFlags.None);
            }
            catch // Silently handle any exceptions
            {
            }
            sendPackettype();
        }

        private void usernameTextField_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
