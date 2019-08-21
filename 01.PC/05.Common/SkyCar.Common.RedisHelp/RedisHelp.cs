using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.Redis;


namespace SkyCar.Common.RedisHelp
{
    public class RedisHelp
    {
        private static Dictionary<string, PooledRedisClientManager> dicPooledRedisClientManager =
            new Dictionary<string, PooledRedisClientManager>();

        /// <summary>
        /// 静态构造方法，初始化链接池管理对象
        /// </summary>
        static RedisHelp()
        {
        }
        /// <summary>
        /// 创建链接池管理对象
        /// </summary>
        private static void CreateManager(string paramKey)
        {
            //定义redis配置信息对象
            RedisConfigInfo redisConfigInfo = RedisConfigInfo.GetConfig();
            string[] writeServerList = {"192.168.2.80:9000"}; //SplitString(redisConfigInfo.WriteServerList, ",");
            string[] readServerList = {"192.168.2.80:9000"};//SplitString(redisConfigInfo.ReadServerList, ",");

            dicPooledRedisClientManager.Add(paramKey, new PooledRedisClientManager(readServerList, writeServerList,
                new RedisClientManagerConfig
                {
                    //写缓存大小
                    MaxWritePoolSize =60,// redisConfigInfo.MaxWritePoolSize,
                    //读缓存大小
                    MaxReadPoolSize =60,// redisConfigInfo.MaxReadPoolSize,
                    //自动启动
                    AutoStart =true,// redisConfigInfo.AutoStart,
                    //默认DB
                    DefaultDb = 0
                }));
        }

        private static string[] SplitString(string strSource, string split)
        {
            return strSource.Split(split.ToArray());
        }

        /// <summary>
        /// 客户端缓存操作对象
        /// </summary>
        public static IRedisClient GetClient(string paramRedisSystemKey)
        {
            if (!dicPooledRedisClientManager.ContainsKey(paramRedisSystemKey))
                CreateManager(paramRedisSystemKey);

            return dicPooledRedisClientManager[paramRedisSystemKey].GetClient();
        }
    }
}
