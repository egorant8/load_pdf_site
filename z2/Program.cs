using Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using MessagingToolkit.QRCode.Codec;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.Office.Core;
using System.Net;
using System.Reflection;
using System;
using Newtonsoft.Json;
using System.Text;

namespace z2
{
    class Program
    {
        const string crown_db = @"\\10.0.8.204\Crown2021\crownBD.mdb";
        static string connectString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + crown_db + ";";
        private static OleDbConnection myConnection;
        static string fullpath = Assembly.GetExecutingAssembly().Location.Replace("z2.exe", "") + @"\";
        static string url = "http://10.0.8.206/load/";
        static WebClient wb = new WebClient();
        static void Main(string[] args)
        {
            wb.Encoding = Encoding.UTF8;
            while (true)
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
                        myConnection = new OleDbConnection(connectString);
                        myConnection.Open();
                        string query = "SELECT * FROM Humans where UNIK=" + unik + " and fam='" + fam + "'";
                        string sq = "";
                        OleDbCommand command = new OleDbCommand(query, myConnection);
                        OleDbDataReader dataReader = command.ExecuteReader();
                        while (dataReader.Read())
                        {
                            Application excelapp = new Application();
                            excelapp.Visible = false;
                            _Workbook workbook = excelapp.Workbooks.Open(fullpath + @"results.xlsx");
                            _Worksheet worksheet = (_Worksheet)workbook.ActiveSheet;
                            string qrtext = "http://vgp1.ru/covid19/Valid_covid.php?unik=" + dataReader[18].ToString() + "&qr=" + dataReader[1].ToString();
                            QRCodeEncoder encoder = new QRCodeEncoder();
                            Bitmap qrcode = encoder.Encode(qrtext);
                            qrcode.Save(@"qrcode\s1.png", ImageFormat.Png);

                            ((Worksheet)excelapp.ActiveWorkbook.Sheets[1]).Select();
                            sq = dataReader[2].ToString();
                            worksheet.Cells[1, 2] = dataReader[2].ToString();
                            worksheet.Cells[2, 2] = dataReader[3].ToString();
                            worksheet.Cells[3, 2] = dataReader[4].ToString();
                            worksheet.Cells[4, 2] = dataReader[2].ToString() + " " + dataReader[3].ToString() + " " + dataReader[4].ToString();
                            worksheet.Cells[5, 2] = dataReader[5].ToString().Replace(" 0:00:00", "");
                            worksheet.Cells[6, 2] = dataReader[7].ToString();
                            worksheet.Cells[7, 2] = dataReader[8].ToString();
                            worksheet.Cells[9, 2] = dataReader[11].ToString().Replace(" 0:00:00", "");
                            worksheet.Cells[10, 2] = dataReader[12].ToString().Replace(" 0:00:00", "");
                            worksheet.Cells[8, 2] = dataReader[13].ToString();
                            worksheet.Cells[11, 2] = dataReader[9].ToString();
                            worksheet.Cells[12, 2] = dataReader[18].ToString();
                            worksheet.Cells[13, 2] = dataReader[26].ToString();
                            Console.WriteLine(dataReader[14]);
                            string query2 = "SELECT * FROM users where login = '" + dataReader[14].ToString() + "'";
                            OleDbCommand command1 = new OleDbCommand(query2, myConnection);
                            OleDbDataReader dataReaders = command1.ExecuteReader();
                            string podp = "";
                            while (dataReaders.Read())
                            {
                                worksheet.Cells[14, 2] = dataReaders[6].ToString();
                                worksheet.Cells[15, 2] = dataReaders[7].ToString();
                                podp = dataReaders[5].ToString();
                            }
                            ((Worksheet)excelapp.ActiveWorkbook.Sheets[3]).Select();
                            ((Worksheet)excelapp.ActiveWorkbook.Sheets[3]).Shapes.AddPicture(fullpath + @"qrcode\s1.png", MsoTriState.msoFalse, MsoTriState.msoCTrue, 15 * 11, 0, 5 * 13, 5 * 15);
                            Console.WriteLine(fullpath + @"signdoc\" + podp + ".png");
                            try
                            {
                                ((Worksheet)excelapp.ActiveWorkbook.Sheets[3]).Shapes.AddPicture(fullpath + @"signdoc\" + podp + ".png", MsoTriState.msoFalse, MsoTriState.msoCTrue, 15 * 11, 235, 5 * 13, 30);
                            }
                            catch (Exception ee)
                            {
                                Console.WriteLine(ee);
                            }

                            string filename = fullpath + @"pdf\" + fam + " " + unik + ".pdf";
                            ((Worksheet)excelapp.ActiveWorkbook.Sheets[3]).ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, filename);

                            workbook.Saved = false;
                            workbook.Close(false);
                            excelapp.Quit();

                            dataReaders.Close();
                            command1.Cancel();
                            wb.DownloadString(url + "del.php?id=" + id);
                            break;
                        }
                        dataReader.Close();
                        command.Cancel();
                        //Console.ReadKey();
                    }
                }
            }
        }
    }
}
