using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glamour2
{
    interface IEntity
    {
        Rectangle getCollision();
        Vector2 getPosition();
        void draw(SpriteBatch sb);
    }
}
