using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glamour2
{
    class UI
    {

        Player[] players;
        Texture2D[] UICorns;

        public UI(ContentManager cm, Player[] players)
        {
            this.players = players;
            UICorns = new Texture2D[4];
            UICorns[0] = cm.Load<Texture2D>("cornUI1");
            UICorns[1] = cm.Load<Texture2D>("cornUI2");
            UICorns[2] = cm.Load<Texture2D>("cornUI3");
            UICorns[3] = cm.Load<Texture2D>("cornUI4");
        }



        public void draw(SpriteBatch sb)
        {
            for (int i = 0; i < players.Length; i++)
            {
                drawPlayerUI(sb, i);
            }
        }

        public void drawPlayerUI(SpriteBatch sb, int p)
        {
            Player player = players[p];
            Vector2 origin = new Vector2(50 + p * 455, 30);
            sb.Draw(UICorns[p], position: origin, Color.White);

            sb.Draw(Game1.WhiteTexture, destinationRectangle:
                new Microsoft.Xna.Framework.Rectangle((int)origin.X + UICorns[p].Width + 10, (int)origin.Y + 30, (int)player.hp * 2, 20),
                color: Color.Red * 0.5f);

            sb.Draw(Game1.WhiteTexture, destinationRectangle:
                new Microsoft.Xna.Framework.Rectangle((int)origin.X + UICorns[p].Width + 10, (int)origin.Y + 50, (int)player.stamina, 20),
                color: Color.DarkGreen * 0.5f);

            sb.Draw(Game1.WhiteTexture, destinationRectangle:
                new Microsoft.Xna.Framework.Rectangle((int)origin.X + UICorns[p].Width + 10, (int)origin.Y + 70, (int)(200 * player.stun / player.maxStun), 20),
                color: Color.Blue * 0.5f);
        }
    }
}
