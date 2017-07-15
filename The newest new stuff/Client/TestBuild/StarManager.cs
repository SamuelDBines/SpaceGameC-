using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace TestBuild
{
    class StarManager
    {
        Stars newStar;
        internal List<Stars> StarList = new List<Stars>(Settings.MaxStarsOnScreen);
        internal List<Stars> StarListToRedraw = new List<Stars>();

        internal bool MvLeft,
                      MvRight,
                      MvUp,
                      MvDown;

        internal bool EnableParallax = Settings.Parallax;

        Random random = new Random();
        Timer StarUpdate = new Timer();
        Timer StarCreate = new Timer();
        internal struct Stars
        {
            internal Rectangle Shape;
            internal bool IsDead;
            internal Color Color;
            internal int Distance;
            internal bool flicker;
            internal bool ToMove;
            internal bool SlowerLayers;

        }
        int StarSpeed;
        int StarSpeedDiagonal;

        PlayerManager PlayerManagerInstance;
        int StarsActive = 0;
        Form MainForm;
        int StarSize;


        public StarManager(Form MainFormTemp,PlayerManager PlayerManagerInstanceTemp)
        {
            PlayerManagerInstance = PlayerManagerInstanceTemp;
            MainForm = MainFormTemp;
            StarUpdate.Enabled = true;
            StarUpdate.Interval = 15;
            StarUpdate.Tick += new System.EventHandler(StarUpdate_tick);


            StarCreate.Enabled = true;
            StarCreate.Interval = 100;
            StarCreate.Tick += new System.EventHandler(StarCreate_tick);

            for (int i = 0; i < Settings.MaxStarsOnScreen; i++)
            {
                Stars newStar = new Stars();
                newStar.SlowerLayers = false;
                newStar = CreateStar(newStar);
                newStar.Shape = new Rectangle(-800 + random.Next(GameScreen.ScreenSize.X + 800), -800 + random.Next(GameScreen.ScreenSize.Y + 800), newStar.Shape.Width, newStar.Shape.Height);
                StarList.Add(newStar);
            }

        }

        private void StarCreate_tick(object sender, EventArgs e)
        {
            int counter = 0;
            for (int i = 0; i < Settings.MaxStarsOnScreen; i++)
            {
                if (!StarList[i].Shape.IntersectsWith(Rectangle.FromLTRB(-1000, -1000, GameScreen.ScreenSize.X + 1000, GameScreen.ScreenSize.Y + 1000)))
                {
                    counter += 1;
                    newStar = CreateStar(newStar);
                    StarList[i] = newStar;

                    if (counter >= Settings.MaxStarsOnScreen)
                        break;
                }
            }

        }
        private void StarUpdate_tick(object sender, EventArgs e)
        {
            int tempSpeed = Settings.Speed*2;

            if (Settings.FlightMode == "Warp")
            {
                tempSpeed = -10+Settings.Speed/2;
                //Settings.MaxStarsOnScreen = Settings.MaxStarsOnScreenBackup + Settings.Speed*2;

            }
            else
            {
                Settings.MaxStarsOnScreen = Settings.MaxStarsOnScreenBackup;

            }

            for (int i = 0; i < Settings.MaxStarsOnScreen-1; i++)
            {
                if (!StarList[i].IsDead)

                {

                    newStar = StarList[i];
                    if (newStar.flicker == true)
                    {
                        newStar.Color = Color.FromArgb(250, newStar.Color.R, newStar.Color.G, newStar.Color.B);
                        newStar.flicker = false;
                    }
                    else if (random.Next(Settings.FlickerFrequency) == 1)
                    {
                        newStar.Color = Color.FromArgb(random.Next(80), newStar.Color.R, newStar.Color.G, newStar.Color.B);
                        newStar.flicker = true;
                    }
                    if (true)
                    {
                        if (tempSpeed / 2 >= 10)
                        {
                            StarSpeed = (tempSpeed / 2 - StarList[i].Distance);
                            StarSpeedDiagonal = (tempSpeed / 2 - StarList[i].Distance);
                        }
                        else
                        {
                            int tempDistance = StarList[i].Distance;
                            while (tempSpeed / 2 - tempDistance < 0)
                                tempDistance -= 1;
                            if (tempSpeed <= 0)
                                tempDistance = 0;
                            StarSpeed = (tempSpeed / 2 - tempDistance);
                            StarSpeedDiagonal = (tempSpeed / 2 - tempDistance);
                        }
                    }
                    else
                    {
                        StarSpeed = (tempSpeed - 5);
                        StarSpeedDiagonal = (int)(tempSpeed - 5);

                    }
                    if (((!newStar.SlowerLayers) || (newStar.SlowerLayers && newStar.ToMove)) && tempSpeed != 0 )
                    {
                        if (((PlayerManagerInstance.ThisPlayer.direction > 338 || PlayerManagerInstance.ThisPlayer.direction < 22) && Settings.FlightMode == "Warp")|| (Settings.FlightMode != "Warp" && PlayerManagerInstance.ThisPlayer.directiontorotate == 0))
                        {
                            newStar.Shape.Y += StarSpeed;
                        }
                        else if ((PlayerManagerInstance.ThisPlayer.direction > 23 && PlayerManagerInstance.ThisPlayer.direction < 67 && Settings.FlightMode == "Warp") || (Settings.FlightMode != "Warp" && PlayerManagerInstance.ThisPlayer.directiontorotate == 1))
                        {
                            newStar.Shape.X -= StarSpeedDiagonal;
                            newStar.Shape.Y += StarSpeedDiagonal;
                        }
                        else if ((PlayerManagerInstance.ThisPlayer.direction > 68 && PlayerManagerInstance.ThisPlayer.direction < 112 && Settings.FlightMode == "Warp") || (Settings.FlightMode != "Warp" && PlayerManagerInstance.ThisPlayer.directiontorotate == 2))
                        {
                            newStar.Shape.X -= StarSpeed;
                        }
                        else if ((PlayerManagerInstance.ThisPlayer.direction > 113 && PlayerManagerInstance.ThisPlayer.direction < 157 && Settings.FlightMode == "Warp") || (Settings.FlightMode != "Warp" && PlayerManagerInstance.ThisPlayer.directiontorotate == 3))
                        {
                            newStar.Shape.X -= StarSpeedDiagonal;
                            newStar.Shape.Y -= StarSpeedDiagonal;
                        }
                        else if ((PlayerManagerInstance.ThisPlayer.direction > 157 && PlayerManagerInstance.ThisPlayer.direction < 202 && Settings.FlightMode == "Warp") || (Settings.FlightMode != "Warp" && PlayerManagerInstance.ThisPlayer.directiontorotate == 4))
                        {
                            newStar.Shape.Y -= StarSpeed;
                        }
                        else if ((PlayerManagerInstance.ThisPlayer.direction > 203 && PlayerManagerInstance.ThisPlayer.direction < 247 && Settings.FlightMode == "Warp") || (Settings.FlightMode != "Warp" && PlayerManagerInstance.ThisPlayer.directiontorotate == 5))
                        {
                            newStar.Shape.X += StarSpeedDiagonal;
                            newStar.Shape.Y -= StarSpeedDiagonal;
                        }
                        else if ((PlayerManagerInstance.ThisPlayer.direction > 248 && PlayerManagerInstance.ThisPlayer.direction < 292 && Settings.FlightMode == "Warp") || (Settings.FlightMode != "Warp" && PlayerManagerInstance.ThisPlayer.directiontorotate == 6))
                        {
                            newStar.Shape.X += StarSpeed;
                        }
                        else if ((PlayerManagerInstance.ThisPlayer.direction > 293 && PlayerManagerInstance.ThisPlayer.direction < 337 && Settings.FlightMode == "Warp") || (Settings.FlightMode != "Warp" && PlayerManagerInstance.ThisPlayer.directiontorotate == 7))
                        {
                            newStar.Shape.X += StarSpeedDiagonal;
                            newStar.Shape.Y += StarSpeedDiagonal;
                        }
                        if (newStar.SlowerLayers)
                            newStar.ToMove = false;

                        StarList[i] = newStar;

                    }
                    else
                    {
                        newStar.ToMove = true;
                        StarList[i] = newStar;

                    }


                    //NewValue = (((OldValue - OldMin) * (NewMax - NewMin)) / (OldMax - OldMin)) + NewMin
                    //newStar.Shape.Y -= PlayerManagerInstance.ThisPlayer.Force.Y;


                    
                }
                
            }

        }

    
        

        public Stars CreateStar(Stars newStar)
        {
            StarsActive += 1;
            newStar.IsDead = false;
            newStar.SlowerLayers = false;
            StarSize = random.Next(25);
            //int SpeedTemp = Settings.Speed;
            int Adder = 0;
            if (StarSize >= 0 && StarSize <= 3)
            {
                newStar.Color = Color.FromArgb(random.Next(100), 245 + random.Next(10), 245 + random.Next(10), 245 + random.Next(10));
                newStar.Distance = random.Next(1) + Adder;
                newStar.SlowerLayers = true;
                newStar.ToMove = true;
                StarSize = 1;
            }
            else if (StarSize >= 4 && StarSize <= 5)
            {
                newStar.Color = Color.FromArgb(20 + random.Next(100), 245 + random.Next(10), 245 + random.Next(10), 245 + random.Next(10));
                newStar.Distance = random.Next(2,3) + Adder;
                newStar.SlowerLayers = true;
                newStar.ToMove = true;
                StarSize = 2;
            }
            else if (StarSize >= 6 && StarSize <= 7)
            {
                newStar.Color = Color.FromArgb(40 + random.Next(100), 245 + random.Next(10), 245 + random.Next(10), 245 + random.Next(10));
                newStar.Distance = random.Next(4, 5) + Adder;
                newStar.SlowerLayers = true;
                newStar.ToMove = true;
                StarSize = 2;
            }
            else if (StarSize >= 8 && StarSize <= 10)
            {
                newStar.Color = Color.FromArgb(50 + random.Next(100), 245 + random.Next(10), 245 + random.Next(10), 245 + random.Next(10));
                newStar.Distance = random.Next(6, 7) + Adder;
                newStar.SlowerLayers = true;
                newStar.ToMove = true;
                StarSize = 2;
            }
            else if (StarSize >= 11 && StarSize <= 12)
            {
                newStar.Color = Color.FromArgb(60 + random.Next(100), 245 + random.Next(10), 245 + random.Next(10), 245 + random.Next(10));
                newStar.Distance = random.Next(8,9) + Adder;
                newStar.SlowerLayers = true;
                newStar.ToMove = true;
                StarSize = 2;
            }
            else if (StarSize >= 13 && StarSize <= 15)
            {
                newStar.Color = Color.FromArgb(75 + random.Next(100), 245 + random.Next(10), 245 + random.Next(10), 245 + random.Next(10));
                newStar.Distance = random.Next(0) + Adder;
                StarSize = 2;
            }
            else if (StarSize >= 16 && StarSize <= 20)
            {
                newStar.Color = Color.FromArgb(75 + random.Next(100), 245 + random.Next(10), 245 + random.Next(10), 245 + random.Next(10));
                newStar.Distance = random.Next(1, 2) + Adder;
                StarSize = 2;
            }
            else if (StarSize >= 21 && StarSize <= 22)
            {
                newStar.Color = Color.FromArgb(80 + random.Next(100), 245 + random.Next(10), 245 + random.Next(10), 245 + random.Next(10));
                newStar.Distance = random.Next(3, 4) + Adder;
                StarSize = 3;
            }
            else if (StarSize >= 23 && StarSize <= 25)
            {
                newStar.Color = Color.FromArgb(100 + random.Next(100), 245 + random.Next(10), 245 + random.Next(10), 245 + random.Next(10));
                newStar.Distance = random.Next(5, 6) + Adder;
                StarSize = 2;
            }

            //newStar.Distance = Math.Abs(newStar.Distance - 9);

            switch (random.Next(4))
            {
                case 0:
                    newStar.Shape = new Rectangle(random.Next(GameScreen.ScreenSize.X), -random.Next(500), StarSize, StarSize);
                    break;
                case 1:
                    newStar.Shape = new Rectangle(-random.Next(500), random.Next(GameScreen.ScreenSize.Y), StarSize, StarSize);
                    break;
                case 2:
                    newStar.Shape = new Rectangle(random.Next(GameScreen.ScreenSize.X), GameScreen.ScreenSize.Y + random.Next(500), StarSize, StarSize);
                    break;
                case 3:
                    newStar.Shape = new Rectangle(GameScreen.ScreenSize.X + random.Next(500), random.Next(GameScreen.ScreenSize.Y), StarSize, StarSize);
                    break;
            }

            return newStar;
            
        }
    }
}
            

