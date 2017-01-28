using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public abstract class GameState
    {
        protected FortressGame Game;

        public GameState(FortressGame game)
        {
            Game = game;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw();
    }
}
