using Memcached.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OA.Common
{
    /// <summary>
    /// 封装缓存操作有关的类
    /// </summary>
    public class MemcachedHelper
    {
        private static readonly MemcachedClient mc = null;
        //静态构造函数，属于类只会执行一次
        static MemcachedHelper()
        {
            //最好放在配置文件中
          
            string[] serverlist = { "127.0.0.1:11211", "10.0.0.132:11211" };

            //初始化池
            SockIOPool pool = SockIOPool.GetInstance();
            pool.SetServers(serverlist);

            pool.InitConnections = 3;
            pool.MinConnections = 3;
            pool.MaxConnections = 5;

            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;

            pool.MaintenanceSleep = 30;
            pool.Failover = true;

            pool.Nagle = false;
            pool.Initialize();

            // 获得客户端实例
            mc = new MemcachedClient
            {
                EnableCompression = false
            };
        }
        /// <summary>
        /// 存储缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool SetMemcached(string key,object value)
        {
            return mc.Set(key, value);
        }

        /// <summary>
        /// 存储缓存及设置绝对过期时间
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool SetMemcached(string key, object value,DateTime time)
        {
            return mc.Set(key, value, time);
        }
        /// <summary>
        /// 取出缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetMemcached(string key)
        {
            return mc.Get(key);
        }
        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool DeleteMemcached(string key)
        {
            if(mc.KeyExists(key))
            {
                return mc.Delete(key);

            }
            else
            {
                return false;
            }
           
        }
    }
}
