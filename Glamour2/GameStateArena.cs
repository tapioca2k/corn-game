using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLua;

namespace Glamour2
{
    class GameStateArena : IGameState
    {
        Game1 g;
        ContentManager cm;
        List<Missile> missiles;
        List<IEntity> allSprites;
        Player[] players;
        Map map;
        UI ui;
        int winner = -1;
        float endTimer = -1;
        Lua lua;
        float globalTimer = 0f;
        float next = 2.5f;


        public GameStateArena(Game1 g, ContentManager cm, string arena)
        {
            this.cm = cm;
            this.g = g;
            map = new Map(cm, arena);
            players = new Player[4] {
                new Player(cm, this, 0, "corn1", map),
                new Player(cm, this, 1, "corn2", map),
                new Player(cm, this, 2, "corn3", map),
                new Player(cm, this, 3, "corn4", map)
            };
            ui = new UI(cm, players);
            allSprites = new List<IEntity>();
            missiles = new List<Missile>();
            lua = new Lua();
            lua["Game"] = g;
            lua["arena"] = this;
        }



        public void handleInput(InputHandler ih, float dt)
        {
            foreach (Player p in players)
            {
                p.handleInput(ih, dt);
            }
            
        }

        public void createMissile(Missile m)
        {
            missiles.Add(m);
            Game1.Music.playSfx("firesfx");
        }
        public void createMissile(float x, float y)
        {
            Vector2 pos = new Vector2(x, y);
            createMissile(new Missile(cm, pos));
            next += 0.75f;
        }

        public void update(float dt)
        {
            globalTimer += dt;

            foreach (Missile m in missiles)
            {
                m.update(dt);
                foreach (Player p in players)
                {
                    if (m.getCollision().Intersects(p.getCollision()) && m.owner != p && p.hp > 0 && p.iFrames <= 0)
                    {
                        p.damage(Missile.DAMAGE);
                        m.finished = true;
                        Game1.Music.playSfx("hitsfx");
                    }
                }
            }
            foreach (Player p1 in players)
            {
                foreach (Player p2 in players)
                {
                    if (!p1.sprite.visible || !p2.sprite.visible) continue;
                    else if (p1 != p2 && p1.getCollision().Intersects(p2.getCollision()) && p2.hp <= 0)
                    {
                        p1.damage(-1000); // full heal
                        p1.setStun(1.5f);
                        Game1.Music.playSfx("eatsfx");
                        p2.sprite.visible = false;
                    }
                }
            }
            missiles.RemoveAll(m => m.finished);

            // execute map lua
            lua["players"] = players;
            lua["next"] = next;
            lua["timer"] = globalTimer;
            lua.DoString(map.luaScript);

            // sprite sorting
            allSprites = new List<IEntity>();
            foreach (Player p in players)
            {
                allSprites.Add((IEntity)p);
            }
            foreach (Missile m in missiles)
            {
                allSprites.Add((IEntity)m);
            }

            for (int i = 1; i < allSprites.Count; i++)
            {
                int j = i;
                while (j > 0 && allSprites[j-1].getPosition().Y > allSprites[j].getPosition().Y)
                {
                    IEntity temp = allSprites[j];
                    allSprites[j] = allSprites[j - 1];
                    allSprites[j - 1] = temp;
                    j--;
                }
            }

            wincon();
            if (endTimer > 0) endTimer -= dt;
            if (endTimer > -1 && endTimer <= 0)
            {
                g.transitionToMenu();
            }
        }

        void wincon()
        {
            int old = winner;
            if (players[0].hp > 0 && players[1].hp <= 0 && players[2].hp <= 0 && players[3].hp <= 0) winner = 0;
            if (players[0].hp <= 0 && players[1].hp > 0 && players[2].hp <= 0 && players[3].hp <= 0) winner = 1;
            if (players[0].hp <= 0 && players[1].hp <= 0 && players[2].hp > 0 && players[3].hp <= 0) winner = 2;
            if (players[0].hp <= 0 && players[1].hp <= 0 && players[2].hp <= 0 && players[3].hp > 0) winner = 3;
            if (winner != old)
            {
                endTimer = 5;
            }
        }

        public void draw(SpriteBatch sb)
        {
            map.draw(sb);

            // draw Sprites
            for (int i = 0; i < allSprites.Count; i++)
            {
                allSprites[i].draw(sb);
            }

            ui.draw(sb);
            if (winner != -1)
            {
                string winString = "PLAYER " + (winner + 1) + " WINS";
                Vector2 winWidth = Game1.font.MeasureString(winString);
                sb.DrawString(Game1.font, winString, new Vector2((Game1.SCREEN_WIDTH / 2)  - (winWidth.X / 2), (Game1.SCREEN_HEIGHT / 2) - (winWidth.Y / 2)), Color.White);
            }
            
        }


    }
}
