using System;

namespace Models
{
    [Serializable]
    internal class GameState
    {
        public int Exp { get; set; }

        public int Lvl { get; set; }
        //Rooms[]

        //make new GameState
        public GameState()
        {
            Exp = 0;
            Lvl = 0;
        }
    }
}