using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessTools
{
    public class ChessGame
    {
       public String Event;
       public String Site;
       public String White;
      public  String Black;
      public  String Result;
        public int WhiteElo;
        public int BlackElo;
        public String EventDate;
        public String Moves;

        public ChessGame(String Event, String Site, String White, String Black, String Result,
            int WhiteElo, int BlackElo, String EventDate, String Moves){

            this.Event = Event;
            this.Site = Site;
            this.White = White;
            this.Black = Black;
            this.Result = Result;
            this.WhiteElo = WhiteElo;
            this.BlackElo = BlackElo;
            this.EventDate = EventDate;
            this.Moves = Moves;
   
            }
    }
}
