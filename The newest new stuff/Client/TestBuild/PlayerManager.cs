using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace TestBuild
{
    class PlayerManager
    {

        int Acceleration = Settings.Acceleration;
        int ForceMax = Settings.Speed;
        int XForce = 0;
        int YForce = 0;
        Form MainForm;
        internal bool MvLeft,
                      MvRight,
                      MvUp,
                      MvDown;
        bool InWarp;
        bool ChargingWarp;

        bool DuringWarp;
        bool WarpXRight;
        bool WarpYDown;
        bool IsYGreaterThanX;
        int WarpAngle;
        bool WarpTurn;
        int WarpMoveTimer;
        int WhenToSlow;
        Timer CheckForKeyPressTimer = new Timer();
        Timer SpeedUp = new Timer();
        Timer Respawn = new Timer();
        Timer CheckWarp = new Timer();
        Timer PlayerCoordinates = new Timer();
        Point VectorToMove;
        public static int TimetoRespawn = -1;
        int TempSpeedMax;
        public static List<Player> PlayerList = new List<Player>();
        int tempDecceleration;
        int tempAcceleration;
        Random random = new Random();
        Bitmap PlayerShipBitmap = (Bitmap)Image.FromFile("Player0.png");

        ClientConnections Connnections;
        internal struct Player
        {
            internal Bitmap PlayerShip;
            internal int PlayerID;
            internal Rectangle PlayerShape;
            internal string UserName;
            internal int Health;
            internal int direction;
            internal int directiontorotate;
            internal Point Location;
            internal bool Enabled;
        }
        internal Player ThisPlayer;
        Point WarpPoint;

        public PlayerManager(Form MainFormLocal, ClientConnections ConnnectionsTemp)
        {
            Connnections = ConnnectionsTemp;
            for (int i = 0; i < 100; i++)
            {
                Player newPlayer = new Player();
                newPlayer.Health = Settings.Health;
                newPlayer.direction = 0;
                newPlayer.Enabled = false;
                newPlayer.Location = new Point(0,0);
                newPlayer.PlayerShape = new Rectangle(GameScreen.ScreenSize.X / 2 -Settings.PlayerWidth/2, GameScreen.ScreenSize.Y / 2 - Settings.PlayerHeight / 2, Settings.PlayerWidth, Settings.PlayerHeight);
                newPlayer.PlayerShip = PlayerShipBitmap;
                PlayerList.Add(newPlayer);
            }
            
            for (int a = 0; a < PlayerShipBitmap.Width; a++)
            {
                for (int b = 0; b < PlayerShipBitmap.Height; b++)
                {
                    Color tempColor = PlayerShipBitmap.GetPixel(a, b);
                    if (tempColor.R < 255- Settings.Red && tempColor.G < 255 - Settings.Green && tempColor.B < 255 - Settings.Blue)
                        PlayerShipBitmap.SetPixel(a, b, Color.FromArgb(tempColor.A, tempColor.R+ Settings.Red, tempColor.G + Settings.Green, tempColor.B + Settings.Blue));
                }
            }
            ThisPlayer.PlayerShip = PlayerShipBitmap;
            ThisPlayer.UserName = Settings.UserName;
            ThisPlayer.Health = Settings.Health;
            ThisPlayer.PlayerID = Settings.PlayerId;
            ThisPlayer.Enabled = true;
            ThisPlayer.direction = 0;
            ThisPlayer.Location = new Point(0, 0);
            ThisPlayer.PlayerShape = new Rectangle(GameScreen.ScreenSize.X / 2 - Settings.PlayerWidth / 2, GameScreen.ScreenSize.Y / 2 - Settings.PlayerHeight / 2, Settings.PlayerWidth, Settings.PlayerHeight);
            MainForm = MainFormLocal;
            CheckForKeyPressTimer.Enabled = true;
            CheckForKeyPressTimer.Interval = Settings.CheckForKeyPressInterval;
            CheckForKeyPressTimer.Tick += new System.EventHandler(CheckForKeyPress_tick);

            Respawn.Enabled = false;
            Respawn.Interval = 1000;
            Respawn.Tick += new System.EventHandler(Respawn_tick);

            PlayerCoordinates.Enabled = true;
            PlayerCoordinates.Interval = 20;
            PlayerCoordinates.Tick += new System.EventHandler(MovePlayerCoords);

            SpeedUp.Enabled = true;
            SpeedUp.Interval = 50;
            SpeedUp.Tick += new System.EventHandler(SpeedUp_tick);

            CheckWarp.Enabled = false;
            CheckWarp.Interval = 5000;
            CheckWarp.Tick += new System.EventHandler(CheckWarp_tick);

            Connnections.sendStruct(ThisPlayer.PlayerID, ThisPlayer.Location.X, ThisPlayer.Location.Y, ThisPlayer.Health, ThisPlayer.direction);

        }

        private void Respawn_tick(object sender, EventArgs e)
        {
            TimetoRespawn-=1;

            if (TimetoRespawn == 0)
            {
                ThisPlayer.Location = new Point(random.Next(-1000, 1000), random.Next(-1000, 1000));
                ThisPlayer.Health = Settings.Health;
                ThisPlayer.Enabled = true;
                ThisPlayer.UserName = Settings.UserName;
                Respawn.Enabled = false;
                TimetoRespawn = -1;
                GameScreen.RespawnLabel.Dispose();
            }
            GameScreen.RespawnLabel.Text = TimetoRespawn.ToString();


        }
        private void CheckWarp_tick(object sender, EventArgs e)
        {
            ChargingWarp = false;
            CheckWarp.Interval = 15;
            InWarp = true;
            DuringWarp = true;
            Settings.FlightMode = "Warp";

            if (WarpXRight)
            {
                if (WarpYDown)
                {
                    if (IsYGreaterThanX && ThisPlayer.Location.Y > WhenToSlow)
                    {
                        InWarp = false;
                        Settings.SpeedMax = TempSpeedMax;
                        Settings.MaxStarsOnScreen = 2000;
                    }
                    else if (!IsYGreaterThanX && ThisPlayer.Location.X > WhenToSlow)
                    {
                        InWarp = false;
                        Settings.SpeedMax = TempSpeedMax;
                        Settings.MaxStarsOnScreen = 2000;
                    }
                }
                else
                {
                    if (IsYGreaterThanX && ThisPlayer.Location.Y < WhenToSlow)
                    {
                        InWarp = false;
                        Settings.SpeedMax = TempSpeedMax;
                        Settings.MaxStarsOnScreen = 2000;
                    }
                    else if (!IsYGreaterThanX && ThisPlayer.Location.X > WhenToSlow)
                    {
                        InWarp = false;
                        Settings.SpeedMax = TempSpeedMax;
                        Settings.MaxStarsOnScreen = 2000;
                    }

                }
            }
            else
            {
                if (!WarpYDown)
                {
                    if (IsYGreaterThanX && ThisPlayer.Location.Y < WhenToSlow)
                    {
                        InWarp = false;
                        Settings.SpeedMax = TempSpeedMax;
                    }
                    else if (!IsYGreaterThanX && ThisPlayer.Location.X < WhenToSlow)
                    {
                        InWarp = false;
                        Settings.SpeedMax = TempSpeedMax;
                    }
                }
                else
                {
                    if (IsYGreaterThanX && ThisPlayer.Location.Y > WhenToSlow)
                    {
                        InWarp = false;
                        Settings.SpeedMax = TempSpeedMax;
                        Settings.MaxStarsOnScreen = 2000;
                    }
                    else if (!IsYGreaterThanX && ThisPlayer.Location.X < WhenToSlow)
                    {
                        InWarp = false;
                        Settings.SpeedMax = TempSpeedMax;
                        Settings.MaxStarsOnScreen = 2000;

                    }

                }

            }



            if (Settings.Speed < 20 && !InWarp)
            {
                Settings.FlightMode = "Combat";
                Settings.Deceleration = tempDecceleration;
                Settings.Acceleration = tempAcceleration;
                Settings.SpeedMax = TempSpeedMax;
                Settings.SpinSpeed = 9;

                DuringWarp = false;
                CheckWarp.Enabled = false;
            }
        }


        public void MovePlayer(String UserName)
        {
            for (int i = 0; i < PlayerList.Count; i++)
            {
                if (PlayerList[i].PlayerID == int.Parse(UserName) && PlayerList[i].Enabled == true)
                    ThisPlayer.Location = PlayerList[i].Location;

            }
        }

        public void WarpPlayer(Point WarpPointTemp)
        {

            if (Settings.FlightMode == "Cruise")
            {
                Settings.FlightMode = "Combat";
                Settings.SpeedMax /= Settings.SpeedMaxCruiseMultiplier;
            }

            WarpPoint = WarpPointTemp;
            WarpXRight = false;
            WarpYDown = false;
            VectorToMove = new Point(WarpPoint.X, WarpPoint.Y);
            Settings.SpinSpeed = 1;
            Point DistanceToTravel = new Point(VectorToMove.X - ThisPlayer.Location.X, VectorToMove.Y - ThisPlayer.Location.Y);

            WarpAngle = (int)(Math.Atan2(DistanceToTravel.X, DistanceToTravel.Y) * (180 / Math.PI));

 

            if (DistanceToTravel.X != Math.Abs(DistanceToTravel.X))
            {
                WarpAngle = (int)(Math.Atan2(DistanceToTravel.X, DistanceToTravel.Y) * (180 / Math.PI));
                WarpAngle = Math.Abs(WarpAngle) + 180;
            }
            else
            {
                WarpAngle = (int)(Math.Atan2(DistanceToTravel.X, DistanceToTravel.Y) * (180 / Math.PI));
                if (WarpAngle > 90)
                {
                    WarpAngle = Math.Abs(WarpAngle) - 90;
                }
                else
                {
                    WarpAngle = Math.Abs(WarpAngle) + 90;
                }
            }

            


            ChargingWarp = true;

            if (WarpPoint.X >= ThisPlayer.Location.X)
                WarpXRight = true;

            if (WarpPoint.Y >= ThisPlayer.Location.Y)
                WarpYDown = true;

            IsYGreaterThanX = false;
            if (Math.Abs(DistanceToTravel.X) <=Math.Abs(DistanceToTravel.Y))
                IsYGreaterThanX = true;

            CheckWarp.Interval = 4000;

            
            TempSpeedMax = Settings.SpeedMax;
            Settings.SpeedMax = 2000;
            tempDecceleration = Settings.Deceleration;
            tempAcceleration = Settings.Acceleration;
            Settings.Deceleration = 40;
            Settings.Speed = 0;
            Settings.Acceleration = 10;
            Settings.FlightMode = "Warp charging";
            CheckWarp.Enabled = true;

            if (Math.Abs(DistanceToTravel.X) > Math.Abs(DistanceToTravel.Y))
            {
                WhenToSlow = (int)(WarpPoint.X * 0.95);

            }else if (Math.Abs(DistanceToTravel.X) <= Math.Abs(DistanceToTravel.Y))
            {
                WhenToSlow = (int)(WarpPoint.Y * 0.98);
            }



        }
        private void MovePlayerCoords(object sender, EventArgs e)
        {
            Point TopLeftWorldCoords;
            for (int i = 0; i < 100; i++)
            {
                Player tempPlayer = PlayerList[i];
                if (tempPlayer.Enabled)
                {
                    TopLeftWorldCoords = new Point(GameScreen.LocationOfPlayer.X - GameScreen.ScreenSize.X / 2, GameScreen.LocationOfPlayer.Y - GameScreen.ScreenSize.Y / 2);
                    TopLeftWorldCoords = new Point(tempPlayer.Location.X - TopLeftWorldCoords.X - Settings.PlayerWidth / 2, tempPlayer.Location.Y - TopLeftWorldCoords.Y - Settings.PlayerHeight / 2);
                    tempPlayer.PlayerShape.Location = TopLeftWorldCoords;
                    PlayerList[i] = tempPlayer;
                }
            }


        }



        private void SpeedUp_tick(object sender, EventArgs e)
        {

            
            if (((MvUp || MvRight || MvDown || MvLeft) && Settings.Speed <= Settings.SpeedMax) || InWarp)
            {
                if (Settings.Speed < Settings.SpeedMax)
                {
                    if (Settings.FlightMode == "Combat" || Settings.FlightMode == "Warp")
                    {
                        Settings.Speed += Settings.Acceleration;
                    }
                    else
                    {
                        Settings.Speed += Settings.Acceleration*2;
                    }
                }

            }else if (Settings.Speed > Settings.SpeedMax  || (!InWarp && DuringWarp))
            {
                if (Settings.Speed < Settings.SpeedMax + Settings.Deceleration * 4)
                {
                    Settings.Speed = Settings.SpeedMax;
                }else
                {
                    Settings.Speed -= Settings.Deceleration * 4;
                }
            }
            else
            {
                if (Settings.Speed > 0)
                    Settings.Speed -= Settings.Deceleration;
                

                if (Settings.Speed <=0)
                    Settings.Speed = 0;

            }
                    
    }

        private void CheckForKeyPress_tick(object sender, EventArgs e)
        {

            if (ThisPlayer.Health <= 0 && TimetoRespawn == 5)
            {                
                Respawn.Enabled = true;
            }

            if (ThisPlayer.direction > 360)
                ThisPlayer.direction = 0;
            if (ThisPlayer.direction < 0)
                ThisPlayer.direction = 360;
            
            Player PlayerTemp = ThisPlayer;

            if (DuringWarp)
            {

                if (PlayerTemp.Location.X < VectorToMove.X && WarpXRight)
                    PlayerTemp.Location = new Point(PlayerTemp.Location.X + Settings.Speed, PlayerTemp.Location.Y);

                if (PlayerTemp.Location.Y < VectorToMove.Y && WarpYDown)
                    PlayerTemp.Location = new Point(PlayerTemp.Location.X, PlayerTemp.Location.Y + Settings.Speed);

                if (PlayerTemp.Location.X > VectorToMove.X && !WarpXRight)
                    PlayerTemp.Location = new Point(PlayerTemp.Location.X - Settings.Speed, PlayerTemp.Location.Y);

                if (PlayerTemp.Location.Y > VectorToMove.Y && !WarpYDown)
                    PlayerTemp.Location = new Point(PlayerTemp.Location.X, PlayerTemp.Location.Y - Settings.Speed);

                WarpMoveTimer += 1;

                if (WarpMoveTimer > (VectorToMove.X + VectorToMove.Y))
                {
                    WarpMoveTimer = 1;

                }
            }



            bool Update = true;
            if (MvUp && MvRight)
            {

                PlayerTemp.directiontorotate = 1;
                if (YForce < ForceMax)
                    YForce += Acceleration;
                PlayerTemp.Location = new Point(PlayerTemp.Location.X + Settings.Speed, PlayerTemp.Location.Y - Settings.Speed);

            }
            else if (MvLeft && MvUp)
            {
                PlayerTemp.directiontorotate = 7;
                if (YForce < ForceMax)
                    YForce += Acceleration;
                PlayerTemp.Location = new Point(PlayerTemp.Location.X - Settings.Speed, PlayerTemp.Location.Y - Settings.Speed);

            }
            else if (MvRight && MvDown)
            {
                PlayerTemp.directiontorotate = 3;
                if (YForce < ForceMax)
                    YForce += Acceleration;
                PlayerTemp.Location = new Point(PlayerTemp.Location.X + Settings.Speed, PlayerTemp.Location.Y + Settings.Speed);
            }
            else if (MvDown && MvLeft)
            {
                PlayerTemp.directiontorotate = 5;
                if (YForce < ForceMax)
                    YForce += Acceleration;
                PlayerTemp.Location = new Point(PlayerTemp.Location.X - Settings.Speed, PlayerTemp.Location.Y + Settings.Speed);

            }
            else if (MvUp)
            {
                PlayerTemp.directiontorotate = 0;
                if (YForce < ForceMax)
                    YForce += Acceleration;
                PlayerTemp.Location = new Point(PlayerTemp.Location.X, PlayerTemp.Location.Y - Settings.Speed);

            }
            else if (MvRight)
            {
                PlayerTemp.directiontorotate = 2;
                if (YForce < ForceMax)
                    YForce += Acceleration;
                PlayerTemp.Location = new Point(PlayerTemp.Location.X + Settings.Speed, PlayerTemp.Location.Y);

            }
            else if (MvDown)
            {
                PlayerTemp.directiontorotate = 4;
                if (YForce < ForceMax)
                    YForce += Acceleration;
                PlayerTemp.Location = new Point(PlayerTemp.Location.X, PlayerTemp.Location.Y + Settings.Speed);
            }

            else if (MvLeft)
            {
                PlayerTemp.directiontorotate = 6;
                if (YForce < ForceMax)
                    YForce += Acceleration;
                PlayerTemp.Location = new Point(PlayerTemp.Location.X - Settings.Speed, PlayerTemp.Location.Y);

            }

            


            if (ChargingWarp || DuringWarp)
            {
                GameScreen.ProjectileManagerInstance.Shoot(-1, ThisPlayer.Location, 0, 4);
                Connnections.sendMissile(-1, 0, 4);

            }

            if (!DuringWarp)
            {
                if (ChargingWarp)
                {
                    if (PlayerTemp.direction != WarpAngle && WarpTurn)
                    {

                        int tempShifter = 180 - WarpAngle;
                        if (PlayerTemp.direction + tempShifter > WarpAngle + tempShifter)
                            PlayerTemp.direction -= 3;
                        if (PlayerTemp.direction + tempShifter <= WarpAngle + tempShifter)
                            PlayerTemp.direction += 3;
                        WarpTurn = false;
                        
                    }
                    else
                    {
                        WarpTurn = true;
                    }

                    
                }
                else
                {



                    switch (PlayerTemp.directiontorotate)
                    {
                        case 1:
                            if (PlayerTemp.direction != 45)
                            {
                                if (PlayerTemp.direction > 45 && PlayerTemp.direction < 225)
                                    PlayerTemp.direction -= Settings.SpinSpeed;
                                if (PlayerTemp.direction <= 45 || PlayerTemp.direction >= 225)
                                    PlayerTemp.direction += Settings.SpinSpeed;
                            }
                            for (int i = 0; i < Settings.Speed / 4; i++)
                            {
                                GameScreen.ProjectileManagerInstance.Shoot(-1, PlayerTemp.Location, 1, 3);
                                //Connnections.sendMissile(-1, 1, 3);

                            }
                            PlayerTemp.Location = new Point(PlayerTemp.Location.X + Settings.Speed, PlayerTemp.Location.Y - Settings.Speed);
                            break;
                        case 7:
                            if (PlayerTemp.direction != 315)
                            {
                                if (PlayerTemp.direction > 315 || PlayerTemp.direction < 135)
                                    PlayerTemp.direction -= Settings.SpinSpeed;
                                if (PlayerTemp.direction <= 315 && PlayerTemp.direction >= 135)
                                    PlayerTemp.direction += Settings.SpinSpeed;
                            }
                            for (int i = 0; i < Settings.Speed / 4; i++)
                            {
                                GameScreen.ProjectileManagerInstance.Shoot(-1, PlayerTemp.Location, 7, 3);
                                //Connnections.sendMissile(-1, 7, 3);
                            }

                            PlayerTemp.Location = new Point(PlayerTemp.Location.X - Settings.Speed, PlayerTemp.Location.Y - Settings.Speed);
                            break;
                        case 3:
                            if (PlayerTemp.direction != 135)
                            {
                                if (PlayerTemp.direction > 135 && PlayerTemp.direction < 315)
                                    PlayerTemp.direction -= Settings.SpinSpeed;
                                if (PlayerTemp.direction <= 135 || PlayerTemp.direction >= 315)
                                    PlayerTemp.direction += Settings.SpinSpeed;
                            }
                            for (int i = 0; i < Settings.Speed / 4; i++)
                            {
                                GameScreen.ProjectileManagerInstance.Shoot(-1, PlayerTemp.Location, 3, 3);
                                //Connnections.sendMissile(-1, 3, 3);
                            }

                            PlayerTemp.Location = new Point(PlayerTemp.Location.X + Settings.Speed, PlayerTemp.Location.Y + Settings.Speed);
                            break;
                        case 5:
                            if (PlayerTemp.direction != 225)
                            {
                                if (PlayerTemp.direction > 225 || PlayerTemp.direction < 45)
                                    PlayerTemp.direction -= Settings.SpinSpeed;
                                if (PlayerTemp.direction <= 225 && PlayerTemp.direction >= 45)
                                    PlayerTemp.direction += Settings.SpinSpeed;
                            }
                            for (int i = 0; i < Settings.Speed / 4; i++)
                            {
                                GameScreen.ProjectileManagerInstance.Shoot(-1, PlayerTemp.Location, 5, 3);
                                //Connnections.sendMissile(-1, 5, 3);
                            }
                            PlayerTemp.Location = new Point(PlayerTemp.Location.X - Settings.Speed, PlayerTemp.Location.Y + Settings.Speed);
                            break;
                        case 0:
                            if (PlayerTemp.direction != 0)
                            {
                                if (PlayerTemp.direction > 0 && PlayerTemp.direction < 180)
                                    PlayerTemp.direction -= Settings.SpinSpeed;
                                if (PlayerTemp.direction <= 360 && PlayerTemp.direction >= 180)
                                    PlayerTemp.direction += Settings.SpinSpeed;
                            }
                            for (int i = 0; i < Settings.Speed / 4; i++)
                            {
                                GameScreen.ProjectileManagerInstance.Shoot(-1, PlayerTemp.Location, 0, 3);
                                //Connnections.sendMissile(-1, 0, 3);                                
                            }
                            PlayerTemp.Location.Y -= Settings.Speed;
                            break;
                        case 2:
                            if (PlayerTemp.direction != 90)
                            {
                                if (PlayerTemp.direction > 90 && PlayerTemp.direction < 270)
                                    PlayerTemp.direction -= Settings.SpinSpeed;
                                if (PlayerTemp.direction >=270 || PlayerTemp.direction <90)
                                    PlayerTemp.direction += Settings.SpinSpeed;
                            }
                            for (int i = 0; i < Settings.Speed / 4; i++)
                            {
                                GameScreen.ProjectileManagerInstance.Shoot(-1, PlayerTemp.Location, 2, 3);
                                //Connnections.sendMissile(-1, 2, 3);
                            }
                            PlayerTemp.Location.X += Settings.Speed;
                            break;
                        case 4:
                            if (PlayerTemp.direction != 180)
                            {
                                if (PlayerTemp.direction > 180 && PlayerTemp.direction <= 360)
                                    PlayerTemp.direction -= Settings.SpinSpeed;
                                if (PlayerTemp.direction < 180 && PlayerTemp.direction >= 0)
                                    PlayerTemp.direction += Settings.SpinSpeed;
                            }
                            for (int i = 0; i < Settings.Speed / 4; i++)
                            {
                                GameScreen.ProjectileManagerInstance.Shoot(-1, PlayerTemp.Location, 4, 3);
                                //Connnections.sendMissile(-1, 4, 3);
                            }
                            PlayerTemp.Location.Y += Settings.Speed;
                            break;
                        case 6:
                            if (PlayerTemp.direction != 270)
                            {
                                if (PlayerTemp.direction > 270 || PlayerTemp.direction < 90)
                                    PlayerTemp.direction -= Settings.SpinSpeed;
                                if (PlayerTemp.direction < 270 && PlayerTemp.direction >= 90)
                                    PlayerTemp.direction += Settings.SpinSpeed;
                            }
                            for (int i = 0; i < Settings.Speed / 4; i++)
                            {
                                GameScreen.ProjectileManagerInstance.Shoot(-1, PlayerTemp.Location, 6, 3);
                                //Connnections.sendMissile(-1, 6, 3);
                            }
                            PlayerTemp.Location.X -= Settings.Speed;
                            break;
                    }

                }


            }





            if (Update)
            {
                ThisPlayer = PlayerTemp;
                Connnections.sendStruct(Settings.PlayerId, ThisPlayer.Location.X, ThisPlayer.Location.Y, ThisPlayer.Health, ThisPlayer.direction);
            }


        }
    }
}
