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
    class Missile : IEntity
    {

        public static int MISSILE_HEIGHT = 69;
        public static int MISSILE_SPEED = 1000;
        public static int DAMAGE = 34;

        public Player owner;
        Sprite sprite;
        Texture2D shadow;

        Vector2 fixedStart;
        Vector2 MISSILE_OFFSET
        {
            get
            {
                if (owner == null) return fixedStart;
                else if (owner.facingRight) return new Vector2(46, 69);
                else return new Vector2(-27, 69);
            }
        }

        float velocity;

        private bool done;
        public bool finished
        {
            get
            {
                return sprite.position.X < -100 || sprite.position.X > Game1.SCREEN_WIDTH || done;
            }
            set
            {
                done = value;
            }
        }

        public Missile(ContentManager cm, Player owner)
        {
            this.owner = owner;
            sprite = new Sprite("Missile", cm, owner.pos + MISSILE_OFFSET, false);
            if (this.owner.facingRight) velocity = MISSILE_SPEED;
            else velocity = -MISSILE_SPEED;
            shadow = cm.Load<Texture2D>("missileShadow");
        }
        public Missile(ContentManager cm, Vector2 pos)
        {
            this.owner = null;
            this.fixedStart = pos;
            sprite = new Sprite("Missile", cm, pos, false);
            velocity = MISSILE_SPEED;
            shadow = cm.Load<Texture2D>("missileShadow");
        }

        public Vector2 getPosition()
        {
            return sprite.position + new Vector2(0, MISSILE_HEIGHT);
        }

        public Rectangle getCollision()
        {
            Vector2 pos = getPosition();
            return new Rectangle(pos.X, pos.Y - 3, sprite.size.X, 6);
        }

        public void update(float dt)
        {
            sprite.position += new Vector2(velocity * dt, 0);
        }

        public void draw(SpriteBatch sb)
        {
            sb.Draw(shadow, position: new Vector2(sprite.position.X, getCollision().Y - 7), color: Color.White);
            sprite.draw(sb);
        }


    }
}
