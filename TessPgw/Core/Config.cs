using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace TessPgw.Core
{
    public class Config
    {
        private static Config _instance;
        private readonly Dictionary<string, string> _values;

        private Config()
        {
            _values = new Dictionary<string, string>
            {
                { "MERCHANT_ID", ConfigurationManager.AppSettings["MERCHANT_ID"] },
                { "MERCHANT_PASSWORD", ConfigurationManager.AppSettings["MERCHANT_PASSWORD"] },
                { "CONFIG_TIMEOUT", ConfigurationManager.AppSettings["CONFIG_TIMEOUT"] ?? "30" }
            };
        }

        public static Config Instance => _instance ??= new Config();

        public string Get(string key)
        {
            return _values.ContainsKey(key) ? _values[key] : null;
        }

        public static string GetBaseUrl(string integrationType)
        {
            string key = integrationType.ToUpper() + "_URL";
            string value = ConfigurationManager.AppSettings[key];

            if (string.IsNullOrWhiteSpace(value))
                throw new Exception($"Base URL not configured for {integrationType}");

            return value;
        }
    }
}
