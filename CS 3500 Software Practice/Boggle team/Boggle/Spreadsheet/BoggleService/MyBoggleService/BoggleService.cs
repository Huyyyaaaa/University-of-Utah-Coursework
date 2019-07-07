using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using static System.Net.HttpStatusCode;

namespace Boggle
{
    public class BoggleService
    {
        // Our "poor man's" database for users and items
        //The sync object is used for synchronization (because multiple threads can be running
        // simultaneously in the service).  The entire state is lost each time
        // the service shuts down, so eventually we'll need to port this to
        // a proper database.
        private readonly static Dictionary<String, UserInfo> users = new Dictionary<String, UserInfo>();
        private readonly static Dictionary<String, GameInfo> games = new Dictionary<String, GameInfo>();
        private readonly static Dictionary<String, GameInfo> pendingGames = new Dictionary<String, GameInfo>();
        private static readonly object sync = new object();
        private static int GUID = 1;

       

        
        /// <summary>
        /// Cancels a join games request
        /// </summary>
        /// <param name="item"></param>
        public void cancelJoinGame(Token item, out HttpStatusCode status)
        {
            status = OK;

            lock (sync)
            {
                if (String.IsNullOrEmpty(item.UserToken))
                {
                    status = Forbidden;
                    return;
                }
                if (!users.ContainsKey(item.UserToken))
                {
                    status = Forbidden;
                    return;
                }
                if (pendingGames.Count <= 0)
                {
                    status = Forbidden;
                    return;
                }
                foreach (GameInfo g in pendingGames.Values)
                {
                    if (item.UserToken == g.Player1.UserToken && g.Player2.UserToken == null)
                    {
                        g.Player1.UserToken = null;
                        status = OK;
                    }
                    else
                    {
                        status = Forbidden;
                        return;
                    }

                }

            }
        }


        /// <summary>
        /// gets teh game status
        /// </summary>
        /// <param name="GameID"></param>
        /// <param name="brief"></param>
        /// <returns></returns>
        public GameInfo GetGameStatus(string GameID, string brief, out HttpStatusCode httpStauts)
        {

            foreach (GameInfo g in pendingGames.Values)
            {
                if (g.GameID.Equals(GameID))
                {
                    httpStauts = OK;
                    GameInfo status = new GameInfo();
                    status.GameState = g.GameState;
                    return status;
                }

            }

            foreach (var g in games)
            {
                if (g.Key.Equals(GameID))
                {
                    httpStauts = OK;
                    GameInfo status = new GameInfo();

                    if (getTimeLeft(g.Value) <= 0)
                    {
                        if (brief.Equals("yes"))
                        {
                            status.GameState = "completed";
                            status.TimeLeft = 0;
                            status.Player1.Nickname = g.Value.Player1.Nickname;
                            status.Player2.Nickname = g.Value.Player2.Nickname;
                            status.Player1.Score = g.Value.Player1.Score;
                            status.Player2.Score = g.Value.Player2.Score;
                            httpStauts = OK;
                            return status;
                        }
                        else
                        {
                            status.GameState = "completed";
                            status.Board = g.Value.Board;
                            status.TimeLimit = g.Value.TimeLimit;
                            status.TimeLeft = 0;
                            status.Player1 = g.Value.Player1;
                            status.Player2 = g.Value.Player2;
                            httpStauts = OK;
                            return status;
                        }
                    }

                    else
                    {
                        status.GameState = "active";
                        if (brief.Equals("yes"))
                        {
                            status.TimeLeft = getTimeLeft(g.Value);
                            status.Player1.Nickname = g.Value.Player1.Nickname;
                            status.Player1.Score = g.Value.Player1.Score;
                            status.Player2.Nickname = g.Value.Player2.Nickname;
                            status.Player2.Score = g.Value.Player2.Score;
                            return status;
                        }
                        else
                        {
                            status.TimeLeft = getTimeLeft(g.Value);
                            status.Player1.Nickname = g.Value.Player1.Nickname.ToString();
                            status.Player1.Score = g.Value.Player1.Score;
                            status.Player2.Nickname = g.Value.Player2.Nickname;
                            status.Player2.Score = g.Value.Player2.Score;
                            status.Board = g.Value.Board;
                            status.TimeLimit = g.Value.TimeLimit;

                            return status;
                        }
                    }
                }
            }

            httpStauts = Forbidden;
            return null;
        }

