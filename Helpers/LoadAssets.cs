using System.Collections.Generic;
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
        public static Asset<Texture2D> Minesweeper;

        static Ass()
        {
            foreach (var field in typeof(Ass).GetFields())
                if (field.FieldType == typeof(Asset<Texture2D>))
                    field.SetValue(null, ModContent.Request<Texture2D>($"JulyJam/Assets/{field.Name}"));
        }
        public static bool Initialized { get; set; }
    }


    public static class MinesweeperTextures
    {
        public static Rectangle GetRectangle(MinesweeperTexturesEnum mte) {
            return new Rectangle(18 * (int)mte, 0, 16, 16);
        }

    }
    public enum MinesweeperTexturesEnum
    {
        Mine = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Flag = 10,
    }
    /// <summary>
    /// System that automatically initializes assets
    /// </summary>
    public class LoadAssets : ModSystem
    {
        public override void Load() => _ = Ass.Initialized;
    }
}
