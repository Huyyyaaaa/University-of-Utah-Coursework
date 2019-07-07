using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Net.HttpStatusCode;
using System.Diagnostics;
using Newtonsoft.Json;
using Boggle;

namespace Boggle
{
    /// <summary>
    /// Provides a way to start and stop the IIS web server from within the test
    /// cases.  If something prevents the test cases from stopping the web server,
    /// subsequent tests may not work properly until the stray process is killed
    /// manually.
    /// </summary>
    public static class IISAgent
    {
        // Reference to the running process
        private static Process process = null;

        /// <summary>
        /// Starts IIS
        /// </summary>
        public static void Start(string arguments)
        {
            if (process == null)
            {
                ProcessStartInfo info = new ProcessStartInfo(Properties.Resources.IIS_EXECUTABLE, arguments);
                info.WindowStyle = ProcessWindowStyle.Minimized;
                info.UseShellExecute = false;
                process = Process.Start(info);
            }
        }

        /// <summary>
        ///  Stops IIS
        /// </summary>
        public static void Stop()
        {
            if (process != null)
            {
                process.Kill();
            }
        }
    }
    [TestClass]
    public class BoggleTests
    {
        private static readonly object sync = new object();
        /// <summary>
        /// This is automatically run prior to all the tests to start the server
        /// </summary>
        [ClassInitialize()]
        public static void StartIIS(TestContext testContext)
        {
            IISAgent.Start(@"/site:""BoggleService"" /apppool:""Clr4IntegratedAppPool"" /config:""..\..\..\.vs\config\applicationhost.config""");
        }

        /// <summary>
        /// This is automatically run when all tests have completed to stop the server
        /// </summary>
        [ClassCleanup()]
        public static void StopIIS()
        {
            IISAgent.Stop();
        }

        private RestTestClient client = new RestTestClient("http://localhost:60000/BoggleService.svc/");

        /// <summary>
        /// Note that DoGetAsync (and the other similar methods) returns a Response object, which contains
        /// the response Stats and the deserialized JSON response (if any).  See RestTestClient.cs
        /// for details.
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            lock (sync)
            {
                UserInfo u = new UserInfo();
                u.Nickname = "Jon";
                Response r = client.DoPostAsync("users", u).Result;
                Assert.AreEqual(Created, r.Status);
            }
            
        }
        [TestMethod]
        public void TestMethod2()
        {
            lock (sync)
            {
                UserInfo u = new UserInfo();
                u.Nickname = null;
                Response r = client.DoPostAsync("users", u).Result;
                Assert.AreEqual(Forbidden, r.Status);
            }

        }

        [TestMethod]
        public void TestMethod3()
        {
            lock (sync)
            {
                UserInfo u = new UserInfo();
                u.Nickname = "";
                Response r = client.DoPostAsync("users", u).Result;
                Assert.AreEqual(Forbidden, r.Status);
            }
        }

        [TestMethod]
        public void TestMethod4()
        {
            lock (sync)
            {

                UserInfo u = new UserInfo();
                u.Nickname = "jon";
                Response r = client.DoPostAsync("users", u).Result;

                PostGame pg1 = new PostGame();
                pg1.UserToken = r.Data["UserToken"];
                pg1.TimeLimit = 30;
                Response pgr1 = client.DoPostAsync("games", pg1).Result;
                Assert.AreEqual(Accepted, pgr1.Status);

                UserInfo u2 = new UserInfo();
                u2.Nickname = "TestPlayer";
                Response r2 = client.DoPostAsync("users", u2).Result;



                PostGame pg2 = new PostGame();
                pg2.UserToken = r2.Data["UserToken"];
                pg2.TimeLimit = 45;
                Response pgr2 = client.DoPostAsync("games", pg2).Result;
                Assert.AreEqual(Created, pgr2.Status);

                string GameID = pgr2.Data["GameID"];



                PlayedWord word = new PlayedWord();
                word.UserToken = r.Data["UserToken"];
                word.Word = "name";
                string url = String.Format("games/{0}", GameID);
                Response g = client.DoPutAsync(word, url).Result;
                Assert.AreEqual(OK, g.Status);

                PlayedWord word2 = new PlayedWord();
                word2.UserToken = r.Data["UserToken"];
                word2.Word = "lane";
                Response g2 = client.DoPutAsync(word2, url).Result;
                Assert.AreEqual(OK, g.Status);

                PlayedWord word3 = new PlayedWord();
                word3.UserToken = r.Data["UserToken"];
                word3.Word = "name";
                Response g3 = client.DoPutAsync(word3, url).Result;
                Assert.AreEqual(OK, g.Status);

                PlayedWord word4 = new PlayedWord();
                word4.UserToken = r2.Data["UserToken"];
                word4.Word = "name";
                Response g4 = client.DoPutAsync(word4, url).Result;
                Assert.AreEqual(OK, g.Status);

                PlayedWord word5 = new PlayedWord();
                word5.UserToken = r2.Data["UserToken"];
                word5.Word = "name";
                Response g5 = client.DoPutAsync(word5, url).Result;
                Assert.AreEqual(OK, g.Status);

            }

        }

