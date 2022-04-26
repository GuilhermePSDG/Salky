using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace Salky.Test
{
    public class FakeConfiguration : IConfiguration
    {
        private Dictionary<string, string> keyValues;

        public FakeConfiguration(Dictionary<string, string> keyValues)
        {
            this.keyValues = keyValues;
        }
        public string this[string key]
        {
            get => this.keyValues[key];
            set => this.keyValues[key] = value;
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            throw new NotImplementedException();
        }
    }
}
