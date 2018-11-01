using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexturePackerSplit_json
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = null;

            if (args.Length <= 0)
            {
                path = Console.ReadLine();
            }
            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "-p":
                            if (args.Length - 1 < i + 1)
                            {
                                Console.WriteLine("参数不足");
                                return;
                            }
                            path = args[i + 1];
                            i++;
                            break;
                        default:
                            Console.WriteLine("命令:" + args[i] + "不存在");
                            break;
                    }
                }
            }

            

            if (path == null)
            {
                Console.WriteLine("参数不足");
                return;
            }

            TexturePackerSplit_json tool = new TexturePackerSplit_json();
            string error = "";
            if (tool.init(path, out error) == false) {
                Console.WriteLine("[TexturePackerSplit]"+ error);
                return;
            }

        }
    }
}
