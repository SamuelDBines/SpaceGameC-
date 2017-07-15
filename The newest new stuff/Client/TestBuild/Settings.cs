using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TestBuild
{
    class Settings
    {

        public static bool Server = true;
        public static bool QuickLoad = false;
        //Server settings
        public static string
           serverPort = "8000",
           Port = "",
           sendPort = "",
           recvPort = "",
           Ip = "";
        public static int
            PlayerId = -1,
            tcpFileSize = 1000,
            filesize = 22,
            PlayerCount = 1;

        public static char
            protocol = '1',
            checkOne = 'Q',
            checkTwo = 'Z',
            checkThree = 'X',
            serverSend = 'S',
            clientRecv = 'C';
        //Player Manager
        public static Color
            PlayerColor = Color.Purple;
        public static int
            Red = 0,
            Green = 0,
            Blue = 0,
            Health = 100,
            Acceleration = 2,
            Deceleration = 2,
            Speed = 0,
            SpeedMax = 5,
            SpeedMaxCruiseMultiplier =5,
            SpeedDiagonal = (int)(Speed*0.95),
            CheckForKeyPressInterval = 15,  //15 upwards
            PlayerWidth = 50,
            PlayerHeight= 50;
        public static bool
            PositionLock = true,
            CollisionOn = true,
            Connected = false;

        public static string
            UserName = "";


        public static string FlightMode = "Combat";
        //Projectile Manager
        public static bool
            NewAlgorithm = true,
            BulletParralax = true,
            Parallax = true;
            
            
        public static int
            SpinSpeed =9,
            Seed = 1,
            LaserRange = 500,
            LaserSpread = 50,
            BulletLayers = 0,
            BulletLayerStart = 20+ Speed,
            FlickerFrequency = 100, //Lower means more flickers
            MaxProjectilesOnScreen = 5000,
            MaxStarsOnScreen = 5000,
            MaxStarsOnScreenBackup = MaxStarsOnScreen,           
            FadeOutSpeed = 100,    //0,100
            ProjectileSize = 5,
            ProjectileSpeed =50,
            ProjectileSpeedDiagonal = (int)(ProjectileSpeed*0.8),
            ProjectileUpdateInterval = 15,  //15 upwards
            ProjectileFireRate = 20,  //15 upwards
            ProjectilesPerClick = 1,
            ProjectilesSpread = 10,
            BulletLife = 100;
    }
}