        /// <summary>
        /// Join a boggle game
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public gameId JoinGame(PostGame item, out HttpStatusCode httpStatus)
        {
            httpStatus = OK;

            lock (sync)
            {
                gameId ID = new gameId();

                if (item.UserToken == null || item.TimeLimit < 0)
                {
                    httpStatus = Forbidden;
                    return null;
                }



                if (item.TimeLimit < 5 || item.TimeLimit > 120 || !users.ContainsKey(item.UserToken))
                {
                    httpStatus = Forbidden;
                    return null;
                }

                if (pendingGames.Count == 0)
                {
                    GameInfo g = new GameInfo();
                    g.GameState = "pending";
                    g.GameID = "" + GUID;
                    g.Player1 = new UserInfo();
                    g.Player2 = new UserInfo();
                    pendingGames.Add(g.GameID, g);
                    GUID++;
                }

                foreach (GameInfo g in pendingGames.Values)
                {
                    if (g.Player1.UserToken != null)
                    {
                        if (g.Player1.UserToken == item.UserToken)
                        {
                            httpStatus = Conflict;
                            return null;
                        }


                    }


                }


                foreach (GameInfo g in pendingGames.Values)
                {
                    if (g.Player1.UserToken == null)
                    {
                        g.Player1.UserToken = item.UserToken;
                        g.TimeLimit = item.TimeLimit;
                        g.Player1.Nickname = GetUserInfo(item);
                        //g.GameID = GUID+"";
                        ID.GameID = g.GameID;
                        httpStatus = Accepted;
                        break;
                    }
                    else if (g.Player2.UserToken == null)
                    {
                        g.Player2.UserToken = item.UserToken;
                        ID.GameID = g.GameID;
                        httpStatus = Created;
                    }

                    if (g.Player1.UserToken != null && g.Player2.UserToken != null)
                    {
                        int.TryParse(ID.GameID, out int i);
                        GameInfo game = startGame(g, item, i, item.TimeLimit);
                        games.Add(g.GameID, game);
                        pendingGames.Remove(i.ToString());
                        break;

                    }

                }


                // GUID++;
                return ID;

            }

        }

        /// <summary>
        /// helper method to start a game after 2 players have joind the samed game
        /// </summary>
        /// <param name="g"></param>
        /// <param name="item"></param>
        /// <param name="gUID"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private GameInfo startGame(GameInfo g, PostGame item, int gUID, int time)
        {
            GameInfo newGame = new GameInfo();



            newGame.Board = new BoggleBoard().ToString();
            newGame.GameState = "active";
            newGame.GameID = gUID.ToString();
            newGame.Player1 = g.Player1;
            newGame.Player2 = g.Player2;
            newGame.Player1.Score = 0;
            newGame.Player2.Score = 0;
            newGame.Player1.Nickname = g.Player1.Nickname.ToString();
            newGame.Player2.Nickname = GetUserInfo(item);
            newGame.Player1.WordsPlayed = new HashSet<Words>();
            newGame.Player2.WordsPlayed = new HashSet<Words>();
            newGame.TimeLimit = getTimeLimit(g.TimeLimit, time);
            newGame.TimeLeft = newGame.TimeLimit;
            newGame.startTime = DateTime.Now;

            //for testing boards
            if (newGame.Player2.Nickname.Equals("TestPlayer"))
            {
                newGame.Board = new BoggleBoard("NAMEGAMEPAINGAIN").ToString();
            }
            return newGame;

        }

        /// <summary>
        /// gets the users nickname 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string GetUserInfo(PostGame item)
        {
            Token t = new Token();
            t.UserToken = item.UserToken;
            users.TryGetValue(t.UserToken, out UserInfo user);

            return user.Nickname;
        }

        /// <summary>
        /// calculates the time limit of a game from both players
        /// </summary>
        /// <param name="timeLimit"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private int getTimeLimit(int timeLimit, int time)
        {
            return (timeLimit + time) / 2;
        }

        /// <summary>
        /// gets the time left of a game
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        private int getTimeLeft(GameInfo g)
        {
            int time = g.TimeLimit;

            DateTime start = g.startTime;
            DateTime now = DateTime.Now;

            int timePassed = (int)now.Subtract(start).TotalSeconds;

            return time - timePassed;
        }