        [TestMethod]
        public void TestMethod5()
        {
            lock (sync)
            {
                UserInfo u = new UserInfo();
                u.Nickname = "jon";
                Response r = client.DoPostAsync("users", u).Result;

                PostGame pg1 = new PostGame();
                pg1.UserToken = r.Data["UserToken"];
                pg1.TimeLimit = 30;
                Response pgr1 = client.DoPostAsync("games", pg1).Result;
                Assert.AreEqual(Accepted, pgr1.Status);

                UserInfo u2 = new UserInfo();
                u2.Nickname = "TestPlayer";
                Response r2 = client.DoPostAsync("users", u2).Result;



                PostGame pg2 = new PostGame();
                pg2.UserToken = r2.Data["UserToken"];
                pg2.TimeLimit = 45;
                Response pgr2 = client.DoPostAsync("games", pg2).Result;
                Assert.AreEqual(Created, pgr2.Status);

                string GameID = pgr2.Data["GameID"];



                PlayedWord word = new PlayedWord();
                word.UserToken = r.Data["UserToken"];
                word.Word = "";
                string url = String.Format("games/{0}", GameID);
                Response g = client.DoPutAsync(word, url).Result;
                Assert.AreEqual(Forbidden, g.Status);
            }
        }

        [TestMethod]
        public void TestMethod6()
        {
            lock (sync)
            {
                UserInfo u = new UserInfo();
                u.Nickname = "jon";
                Response r = client.DoPostAsync("users", u).Result;

                PostGame pg1 = new PostGame();
                pg1.UserToken = null;
                pg1.TimeLimit = 30;
                Response pgr1 = client.DoPostAsync("games", pg1).Result;
                Assert.AreEqual(Forbidden, pgr1.Status);
            }

        }

        [TestMethod]
        public void TestMethod7()
        {
            lock (sync)
            {
                UserInfo u = new UserInfo();
                u.Nickname = "jon";
                Response r = client.DoPostAsync("users", u).Result;

                PostGame pg1 = new PostGame();
                pg1.UserToken = r.Data["UserToken"];
                pg1.TimeLimit = 3;
                Response pgr1 = client.DoPostAsync("games", pg1).Result;
                Assert.AreEqual(Forbidden, pgr1.Status);
            }
        }

        [TestMethod]
        public void TestMethod8()
        {
            lock (sync)
            {
                UserInfo u = new UserInfo();
                u.Nickname = "jon";
                Response r = client.DoPostAsync("users", u).Result;

                PostGame pg1 = new PostGame();
                pg1.UserToken = r.Data["UserToken"];
                pg1.TimeLimit = 30;
                Response pgr1 = client.DoPostAsync("games", pg1).Result;
                Assert.AreEqual(Accepted, pgr1.Status);

                Token t = new Token();
                t.UserToken = r.Data["UserToken"];
                Response r2 = client.DoPutAsync(t, "games").Result;
                Assert.AreEqual(OK, r2.Status);
            }
        }

        [TestMethod]
        public void TestMethod9()
        {
            lock (sync)
            {
                UserInfo u = new UserInfo();
                u.Nickname = "jon";
                Response r = client.DoPostAsync("users", u).Result;

                PostGame pg1 = new PostGame();
                pg1.UserToken = r.Data["UserToken"];
                pg1.TimeLimit = 30;
                Response pgr1 = client.DoPostAsync("games", pg1).Result;
                Assert.AreEqual(Accepted, pgr1.Status);

                Token t = new Token();
                t.UserToken = "";
                Response r2 = client.DoPutAsync(t, "games").Result;
                Assert.AreEqual(Forbidden, r2.Status);
            }
        }

