using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

class Programm
{
    private static readonly HttpClient httpClient = new HttpClient();
    private static string webhookUrl;
    [DllImport("kernel32.dll")]
    public static extern IntPtr GetConsoleWindow();
    static Random _random = new Random();
    static void ClearScreen()
    {
        // check os
        if (Environment.OSVersion.Platform == PlatformID.Unix)
        {
            // (Linux/Mac)
            Console.Clear();
        }
        else if (Environment.OSVersion.Platform == PlatformID.Win32NT ||
                 Environment.OSVersion.Platform == PlatformID.Win32S ||
                 Environment.OSVersion.Platform == PlatformID.WinCE)
        {
            // Windows
            Console.Clear();
        }
    }

    static void TypeWriterAnimation(string text, double delay = 0.05)
    {
        foreach (char c in text)
        {
            Console.Write(c);
            Thread.Sleep((int)(delay * 600));
        }
        Console.WriteLine();
    }

    static void LoadingScreen()
    {
        ClearScreen();
        Console.WriteLine("Loading:");

        string[] animation = {
            "[■□□□□□□□□□]", "[■■□□□□□□□□]", "[■■■□□□□□□□]", "[■■■■□□□□□□]",
            "[■■■■■□□□□□]", "[■■■■■■□□□□]", "[■■■■■■■□□□]", "[■■■■■■■■□□]",
            "[■■■■■■■■■□]", "[■■■■■■■■■■]"
        };

        // loading animation. doesnt do anything is just for good design. and i was bored
        for (int i = 0; i < animation.Length; i++)
        {
            Thread.Sleep(200);
            Console.Write("\r" + animation[i % animation.Length]);
        }

        Console.WriteLine();
        ClearScreen();
    }

    static void GenerateRandomTitle()
    {
        while (true)
        {
            string randomTitle = new string(Enumerable.Range(0, 10)
                .Select(_ => (char)_random.Next(65, 91)) // ASCII-letters A-Z
                .ToArray());


            string randomTitleDone = (randomTitle + " made by literally.ag7 " + randomTitle);
            Console.Title = randomTitleDone;
            Thread.Sleep(50);
        }
    }

    static void Terminate()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Programm will terminate in 3 Secs...");
        Console.ResetColor();

        Thread.Sleep(3000);

        Process.GetCurrentProcess().Kill();
    }

    static void DisplayTitle()
    {
        ConsoleColor[] colors = {
            ConsoleColor.Red,
            ConsoleColor.Green,
            ConsoleColor.Yellow,
            ConsoleColor.Blue,
            ConsoleColor.Magenta,
            ConsoleColor.Cyan,
            ConsoleColor.White
        };

        ConsoleColor randomColor = colors[_random.Next(colors.Length)];

        string title = @"
###################################################################### 

  _____     _           _  ___ _ _           
 | ____|___| |__   ___ | |/ (_) | | ___ _ __ 
 |  _| / __| '_ \ / _ \| ' /| | | |/ _ \ '__|
 | |__| (__| | | | (_) | . \| | | |  __/ |   
 |_____\___|_| |_|\___/|_|\_\_|_|_|\___|_|   
                                             
  

                                        Made by ag7-dev.de with <3
THIS PROJECT IS OPEN SOURCE AND FREE!
IF YOU PAYD FOR THIS YOU GOT SCAMMED! 
######################################################################                                                      
        ";

        Console.ForegroundColor = randomColor;
        Console.WriteLine(title);
        Console.ResetColor();
    }

    static async Task Main(string[] args)
    {
        IntPtr hwnd = GetConsoleWindow();
        Thread randomTitleThread = new Thread(GenerateRandomTitle);
        randomTitleThread.Start();

        //loadingscreen
        LoadingScreen();
        webhookUrl = "None";

        while (true)
        {
            ClearScreen();
            DisplayTitle();
            Console.ForegroundColor = ConsoleColor.Magenta; 
            Console.WriteLine($"Webhook: {webhookUrl}");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("######################################");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("1. Set Webhook: ");
            Console.WriteLine("2. Test Webhook: ");
            Console.WriteLine("3. ");
            Console.WriteLine("4. ");
            Console.WriteLine("5. ");
            Console.WriteLine("6. ");
            Console.WriteLine("7. ");
            Console.WriteLine("8. ");
            Console.WriteLine("9. Exit");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("######################################");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("[ > ] ");
            Console.ResetColor();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ClearScreen();
                    AskForWebhook();
                    break;

                case "2":
                    ClearScreen();
                    TestWebhook();
                    break;

                case "3":
                    ClearScreen();
                    break;

                case "4":
                    ClearScreen();
                    break;

                case "5":
                    ClearScreen();
                    break;

                case "6":
                    ClearScreen();
                    break;

                case "7":
                    ClearScreen();
                    break;

                case "8":
                    ClearScreen();
                    break;

                case "9":
                    ClearScreen();
                    Terminate();
                    break;

                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n[ ! ] Invalid Input");
                    Console.ResetColor();
                    Thread.Sleep( 1000 );
                    break;
            }
        }

        }

    public static async Task LogWebHook(string webhookUrl, string name, string path, string status, string method)
    {
        using (HttpClient client = new HttpClient())
        {
            // Erstellen des Embed-Objekts
            var embed = new
            {
                content = "",
                embeds = new[]
                {
                    new
                    {
                        title = "Echo AC Blocked / Killed",
                        description = "",
                        fields = new[]
                        {
                            new { name = "Name", value = name },
                            new { name = "Path", value = path },
                            new { name = "Status", value = status },
                            new { name = "Method", value = method },
                            new { name = "Time", value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }
                        },
                        footer = new
                        {
                            text = "Powered by EchoKiller. Made by ag7-dev.de"
                        }
                    }
                }
            };

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(embed);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await client.PostAsync(webhookUrl, content);
        }
    }

    private static async Task AskForWebhook()
    {
        bool isValidWebhook = false;

        while (!isValidWebhook)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("[ > ] Please enter a webhook: ");
            webhookUrl = Console.ReadLine();

            isValidWebhook = await IsWebhookValid(webhookUrl);

            if (isValidWebhook)
            {
                Console.WriteLine("\n");
                Console.WriteLine($"[ + ] Webhook Saved: {webhookUrl}");
                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("\n[ - ] The entered webhook is not valid. Please try again.");
                Thread.Sleep(1000);
            }

            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(1000); 
        }
    }

    private static async Task<bool> IsWebhookValid(string url)
    {
        try
        {
            var response = await httpClient.GetAsync(url);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException)
        {
            return false;
        }
    }


    private static async Task TestWebhook()
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write("[ # ] Testing Webhook. ");
        LogWebHook(webhookUrl, "Test", "TestPath", "TestStatus", "TestMethod" );
    }



}