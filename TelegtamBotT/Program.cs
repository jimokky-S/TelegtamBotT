using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using System.Net.Http;
using System.IO;
using System.ComponentModel.Design;
using static System.Console;
using System.Threading;

namespace TelegtamBotT
{
    public class Program
    {
        static string Token = "7257944540:AAFBV2rs0psoBWR_m7UxY5md07qweWPNiWU";
        static WebClient Client = new WebClient();
        static void Main(string[] args)
        {
            frmmainbot frmmainbot = new frmmainbot();
            frmmainbot.Show();
           
            long offset = 0;
            string url = $"https://api.telegram.org/bot{Token}/";
            while (true)
            {
                string data = Client.DownloadString($"{url}getupdates?offset={offset}");
                var dataJson = JsonConvert.DeserializeObject<Root>(data);
                foreach (var item in dataJson.result)
                {
                    try
                    {
                        if (item.message.text == "Photo")
                        {
                            string srcPic = @"D:\1.jpg";
                            string urlPic = $"{url}sendPhoto?chat_id={item.message.chat.id}";
                            var client = new HttpClient();
                            using (var content = new MultipartFormDataContent())
                            {
                                content.Add(new StreamContent(new MemoryStream(File.ReadAllBytes(srcPic))), "photo", "1.jpg");
                                using (var messege = client.PostAsync(urlPic, content).Result)
                                {
                                    WriteLine(messege.Content.ReadAsStringAsync().Result);
                                    Task.Run(() =>
                                    {
                                        var messgeIDelete = item.message.message_id;
                                        Thread.Sleep(5000);
                                        client.DeleteAsync(urlPic);
                                    });
                                }
                            }
                        }
                        else
                            Client.DownloadString($"{url}sendmessage?chat_id={item.message.chat.id}&text={item.message.text}");
                        offset = item.update_id + 1;
                        WriteLine($"{item.message.date}\r\n");
                        WriteLine($"{item.message.message_id}\r\n____________________________");
                    }
                    catch (Exception)
                    {
                        Client.DownloadString($"{url}sendmessage?chat_id={item.message.chat.id}&text={item.message.chat.first_name}");
                        offset = item.update_id + 1;
                        WriteLine($"{item.message.date}\r\n");
                        WriteLine($"{item.message.message_id}\r\n____________________________");
                    }
                }
            }
        }
    }
}
