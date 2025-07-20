#if DEBUG
using Microsoft.Xna.Framework.Input;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terrasweeper.Common.Configs;

namespace Terrasweeper.Common.Keybinds
{
    /// <summary>
    /// DEBUG ONLY Keybind System for quick navigation and configuration.
    /// </summary>
    public class KeybindSystem : ModSystem
    {
        public static ModKeybind OpenConfigKeybind { get; private set; }

        public override void Load()
        {
            // Register keybinds here
            OpenConfigKeybind = KeybindLoader.RegisterKeybind(
                mod: ModContent.GetInstance<Terrasweeper>(),
                name: "OpenConfig",
                defaultBinding: Keys.Y
            );
        }

        public override void Unload()
        {
            // Not required if your AssemblyLoadContext is unloading properly, but nulling out static fields can help you figure out what's keeping it loaded.
            OpenConfigKeybind = null;
        }
    }

    public class KeybindPlayer : ModPlayer
    {
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (KeybindSystem.OpenConfigKeybind.JustPressed)
            {
                Config.C.Open();
            }
        }
    }
}
#endif