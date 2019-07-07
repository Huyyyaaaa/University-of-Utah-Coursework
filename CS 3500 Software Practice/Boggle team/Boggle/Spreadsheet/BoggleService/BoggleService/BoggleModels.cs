using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


/// <summary>
/// This contains definitions of classes that show up in headers of the
/// service methods that are declared in IBoggleService.cs and implemented in
/// BoggleService.svc.cs. Objects of the following types are either passed as
/// parameters to or returned as results from the service methods.
/// </summary>

namespace Boggle
{

    
    public class UserInfo
    {        
        public string Nickname { get; set; }
        public int Score { get; set; }
        public string UserToken { get; set; }
        public HashSet<Words> WordsPlayed { get; set; }
    }


    public class Words
    {
        public string Word { get; set; }
        public int Score { get; set; }

    }

    public class GameInfo
    {
        public string GameID { get; set; }
        public string GameState { get; set; }
        public string Board { get; set; }
        public int TimeLimit { get; set; }
        public int TimeLeft { get; set; }
        public UserInfo Player1 = new UserInfo();
        public UserInfo Player2 = new UserInfo();
        public DateTime startTime { get; set; }
    }

    public class Token
    {
        public string UserToken { get; set; }
    }

    public class UserName
    {
        public string Nickname { get; set; }
    }

    public class PostGame
    {
        public string UserToken { get; set; }
        public int TimeLimit { get; set; }
    }

    public class gameId
    {
        public string GameID { get; set; }
    }

    public class WordScore
    {
        public int Score { get; set; }
    }

    public class PlayedWord
    {
        public string UserToken { get; set; }
        public string Word { get; set; }
    }
}