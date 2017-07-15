using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestBuild
{

    public partial class MainMenu : Form
    {
        Bitmap PlayerShipBitmap = (Bitmap)Image.FromFile("Player0.png");
        Bitmap PlayerShipBitmapTemp;
        int Red = 0;
        int Green = 0;
        int Blue = 0;
        float Angle = 0;
        Timer RefreshTimer = new Timer();
        Rectangle Box = new Rectangle(110, 18, 160, 160);
        ClientConnections Connections = new ClientConnections(false);
        String ReceivedText;
        List<Button> ServerListings = new List<Button>();

        public MainMenu()
        {
            PlayerShipBitmapTemp = PlayerShipBitmap;
            this.DoubleBuffered = true;

            RefreshTimer.Enabled = true;
            RefreshTimer.Interval = 20;
            RefreshTimer.Tick += new System.EventHandler(ScreenRefresh_tick);

            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Icon = new Icon("ProgramIcon.ico");
            InitializeComponent();
        }

        private void SearchForServers()
        {
            ReceivedText = Connections.findServer();
            if (ReceivedText != "")
            {
                //PageForLocalServers.Controls.Clear();
                String IPAddress = ReceivedText.Substring(0, ReceivedText.IndexOf(" "));
                ReceivedText = ReceivedText.Remove(0, ReceivedText.IndexOf(" ") + 1);
                String Port = ReceivedText.Substring(0, ReceivedText.IndexOf(" "));
                ReceivedText = ReceivedText.Remove(0, ReceivedText.IndexOf(" ") + 1);
                String ServerMessage = ReceivedText.Substring(0, ReceivedText.IndexOf("\0"));
                Button Connect = new Button();
                Connect.Width = 200;
                Connect.Height = 50;
                Connect.Click += Connect_Click;
                Connect.TextAlign = ContentAlignment.TopLeft;
                Connect.Text = "IP: " + IPAddress + "  Port: " + Port +"\n" + ServerMessage;
                ServerListings.Add(Connect);

                for (int i=0;i< ServerListings.Count; i++)
                {
                    PageForLocalServers.Controls.Add(ServerListings[i]);
                }

            }
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            Button Clicked = (Button)sender;
            String ServerDetails = Clicked.Text;
            ServerDetails = ServerDetails.Remove(0, ServerDetails.IndexOf(" ") + 1);
            Settings.Ip = ServerDetails.Substring(0, ServerDetails.IndexOf(" "));
            ServerDetails = ServerDetails.Remove(0, ServerDetails.IndexOf(" ") + 8);
            Settings.Port = ServerDetails.Substring(0, ServerDetails.IndexOf("\n"));
            SetDetails();
            LoadGameBrowser();
        }

        private void ScreenRefresh_tick(object sender, EventArgs e)
        {
            
            if (Angle == 360)
                Angle = 0;
            Angle+=0.6f;
            Invalidate(Box);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ChangeRGB();
            PlayerShipBitmapTemp.SetResolution(Box.Width-20, Box.Height-20);
            //PlayerShipBitmap
            e.Graphics.DrawImage(RotateImage(PlayerShipBitmapTemp, Angle), Box);
            PlayerShipBitmapTemp = (Bitmap)Image.FromFile("Player0.png"); ;
        }

        public Bitmap RotateImage(Bitmap b, float angle)
        {
            //create a new empty bitmap to hold rotated image

            Bitmap returnBitmap = new Bitmap(b.Width, b.Height);
            //make a graphics object from the empty bitmap
            using (Graphics g = Graphics.FromImage(returnBitmap))
            {
                //move rotation point to center of image
                g.TranslateTransform((float)b.Width / 2, (float)b.Height / 2);
                //rotate
                g.RotateTransform(angle);
                //move image back
                g.TranslateTransform(-(float)b.Width / 2, -(float)b.Height / 2);
                //draw passed in image onto graphics object
                g.DrawImage(b, new Point(15, 15));
            }
            return returnBitmap;
        }


        public void ChangeRGB()
        {           
            for (int a = 0; a < PlayerShipBitmapTemp.Width; a++)
            {
                for (int b = 0; b < PlayerShipBitmapTemp.Height; b++)
                {
                    Color tempColor = PlayerShipBitmapTemp.GetPixel(a, b);

                    if (tempColor.R < 255 - Red && tempColor.G < 255 - Green && tempColor.B < 255 - Blue)
                        PlayerShipBitmapTemp.SetPixel(a, b, Color.FromArgb(tempColor.A, tempColor.R + Red, tempColor.G + Green, tempColor.B + Blue));
                }
            }
        }

        private void LoadGameBrowser()
        {
            if (!String.IsNullOrWhiteSpace(Settings.UserName) && Char.IsLetter(Settings.UserName[0]))
            {

                this.Hide();
                GameScreen instance = new GameScreen();
                instance.ShowDialog();
            }
            else
            {
                UserNameTextBox.BackColor = Color.Red;
            }
            

        }
        private void SetDetails()
        {
            try
            {
                Settings.sendPort = (int.Parse(Settings.Port) + 1).ToString();
                Settings.recvPort = (int.Parse(Settings.Port) + 2).ToString();
            }
            catch
            {

            }
            Settings.Red = Red;
            Settings.Green = Green;
            Settings.Blue = Blue;
            Settings.UserName = UserNameTextBox.Text;

        }
        private void LoadGameCustom()
        {
            Settings.Ip = IPTextBox.Text.Replace(" ", "");
            Settings.Port = PortTextBox.Text;

            SetDetails();

            if (!String.IsNullOrWhiteSpace(Settings.UserName) && Char.IsLetter(Settings.UserName[0]))
            {

                this.Hide();
                GameScreen instance = new GameScreen();
                instance.ShowDialog();
            }
            else
            {
                UserNameTextBox.BackColor = Color.Red;
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadGameCustom();
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            Application.Exit();
        }

        private void RedBar_Scroll(object sender, EventArgs e)
        {
            Red = RedBar.Value;
        }
        private void GreenBar_Scroll(object sender, EventArgs e)
        {
            Green = GreenBar.Value;
        }


        private void BlueBar_Scroll(object sender, EventArgs e)
        {
            Blue = BlueBar.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SearchForServers();
        }

        private void UserNameTextBox_TextChanged(object sender, EventArgs e)
        {
            UserNameTextBox.BackColor = Color.White;
        }
    }
}
