using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Office.Core;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Reflection;
using System.Diagnostics;

namespace Загрузка_на_сайт
{
    class Program
    {
        static string fullpath = Assembly.GetExecutingAssembly().Location.Replace("Загрузка на сайт.exe","") + @"\";
        static WebClient wb = new WebClient();
        static string url = "http://vgp1.ru/covid19load/";
        static string urlClose_Server = "http://10.0.8.206/load/";
        //static string url = "http://localhost/load/";

        static void Main(string[] args)
        {
            
            wb.Encoding = Encoding.UTF8;
            while (true)
            {
                try
                {
                    string count_ = wb.DownloadString(url + "coun.php");

                    for (int i = 0; i < Convert.ToInt32(count_); i++)
                    {
                        string json = wb.DownloadString(url + "all.php?of=" + 0);
                        dynamic stuff = JsonConvert.DeserializeObject(json.Replace("[", "").Replace("]", ""));
                        Console.WriteLine(json);
                        if (json != "null")
                        {
                            string fam = stuff.fam;
                            string unik = stuff.unik;
                            string id = stuff.id;

                            string Filename = fam + "&unik=" + unik;
                            wb.DownloadString(urlClose_Server + "zapros.php?fam=" + Filename);
                            while (true)
                            {
                                try
                                {
                                    Filename = fam + " " + unik;
                                    Console.WriteLine(@"pdf\" + Filename + ".pdf");
                                    wb.Headers.Add("Content-Type", "binary/octet-stream");
                                    byte[] result = wb.UploadFile(url + "?fio=" + Filename, "POST", @"pdf\" + Filename + ".pdf");
                                    string s = Encoding.UTF8.GetString(result, 0, result.Length);
                                    Console.WriteLine(s);
                                    wb.DownloadString(url + "del.php?id=" + id);
                                    break;
                                }
                                catch (WebException ee)
                                {
                                    Console.WriteLine(ee.Message);
                                }
                                Thread.Sleep(1500);
                            }
                            File.Delete(@"pdf\" + Filename + ".pdf");
                        }


                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee);
                }
            }
            Console.ReadKey();
        }
    }
}
