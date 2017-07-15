using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace TestBuild
{
    class ProjectileManager
    {

        System.Windows.Forms.Timer ProjectileTimer = new System.Windows.Forms.Timer();
        Form MainForm;
        internal List<Projectiles> MissileList = new List<Projectiles>(Settings.MaxProjectilesOnScreen);

        int MaxProjectilesOnScreen = Settings.MaxProjectilesOnScreen,
            ProjectileSpeed = Settings.ProjectileSpeed,
            ProjectileSpeedDiagonal = Settings.ProjectileSpeedDiagonal;
        Projectiles newMissile;
        public int MissilesActive;
        internal bool MvLeft,
                      MvRight,
                      MvUp,
                      MvDown;
        Rectangle tempRect;
        Random random = new Random();
        PlayerManager PlayerManagerInstance;
        internal struct Projectiles
        {
            internal int WeaponType;
            internal int ShooterId;
            internal Rectangle Shape;
            internal int Direction;
            internal Point Location;
            internal bool IsDead;
            internal int TimeToLive;
            internal Color Color;
            internal int Layer;
        }
        Point TopLeftWorldCoords;

        public ProjectileManager(Form MainFormLocal, PlayerManager PlayerManagerInstanceTemp)
        {
            PlayerManagerInstance = PlayerManagerInstanceTemp;
            MainForm = MainFormLocal;
            for (int i = 0; i < MaxProjectilesOnScreen; i++)
            {
                newMissile = new Projectiles();
                newMissile.IsDead = true;
                newMissile.Layer = random.Next(10) + Settings.Speed - 10;
                newMissile.TimeToLive = Settings.BulletLife;
                MissileList.Add(newMissile);
            }

            ProjectileTimer.Enabled = true;
            ProjectileTimer.Interval = 15;
            ProjectileTimer.Tick += new System.EventHandler(MoveProjectile_tick);


        }


        public void Shoot(int PlayerId, Point Pos, int Direction, int WeaponType)
        {

            if (WeaponType == 1)
            {
                for (int j = 0; j < Settings.ProjectilesPerClick; j++)
                {
                    for (int i = 1; i < MaxProjectilesOnScreen; i++)
                    {
                        if (MissileList[i].IsDead)
                        {

                            newMissile = MissileList[i];
                            newMissile.Location = PlayerManagerInstance.ThisPlayer.Location;
                            newMissile.WeaponType = 1;
                            MissilesActive += 1;
                            newMissile.ShooterId = PlayerId;
                            newMissile.TimeToLive = Settings.BulletLife;
                            newMissile.Color = Color.FromArgb(255, random.Next(255), random.Next(255), random.Next(255));
                            //newMissile.Color = Color.FromArgb(random.Next(255), 255, 255, 255);

                            Point BulletSpread = new Point(random.Next(Settings.ProjectilesSpread), random.Next(Settings.ProjectilesSpread));
                            newMissile.IsDead = false;
                            //newMissile.Shape = new Rectangle(Pos.X + BulletSpread, Pos.Y + BulletSpread, Settings.ProjectileSize, Settings.ProjectileSize);

                            TopLeftWorldCoords = new Point(Pos.X - (GameScreen.ScreenSize.X / 2), Pos.Y - (GameScreen.ScreenSize.Y / 2));
                            newMissile.Location = new Point(Pos.X + BulletSpread.X - 50, Pos.Y + BulletSpread.Y - 50);
                            TopLeftWorldCoords = new Point(MissileList[i].Location.X - TopLeftWorldCoords.X, MissileList[i].Location.Y - TopLeftWorldCoords.Y);
                            newMissile.Shape = new Rectangle(TopLeftWorldCoords.X, TopLeftWorldCoords.Y, Settings.ProjectileSize, Settings.ProjectileSize);

                            if (Settings.BulletParralax)
                                newMissile.Layer = random.Next(Settings.BulletLayers) + Settings.Speed - Settings.BulletLayerStart;
                            //newMissile.Shape = new Rectangle(Pos.X+ random.Next(Settings.ProjectilesSpread)- Settings.ProjectilesSpread/2, Pos.Y + random.Next(Settings.ProjectilesSpread) - Settings.ProjectilesSpread / 2, random.Next(Settings.ProjectileSize), random.Next(Settings.ProjectileSize));
                            newMissile.Direction = Direction;
                            MissileList[i] = newMissile;
                            break;
                        }
                    }
                }
            }
            else if (WeaponType == 2)
            {


                newMissile.WeaponType = 2;
                newMissile.Direction = Direction;
                newMissile.ShooterId = PlayerId;
                newMissile.TimeToLive = 10;
                Pos = new Point(Pos.X + 50, Pos.Y + 50);

                switch (Direction)
                {
                    case 0:
                        newMissile.Shape = new Rectangle(Pos.X + random.Next(-Settings.LaserSpread, Settings.LaserSpread), (int)(Pos.Y - Settings.LaserRange * 0.9 + random.Next(-Settings.LaserSpread / 10, Settings.LaserSpread / 10)), 1, 1);
                        break;
                    case 2:
                        newMissile.Shape = new Rectangle(Pos.X + Settings.LaserRange + random.Next(-Settings.LaserSpread / 10, Settings.LaserSpread / 10), Pos.Y + random.Next(-Settings.LaserSpread, Settings.LaserSpread), 1, 1);
                        break;
                    case 4:
                        newMissile.Shape = new Rectangle(Pos.X + random.Next(-Settings.LaserSpread, Settings.LaserSpread), (int)(Pos.Y + Settings.LaserRange * 0.9 + random.Next(-Settings.LaserSpread / 10, Settings.LaserSpread / 10)), 1, 1);
                        break;
                    case 6:
                        newMissile.Shape = new Rectangle(Pos.X - Settings.LaserRange + random.Next(-Settings.LaserSpread / 10, Settings.LaserSpread / 10), Pos.Y + random.Next(-Settings.LaserSpread, Settings.LaserSpread), 1, 1);
                        break;
                    case 1:
                        newMissile.Shape = new Rectangle((int)(Pos.X + Settings.LaserRange * 0.7 + random.Next(-Settings.LaserSpread / 2, Settings.LaserSpread / 2)), (int)(Pos.Y - Settings.LaserRange * 0.7 + random.Next(-Settings.LaserSpread / 2, Settings.LaserSpread / 2)), 1, 1);
                        break;
                    case 3:
                        newMissile.Shape = new Rectangle((int)(Pos.X + Settings.LaserRange * 0.7 + random.Next(-Settings.LaserSpread / 2, Settings.LaserSpread / 2)), (int)(Pos.Y + Settings.LaserRange * 0.7 + random.Next(-Settings.LaserSpread / 2, Settings.LaserSpread / 2)), 1, 1);
                        break;
                    case 5:
                        newMissile.Shape = new Rectangle((int)(Pos.X - Settings.LaserRange * 0.7 + random.Next(-Settings.LaserSpread / 2, Settings.LaserSpread / 2)), (int)(Pos.Y + Settings.LaserRange * 0.7 + random.Next(-Settings.LaserSpread / 2, Settings.LaserSpread / 2)), 1, 1);
                        break;
                    case 7:
                        newMissile.Shape = new Rectangle((int)(Pos.X - Settings.LaserRange * 0.7 + random.Next(-Settings.LaserSpread / 2, Settings.LaserSpread / 2)), (int)(Pos.Y - Settings.LaserRange * 0.7 + random.Next(-Settings.LaserSpread / 2, Settings.LaserSpread / 2)), 1, 1);
                        break;
                }

                Point TopLeftWorldCoords = new Point(PlayerManagerInstance.ThisPlayer.Location.X - GameScreen.ScreenSize.X / 2, PlayerManagerInstance.ThisPlayer.Location.Y - GameScreen.ScreenSize.Y / 2);
                tempRect = MissileList[0].Shape;
                TopLeftWorldCoords = new Point(PlayerManagerInstance.ThisPlayer.Location.X - TopLeftWorldCoords.X, PlayerManagerInstance.ThisPlayer.Location.Y - TopLeftWorldCoords.Y);
                newMissile.Shape.Location = TopLeftWorldCoords;


                MissileList[0] = newMissile;


            }
            else if (WeaponType == 3)
            {

                for (int i = 0; i < MaxProjectilesOnScreen; i++)
                {
                    if (MissileList[i].IsDead)
                    {
                        newMissile = MissileList[i];

                        newMissile.Location = PlayerManagerInstance.ThisPlayer.Location;
                        newMissile.WeaponType = 3;
                        newMissile.ShooterId = -1;
                        newMissile.TimeToLive = 6;
                        newMissile.Color = Color.FromArgb(230, random.Next(100, 180), random.Next(100), random.Next(0));

                        //newMissile.Color = Color.FromArgb(random.Next(255), 255, 255, 255);

                        Point BulletSpread = new Point(random.Next(-10, 10), random.Next(-10, 10));
                        newMissile.IsDead = false;
                        //newMissile.Shape = new Rectangle(Pos.X + BulletSpread, Pos.Y + BulletSpread, Settings.ProjectileSize, Settings.ProjectileSize);


                        TopLeftWorldCoords = new Point(Pos.X - (GameScreen.ScreenSize.X / 2), Pos.Y - (GameScreen.ScreenSize.Y / 2));
                        newMissile.Location = new Point(Pos.X + BulletSpread.X - 3, Pos.Y + BulletSpread.Y - 3);
                        TopLeftWorldCoords = new Point(PlayerManagerInstance.ThisPlayer.Location.X - TopLeftWorldCoords.X, PlayerManagerInstance.ThisPlayer.Location.Y - TopLeftWorldCoords.Y);
                        newMissile.Shape = new Rectangle(TopLeftWorldCoords.X, TopLeftWorldCoords.Y, Settings.ProjectileSize, Settings.ProjectileSize);


                        //newMissile.Shape = new Rectangle(Pos.X+ random.Next(Settings.ProjectilesSpread)- Settings.ProjectilesSpread/2, Pos.Y + random.Next(Settings.ProjectilesSpread) - Settings.ProjectilesSpread / 2, random.Next(Settings.ProjectileSize), random.Next(Settings.ProjectileSize));
                        newMissile.Direction = -1;
                        MissileList[i] = newMissile;
                        break;
                    }
                }
            }
            else if (WeaponType == 4)
            {

                for (int i = 0; i < MaxProjectilesOnScreen; i++)
                {
                    if (MissileList[i].IsDead)
                    {
                        newMissile = MissileList[i];

                        newMissile.Location = PlayerManagerInstance.ThisPlayer.Location;
                        newMissile.WeaponType = 4;
                        newMissile.ShooterId = -1;
                        newMissile.TimeToLive = 5;
                        newMissile.Color = Color.FromArgb(255, random.Next(200, 255), random.Next(200, 255), 255);

                        //newMissile.Color = Color.FromArgb(random.Next(255), 255, 255, 255);

                        Point BulletSpread = new Point(random.Next(-20, 20), random.Next(-20, 20));
                        newMissile.IsDead = false;
                        //newMissile.Shape = new Rectangle(Pos.X + BulletSpread, Pos.Y + BulletSpread, Settings.ProjectileSize, Settings.ProjectileSize);


                        TopLeftWorldCoords = new Point(Pos.X - (GameScreen.ScreenSize.X / 2), Pos.Y - (GameScreen.ScreenSize.Y / 2));
                        newMissile.Location = new Point(Pos.X + BulletSpread.X - 3, Pos.Y + BulletSpread.Y - 3);
                        TopLeftWorldCoords = new Point(PlayerManagerInstance.ThisPlayer.Location.X - TopLeftWorldCoords.X, PlayerManagerInstance.ThisPlayer.Location.Y - TopLeftWorldCoords.Y);
                        newMissile.Shape = new Rectangle(TopLeftWorldCoords.X, TopLeftWorldCoords.Y, Settings.ProjectileSize * 2, Settings.ProjectileSize * 2);


                        //newMissile.Shape = new Rectangle(Pos.X+ random.Next(Settings.ProjectilesSpread)- Settings.ProjectilesSpread/2, Pos.Y + random.Next(Settings.ProjectilesSpread) - Settings.ProjectilesSpread / 2, random.Next(Settings.ProjectileSize), random.Next(Settings.ProjectileSize));
                        newMissile.Direction = random.Next(0, 7);
                        MissileList[i] = newMissile;
                        break;
                    }
                }
            }
            else if (WeaponType == 5)
            {
                int multiplier = 1;
                if (PlayerId == -2)
                    multiplier = 2;
                for (int k = 0; k < 8* multiplier; k++)
                {
                    for (int j = 0; j < 100; j++)
                    {
                        for (int i = 0; i < MaxProjectilesOnScreen; i++)
                        {
                            if (MissileList[i].IsDead)
                            {

                                newMissile = MissileList[i];
                                newMissile.Location = PlayerManagerInstance.ThisPlayer.Location;
                                newMissile.WeaponType = 5;
                                newMissile.ShooterId = -1;
                                newMissile.TimeToLive = 8;
                                newMissile.Color = Color.FromArgb(255, random.Next(240, 255), random.Next(50), random.Next(0, 50));
                                
                                Point BulletSpread = new Point(random.Next(100), random.Next(100));
                                newMissile.IsDead = false;
                                //newMissile.Shape = new Rectangle(Pos.X + BulletSpread, Pos.Y + BulletSpread, Settings.ProjectileSize, Settings.ProjectileSize);
                                if (PlayerId == -2)
                                {
                                    newMissile.ShooterId = -2;
                                    newMissile.TimeToLive = 15;
                                    newMissile.Color = Color.FromArgb(255, random.Next(240, 255), random.Next(20), random.Next(0, 100));
                                    BulletSpread = new Point(random.Next(-250, 250), random.Next(-250, 250));
                                }
                                TopLeftWorldCoords = new Point(Pos.X - (GameScreen.ScreenSize.X / 2), Pos.Y - (GameScreen.ScreenSize.Y / 2));
                                newMissile.Location = new Point(Pos.X + BulletSpread.X - 50, Pos.Y + BulletSpread.Y - 50);
                                TopLeftWorldCoords = new Point(MissileList[i].Location.X - TopLeftWorldCoords.X, MissileList[i].Location.Y - TopLeftWorldCoords.Y);
                                newMissile.Shape = new Rectangle(TopLeftWorldCoords.X, TopLeftWorldCoords.Y, Settings.ProjectileSize, Settings.ProjectileSize);

                                

                                //newMissile.Shape = new Rectangle(Pos.X+ random.Next(Settings.ProjectilesSpread)- Settings.ProjectilesSpread/2, Pos.Y + random.Next(Settings.ProjectilesSpread) - Settings.ProjectilesSpread / 2, random.Next(Settings.ProjectileSize), random.Next(Settings.ProjectileSize));
                                newMissile.Direction = k;
                                MissileList[i] = newMissile;
                                break;
                            }
                        }
                    }
                }
            }
            else if (WeaponType == 6)
            {
                for (int i = 0; i < MaxProjectilesOnScreen; i++)
                {
                    if (MissileList[i].IsDead)
                    {

                        newMissile = MissileList[i];
                        newMissile.WeaponType = 6;
                        newMissile.ShooterId = -1;
                        newMissile.TimeToLive = 500;
                        newMissile.Color = Color.FromArgb(255, random.Next(240, 255), random.Next(50), random.Next(0, 50));

                        Point BulletSpread = new Point(random.Next(-250, 250), random.Next(-250, 250));
                        newMissile.IsDead = false;
                        //newMissile.Shape = new Rectangle(Pos.X + BulletSpread, Pos.Y + BulletSpread, Settings.ProjectileSize, Settings.ProjectileSize);

                        TopLeftWorldCoords = new Point(Pos.X - (GameScreen.ScreenSize.X / 2), Pos.Y - (GameScreen.ScreenSize.Y / 2));
                        newMissile.Location = new Point(Pos.X + BulletSpread.X, Pos.Y + BulletSpread.Y);
                        TopLeftWorldCoords = new Point(MissileList[i].Location.X - TopLeftWorldCoords.X, MissileList[i].Location.Y - TopLeftWorldCoords.Y);
                        newMissile.Shape = new Rectangle(TopLeftWorldCoords.X, TopLeftWorldCoords.Y, Settings.ProjectileSize*4, Settings.ProjectileSize*4);

                        TopLeftWorldCoords = new Point(PlayerManagerInstance.ThisPlayer.Location.X - GameScreen.ScreenSize.X / 2, PlayerManagerInstance.ThisPlayer.Location.Y - GameScreen.ScreenSize.Y / 2);
                        tempRect = MissileList[i].Shape;
                        TopLeftWorldCoords = new Point(MissileList[i].Location.X - TopLeftWorldCoords.X, MissileList[i].Location.Y - TopLeftWorldCoords.Y);
                        newMissile.Shape.Location = TopLeftWorldCoords;

                        //newMissile.Shape = new Rectangle(Pos.X+ random.Next(Settings.ProjectilesSpread)- Settings.ProjectilesSpread/2, Pos.Y + random.Next(Settings.ProjectilesSpread) - Settings.ProjectilesSpread / 2, random.Next(Settings.ProjectileSize), random.Next(Settings.ProjectileSize));
                        newMissile.Direction = random.Next(7);
                        MissileList[i] = newMissile;
                        break;
                    }
                }
            }
        }


        private Projectiles MoveMissileWithSetSpeed(Projectiles newMissileLocal, int iLocal,int SpeedLocal)
        {
            switch (MissileList[iLocal].Direction)
            {
                case 6:
                    newMissileLocal.Location.X -= SpeedLocal;
                    break;
                case 2:
                    newMissileLocal.Location.X += SpeedLocal;
                    break;
                case 0:
                    newMissileLocal.Location.Y -= SpeedLocal;
                    break;
                case 4:
                    newMissileLocal.Location.Y += SpeedLocal;
                    break;
                case 7:
                    newMissileLocal.Location.X -= (int)(0.95 * SpeedLocal);
                    newMissileLocal.Location.Y -= (int)(0.95 * SpeedLocal);
                    break;
                case 5:
                    newMissileLocal.Location.X -= (int)(0.95 * SpeedLocal);
                    newMissileLocal.Location.Y += (int)(0.95 * SpeedLocal);
                    break;
                case 1:
                    newMissileLocal.Location.X += (int)(0.95 * SpeedLocal);
                    newMissileLocal.Location.Y -= (int)(0.95 * SpeedLocal);
                    break;
                case 3:
                    newMissileLocal.Location.X += (int)(0.95 * SpeedLocal);
                    newMissileLocal.Location.Y += (int)(0.95 * SpeedLocal);
                    break;
            }
            return newMissileLocal;
        }


        internal void MoveProjectile_tick(object sender, EventArgs e)
        {
            for (int i = 0; i < Settings.MaxProjectilesOnScreen; i++)
            {

                newMissile = MissileList[i];
                if (newMissile.TimeToLive <= 0)
                {
                    //MissilesActive -= 1;
                    newMissile.IsDead = true;
                    MissileList[i] = newMissile;
                }
                else 
                if (!MissileList[i].IsDead)
                {
                    newMissile.TimeToLive -= 1;

                    if (!Settings.NewAlgorithm)
                        MainForm.Invalidate(MissileList[i].Shape);

                    int TempSpeed = 0;
                    if (MissileList[i].WeaponType == 6)
                    {
                        TempSpeed = 2;
                    }else
                    {
                        TempSpeed = Settings.ProjectileSpeed;
                    }

                    newMissile = MoveMissileWithSetSpeed(newMissile,i, TempSpeed);


                    Point TopLeftWorldCoords = new Point(PlayerManagerInstance.ThisPlayer.Location.X - GameScreen.ScreenSize.X / 2, PlayerManagerInstance.ThisPlayer.Location.Y - GameScreen.ScreenSize.Y / 2);
                    tempRect = MissileList[i].Shape;
                    TopLeftWorldCoords = new Point(MissileList[i].Location.X - TopLeftWorldCoords.X, MissileList[i].Location.Y - TopLeftWorldCoords.Y);
                    newMissile.Shape.Location = TopLeftWorldCoords;
                    

                    MissileList[i] = newMissile;

                    if (!Settings.NewAlgorithm)
                        MainForm.Invalidate(MissileList[i].Shape);
                }
            }
        }
    }
}
