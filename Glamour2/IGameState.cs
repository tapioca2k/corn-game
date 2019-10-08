using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glamour2
{
    public interface IGameState
    {

        void handleInput(InputHandler ih, float dt);
        void update(float dt);
        void draw(SpriteBatch sb);


    }
}
