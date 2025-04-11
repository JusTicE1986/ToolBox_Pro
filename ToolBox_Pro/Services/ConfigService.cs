using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ToolBox_Pro.Models;

namespace ToolBox_Pro.Services
{
    public class ConfigService
    {
        private readonly string _configPath;

        public ConfigData Config { get; private set; }

        public ConfigService()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            _configPath = Path.Combine(basePath, "Config","config.json");
        }

        public void Load()
        {
            if (!File.Exists(_configPath))
            {
                Config = new ConfigData(); // leeres Objekt statt null
                return;
            }

            var json = File.ReadAllText(_configPath);

            try
            {
                Config = JsonSerializer.Deserialize<ConfigData>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new ConfigData();

            }
            catch (Exception ex)
            {
                Config = new ConfigData();
            }
        }
    }
}
