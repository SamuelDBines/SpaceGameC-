using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestBuild
{
    partial class GameScreen : Form
    {
        public static int Spin;
        MouseEventArgs mouse;
        public static ProjectileManager ProjectileManagerInstance;
        StarManager StarManagerInstance;
        public static PlayerManager PlayerManagerInstance;
        int Counter = 0;
        public static Planets PlanetsInstance;
        ClientConnections connection;
        public static int Score = 0;
        bool ShowNames = false;
        Boolean WarpBoxEnabled = false;
        public static Point LocationOfPlayer;
        bool MousePressedDown,
             ShootLeft = false,
             ShootRight = false,
             ShootUp = false,
             ShootDown = false,
             ShootAll = false;
        SolidBrush ProjectileBrush,
                   PlayerBrush = new SolidBrush(Settings.PlayerColor),
                   StarBrush;
        Panel WarpPanel;
        public static Label RespawnLabel;
        Panel PausePanel;
        Timer ShootingInterval = new Timer();
        Random random = new Random();
        List<Bitmap> PlayerImage = new List<Bitmap>();
        List<Bitmap> PlanetImages = new List<Bitmap>();
        bool Paused = false;
        bool Chat = false;
        ProjectileManager.Projectiles newMissile;
        Timer ScreenRefresh = new Timer();
        static Timer TimeToHideInfoScreen = new Timer();
        Rectangle ScreenRect;
        int WeaponType = 1;
        public static Point ScreenSize;
        string WeaponTypeString = "Gatling gun";


        Label PauseLabel;
        Label ChatLabel;
        Button resumeButton;
        Button respawnButton;
        Button leaveButton;
        Button sendButton;
        RichTextBox chatBox;
        TextBox messageBox;
        public static RichTextBox infoBox;
        Button closeButton;
        
        TextBox warpx;
        TextBox warpy;


        public GameScreen()
        {
            chatBox = new RichTextBox();
            messageBox = new TextBox();
            infoBox = new RichTextBox();
            this.Controls.Add(chatBox);
            this.Controls.Add(messageBox);
            this.Controls.Add(infoBox);
           
            infoBox.Hide();
            chatBox.Hide();
            messageBox.Hide();

            //this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            ScreenRect = Rectangle.FromLTRB(0, 0, Screen.FromControl(this).Bounds.Width, Screen.FromControl(this).Bounds.Height);
            ScreenSize.X = Screen.FromControl(this).Bounds.Width;
            ScreenSize.Y = Screen.FromControl(this).Bounds.Height;
            
            PlanetImages.Add((Bitmap) Image.FromFile("RedPlanet.png"));
            PlanetImages.Add((Bitmap) Image.FromFile("BrownPlanet.png"));
            PlanetImages.Add((Bitmap) Image.FromFile("IcePlanet.png"));
            PlanetImages.Add((Bitmap) Image.FromFile("BluePlanet.png"));
            PlanetImages.Add((Bitmap)Image.FromFile("GreenPlanet.png"));


            this.BackColor = Color.Black;


            ShootingInterval.Enabled = true;
            ShootingInterval.Interval = Settings.ProjectileFireRate;
            ShootingInterval.Tick += new System.EventHandler(ShootingFireRate_tick);

            ScreenRefresh.Enabled = true;
            ScreenRefresh.Interval = 1;
            ScreenRefresh.Tick += new System.EventHandler(ScreenRefresh_tick);                       

            TimeToHideInfoScreen.Enabled = true;
            TimeToHideInfoScreen.Interval = 2000;
            TimeToHideInfoScreen.Tick += new System.EventHandler(InfoScreen_tick);

            this.DoubleBuffered = true;
            PlanetsInstance = new Planets();
            connection = new ClientConnections(true);
            PlayerManagerInstance = new PlayerManager(this, connection);
            ProjectileManagerInstance = new ProjectileManager(this, PlayerManagerInstance);
            StarManagerInstance = new StarManager(this, PlayerManagerInstance);
            InitializeComponent();
        }



        private void GameScreen_Resize(object sender, EventArgs e)
        {
            if (Settings.PositionLock)
               // PlayerManagerInstance.ThisPlayer.PlayerShape.Location = new Point(this.Bounds.Width / 2 - PlayerManagerInstance.ThisPlayer.PlayerShape.Width / 2, this.Bounds.Height / 2 - PlayerManagerInstance.ThisPlayer.PlayerShape.Height / 2);
            ScreenRect = Rectangle.FromLTRB(0, 0, Screen.FromControl(this).Bounds.Width, Screen.FromControl(this).Bounds.Height);

            ScreenSize.X = Screen.FromControl(this).Bounds.Width;
            ScreenSize.Y = Screen.FromControl(this).Bounds.Height;
        }

        private void ScreenRefresh_tick(object sender, EventArgs e)
        {

            if (Settings.NewAlgorithm)
                Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ScoreScreen.Text = "Id "+Settings.PlayerId +"    User "+Settings.UserName+ "  Total Players "+ Settings.PlayerCount + "  Health  " + PlayerManagerInstance.ThisPlayer.Health+"  " +Settings.FlightMode + " Mode   X = " + PlayerManagerInstance.ThisPlayer.Location.X + " Y = " + PlayerManagerInstance.ThisPlayer.Location.Y + "    Speed = " + Settings.Speed + "m/s";
            base.OnPaint(e);
            for (int i = 0; i < Settings.MaxProjectilesOnScreen; i++)
            {
                ProjectileManager.Projectiles tempProjectile = ProjectileManagerInstance.MissileList[i];
                if (tempProjectile.WeaponType == 2 && tempProjectile.TimeToLive != 0)
                {
                    Pen Laser = new Pen(Color.Red);
                    e.Graphics.DrawLine(Laser, PlayerManagerInstance.ThisPlayer.PlayerShape.X + 50, PlayerManagerInstance.ThisPlayer.PlayerShape.Y + 50, ProjectileManagerInstance.MissileList[0].Shape.X, ProjectileManagerInstance.MissileList[0].Shape.Y);
                    newMissile = ProjectileManagerInstance.MissileList[i];
                    newMissile.TimeToLive -= 1;
                    if (newMissile.TimeToLive == 0)
                        newMissile.IsDead = true;
                    ProjectileManagerInstance.MissileList[i] = newMissile;
                    if (ProjectileManagerInstance.MissileList[i].Shape.IntersectsWith(PlayerManagerInstance.ThisPlayer.PlayerShape))
                        PlayerManagerInstance.ThisPlayer.Health -= 1;

                }
                else

               if (tempProjectile.Shape.IntersectsWith(ScreenRect))
                {

                    for (int j = 0; j < PlanetsInstance.PlanetListInView.Count; j++)
                    {
                        Planets.PlanetObject tempPlanet = PlanetsInstance.PlanetListInView[j];
                        if (tempPlanet.InView && tempPlanet.Health>0)
                        {
                            if (tempProjectile.WeaponType == 1 && !tempProjectile.IsDead
                            && (tempProjectile.Shape.IntersectsWith(tempPlanet.HitBoxes[0]) || tempProjectile.Shape.IntersectsWith(tempPlanet.HitBoxes[1])
                            || tempProjectile.Shape.IntersectsWith(tempPlanet.HitBoxes[2])))
                            {                                
                                newMissile = tempProjectile;
                                newMissile.IsDead = true;
                                ProjectileManagerInstance.MissileList[i] = newMissile;
                                int tempPositionInList = tempPlanet.PositionInList;
                                tempPlanet = PlanetsInstance.PlanetList[tempPositionInList];
                                tempPlanet.Health -= 1;
                                Point TempPoint = tempPlanet.Location;
                                TempPoint.X += tempPlanet.Size / 2;
                                TempPoint.Y += tempPlanet.Size / 2;
                                ProjectileManagerInstance.Shoot(-1, TempPoint, 0, 6);
                                PlanetsInstance.PlanetList[tempPositionInList] = tempPlanet;

                            }
                        }
                    }
                    if (Settings.CollisionOn && !tempProjectile.IsDead
                    && tempProjectile.Shape.IntersectsWith(PlayerManagerInstance.ThisPlayer.PlayerShape)
                    && Settings.PlayerId != tempProjectile.ShooterId && tempProjectile.ShooterId != -1)
                    {
                        newMissile = tempProjectile;
                        newMissile.IsDead = true;
                        Score += 1;
                        PlayerManagerInstance.ThisPlayer.Health -= 1;
                        //ScoreScreen.Text = Score.ToString();
                        ProjectileManagerInstance.MissileList[i] = newMissile;

                    }

                    else
                    {
                        int tempFadeOut = Settings.BulletLife;
                        if (tempProjectile.ShooterId == -1 || tempProjectile.WeaponType == 5)
                            tempFadeOut = 10;


                        if (ProjectileManagerInstance.MissileList[i].TimeToLive < (Settings.FadeOutSpeed * tempFadeOut / 100))
                        {
                            ProjectileBrush = new SolidBrush(Color.FromArgb((((tempProjectile.TimeToLive - 0) * (tempProjectile.Color.A - 0)) / (tempFadeOut - 0)) + 0,
                                tempProjectile.Color.R, tempProjectile.Color.G, tempProjectile.Color.B));
                        }
                        else
                        {
                            ProjectileBrush = new SolidBrush(tempProjectile.Color);
                        }
                        if (!tempProjectile.IsDead)
                            e.Graphics.FillRectangle(ProjectileBrush, tempProjectile.Shape);
                    }

                }
            }



            for (int i = 0; i < Settings.MaxStarsOnScreen; i++)
            {
                StarManager.Stars tempStar = StarManagerInstance.StarList[i];
                if (tempStar.Shape.IntersectsWith(ScreenRect))
                {
                    StarBrush = new SolidBrush(tempStar.Color);
                    e.Graphics.FillRectangle(StarBrush, tempStar.Shape);
                }
            }

            for (int i = 0; i< PlanetsInstance.PlanetListInView.Count; i++)
            {
                Planets.PlanetObject tempPlanet = PlanetsInstance.PlanetListInView[i];
                if (tempPlanet.InView && tempPlanet.Health >= 0)
                {
                    //SolidBrush PlanetBrush = new SolidBrush(PlanetsInstance.PlanetList[i].PlanetColor);
                    //e.Graphics.FillEllipse(PlanetBrush, PlanetsInstance.PlanetList[i].ScreenLocation.X, PlanetsInstance.PlanetList[i].ScreenLocation.Y, PlanetsInstance.PlanetList[i].Size, PlanetsInstance.PlanetList[i].Size);
                    
                    if (tempPlanet.Health > 0)
                    {
                        e.Graphics.DrawImage(PlanetImages[tempPlanet.PlanetImage], tempPlanet.ScreenLocation.X, tempPlanet.ScreenLocation.Y, tempPlanet.Size, tempPlanet.Size);
                    }else
                    {
                        Point TempPoint = tempPlanet.Location;
                        TempPoint.X += tempPlanet.Size / 2;
                        TempPoint.Y += tempPlanet.Size / 2;
                        ProjectileManagerInstance.Shoot(-2, TempPoint, 0, 5);
                        int tempPosition = tempPlanet.PositionInList;
                        tempPlanet = PlanetsInstance.PlanetList[tempPlanet.PositionInList];
                        tempPlanet.Health = -1;
                        PlanetsInstance.PlanetList[tempPosition] = tempPlanet;
                    }

                }

            }

            int PlayerCounter = 0;

            for (int i = -1; i < PlayerManager.PlayerList.Count; i++)
            {

                PlayerManager.Player TempPlayer;
                if (i == -1)
                {
                    TempPlayer = PlayerManagerInstance.ThisPlayer;
                    LocationOfPlayer = TempPlayer.Location;
                    
                }
                else
                {
                    TempPlayer = PlayerManager.PlayerList[i];
                }


                if ((i != -1 && TempPlayer.Enabled) || (i == -1 && PlayerManagerInstance.ThisPlayer.Enabled))
                {
                    

                    if (PlayerManagerInstance.ThisPlayer.Health <= 0 && PlayerManager.TimetoRespawn == -1)
                    {
                        RespawnLabel = new Label();
                        RespawnLabel.AutoSize = true;
                        RespawnLabel.Text = "5";
                        RespawnLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        RespawnLabel.ForeColor = Color.BlueViolet;
                        RespawnLabel.Location = new Point(ScreenSize.X/2,ScreenSize.Y / 2);
                        PlayerManager.TimetoRespawn = 5;
                        Controls.Add(RespawnLabel);

                    }
                    if (PlayerManager.TimetoRespawn == 0)
                    {
                        RespawnLabel.Dispose();
                    }
                                       

                    if (TempPlayer.Health <= 0 && TempPlayer.Enabled)
                    {
                        TempPlayer.Enabled = false;
                        ProjectileManagerInstance.Shoot(Settings.PlayerId, TempPlayer.Location, 0, 5);
                        if (i == -1)
                        {
                            PlayerManagerInstance.ThisPlayer = TempPlayer;
                        }
                        else
                        {
                            PlayerManager.PlayerList[i] = TempPlayer;
                        }
                    }

                    if (TempPlayer.PlayerShape.IntersectsWith(ScreenRect) && TempPlayer.Health>0 & TempPlayer.Health <= 100)
                    {                       
                        
                        TempPlayer.PlayerShape.Inflate(-10, -10);
                        TempPlayer.PlayerShip.SetResolution(TempPlayer.PlayerShip.Width+5, TempPlayer.PlayerShip.Height+5);
                        //Rectangle ImageRect = Rectangle.FromLTRB(TempPlayer.PlayerShape.X - 50, TempPlayer.PlayerShape.Y - 50, TempPlayer.PlayerShape.X + TempPlayer.PlayerShape.Width + 50, TempPlayer.PlayerShape.X + TempPlayer.PlayerShape.Height + 50);


                        e.Graphics.DrawImage(RotateImage(TempPlayer.PlayerShip, TempPlayer.direction), TempPlayer.PlayerShape);
                        if (ShowNames)
                        {
                            Point temp = TempPlayer.PlayerShape.Location;
                            temp.Y -= 30;
                            temp.X -= 10;
                            e.Graphics.DrawString(TempPlayer.UserName, new Font("Arial", 12), new SolidBrush(Color.White),temp);
                           
                        }

                    }
                }
            }
            //Settings.PlayerCount = PlayerCounter;

        }


        public static Bitmap RotateImage(Bitmap b, float angle)
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
                g.DrawImage(b, new Point(0, 0));
            }
            return returnBitmap;
        }

        private void CreateWarpBox()
        {
            WarpPanel = new Panel();
            WarpPanel.Location = new Point(ScreenSize.X / 2 - 90, ScreenSize.Y / 2 + 60);
            WarpPanel.Width = 180;
            WarpPanel.BorderStyle = BorderStyle.FixedSingle;
            WarpPanel.Height = 180;
            WarpPanel.BackColor = Color.Gray;
            this.Controls.Add(WarpPanel);

            Label warpxlabel = new Label();
            warpxlabel.Text = "X coord:";
            warpxlabel.AutoSize = true;
            warpxlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            warpxlabel.ForeColor = Color.Black;
            warpxlabel.Location = new Point(0, 10);
            WarpPanel.Controls.Add(warpxlabel);

            Label warpYlabel = new Label();
            warpYlabel.Text = "Y coord:";
            warpYlabel.AutoSize = true;
            warpYlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            warpYlabel.ForeColor = Color.Black;
            warpYlabel.Location = new Point(0, 50);
            WarpPanel.Controls.Add(warpYlabel);

            warpx = new TextBox();
            warpx.Name = "warpx";
            warpx.Text = "";
            warpx.Width = 80;
            warpx.Location = new Point(80, 14);
            WarpPanel.Controls.Add(warpx);

            warpy = new TextBox();
            warpy.Name = "warpy";
            warpy.Text = "";
            warpy.Width = 80;
            warpy.Location = new Point(80, 54);
            WarpPanel.Controls.Add(warpy);


            Button warpButton = new Button();
            warpButton.Click += WarpButton_Click;
            warpButton.Text = "Warp";
            warpButton.Width = 150;
            warpButton.Height = 40;
            warpButton.Location = new Point(15, 80);
            WarpPanel.Controls.Add(warpButton);

            Button Close = new Button();
            Close.Click += CloseWarpButton_Click;
            Close.Text = "Close";
            Close.Width = 150;
            Close.Height = 40;
            Close.Location = new Point(15, 130);
            WarpPanel.Controls.Add(Close);
        }

        private void CreatePauseBox()
        {
            PauseLabel = new Label();
            PauseLabel.Text = "Pause menu";
            PauseLabel.AutoSize = true;
            PauseLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            PauseLabel.ForeColor = Color.BlueViolet;
            PauseLabel.Location = new Point(ScreenSize.X / 2 -120, ScreenSize.Y / 2 -250);
            this.Controls.Add(PauseLabel);

            resumeButton = new Button();
            resumeButton.Click += ResumeButton_Click;
            resumeButton.Text = "Resume";
            resumeButton.Width = 300;
            resumeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            resumeButton.ForeColor = Color.BlueViolet;
            resumeButton.Height = 100;
            resumeButton.Location = new Point(ScreenSize.X / 2-150, ScreenSize.Y / 2-200);
            this.Controls.Add(resumeButton);

            respawnButton = new Button();
            respawnButton.Click += RespawnButton_Click;
            respawnButton.Text = "Respawn";
            respawnButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            respawnButton.ForeColor = Color.BlueViolet;
            respawnButton.Width = 300;
            respawnButton.Height = 100;
            respawnButton.Location = new Point(ScreenSize.X / 2-150, ScreenSize.Y / 2-50);
            this.Controls.Add(respawnButton);


            leaveButton = new Button();
            leaveButton.Click += LeaveButton_Click;
            leaveButton.Text = "Leave";
            leaveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            leaveButton.ForeColor = Color.BlueViolet;
            leaveButton.Width = 300;
            leaveButton.Height = 100;
            leaveButton.Location = new Point(ScreenSize.X / 2-150, ScreenSize.Y / 2+100);
            this.Controls.Add(leaveButton);

        }

        private void CreateChatBox() {


            ChatLabel = new Label();
            ChatLabel.Text = "Chat box";
            ChatLabel.AutoSize = true;
            ChatLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ChatLabel.ForeColor = Color.BlueViolet;
            ChatLabel.Location = new Point(10, ScreenSize.Y - 250);
            this.Controls.Add(ChatLabel);
            
            chatBox.Width = 300;
            chatBox.Height = 120;
            chatBox.ReadOnly = true;            
            chatBox.Location = new Point(10, ScreenSize.Y - 210);
            this.Controls.Add(chatBox);
            

            sendButton = new Button();
            sendButton.Click += SendMessage_Click;
            sendButton.Text = "Send";
            sendButton.Width = 80;
            sendButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            sendButton.ForeColor = Color.BlueViolet;
            sendButton.Height = 30;
            sendButton.Location = new Point(320, ScreenSize.Y - 80);
            this.Controls.Add(sendButton);

            closeButton = new Button();
            closeButton.Click += CloseChatButton_Click;
            closeButton.Text = "Close";
            closeButton.Width = 80;
            closeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            closeButton.ForeColor = Color.BlueViolet;
            closeButton.Height = 30;
            closeButton.Location = new Point(320, ScreenSize.Y - 120);
            this.Controls.Add(closeButton);

            //messageBox.Click += RespawnButton_Click;
            messageBox.Text = "";
            messageBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            messageBox.ForeColor = Color.BlueViolet;
            messageBox.Width = 300;
            messageBox.Height = 30;
            messageBox.Location = new Point(10, ScreenSize.Y - 80);
            messageBox.Show();
            chatBox.Show();
            messageBox.Focus();
            Chat = true;

        }

        private void messageBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {
                CloseMesageBox();
            }
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage();
            }
        }

        public void Respawn()
        {

            RespawnLabel = new Label();
            RespawnLabel.AutoSize = true;
            RespawnLabel.Text = "5";
            RespawnLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            RespawnLabel.ForeColor = Color.BlueViolet;
            RespawnLabel.Location = new Point(ScreenSize.X / 2, ScreenSize.Y / 2);
            Controls.Add(RespawnLabel);

        }

        private void SendMessage() {

            if (messageBox.Text != "") {
                
                String MessageToSend;
                
                MessageToSend = Settings.UserName + ":" + messageBox.Text;
                chatBox.AppendText(MessageToSend);
                chatBox.AppendText("\n");
                chatBox.SelectionStart = chatBox.Text.Length;
                chatBox.ScrollToCaret();
                messageBox.Text = "";
                MessageToSend = MessageToSend + "\0";
                if (messageBox.Text.StartsWith("/")){
                    MessageToSend = messageBox.Text;
                }
                connection.sendTCPPacket(MessageToSend);

            }
        }
        
        public void ReceiveMessage(String message) {
            chatBox.AppendText(message);
            chatBox.AppendText("\n");
            chatBox.SelectionStart = chatBox.Text.Length;
            chatBox.ScrollToCaret();
        }
        

        private void CloseMesageBox() {

            messageBox.Hide();
            sendButton.Dispose();
            chatBox.Hide();
            this.Focus();
            Chat = false;
            closeButton.Dispose();
            ChatLabel.Dispose();
        }

        private void InfoScreen_tick(object sender, EventArgs e)
        {
            TimeToHideInfoScreen.Enabled = false;
            infoBox.Hide();
            this.Focus();

        }


        public static void InfoFeed()
        {
            infoBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            infoBox.ForeColor = Color.BlueViolet;
            infoBox.BackColor = Color.Black;
            infoBox.BorderStyle = BorderStyle.None;
            infoBox.Width = 300;
            infoBox.Height = 200;
            infoBox.Location = new Point(ScreenSize.X - 310, ScreenSize.Y - 200);

            infoBox.Show();
            TimeToHideInfoScreen.Enabled = true;


        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Escape)
            {
                if (!Paused)
                {
                    if (WarpBoxEnabled)
                    {
                        WarpPanel.Dispose();
                        WarpBoxEnabled = false;
                    }
                    CreatePauseBox();
                    Paused = true;
                }
                else
                {
                    PauseLabel.Dispose() ;
                    resumeButton.Dispose();
                    respawnButton.Dispose();
                    leaveButton.Dispose();
                    Paused = false;
                }
            }
            if (e.KeyCode == Keys.Enter && !Chat)
            {
                if (!Paused)
                {
                    CreateChatBox();
                    //Paused = true;
                }
                else
                {
                    //PausePanel.Dispose();
                    //Paused = false;
                }
            }
            if (PlayerManagerInstance.ThisPlayer.Health > 0 && Paused == false)
            {
                switch (e.KeyCode)
                {
                    case Keys.C:
                        ShowNames = !ShowNames;
                        break;
                    case Keys.G:
                        if (!WarpBoxEnabled)
                        {
                            WarpBoxEnabled = true;
                            CreateWarpBox();
                        }else
                        {
                            WarpPanel.Dispose();
                            WarpBoxEnabled = false;
                        }
                        break;
                    case Keys.CapsLock:
                        if (Settings.FlightMode == "Combat")
                        {
                            Settings.FlightMode = "Cruise";
                            Settings.SpeedMax *= Settings.SpeedMaxCruiseMultiplier;
                        }
                        else
                        {
                            Settings.FlightMode = "Combat";
                            Settings.SpeedMax /= Settings.SpeedMaxCruiseMultiplier;
                        }
                        break;
                    case Keys.D1:
                        WeaponType = 1;
                        WeaponTypeString = "Gatling gun";
                        break;
                    case Keys.D2:
                        WeaponType = 2;
                        WeaponTypeString = "Laser          ";
                        break;
                    case Keys.PageDown:
                        if (Settings.Speed != 0)
                            Settings.Speed -= 1;
                        break;
                    case Keys.PageUp:
                        Settings.Speed += 1;
                        break;
                    case Keys.D0:

                        break;
                    case Keys.A:
                        PlayerManagerInstance.MvLeft = true;
                        StarManagerInstance.MvLeft = true;
                        ProjectileManagerInstance.MvLeft = true;

                        break;
                    case Keys.D:
                        PlayerManagerInstance.MvRight = true;
                        StarManagerInstance.MvRight = true;
                        ProjectileManagerInstance.MvRight = true;
                        break;
                    case Keys.W:
                        PlayerManagerInstance.MvUp = true;
                        StarManagerInstance.MvUp = true;
                        ProjectileManagerInstance.MvUp = true;
                        break;
                    case Keys.S:
                        PlayerManagerInstance.MvDown = true;
                        StarManagerInstance.MvDown = true;
                        ProjectileManagerInstance.MvDown = true;
                        break;
                    case Keys.Left:
                        ShootLeft = true;
                        break;
                    case Keys.Right:
                        ShootRight = true;
                        break;
                    case Keys.Up:
                        ShootUp = true;
                        break;
                    case Keys.Down:
                        ShootDown = true;
                        break;
                    case Keys.Space:
                        ShootAll = true;
                        break;
                }

            }
        }
        private void ResumeButton_Click(object sender, EventArgs e)
        {
            PauseLabel.Dispose();
            resumeButton.Dispose();
            respawnButton.Dispose();
            leaveButton.Dispose();
            Paused = false;
        }

        private void CloseChatButton_Click(object sender, EventArgs e)
        {
            CloseMesageBox();
        }

        private void SendMessage_Click(object sender, EventArgs e)
        {            
            SendMessage();
        }

        private void RespawnButton_Click(object sender, EventArgs e)
        {
            PlayerManagerInstance.ThisPlayer.Health = 0;
            PauseLabel.Dispose();
            resumeButton.Dispose();
            respawnButton.Dispose();
            leaveButton.Dispose();
            Paused = false;
        }

        private void LeaveButton_Click(object sender, EventArgs e)
        {
            connection.Close_Socket_and_Exit();
            
        }


        private void CloseWarpButton_Click(object sender, EventArgs e)
        {
            WarpPanel.Dispose();
            WarpBoxEnabled = false;
        }

        private void WarpButton_Click(object sender, EventArgs e)
        {
            int WarpCoordsX;
            int WarpCoordsY;
            WarpBoxEnabled = false;

            int.TryParse(warpx.Text,out WarpCoordsX);
            int.TryParse(warpy.Text,out WarpCoordsY);
            WarpPanel.Dispose();
            PlayerManagerInstance.WarpPlayer(new Point(WarpCoordsX, WarpCoordsY));
        }


        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            
            switch (e.KeyCode)
            {
                case Keys.C:
                    //ShowNames = false;
                    break;
                case Keys.A:
                    PlayerManagerInstance.MvLeft = false;
                    StarManagerInstance.MvLeft = false;
                    ProjectileManagerInstance.MvLeft = false;
                    break;
                case Keys.D:
                    PlayerManagerInstance.MvRight = false;
                    StarManagerInstance.MvRight = false;
                    ProjectileManagerInstance.MvRight = false;
                    break;
                case Keys.W:
                    PlayerManagerInstance.MvUp = false;
                    StarManagerInstance.MvUp = false;
                    ProjectileManagerInstance.MvUp = false;
                    break;
                case Keys.S:
                    PlayerManagerInstance.MvDown = false;
                    StarManagerInstance.MvDown = false;
                    ProjectileManagerInstance.MvDown = false;
                    break;
                case Keys.Left:
                    ShootLeft = false;
                    break;
                case Keys.Right:
                    ShootRight = false;
                    break;
                case Keys.Up:
                    ShootUp = false;
                    break;
                case Keys.Down:
                    ShootDown = false;
                    break;
                case Keys.Space:
                    ShootAll = false;
                    break;
            }
            
        }


        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

            MousePressedDown = true;
        }
        

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            MousePressedDown = false;
        }
        

        private void GameScreen_MouseMove(object sender, MouseEventArgs e)
        {
            mouse = e;
        }


        private void ShootingFireRate_tick(object sender, EventArgs e)
        {
            String temp = "";
            if (Settings.Server)
                temp = connection.recieveTCPPacket();
            if (temp != "")
                ReceiveMessage(temp);

            if (Settings.FlightMode == "Combat")
            {
                Point PlayerLocation = PlayerManagerInstance.ThisPlayer.Location;
                PlayerLocation.X += 43;
                PlayerLocation.Y += 44;

                if (MousePressedDown)
                {
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, mouse.Location, 2, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, mouse.Location, 1, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, mouse.Location, 7, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, mouse.Location, 3, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, mouse.Location, 5, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, mouse.Location, 0, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, mouse.Location, 2, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, mouse.Location, 4, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, mouse.Location, 6, WeaponType);
                }

                if (ShootAll)
                {
                    connection.sendMissile(Settings.PlayerId, 0, 5);

                    ProjectileManagerInstance.Shoot(Settings.PlayerId, PlayerLocation, 1, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, PlayerLocation, 7, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, PlayerLocation, 3, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, PlayerLocation, 5, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, PlayerLocation, 0, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, PlayerLocation, 2, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, PlayerLocation, 4, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, PlayerLocation, 6, WeaponType);
                }
                else if (ShootUp && ShootRight)
                {
                    connection.sendMissile(Settings.PlayerId, 1, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, PlayerLocation, 1, WeaponType);
                }
                else if (ShootLeft && ShootUp)
                {
                    connection.sendMissile(Settings.PlayerId, 7, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, PlayerLocation, 7, WeaponType);
                }
                else if (ShootRight && ShootDown)
                {
                    connection.sendMissile(Settings.PlayerId, 3, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, PlayerLocation, 3, WeaponType);
                }
                else if (ShootDown && ShootLeft)
                {
                    connection.sendMissile(Settings.PlayerId, 5, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, PlayerLocation, 5, WeaponType);
                }
                else if (ShootUp)
                {
                    connection.sendMissile(Settings.PlayerId, 0, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, PlayerLocation, 0, WeaponType);
                }
                else if (ShootRight)
                {
                    connection.sendMissile(Settings.PlayerId, 2, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, PlayerLocation, 2, WeaponType);
                }
                else if (ShootDown)
                {
                    connection.sendMissile(Settings.PlayerId, 4, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, PlayerLocation, 4, WeaponType);
                }
                else if (ShootLeft)
                {
                    connection.sendMissile(Settings.PlayerId, 6, WeaponType);
                    ProjectileManagerInstance.Shoot(Settings.PlayerId, PlayerLocation, 6, WeaponType);
                }
            }
        }
    }
}
