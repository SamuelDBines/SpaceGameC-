using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;


namespace TestBuild
{

    class Planets
    {

        public List<PlanetObject> PlanetList = new List<PlanetObject>();
        public List<PlanetObject> PlanetListInView = new List<PlanetObject>();

        internal struct PlanetObject
        {
            internal Point Location;
            internal Point ScreenLocation;
            internal int PlanetImage;
            internal int Size;
            internal bool InView;
            internal List<Rectangle> HitBoxes;
            internal int Health;
            internal int PositionInList;



        }
        Random random;
        Point TopLeftWorldCoords;
        Timer PlanetTimer = new Timer();
        bool SeedApplied = false;
        public Planets()
        {
            PlanetTimer.Enabled = false;
            PlanetTimer.Interval = 15;
            PlanetTimer.Tick += new System.EventHandler(MovePlanetsWorldCoords_tick);
        }

        public void SeededPlanets()
        {

            if (Settings.Seed != -1 && !SeedApplied)
            {
                random = new Random(Settings.Seed);
                SeedApplied = true;
                PlanetTimer.Enabled = true;

            }
            if (SeedApplied)
            {
                for (int i = 0; i < 50000; i++)
                {

                    PlanetObject newPlanet = new PlanetObject();
                    newPlanet.InView = false;
                    newPlanet.PositionInList = i;
                    newPlanet.Location.X = random.Next(-1000000, 1000000);
                    newPlanet.Location.Y = random.Next(-1000000, 1000000);
                    newPlanet.ScreenLocation = new Point(-10000, -10000);
                    newPlanet.Size = random.Next(500, 1500);
                    newPlanet.PlanetImage = random.Next(0,5);
                    newPlanet.Health = 200;
                    Point CenterCoordinates = new Point(newPlanet.Location.X + newPlanet.Size / 2, newPlanet.Location.Y + newPlanet.Size / 2);
                    newPlanet.HitBoxes = new List<Rectangle>(3);
                    newPlanet.HitBoxes.Add(Rectangle.FromLTRB(CenterCoordinates.X - newPlanet.Size / 2, CenterCoordinates.Y - newPlanet.Size / 5, CenterCoordinates.X + newPlanet.Size / 2, CenterCoordinates.Y + newPlanet.Size / 5));
                    newPlanet.HitBoxes.Add(Rectangle.FromLTRB(CenterCoordinates.X - newPlanet.Size / 5, CenterCoordinates.Y - newPlanet.Size / 2, CenterCoordinates.X + newPlanet.Size / 5, CenterCoordinates.Y + newPlanet.Size / 2));
                    newPlanet.HitBoxes.Add(Rectangle.FromLTRB((int)(CenterCoordinates.X - newPlanet.Size *0.66), (int)(CenterCoordinates.Y - newPlanet.Size *0.66), (int)(CenterCoordinates.X + newPlanet.Size *0.66),(int)( CenterCoordinates.Y + newPlanet.Size / 2)));


                    PlanetList.Add(newPlanet);
                }
            }
        }



        private void MovePlanetsWorldCoords_tick(object sender, EventArgs e)
        {
            PlanetListInView.Clear();
            for (int i = 0; i < PlanetList.Count; i++)
            {
                PlanetObject tempPlanet = PlanetList[i];
                if ((tempPlanet.Location.X > GameScreen.LocationOfPlayer.X - 8000 && tempPlanet.Location.X < GameScreen.LocationOfPlayer.X + 8000) &&
                        (tempPlanet.Location.Y > GameScreen.LocationOfPlayer.Y - 8000 && tempPlanet.Location.Y < GameScreen.LocationOfPlayer.Y + 8000))
                {

                    TopLeftWorldCoords = new Point(GameScreen.LocationOfPlayer.X - GameScreen.ScreenSize.X / 2, GameScreen.LocationOfPlayer.Y - GameScreen.ScreenSize.Y / 2);
                    TopLeftWorldCoords = new Point(tempPlanet.Location.X - TopLeftWorldCoords.X, tempPlanet.Location.Y - TopLeftWorldCoords.Y);
                    tempPlanet.ScreenLocation = TopLeftWorldCoords;
                    tempPlanet.InView = true;
                    PlanetListInView.Add(PlanetList[i]);
                    //System.Console.WriteLine("Health :    "+tempPlanet.Health);
                    PlanetList[i] = tempPlanet;
                    Point CenterCoordinates = new Point(tempPlanet.ScreenLocation.X + tempPlanet.Size / 2, tempPlanet.ScreenLocation.Y + tempPlanet.Size / 2);
                    tempPlanet.HitBoxes[0] = (Rectangle.FromLTRB((int)(CenterCoordinates.X - tempPlanet.Size * 0.45), (int)(CenterCoordinates.Y - tempPlanet.Size / 5), (int)(CenterCoordinates.X + tempPlanet.Size * 0.45), (int)(CenterCoordinates.Y + tempPlanet.Size / 5)));
                    tempPlanet.HitBoxes[1] = (Rectangle.FromLTRB((int)(CenterCoordinates.X - tempPlanet.Size / 5), (int)(CenterCoordinates.Y - tempPlanet.Size * 0.45), (int)(CenterCoordinates.X + tempPlanet.Size / 5), (int)(CenterCoordinates.Y + tempPlanet.Size * 0.45)));
                    tempPlanet.HitBoxes[2] = (Rectangle.FromLTRB((int)(CenterCoordinates.X - tempPlanet.Size * 0.33), (int)(CenterCoordinates.Y - tempPlanet.Size * 0.33), (int)(CenterCoordinates.X + tempPlanet.Size * 0.33), (int)(CenterCoordinates.Y + tempPlanet.Size * 0.33)));


                }
                else
                {
                    tempPlanet.InView = false;
                    PlanetList[i] = tempPlanet;

                }


            }
        }
    }
}
