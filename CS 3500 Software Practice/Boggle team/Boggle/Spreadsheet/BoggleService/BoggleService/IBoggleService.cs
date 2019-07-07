using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Boggle
{
    [ServiceContract]
    public interface IBoggleService
    {
        /// <summary>
        /// Sends back index.html as the response body.
        /// </summary>
        [WebGet(UriTemplate = "/api")]
        Stream API();

        
        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [WebInvoke(Method = "POST", UriTemplate = "/users")]
        Token RegisterUser(UserName user);

        /// <summary>
        /// join a game
        /// </summary>
        [WebInvoke(Method = "POST", UriTemplate = "/games")]
        gameId JoinGame(PostGame user);

        /// <summary>
        /// cancel a join game request
        /// </summary>
        /// <param name="token"></param>
        [WebInvoke(Method = "PUT", UriTemplate = "/games")]
        void cancelJoinGame(Token user);

        /// <summary>
        /// plays a word and returns the word score
        /// </summary>
        /// <param name="GameID"></param>
        /// <returns></returns>
        [WebInvoke(Method = "PUT", UriTemplate = "/games/{GameID}")]
        WordScore playWord(string GameID, PlayedWord word);

        [WebInvoke(Method = "GET", UriTemplate = "games/{GameID}?Brief={brief}")]
        GameInfo GetGameStatus(string GameID, string brief);
    }

}
