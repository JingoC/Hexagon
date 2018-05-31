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
            string json = "[{\"Name\": \"defaultButton\", \"Type\": \"Texture2D\"},{\"Name\": \"defaultButtonPressed\", \"Type\": \"Texture2D\"},{\"Name\": \"defaultFont\", \"Type\": \"Font\"},{\"Name\": \"hexagon_blue\", \"Type\": \"Texture2D\"},{\"Name\": \"hexagon_green\", \"Type\": \"Texture2D\"},{\"Name\": \"hexagon_red\", \"Type\": \"Texture2D\"},{\"Name\": \"hexagon_yellow\", \"Type\": \"Texture2D\"},{\"Name\": \"hexagon_blue_checked\", \"Type\": \"Texture2D\"},{\"Name\": \"hexagon_gray\", \"Type\": \"Texture2D\"},{\"Name\": \"hexagon_white\", \"Type\": \"Texture2D\"},{\"Name\": \"user_button\", \"Type\": \"Texture2D\"}]";
            try
            {
                var json_list = JsonConvert.DeserializeObject<List<ResoureceInfo>>(json);
                foreach(var item in json_list)
                {
                    AddResource(item.Name, item.Type);
                }
                return true;
            }
            catch
            {
                return false;
            }
#endif
        }

        static public void AddResource(string name, TypeResource type)
        {
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
    }
}
