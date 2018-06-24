using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.IsolatedStorage;

namespace WinSystem.System
{
    
    using Newtonsoft.Json;

    public class Storage
    {
        public static void WriteAllText(string filePath, string text)
        {
#if ANDROID
            using (var istrg = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null))
            {
                using (var fstream = istrg.OpenFile(filePath, FileMode.Create, FileAccess.Write))
                {
                    byte[] bytes = Encoding.Default.GetBytes(text.ToCharArray());
                    fstream.Write(bytes, 0, bytes.Count());
                }
            }
#else
            File.WriteAllText(filePath, text);
#endif
        }

        public static string ReadAllText(string filePath)
        {
#if ANDROID
            try
            {
                string content = String.Empty;
                using (var istrg = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Domain | IsolatedStorageScope.Assembly, null, null))
                {
                    using (var fstream = istrg.OpenFile(filePath, FileMode.Open, FileAccess.Read))
                    {
                        byte[] bytes = new byte[fstream.Length];
                        fstream.Read(bytes, 0, (int)fstream.Length);
                        content = Encoding.Default.GetString(bytes);
                    }
                }

                return content;
            }
            catch (FileNotFoundException e)
            {
                return String.Empty;
            }
#else
            bool isExists = File.Exists(fileSettingsPath);
            if (isExists)
                return File.ReadAllText(filePath);

            return String.Empty;
#endif
        }

        public static string Serialize<T>(T item)
        {
            return JsonConvert.SerializeObject(item);
        }

        public static T Deserialize<T>(string content)
        {
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
