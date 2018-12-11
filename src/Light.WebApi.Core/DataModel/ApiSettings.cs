﻿using System;
namespace Light.WebApi.Core
{
    public class ApiSettings
    {
        public string CacheType { get; set; }

        public string RedisConfig { get; set; }

        public string TokenKey { get; set; }

        public int? CacheExpiry { get; set; }
    }
}
