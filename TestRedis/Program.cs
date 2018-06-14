using OA.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestRedis
{
    class Program
    {
        static void Main(string[] args)
        {
            RedisHelper.StringSet(CacheFolderEnum.Folder1, "string", "hello world", 60);
        }
    }
}
