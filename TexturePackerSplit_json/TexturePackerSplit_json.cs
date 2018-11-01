using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexturePackerSplit_json
{
    public class TexturePackerSplit_json
    {
        private string jsonPath;
        private string pngPath;
        private string savePath;
        private Bitmap png;

        public TexturePackerSplit_json()
        {
            
        }

        public bool init(string path,out string err) {
            err = "";
            jsonPath = path;
            
            
            //判断文件是否存在
            if (File.Exists(path) == false)
            {
                err = "文件不存在";
                return false;
            }

            //判断文件是否为json
            if (Path.GetExtension(path) != ".json")
            {
                err = "图集数据必须为json";
                return false;
            }

            //判断Png是否存在
            pngPath = Path.ChangeExtension(path, "png");
            if(File.Exists(pngPath)==false)
            {
                err = "同名png不存在";
                return false;
            }

            //加载png
            png = new Bitmap(pngPath);
            if (png == null)
            {
                err = "创建png失败";
                return false;
            }

            //创建目录
            savePath = Path.GetDirectoryName(path)+"/"+Path.GetFileNameWithoutExtension(path);
            if (Directory.Exists(savePath)==false)
            {
                Directory.CreateDirectory(savePath);
            }

            //解析json
            StreamReader sr = new StreamReader(jsonPath, Encoding.Default);
            string content;
            string str_json="";
            while ((content = sr.ReadLine()) != null)
            {
                str_json += content.ToString()+"\n";
            }

            Console.WriteLine("[TexturePackerSplit]生成目录->" + savePath);
            //遍历子图像数据并切图
            JObject jObject = (JObject)JsonConvert.DeserializeObject(str_json);
            foreach (JProperty v in jObject["frames"])
            {
                //获取子图像数据
                int x = Convert.ToInt32(v.Value["frame"]["x"]);
                int y = Convert.ToInt32(v.Value["frame"]["y"]);
                int w = Convert.ToInt32(v.Value["frame"]["w"]);
                int h = Convert.ToInt32(v.Value["frame"]["h"]);
                Rectangle range = new Rectangle(x,y,w,h);

                //克隆子图像
                Bitmap child = img_tailor(png, range);
                string saveFullName = savePath + "/" + v.Name;
                img2file(child, saveFullName);

                Console.WriteLine("[TexturePackerSplit]生成图像->" + v.Name);
            }


            return true;
        }

        public static Bitmap img_tailor(Bitmap src, Rectangle range)
        {
             return src.Clone(range, System.Drawing.Imaging.PixelFormat.DontCare);
        }
        
        //图片生成
        public static void img2file(Bitmap b, string filepath)
        {
            b.Save(filepath);
        }
    }
}
