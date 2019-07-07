using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ChessTools
{
    public class PGNReader
    {

       public List<ChessGame> readPGNFile(string file)
        {
            List<ChessGame> chessGames = new List<ChessGame>();
            string[] lines = File.ReadAllLines(file);

            int empty = 0;
            string event_val = "";
            string site = "";
            string event_date = "";
            string white = "";
            string black = "";
            int white_elo = 0;
            int black_elo = 0;
            string result = "";
            string moves = "";

            foreach(string s in lines)
            {

                int start_index;
                int end_index;
                

                if (s.StartsWith("[Event "))
                {
                    start_index = s.IndexOf(' ')+2;
                    end_index = (s.Length - 2)- start_index;
                    event_val = s.Substring(start_index, end_index);
                }
                if (s.StartsWith("[Site"))
                {
                    start_index = s.IndexOf(' ') + 2;
                    end_index = (s.Length - 2) - start_index;
                    site = s.Substring(start_index, end_index);

                }
                if (s.StartsWith("[EventDate"))
                {
                    start_index = s.IndexOf(' ') + 2;
                    end_index = (s.Length - 2) - start_index;
                    event_date = s.Substring(start_index, end_index);

                }
                if (s.StartsWith("[White "))
                {
                    start_index = s.IndexOf(' ') + 2;
                    end_index = (s.Length - 2) - start_index;
                    white = s.Substring(start_index, end_index);
                }
                if (s.StartsWith("[Black "))
                {
                    start_index = s.IndexOf(' ') + 2;
                    end_index = (s.Length - 2) - start_index;
                    black = s.Substring(start_index, end_index);
                }
                if (s.StartsWith("[WhiteElo"))
                {
                    start_index = s.IndexOf(' ') + 2;
                    end_index = (s.Length - 2) - start_index;
                    white_elo = Int32.Parse(s.Substring(start_index, end_index));
                }
                if (s.StartsWith("[BlackElo"))
                {
                    start_index = s.IndexOf(' ') + 2;
                    end_index = (s.Length - 2) - start_index;
                    black_elo = Int32.Parse(s.Substring(start_index, end_index));
                }
                if (s.StartsWith("[Result"))
                {
                    start_index = s.IndexOf(' ') + 2;
                    end_index = (s.Length - 2) - start_index;
                    result = s.Substring(start_index, end_index);

                    if (result == "1-0") result = "W";
                    if (result == "0-1") result = "B";
                    if (result == "1/2-1/2") result = "D";
                }
                if (s == String.Empty)
                {
                    empty++;
                }
                if (empty == 1)
                {
                    moves += s;
                }
                if(empty == 2)
                {
                    chessGames.Add(new ChessGame(event_val, site, white, black, result, white_elo, black_elo, event_date, moves));
                    empty = 0;
                    event_val = "";
                    site = "";
                    event_date = "";
                    white = "";
                    black = "";
                    white_elo = 0;
                    black_elo = 0;
                    result = "";
                    moves = "";

                }
                
            }

            return chessGames;
        }
    }
}
