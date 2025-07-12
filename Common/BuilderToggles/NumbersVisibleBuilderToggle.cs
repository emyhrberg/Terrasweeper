using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace JulyJam.Common.BuilderToggles
{
    public static class NumbersVisibleState
    {
        public static bool Visible { get; set; } = true;
    }

    public class NumbersVisibleBuilderToggle : BuilderToggle
    {
        public static LocalizedText OnText { get; private set; }
        public static LocalizedText OffText { get; private set; }

        public override bool Active() => true; // change to false when on release build

        public override int NumberOfStates => 2;

        public override void SetStaticDefaults()
        {
            OnText = this.GetLocalization(nameof(OnText));
            OffText = this.GetLocalization(nameof(OffText));
        }

        public override string DisplayValue()
        {
            if (CurrentState == 0)
                return OnText.Value;
            else
                return OffText.Value;
        }

        public override bool OnLeftClick(ref SoundStyle? sound)
        {
            // Toggle the state
            NumbersVisibleState.Visible = !NumbersVisibleState.Visible;

            sound = NumbersVisibleState.Visible ? SoundID.MenuClose : SoundID.MenuClose;
            return true; // Returning true will actually toggle the state.
                         // * Returning false will not toggle the state, but will still play the sound. */
        }

        public override bool Draw(SpriteBatch spriteBatch, ref BuilderToggleDrawParams drawParams)
        {
            drawParams.Frame = drawParams.Texture.Frame(1, 2, 0, CurrentState);
            return base.Draw(spriteBatch, ref drawParams);
        }
    }
}