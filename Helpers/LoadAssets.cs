using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ModLoader;

namespace JulyJam.Helpers
{
    public static class Ass
    {
        // Add assets here
        public static Asset<Texture2D> Mine;
        public static Asset<Texture2D> Flag;

        static Ass()
        {
            foreach (var field in typeof(Ass).GetFields())
                if (field.FieldType == typeof(Asset<Texture2D>))
                    field.SetValue(null, ModContent.Request<Texture2D>($"JulyJam/Assets/{field.Name}"));
        }
        public static bool Initialized { get; set; }
    }

    public class LoadAssets : ModSystem
    {
        public override void Load() => _ = Ass.Initialized;
    }
}