        /// <summary>
        /// play a word given the GameID and the word object
        /// </summary>
        /// <param name="GameID"></param>
        /// <param name="word"></param>
        /// <returns></returns>
        public WordScore playWord(string GameID, PlayedWord word, out HttpStatusCode httpStatus)
        {
            httpStatus = OK;
            Words newWord = new Words();
            WordScore pw = new WordScore();


            GameInfo pg = new GameInfo();
            GameInfo g = new GameInfo();

            if (String.IsNullOrEmpty(word.Word) || String.IsNullOrEmpty(GameID) || word.Word.Trim().Length > 30)
            {
                httpStatus = Forbidden;
                pw.Score = -1;
                return pw;
            }

            try
            {
                pendingGames.TryGetValue(GameID.ToString(), out pg);
                if (pg.GameState.Equals("pending"))
                {
                    httpStatus = Conflict;
                    pw.Score = -1;
                    return pw;
                }
            }
            catch (Exception)
            {

            }



            games.TryGetValue(GameID.ToString(), out g);


            if (!g.GameState.Equals("active"))
            {
                httpStatus = Conflict;
                pw.Score = -1;
                return pw;
            }

            if (getTimeLeft(g) <= 0)
            {
                httpStatus = Conflict;
                pw.Score = 0;
                return pw;
            }

            if (!word.UserToken.Equals(g.Player1.UserToken.ToString()) && !word.UserToken.Equals(g.Player2.UserToken.ToString()))
            {
                httpStatus = Forbidden;
                pw.Score = 0;
                return pw;
            }

            if (new BoggleBoard(g.Board).CanBeFormed(word.Word.Trim()))
            {
                if (word.UserToken.Equals(g.Player1.UserToken))
                {
                    foreach (var w in g.Player1.WordsPlayed)
                    {
                        if (w.Word.Equals(word.Word.Trim()))
                        {
                            newWord.Score = 0;
                            newWord.Word = word.Word.Trim();
                            g.Player1.WordsPlayed.Add(newWord);
                            httpStatus = OK;
                            pw.Score = newWord.Score;
                            return pw;
                        }
                    }
                    if (WordValid(word.Word))
                    {
                        newWord.Score = ScoreWord(word.Word);
                        newWord.Word = word.Word.Trim();
                        g.Player1.WordsPlayed.Add(newWord);
                        g.Player1.Score += newWord.Score;
                        httpStatus = OK;
                        pw.Score = newWord.Score;
                        return pw;
                    }


                }

                else
                {
                    foreach (var w in g.Player2.WordsPlayed)
                    {
                        if (w.Word.Equals(word.Word.Trim()))
                        {
                            newWord.Score = 0;
                            newWord.Word = word.Word.Trim();
                            g.Player2.WordsPlayed.Add(newWord);
                            httpStatus = OK;
                            pw.Score = newWord.Score;
                            return pw;
                        }
                    }
                    if (WordValid(word.Word))
                    {
                        newWord.Score = ScoreWord(word.Word);
                        newWord.Word = word.Word.Trim();
                        g.Player2.WordsPlayed.Add(newWord);
                        g.Player2.Score += newWord.Score;
                        httpStatus = OK;
                        pw.Score = newWord.Score;
                        return pw;
                    }

                }
            }
            else
            {

                if (word.UserToken.Equals(g.Player1.UserToken))
                {
                    newWord.Score = ScoreWord(word.Word);
                    newWord.Word = word.Word.Trim();
                    g.Player1.WordsPlayed.Add(newWord);
                    g.Player1.Score += newWord.Score;
                    httpStatus = OK;
                    pw.Score = newWord.Score;
                    return pw;
                }
                else
                {
                    newWord.Score = ScoreWord(word.Word);
                    newWord.Word = word.Word.Trim();
                    g.Player2.WordsPlayed.Add(newWord);
                    g.Player2.Score += newWord.Score;
                    httpStatus = OK;
                    pw.Score = newWord.Score;
                    return pw;
                }

            }


            return pw;
        }

        /// <summary>
        /// helper method to score word
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        private int ScoreWord(string word)
        {
            if (WordValid(word))
            {
                switch (word.Length)
                {
                    case 1:
                    case 2:
                        return 0;
                    case 3:
                    case 4:
                        return 1;
                    case 5:
                        return 2;
                    case 6:
                        return 3;
                    case 7:
                        return 5;
                    default:
                        return 11;
                }
            }

            return -1;

        }

        /// <summary>
        /// register a user with service
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Token RegisterUser(UserName user, out HttpStatusCode httpStatus)
        {
            lock (sync)
            {
                if (user.Nickname == null || user.Nickname.Trim().Length <= 0)
                {
                    httpStatus = Forbidden;
                    return null;
                }

                string UID = Guid.NewGuid().ToString();
                Token t = new Token();
                t.UserToken = UID;
                UserInfo userinfo = new UserInfo();
                userinfo.UserToken = UID;
                userinfo.Nickname = user.Nickname;
                users.Add(UID, userinfo);
                httpStatus = Created;
                return t;
            }

        }

        /// <summary>
        /// helper method to check if word is found in dictionary.txt
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        private bool WordValid(string w)
        {
            if (w == null)
            {
                return false;
            }

            string line;
            using (StreamReader file = new System.IO.StreamReader("dictionary.txt"))
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Equals(w.Trim().ToUpper()))
                    {
                        return true;
                    }
                }
            }

            return false;
        }


    }
}
