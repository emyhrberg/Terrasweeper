using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terrasweeper.Common.BuilderToggles
{
    public class ShowMinesBuilderToggle : BuilderToggle
    {
        public override bool Active() => true;

        public override int NumberOfStates => 2;

        public override void SetStaticDefaults()
        {
        }

        public override string DisplayValue()
        {
            if (CurrentState == 0)
                return "Show Mines On";
            else
                return "Show Mines Off";
        }

        public override bool OnLeftClick(ref SoundStyle? sound)
        {
            // Toggle visibility between the states
            CurrentState = (CurrentState + 1) % NumberOfStates;

            sound = SoundID.MenuTick;
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