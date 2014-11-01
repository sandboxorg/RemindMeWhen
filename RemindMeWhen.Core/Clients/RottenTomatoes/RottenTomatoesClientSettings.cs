﻿using System;
using System.Collections.Generic;

namespace Knapcode.RemindMeWhen.Core.Clients.RottenTomatoes
{
    public class RottenTomatoesClientSettings
    {
        public RottenTomatoesClientSettings()
        {
            RateLimits = new[]
            {
                new RateLimit {Count = 5, Period = TimeSpan.FromSeconds(1)},
                new RateLimit {Count = 10000, Period = TimeSpan.FromDays(1)},
            };
        }

        public string Key { get; set; }
        public IEnumerable<RateLimit> RateLimits { get; set; }
    }
}