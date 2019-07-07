using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using CustomNetworking;

namespace Boggle
{
    class SocketComm
    {
        //string socket for socket communication
        private SS socket;
        //boggle service
        private BoggleService service;
        //array to hold request objects
        private object[] request = new object[6];
        //content length
        private int contentLength;
        //regex pattern for content length       
        private static readonly Regex contentLengthPattern = new Regex(@"^content-length: (\d+)", RegexOptions.IgnoreCase);
        //regex pattern for game status
        private static readonly Regex gameStatusPattern = new Regex("(?:(/BoggleService.svc/))games/(.*)\\?(B|b)rief=(.*)", RegexOptions.IgnoreCase);


        /// <summary>
        /// creates a server and calls begin receive
        /// </summary>
        /// <param name="s"></param>
        public SocketComm(SS s)
        {
            socket = s;
            service = new BoggleService();
   

            socket.BeginReceive(Received, null);
        }

        /// <summary>
        /// handles receiving http requests
        /// </summary>
        /// <param name="s"></param>
        /// <param name="p"></param>
        private void Received(string s, object p)
        {

            
            
                if (s.Trim().Length == 0 && contentLength > 0)
                {
                    socket.BeginReceive(getProperRequest, null, contentLength);
                    Console.WriteLine(s);
                    return;
                }
            Match match = gameStatusPattern.Match(s);
             if (match.Success)
             {
                parseLine(s);
                socket.BeginReceive(getProperRequest, null);
                Console.WriteLine(s);
                return;
            }

                Console.WriteLine(s);
                for (int i = 0; i < s.Length; i++)
                {

                    if (s[i] == '\r')
                    {
                        parseLine(s);
                        socket.BeginReceive(Received, null);
                    }
                    Match m = contentLengthPattern.Match(s);
                    if (m.Success)
                    {
                        contentLength = int.Parse(m.Groups[1].ToString());
                    }


                }
           
            
            
        }

     