        [TestMethod]
        public void TestMethod10()
        {
            lock (sync)
            {


                UserInfo u = new UserInfo();
                u.Nickname = "jon";
                Response r = client.DoPostAsync("users", u).Result;

                PostGame pg1 = new PostGame();
                pg1.UserToken = r.Data["UserToken"];
                pg1.TimeLimit = 30;
                Response pgr1 = client.DoPostAsync("games", pg1).Result;
                Assert.AreEqual(Created, pgr1.Status);

                UserInfo u2 = new UserInfo();
                u2.Nickname = "TestPlayer";
                Response r2 = client.DoPostAsync("users", u2).Result;

                PostGame pg2 = new PostGame();
                pg2.UserToken = r2.Data["UserToken"];
                pg2.TimeLimit = 45;
                Response pgr2 = client.DoPostAsync("games", pg2).Result;
                Assert.AreEqual(Accepted, pgr2.Status);


                Token t = new Token();
                t.UserToken = r2.Data["UserToken"];
                Response r3 = client.DoPutAsync(t, "games").Result;
                Assert.AreEqual(OK, r3.Status);
            }
        }

        [TestMethod]
        public void TestMethod11()
        {
            lock (sync)
            {


                UserInfo u = new UserInfo();
                u.Nickname = "jon";
                Response r = client.DoPostAsync("users", u).Result;

                PostGame pg1 = new PostGame();
                pg1.UserToken = r.Data["UserToken"];
                pg1.TimeLimit = 10;
                Response pgr1 = client.DoPostAsync("games", pg1).Result;
                Assert.AreEqual(Accepted, pgr1.Status);

                UserInfo u2 = new UserInfo();
                u2.Nickname = "TestPlayer";
                Response r2 = client.DoPostAsync("users", u2).Result;



                PostGame pg2 = new PostGame();
                pg2.UserToken = r2.Data["UserToken"];
                pg2.TimeLimit = 10;
                Response pgr2 = client.DoPostAsync("games", pg2).Result;
                Assert.AreEqual(Created, pgr2.Status);

                string GameID = pgr2.Data["GameID"];



                PlayedWord word = new PlayedWord();
                word.UserToken = r.Data["UserToken"];
                word.Word = "name";
                string url = String.Format("games/{0}", GameID);
                Response g = client.DoPutAsync(word, url).Result;
                Assert.AreEqual(OK, g.Status);

                PlayedWord word2 = new PlayedWord();
                word2.UserToken = r.Data["UserToken"];
                word2.Word = "lane";
                Response g2 = client.DoPutAsync(word2, url).Result;
                Assert.AreEqual(OK, g.Status);

                PlayedWord word3 = new PlayedWord();
                word3.UserToken = r.Data["UserToken"];
                word3.Word = "name";
                Response g3 = client.DoPutAsync(word3, url).Result;
                Assert.AreEqual(OK, g.Status);

                PlayedWord word4 = new PlayedWord();
                word4.UserToken = r2.Data["UserToken"];
                word4.Word = "name";
                Response g4 = client.DoPutAsync(word4, url).Result;
                Assert.AreEqual(OK, g.Status);

                PlayedWord word5 = new PlayedWord();
                word5.UserToken = r2.Data["UserToken"];
                word5.Word = "name";
                Response g5 = client.DoPutAsync(word5, url).Result;
                Assert.AreEqual(OK, g.Status);


                Response res = client.DoGetAsync("games/{0}?Brief={1}", GameID, "yes").Result;
                Assert.AreEqual(OK, res.Status);

                Response res2 = client.DoGetAsync("games/{0}?Brief={1}", GameID, "no").Result;
                Assert.AreEqual(OK, res.Status);

                System.Threading.Thread.Sleep(10000);

                Response res3 = client.DoGetAsync("games/{0}?Brief={1}", GameID, "yes").Result;
                Assert.AreEqual(OK, res.Status);

                Response res4 = client.DoGetAsync("games/{0}?Brief={1}", GameID, "no").Result;
                Assert.AreEqual(OK, res.Status);

            }
        }

        [TestMethod]
        public void TestMethod12()
        {
            lock (sync)
            {
                UserInfo u = new UserInfo();
                u.Nickname = "jon";
                Response r = client.DoPostAsync("users", u).Result;

                PostGame pg1 = new PostGame();
                pg1.UserToken = r.Data["UserToken"];
                pg1.TimeLimit = 10;
                Response pgr1 = client.DoPostAsync("games", pg1).Result;
                Assert.AreEqual(Accepted, pgr1.Status);

                string GameID = pgr1.Data["GameID"];


                Response res = client.DoGetAsync("games/{0}?Brief={1}", GameID, "yes").Result;
                Assert.AreEqual(OK, res.Status);

            }


        }
    }
}
