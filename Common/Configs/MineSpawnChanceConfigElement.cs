using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader.UI;

namespace Terrasweeper.Common.Configs
{
    class MineSpawnChanceConfigElement : LockedSliderElement<float>
    {
        public static MineSpawnChanceToggleConfigElement element;

        public float Value { get; set; }

        // ctor
        public MineSpawnChanceConfigElement() : base()
        {
            Min = 0f;
            Max = 100f;
            Increment = 1f;
        }

        public override int NumberTicks => (int)((Max - Min) / Increment);

        public override float TickIncrement => Increment;

        public override float Proportion { get => (Value - Min) / (Max - Min); set => Value = MathHelper.Lerp(Min, Max, value); }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            backgroundColor = element.Value ? UICommon.DefaultUIBlue : Color.Gray;
            TextDisplayFunction = () => Language.GetTextValue("Mods.Terrasweeper.ConfigTest.T", GetValue());



            //TextDisplayFunction = () => $"Mine Spawn Chance: {GetValue()}%"; // NO, use Language instead

        }
    }
}