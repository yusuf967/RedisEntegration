using RedisEntegration.Interface;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisEntegration.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer redisConnection;
        private readonly IDatabase cache;
        public RedisCacheService(IConnectionMultiplexer redisConnection)
        {
            this.redisConnection = redisConnection;
            this.cache = redisConnection.GetDatabase();
        }

        public async Task Clear(string key)
        {
            await cache.KeyDeleteAsync(key);
        }

        public void ClearAll()
        {
           var redisEndPoints = redisConnection.GetEndPoints(true);

            foreach(var redisEndPoint in redisEndPoints)
            {
                var redisServer = redisConnection.GetServer(redisEndPoint);
                redisServer.FlushAllDatabases();
            }
        }

        public async Task<string> GetValueAsync(string key)
        {
            return await cache.StringGetAsync(key);
        }

        public async  Task<bool> SetValueAsync(string key, string value)
        {
            return await cache.StringSetAsync(key, value, TimeSpan.FromHours(1));
        }
    }
}
