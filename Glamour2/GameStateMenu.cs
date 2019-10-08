using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glamour2
{
    class GameStateMenu : IGameState
    {
        Game1 g;

        Texture2D[] corns;

        string[] maps;
        Texture2D[] mapAssets;
        int selectedMap;

        Sprite[] readySprites;
        bool[] ready;

        float countdown;

        int prev;

        float creditsOffset = 0;
        static float CREDITS_SPEED = 10;
        float creditsWidth;
        string credits = "Game by Andrew (https://www.twitch.tv/tapioca) - All SFX from Street Fighter II, Razor Freestyle Scooter, and Glover - Shoutouts to everyone in twitch " +
            "chat who helped make the game - Sp0ck1 Yimmo_ Archmagus Takaox Thebellossom Daro16 hydromedia ugyuu light_general_6019 Antersus Krzyforbacon vs_deluge KuroroGabriel " +
            "LeftenantDan sg4e Esca_zzz tomwtwitch EnDirectDuNord LucarioZealot - ";

        public GameStateMenu(Game1 g, ContentManager cm)
        {
            this.g = g;
            creditsWidth = Game1.arial.MeasureString(credits).X;

            corns = new Texture2D[4]
            {
                cm.Load<Texture2D>("cornBig1"),
                cm.Load<Texture2D>("cornBig2"),
                cm.Load<Texture2D>("cornBig3"),
                cm.Load<Texture2D>("cornBig4")
            };

            readySprites = new Sprite[4]
            {
                new Sprite("Ready", cm, Vector2.Zero),
                new Sprite("Ready", cm, Vector2.Zero),
                new Sprite("Ready", cm, Vector2.Zero),
                new Sprite("Ready", cm, Vector2.Zero)
            };
            ready = new bool[4];

            maps = new string[5]
            {
                "bridge",
                "hell",
                "skyscraper",
                "science",
                "firing squad"
            };
            mapAssets = new Texture2D[maps.Length];
            for (int i = 0; i < maps.Length; i++)
            {
                mapAssets[i] = cm.Load<Texture2D>(maps[i]);
            }
            selectedMap = 0;

            countdown = -1;


        }

        public void handleInput(InputHandler ih, float dt)
        {
            if (ready[0] && ready[1] && ready[2] && ready[3] && countdown == -1)
            {
                countdown = 3;
                Game1.Music.playSfx("threesfx");
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                if (ih.isButtonPressed(i, 'a') || ih.isKeyPressed((Keys)i+49))
                {
                    ready[i] = true;
                    readySprites[i].update(2);
                    Game1.Music.playSfx("readysfx");
                }
            }

            if ((ih.isButtonPressed(0, 'r') || ih.isKeyPressed(Keys.D)) && countdown == -1)
            {
                selectedMap = ++selectedMap % maps.Length;
                Game1.Music.playSfx("selectsfx");
            }
            else if ((ih.isButtonPressed(0, 'l') || ih.isKeyPressed(Keys.A)) && countdown == -1)
            {
                selectedMap--;
                if (selectedMap == -1) selectedMap += maps.Length;
                Game1.Music.playSfx("selectsfx");
            }
        }

        public void update(float dt)
        {
            if (countdown > 0) countdown -= dt;
            else if (countdown > -1)
            {
                g.transitionToGame(maps[selectedMap]);
            }

            int time = (int)Math.Ceiling(countdown);
            if (time == 2 && prev == 3) Game1.Music.playSfx("twosfx");
            else if (time == 1 && prev == 2) Game1.Music.playSfx("onesfx");
            else if (time == 0 && prev == 1) Game1.Music.playSfx("fightsfx");
            prev = time;

            creditsOffset -= CREDITS_SPEED * dt;
            if (creditsOffset < -creditsWidth) creditsOffset = 0;
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(mapAssets[selectedMap], position: Vector2.Zero, color: Color.White);
            sb.Draw(Game1.WhiteTexture, destinationRectangle: new Microsoft.Xna.Framework.Rectangle(0, 0, Game1.SCREEN_WIDTH, Game1.SCREEN_HEIGHT), color: Color.Black * .3f);

            Vector2 nameSize = Game1.font.MeasureString(Game1.GAME_NAME);
            sb.DrawString(Game1.font, Game1.GAME_NAME, new Vector2(Game1.SCREEN_WIDTH / 2, 20) - new Vector2(nameSize.X / 2, 0), Color.White);

            string controls = "(X) Corn Missile (A) Dash (B) Corn Invisibility";
            Vector2 controlsSize = Game1.arial.MeasureString(controls);
            sb.DrawString(Game1.arial, controls, new Vector2(Game1.SCREEN_WIDTH / 2, 120) - new Vector2(controlsSize.X / 2, 0), Color.White);


            String formatted = "-" + maps[selectedMap] + "-";
            if (countdown > -1)
            {
                int time = (int)Math.Ceiling(countdown);
                formatted = "-" + time + "-";
            }
            Vector2 textWidth = Game1.font.MeasureString(formatted);
            sb.DrawString(Game1.font, formatted, new Vector2(Game1.SCREEN_WIDTH / 2, 250) - new Vector2(textWidth.X / 2, 0), Color.White);

            for (int x = 0; x < 4; x++)
            {
                sb.Draw(corns[x], position: new Vector2(-(corns[x].Width / 2) + 240 + 480 * x, 500), color: Color.White);
                readySprites[x].draw(sb, new Vector2(-82.5f + 240 + 480 * x, 800), color: Color.White);
            }

            sb.DrawString(Game1.arial, credits, new Vector2(creditsOffset, 1080 - 20), Color.White);
            sb.DrawString(Game1.arial, credits, new Vector2(creditsOffset + creditsWidth, 1080 - 20), Color.White);
        }
    }
}
