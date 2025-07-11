using System.Reflection;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace JulyJam.Helpers
{
    /// <summary>
    /// Static class to hold all assets used in the mod.
    /// Add a public static Asset<Texture2D> field for each asset you want to load.
    /// Then use them like Ass.EditorIcon.Value in your code.
    /// They should all be initialized automatically.
    /// </summary>
    public static class Ass
    {
        // Add assets here
        // public static Asset<Texture2D> EditorIcon;
        public static Asset<Texture2D> Mine;
        // This bool automatically initializes all specified assets
        public static bool Initialized { get; set; }

        static Ass()
        {
            foreach (FieldInfo field in typeof(Ass).GetFields())
            {
                if (field.FieldType == typeof(Asset<Texture2D>))
                {
                    string modName = "JulyJam";
                    string path = field.Name;
                    var asset = ModContent.Request<Texture2D>($"{modName}/Assets/{path}", AssetRequestMode.AsyncLoad);
                    field.SetValue(null, asset);
                }
            }
        }
    }

    /// <summary>
    /// System that automatically initializes assets
    /// </summary>
    public class LoadAssets : ModSystem
    {
        public override void Load()
        {
            _ = Ass.Initialized;
        }
    }
}