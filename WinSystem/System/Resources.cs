using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WinSystem.System
{
    using Microsoft.Xna.Framework.Graphics;
    using Newtonsoft.Json;
    
    public enum TypeResource
    {
        Texture2D = 1,
        Font = 2
    }
    
    static public class Resources
    {
        class ResoureceInfo
        {
            public string Name { get; set; }
            public TypeResource Type { get; set; }
        }

        static Dictionary<string, Object> resources { get; set; } = new Dictionary<string, object>();
        static string fileResource = "Resources.json";

        static public bool LoadResource()
        {
#if !ANDROID
            if (!File.Exists(fileResource))
            {
                File.Create(fileResource);
                return true;
            }
            
            try
            {
                JsonConvert.DeserializeObject<List<ResoureceInfo>>(File.ReadAllText(Resources.fileResource)).ForEach((x) => AddResource(x.Name, x.Type));
                return true;
            }
            catch
            {
                return false;
            }
#else
            // { "Name": "defaultButton", "Type": "Texture2D"},
            string json = "[" +
                "{\"Name\": \"defaultButton\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"defaultButtonPressed\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"defaultToggleOn\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"defaultToggleOff\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"defaultChangerDown\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"defaultChangerUp\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_green\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_red\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_yellow\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_brown\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_purple\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_orange\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_limegreen\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_blue\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_aqua\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_aqua_checked\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_gray\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"hexagon_white\", \"Type\": \"Texture2D\"}," +
                "{\"Name\": \"defaultFont\", \"Type\": \"Font\"}," +
                "]";

            var json_list = JsonConvert.DeserializeObject<List<ResoureceInfo>>(json);
            foreach (var item in json_list)
            {
                Resources.AddResource(item.Name, item.Type);
            }
            return true;
#endif
        }

        static public void AddResource(string name, TypeResource type)
        {
            var graphics = GraphicsSingleton.GetInstance();
            var content = GraphicsSingleton.GetInstance().Content;

            switch (type)
            {
                case TypeResource.Texture2D: resources.Add(name, content.Load<Texture2D>(name)); break;
                case TypeResource.Font: resources.Add(name, content.Load<SpriteFont>(name)); break;
                default: throw new Exception("Undefined resource type");
            }
        }

        public static Object GetResource(string name)
        {
            return resources.Keys.Any((x)=>x.Equals(name)) ? resources[name] : null;
        }
        
        public static List<Object> GetResources(List<string> names)
        {
            List<Object> getResources = new List<object>();

            foreach(var item in names)
            {
                if (resources.Any((x) => x.Key.Equals(item)))
                {
                    getResources.Add(resources.FirstOrDefault((x) => x.Key.Equals(item)).Value);
                }
            }

            return getResources;
        }
    }
}
