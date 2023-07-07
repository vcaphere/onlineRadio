
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System.Drawing;
using System.Net;
using System.Xml;
using YandexMusicApi;


namespace RimRadio.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public async Task GetRespond()
        {
                string MusicUrl = "";
            int rowCount = 0;
            try
            {
                FileInfo exFile = new FileInfo(@"ВАШЕ РАСПОЛОЖЕНИЕ XLSX ФАЙЛА ЗДЕСЬ");
                using (ExcelPackage package = new ExcelPackage(exFile))
                {
                    ExcelWorksheet sheet = package.Workbook.Worksheets[0];
                rowCount = sheet.Dimension.End.Row;

                MusicUrl = sheet.Cells[Const.WhichNowPlay, 1].Value.ToString();

                    /*string[] mUrls = MusicUrl.Split("?track-id=");
                    string GetTrackId = mUrls[1].Replace("&play=false","");
                    */

                    YandexMusicApi.Token.token = "ВАШ ТОКЕН ЯНДЕКС МУЗЫКИ"; //here is your token from yandex music
                    string getUrlTrack = Track.GetDownloadInfoWithToken(MusicUrl).ToString().Split("\"downloadInfoUrl\": \"")[1].Split("\"")[0];

                    string xmlStr;
                    using (var wc = new WebClient())
                    {
                        xmlStr = wc.DownloadString(getUrlTrack);
                    }
                    var xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlStr);


                    string getUrlAudio = "https://" + xmlDoc.InnerXml.ToString().Split(">")[3].Replace("</host", "") + "/get-mp3/" +
                xmlDoc.InnerXml.ToString().Split(">")[11].Replace("</s", "") + "/" +
                xmlDoc.InnerXml.ToString().Split(">")[7].Replace("</ts", "") + "/" +
                xmlDoc.InnerXml.ToString().Split(">")[5].Replace("</path", "");


                    string Title = Track.GetTrackSimilar(MusicUrl).ToString().Split("\"title\": \"")[1].Split("\"")[0];
                    string Artist = Track.GetTrackSimilar(MusicUrl).ToString().Split("\"name\": \"")[2].Split("\"")[0];

                    int durationMss = Convert.ToInt32(Track.GetTrackSimilar(MusicUrl).ToString().Split("\"durationMs\": ")[1].Split(",")[0]);

                    int minutes = Convert.ToInt32(Track.GetTrackSimilar(MusicUrl).ToString().Split("\"durationMs\": ")[1].Split(",")[0]) / 1000 / 60;
                    int seconds = Convert.ToInt32(Track.GetTrackSimilar(MusicUrl).ToString().Split("\"durationMs\": ")[1].Split(",")[0]) / 1000 - 60 * minutes;
                    string duration = minutes + ":" + seconds;
                    string GetTrackPicture = Track.GetTrackSimilar(MusicUrl).ToString();


                    var list = new Dictionary<int, int>();


                    System.Net.WebRequest request =
       System.Net.WebRequest.Create(
       "https://" + GetTrackPicture.Split("\"coverUri\": \"")[1].Split("\"")[0].Replace("%%", "50x50"));
                    System.Net.WebResponse response = request.GetResponse();
                    System.IO.Stream responseStream =
                        response.GetResponseStream();
                    Bitmap myImage = new Bitmap(responseStream);
                    using (var bitmap = myImage)
                    {
                        var colorsWithCount =
                            GetPixels(bitmap)
                                .GroupBy(color => color)
                                .Select(grp =>
                                    new
                                    {
                                        Color = grp.Key,
                                        Count = grp.Count()
                                    })
                                .OrderByDescending(x => x.Count)
                                .Take(7);

                        int t = 0;
                        string[] rgb = new string[7];
                        foreach (var colorWithCount in colorsWithCount)
                        {
                            /*Console.WriteLine("{0}",
                                colorWithCount.Color);*/
                            rgb[t] = colorWithCount.Color.ToString().Replace("Color [A=", " ").Replace(", R=", " ").Replace(", G=", " ").Replace(", B=", " ").Replace("]", "");
                            t++;
                        }
                        Const.RGB = rgb[0];
                        if (Convert.ToInt32(rgb[0].Split(" ")[2]) <= Convert.ToInt32(rgb[1].Split(" ")[2]) &&
                            Convert.ToInt32(rgb[0].Split(" ")[3]) <= Convert.ToInt32(rgb[1].Split(" ")[3]) &&
                            Convert.ToInt32(rgb[0].Split(" ")[4]) <= Convert.ToInt32(rgb[1].Split(" ")[4]))
                        {
                            Const.RGB = rgb[1];
                            if (Convert.ToInt32(rgb[1].Split(" ")[2]) <= Convert.ToInt32(rgb[2].Split(" ")[2]) &&
                            Convert.ToInt32(rgb[1].Split(" ")[3]) <= Convert.ToInt32(rgb[2].Split(" ")[3]) &&
                            Convert.ToInt32(rgb[1].Split(" ")[4]) <= Convert.ToInt32(rgb[2].Split(" ")[4]))
                            {
                                Const.RGB = rgb[2];
                                if (Convert.ToInt32(rgb[2].Split(" ")[2]) <= Convert.ToInt32(rgb[3].Split(" ")[2]) &&
                            Convert.ToInt32(rgb[2].Split(" ")[3]) <= Convert.ToInt32(rgb[3].Split(" ")[3]) &&
                            Convert.ToInt32(rgb[2].Split(" ")[4]) <= Convert.ToInt32(rgb[3].Split(" ")[4]))
                                {
                                    Const.RGB = rgb[3];
                                    if (Convert.ToInt32(rgb[3].Split(" ")[2]) <= Convert.ToInt32(rgb[4].Split(" ")[2]) &&
                            Convert.ToInt32(rgb[3].Split(" ")[3]) <= Convert.ToInt32(rgb[4].Split(" ")[3]) &&
                            Convert.ToInt32(rgb[3].Split(" ")[4]) <= Convert.ToInt32(rgb[4].Split(" ")[4]))
                                    {
                                        Const.RGB = rgb[4];
                                        if (Convert.ToInt32(rgb[4].Split(" ")[2]) <= Convert.ToInt32(rgb[5].Split(" ")[2]) &&
                            Convert.ToInt32(rgb[4].Split(" ")[3]) <= Convert.ToInt32(rgb[5].Split(" ")[3]) &&
                            Convert.ToInt32(rgb[4].Split(" ")[4]) <= Convert.ToInt32(rgb[5].Split(" ")[4]))
                                        {
                                            Const.RGB = rgb[5];
                                            if (Convert.ToInt32(rgb[5].Split(" ")[2]) <= Convert.ToInt32(rgb[6].Split(" ")[2]) &&
                            Convert.ToInt32(rgb[5].Split(" ")[3]) <= Convert.ToInt32(rgb[6].Split(" ")[3]) &&
                            Convert.ToInt32(rgb[5].Split(" ")[4]) <= Convert.ToInt32(rgb[6].Split(" ")[4]))
                                            {
                                                Const.RGB = rgb[6];
                                            }
                                        }
                                    }
                                }
                            }
                        }

                    }

                    static IEnumerable<Color> GetPixels(Bitmap bitmap)
                    {
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            for (int y = 0; y < bitmap.Height; y++)
                            {
                                Color pixel = bitmap.GetPixel(x, y);
                                yield return pixel;
                            }
                        }
                    }
                    



                    string CoverUrl = "https://" + GetTrackPicture.Split("\"coverUri\": \"")[1].Split("\"")[0].Replace("%%", "400x400") + "ОЙ" + getUrlAudio + "ОЙ" + Title + "ОЙ" + Artist + "ОЙ" + duration + "ОЙ" + durationMss + "ОЙ";

                    Const.Cover = CoverUrl;
                }
                int durationMs = Convert.ToInt32(Track.GetTrackSimilar(MusicUrl).ToString().Split("\"durationMs\": ")[1].Split(",")[0])/1000;

                Thread.Sleep(durationMs*1000);
                Const.WhichNowPlay++;
                Const.duration=0;


                await GetRespond();
            }
            catch (Exception ex)
            {
                Const.WhichNowPlay = 2;
                Const.duration = 0;
                await GetRespond();
            }


        }
        public async Task EverySec()
        {
            Thread.Sleep(1000);
            Const.duration++;
            await EverySec();
        }

        public JsonResult OnGetMusicAsync()
        {
            if (Const.isFirstStart == false)
            {
                Const.isFirstStart = true;
                GetRespond();
            }
            if(Const.isFirstStartSec == false)
            {
                Const.isFirstStartSec = true;
                EverySec();
            }
            string resp = Const.Cover + Const.duration + "ОЙ" + Const.RGB;
                return new JsonResult(resp);
            }

        }

    
}