        /// <summary>
        /// gets the correct request type and calls the service method for a response
        /// </summary>
        /// <param name="line"></param>
        /// <param name="p"></param>
        private void getProperRequest(string line, object p = null)
        {
            request[5] = line;
            if((string)request[0] == "POST")
            {
                if((string)request[1] == "games")
                {
                    JoinGame((string)request[5]);
                }
                else
                {
                    RegisterUser((string)request[5]);
                }
            }
            else if((string)request[0] == "PUT")
            {
                if((string)request[1] == "games" && (string)request[2] == null)
                {
                    CancelGame((string)request[5]);
                }
                else
                {
                    PlayWord((string)request[5], (string)request[2]);
                }
            }
            else 
            {
                GetGameStatus((string)request[5], (string)request[2], (string)request[3]);
            }
        }
        /// <summary>
        /// response for game status
        /// </summary>
        /// <param name="body"></param>
        /// <param name="gameid"></param>
        /// <param name="brief"></param>
        private void GetGameStatus(string body, string gameid, string brief)
        {
            HttpStatusCode httpStatus;
            GameInfo game;

            if (brief.Equals("yes"))
            {
                game = service.GetGameStatus(gameid, "yes", out httpStatus);
            }
            else
            {
                game = service.GetGameStatus(gameid, "no", out httpStatus);
            }

            
            string res = ("HTTP/1.1 " + (int)httpStatus + " " + httpStatus + "\r\n");
            if ((int)httpStatus / 100 == 2)
            {
                string result = JsonConvert.SerializeObject(game);
                res += ("Content-Type: application/json\r\n");
                res += ("Content-Length: " + Encoding.UTF8.GetByteCount(result)+ "\r\n");
                res += "\r\n";
                res += result;
            }
            else
            {
                res += "\r\n";
            }
            
            socket.BeginSend(res, (x, y) => { socket.Shutdown(SocketShutdown.Both); }, null);
        }
        /// <summary>
        /// server response for for play word
        /// </summary>
        /// <param name="body"></param>
        /// <param name="gameid"></param>
        private void PlayWord(string body, string gameid)
        {
            HttpStatusCode httpStatus;
            PlayedWord word = JsonConvert.DeserializeObject<PlayedWord>(body);
            WordScore score = service.playWord(gameid, word, out httpStatus);

            string res = ("HTTP/1.1 " + (int)httpStatus + " " + httpStatus + "\r\n");
            if ((int)httpStatus / 100 == 2)
            {
                string result = JsonConvert.SerializeObject(score);
                res += ("Content-Type: application/json\r\n");
                res += ("Content-Length: " + Encoding.UTF8.GetByteCount(result) + "\r\n");
                res += "\r\n";
                res += result;
            }
            else
            {
                res += "\r\n";
            }
            socket.BeginSend(res, (x, y) => { socket.Shutdown(SocketShutdown.Both); }, null);

        }
        /// <summary>
        /// server response for cancel game
        /// </summary>
        /// <param name="body"></param>
        private void CancelGame(string body)
        {
            HttpStatusCode httpStatus;
            Token token = JsonConvert.DeserializeObject<Token>(body);
            service.cancelJoinGame(token, out httpStatus);
            string res = ("HTTP/1.1 " + (int)httpStatus + " " + httpStatus + "\r\n");
                         
                res += ("Content-Type: application/json\r\n");
                                          
            res += ("\r\n");
            socket.BeginSend(res, (x, y) => { socket.Shutdown(SocketShutdown.Both); }, null);

        }
        /// <summary>
        /// server response method for register user on boggle service
        /// </summary>
        /// <param name="body"></param>
        private void RegisterUser(string body)
        {
            HttpStatusCode httpStatus;
            UserName user = JsonConvert.DeserializeObject<UserName>(body);
            Token token = service.RegisterUser(user, out httpStatus);

            string res = ("HTTP/1.1 " + (int)httpStatus + " " + httpStatus + "\r\n");
            if ((int)httpStatus / 100 == 2)
            {
                string result = JsonConvert.SerializeObject(token);
                res += ("Content-Type: application/json\r\n");
                res += ("Content-Length: " + Encoding.UTF8.GetByteCount(result) + "\r\n");
                res += "\r\n";
                res += result;

            }
            else
            {
                res += "\r\n";
            }
            
            socket.BeginSend(res, (x, y) => { socket.Shutdown(SocketShutdown.Both); }, null);
        }
        /// <summary>
        /// server response for join game
        /// </summary>
        /// <param name="body"></param>
        private void JoinGame(string body)
        {
            HttpStatusCode httpStatus;
            PostGame post = JsonConvert.DeserializeObject<PostGame>(body);
            gameId game = service.JoinGame(post, out httpStatus);

            string res = ("HTTP/1.1 " + (int)httpStatus + " " + httpStatus + "\r\n");
            if ((int)httpStatus / 100 == 2)
            {
                string result = JsonConvert.SerializeObject(game);
                res += ("Content-Type: application/json\r\n");
                res += ("Content-Length: " + Encoding.UTF8.GetByteCount(result)+ "\r\n");
                res += "\r\n";
                res += result;
            }
            else
            {
                res += "\r\n";
            }
            
            socket.BeginSend(res, (x, y) => { socket.Shutdown(SocketShutdown.Both); }, null);
        }
        /// <summary>
        /// parses the http request into a request array object
        /// </summary>
        /// <param name="line"></param>
        private void parseLine(string line)
        {
            Regex regex;
            Match match;
            string regexStart = "(?:(/BoggleService.svc/))";
            string request;

            if(IsHttpRequest(line, out request))
            {
                if(request == "GET")
                {
                    if((match = (regex = new Regex(regexStart.Insert(23, "games/(.*)\\?(B|b)rief=(.*)"))).Match(line)).Success)
                    {
                        this.request[0] = "GET";
                        this.request[1] = "games";
                        this.request[2] = match.Groups[2].Value;
                        this.request[3] = match.Groups[4].Value;
                        return;
                    }
                }
                if(request == "PUT")
                {
                    if((match = (regex = new Regex(regexStart.Insert(23,"games/(\\d+)"))).Match(line)).Success)
                    {
                        this.request[0] = "PUT";
                        this.request[1] = "games";
                        this.request[2] = match.Groups[2].ToString();
                    }
                    else if ((match = (regex = new Regex(regexStart.Insert(23, "games"))).Match(line)).Success)
                    {
                        this.request[0] = "PUT";
                        this.request[1] = "games";
                    }
                }
                else
                {
                    if ((match = (regex = new Regex(regexStart.Insert(23, "games"))).Match(line)).Success)
                    {
                        this.request[0] = "POST";
                        this.request[1] = "games";
                    }
                    else if ((match = (regex = new Regex(regexStart.Insert(23, "users"))).Match(line)).Success)
                    {
                        this.request[0] = "POST";
                        this.request[1] = "users";
                    }
                }
            }
            else if (IsContentLength(line))
            {
                string length = line.Substring(16);
                this.request[4] = int.Parse(length);
            }
            else if (IsRequestBody(line))
            {
                this.request[5] = line;
            }

        }
        /// <summary>
        /// checks the http request line for the request body
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool IsRequestBody(string line)
        {
            if(line[0] == '{')
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// checks the http request line for content-length
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private bool IsContentLength(string line)
        {
            if(line[0] == 'C')
            {
                if (line.Contains("Length"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// checks for the http request type
        /// </summary>
        /// <param name="line"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool IsHttpRequest(string line, out string request)
        {
            if(line[0] == 'G')
            {
                request = "GET";
                return true;
            }
            else if(line[0] == 'P')
            {
                if(line[1] == 'U')
                {
                    request = "PUT";
                    return true;
                }
                else
                {
                    request = "POST";
                    return true;
                }
            }
            else
            {
                request = null;
                return true;
            }
        }
    }
}
