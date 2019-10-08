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



    class Player : IEntity
    {

        public static float MOVE_SPEED = 300;
        public static float STAMINA_REGEN = 30;

        public static float DASH_STAMINA = 30;
        public static float DASH_SPEED = 1500;

        public static float INVIS_STAMINA = 120;

        public static float MISSILE_STAMINA = 35;

        private float health;
        public float hp
        {
            get
            {
                return health;
            }
            set
            {
                health = value;
                if (health <= 0) {
                    if (hp > -1000) Game1.Music.playSfx("deathsfx");
                    sprite.animHandler.setAnimation(1, true);
                }
                else if (health > 100)
                {
                    health = 100;
                }
            }
        }
        public float stamina;

        int gamepad;
        Map map;
        ContentManager cm;
        GameStateArena state;
        Texture2D shadow;

        public Sprite sprite;
        public Vector2 pos
        {
            get
            {
                return sprite.position;
            }
            set
            {
                sprite.position = value;
            }
        }

        public bool facingRight
        {
            get
            {
                return this.sprite.facingRight;
            }
            set
            {
                this.sprite.facingRight = value;
            }
        }

        public Vector2 getPosition()
        {
            return sprite.feetPosition;
        }

        public Rectangle getCollision()
        {
            return new Rectangle(pos.X, pos.Y + sprite.size.Y - 25, sprite.size.X, 50);
        }

        public Vector2 makePos(int x, int y)
        {
            return new Vector2(x, y);
        }

        // mechanics variables
        float dashTime = 0;
        Vector2 dashDirection;
        List<Vector2> afterimages;
        float invisTimer = 0;
        bool invis = false;
        float rechargeBuffer = 0;
        public float iFrames = 0;
        float redFlash = 0;
        bool falling;
        float dashCooldown = 0;

        public float stun = 0f;
        public float maxStun = 0f;

        public Player(ContentManager cm, GameStateArena state, int gamepad, string filename, Map map)
        {
            this.cm = cm;
            this.state = state;
            this.map = map;
            this.sprite = new Sprite(filename, cm, map.startPositions[gamepad]);
            this.pos = pos;
            this.gamepad = gamepad;

            hp = 100;
            stamina = 200;

            afterimages = new List<Vector2>();

            shadow = cm.Load<Texture2D>("playerShadow");
        }

        public void handleInput(InputHandler ih, float dt)
        {
            if (rechargeBuffer > 0) rechargeBuffer -= dt;
            else stamina = Math.Min(200, stamina + (STAMINA_REGEN * dt));
            if (iFrames > 0) iFrames -= dt;
            if (redFlash > 0) redFlash -= dt;
            if (stun > 0) stun -= dt;
            if (dashCooldown > 0) dashCooldown -= dt;


            if (falling)
            {
                move(0, 1500 * dt, false);
            }
            if (hp <= 0 || dashTime > 0)
            {
                iFrames = 0.2f;
                update(dt);
                return;
            }
            else if (stun > 0)
            {
                return;
            }

            Vector2 direc = Vector2.Zero;

            // movement
            bool moving = false;
            if (ih.isButtonHeld(gamepad, 'u') || (ih.isKeyHeld(Keys.W) && gamepad == 0))
            {
                direc.Y = -1;
                moving = true;
                move(0, -MOVE_SPEED * dt);
            }
            else if (ih.isButtonHeld(gamepad, 'd') || (ih.isKeyHeld(Keys.S) && gamepad == 0))
            {
                direc.Y = 1;
                moving = true;
                move(0, MOVE_SPEED * dt);
            }
            if (ih.isButtonHeld(gamepad, 'l') || (ih.isKeyHeld(Keys.A) && gamepad == 0))
            {
                direc.X = -1;
                moving = true;
                move(-MOVE_SPEED * dt, 0);
                facingRight = false;
            }
            else if (ih.isButtonHeld(gamepad, 'r') || (ih.isKeyHeld(Keys.D) && gamepad == 0))
            {
                direc.X = 1;
                moving = true;
                move(MOVE_SPEED * dt, 0);
                facingRight = true;
            }

            if (moving)
            {
                update(dt); // walk cycle
            }

            // powers
            if (ih.isButtonHeld(gamepad, 'b') || (ih.isKeyHeld(Keys.J) && gamepad == 0)) // invisibility
            {
                float staminaLoss = INVIS_STAMINA * dt;
                if (stamina - staminaLoss >= 0)
                {
                    invisTimer += dt;
                    if (invisTimer > 0.6) invisTimer = 0;
                    else if (invisTimer < 0.55) sprite.visible = false;
                    else sprite.visible = true;
                    stamina -= staminaLoss;
                    invis = true;
                }
                else // forced out due to 0 stamina
                {
                    invis = false;
                    invisTimer = 0;
                    sprite.visible = true;
                }
            }
            else // not holding button
            {
                invis = false;
                sprite.visible = true;
                invisTimer = 0f;
            }

            if ((ih.isButtonPressed(gamepad, 'a') || (ih.isKeyPressed(Keys.K) && gamepad == 0)) && !invis) // dash
            {
                if (dashCooldown <= 0 && stamina - DASH_STAMINA >= 0 && direc != Vector2.Zero)
                {
                    afterimages = new List<Vector2>();
                    dashTime = 0.15f;
                    dashDirection = direc;
                    stamina -= DASH_STAMINA;
                    Game1.Music.playSfx("dashsfx");
                }
            }
            if (ih.isButtonPressed(gamepad, 'x') || (ih.isKeyPressed(Keys.L) && gamepad == 0))
            {
                if (stamina - MISSILE_STAMINA >= 0)
                {
                    Missile cornMissile = new Missile(cm, this);
                    state.createMissile(cornMissile);
                    stamina -= MISSILE_STAMINA;
                }
            }

        }

        public void damage(float amnt)
        {
            if (iFrames <= 0)
            {
                hp -= amnt;
                if (amnt > 0)
                {
                    iFrames = 1f;
                    redFlash = 1f;
                }
            }
        }

        public void setStun(float amnt)
        {
            maxStun = amnt;
            stun = amnt;
        }
        public void setRedFlash(float amnt)
        {
            redFlash = amnt;
        }

        public void move(float x, float y, bool collision = true)
        {
            Vector2 oldPos = pos;
            if (!collision || ((pos.X + x >= map.xHorizons.X && pos.X + x + sprite.size.X < map.xHorizons.Y)
                && !map.checkCollision(new Vector2(pos.X + x, pos.Y + sprite.size.Y)))) pos = new Vector2(pos.X + x, pos.Y);

            if (!collision || ((getPosition().Y + y > map.yHorizons.X && getPosition().Y + y < map.yHorizons.Y)
                && !map.checkCollision(new Vector2(pos.X, pos.Y + y + sprite.size.Y)))) pos = new Vector2(pos.X, pos.Y + y);

            if (dashTime > 0 && pos != oldPos)
            {
                afterimages.Add(oldPos);
            }
        }

        public void fallDeath()
        {
            if (!falling && dashTime <= 0)
            {
                Game1.Music.playSfx("screamsfx");
                iFrames = 0;
                falling = true;
                damage(100000);
            }
        }


        public void update(float dt)
        {
            if (dashTime > 0f)
            {
                Vector2 dNorm = Vector2.Normalize(dashDirection);
                move(dNorm.X * DASH_SPEED * dt, dNorm.Y * DASH_SPEED * dt);
                dashTime -= dt;
                if (dashTime <= 0)
                {
                    dashCooldown = 0.25f;
                }
            }

            this.sprite.update(dt * 1000);
        }

        public void draw(SpriteBatch sb)
        {
            if (dashTime > 0)
            {
                foreach (Vector2 vec in afterimages)
                {
                    sprite.draw(sb, vec, Color.White * 0.2f);
                }
            }
            Color col = Color.White;
            if (redFlash > 0 && hp > 0) col = Color.Red * 0.5f;
            if (hp > 0 && !invis) sb.Draw(shadow, position: new Vector2(sprite.position.X + -2, sprite.position.Y + sprite.size.Y - 25), color: Color.White);
            sprite.draw(sb, sprite.position, col);
        }

    }
}
