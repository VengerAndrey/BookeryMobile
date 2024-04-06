using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace BookeryMobile.Data
{
    public class AppConfiguration
    {
        private static AppConfiguration? _instance;
        private readonly JObject _secrets;

        private const string Namespace = "BookeryMobile";
        private const string FileName = "appsettings.json";

        private AppConfiguration()
        {
            var resourceName = $"{Namespace}.{FileName}";
            try
            {
                var assembly = typeof(AppConfiguration).GetTypeInfo().Assembly;
                var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                {
                    throw new ArgumentNullException();
                }

                using var reader = new StreamReader(stream);
                var json = reader.ReadToEnd();
                _secrets = JObject.Parse(json);
            }
            catch (Exception)
            {
                Debug.WriteLine($"Unable to load configuration file {resourceName}.");
                _secrets = new JObject();
            }
        }

        public static AppConfiguration Instance => _instance ??= new AppConfiguration();

        public string this[string name]
        {
            get
            {
                try
                {
                    var path = name.Split(':');

                    var node = _secrets[path[0]];

                    for (var index = 1; index < path.Length; index++)
                    {
                        if (node == null)
                        {
                            throw new ArgumentNullException();
                        }

                        node = node[path[index]];
                    }

                    if (node == null)
                    {
                        throw new ArgumentNullException();
                    }

                    return node.ToString();
                }
                catch (Exception)
                {
                    Debug.WriteLine($"Unable to retrieve configuration value for key '{name}'.");
                    return string.Empty;
                }
            }
        }
    }
}