using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Terrasweeper.Common.BuilderToggles
{
    public class NumbersTransparencyBuilderToggle : BuilderToggle
    {
        public static LocalizedText Transparency0 { get; private set; }
        public static LocalizedText Transparency50 { get; private set; }
        public static LocalizedText Transparency100 { get; private set; }

        public override bool Active() => true; // change to false when on release build

        public override int NumberOfStates => 3;

        public override void SetStaticDefaults()
        {
            Transparency0 = this.GetLocalization(nameof(Transparency0));
            Transparency50 = this.GetLocalization(nameof(Transparency50));
            Transparency100 = this.GetLocalization(nameof(Transparency100));
        }

        public override string DisplayValue()
        {
            if (CurrentState == 0)
                return Transparency100.Value;
            else if (CurrentState == 1)
                return Transparency50.Value;
            else
                return Transparency0.Value;
        }

        public override bool OnLeftClick(ref SoundStyle? sound)
        {
            return base.OnLeftClick(ref sound);
        }

        public override void OnRightClick()
        {
            CurrentState -= 1;
            if (CurrentState < 0)
            {
                CurrentState = NumberOfStates - 1;
            }

            SoundEngine.PlaySound(SoundID.MenuTick);
        }

        public override bool Draw(SpriteBatch spriteBatch, ref BuilderToggleDrawParams drawParams)
        {
            drawParams.Frame = drawParams.Texture.Frame(1, 3, 0, CurrentState);
            return base.Draw(spriteBatch, ref drawParams);
        }
    }
}