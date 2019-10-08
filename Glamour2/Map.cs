using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glamour2
{
    class Map
    {
        Texture2D background;
        public Vector2 yHorizons;
        public Vector2 xHorizons;

        public Vector2[] startPositions;

        public string luaScript;
        List<Rectangle> extraCollision;

        public Map(ContentManager cm, string filename)
        {
            background = cm.Load<Texture2D>(filename);
            StreamReader sr = new StreamReader(cm.RootDirectory + "/" + filename + ".txt");
            yHorizons = Game1.parseVector2(sr.ReadLine());
            xHorizons = Game1.parseVector2(sr.ReadLine());
            startPositions = new Vector2[4]
            {
                Game1.parseVector2(sr.ReadLine()),
                Game1.parseVector2(sr.ReadLine()),
                Game1.parseVector2(sr.ReadLine()),
                Game1.parseVector2(sr.ReadLine())
            };

            extraCollision = new List<Rectangle>();
            while (!sr.EndOfStream)
            {
                extraCollision.Add(Game1.parseRectangle(sr.ReadLine()));
            }
            sr.Close();

            StreamReader scriptr = new StreamReader(cm.RootDirectory + "/" + filename + ".lua");
            luaScript = scriptr.ReadToEnd();
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(background, position: Vector2.Zero, color: Color.White);
        }

        public bool checkCollision(Vector2 proposed)
        {
            foreach (Rectangle rect in extraCollision)
            {
                if (rect.Contains(proposed)) return true;
            }
            return false;
        }
    }
}
