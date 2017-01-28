using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fortress
{
    public class Session
    {
        public FortressGame Game;
        public Map Map;

        public Session(FortressGame game)
        {
            Game = game;
            Map = new Map(this, 200, 10, 200);
        }
    }
}
