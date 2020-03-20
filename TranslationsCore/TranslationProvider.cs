using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;

namespace TranslationsCore
{
    public class TranslationProvider
    {
        public string Error { get; set; }
        public string NoDevicesFoundRetry { get; set; }
        public string SearchForDevices { get; set; }
        public string Retry { get; set; }
        public string Close { get; set; }

        public static TranslationProvider LoadCulture(int langId)
        {
            TranslationProvider translationProvider = new TranslationProvider();

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(langId);

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "TranslationsCore.Resources.2057.json";

            string result;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            resourceName = $"TranslationsCore.Resources.{langId}.json";

            Dictionary<string, string> translations = new Dictionary<string, string>();
            Dictionary<string, string> translationsFallback = new Dictionary<string, string>();

            translationsFallback = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                    translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                }
            }

            foreach (var kvp in translations)
            {
                if (translationsFallback.ContainsKey(kvp.Key))
                {
                    translationsFallback[kvp.Key] = kvp.Value;
                }
            }

            foreach (PropertyInfo propertyInfo in translationProvider.GetType().GetProperties())
            {
                foreach (var kvp in translationsFallback)
                {
                    if (propertyInfo.Name == kvp.Key)
                    {
                        propertyInfo.SetValue(translationProvider, kvp.Value, null);
                    }
                }
            }

            return translationProvider;
        }
    }
}
