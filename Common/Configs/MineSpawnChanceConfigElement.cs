using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader.Config.UI;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Terrasweeper.Common.Configs
{
    class MineSpawnChanceConfigElement : FloatElement
    {
        public static MineSpawnChanceToggleConfigElement element;
        private bool IsLocked => !element?.Value ?? true;

        // ctor
        public MineSpawnChanceConfigElement() : base()
        {
            Min = 0f;
            Max = 100f;
            Increment = 1f;
            TextDisplayFunction = () => Language.GetTextValue("Mods.Terrasweeper.ConfigTest.Text", GetValue());
        }

        public override float Proportion
        {
            get => base.Proportion;
            set
            {
                if (!IsLocked)
                    base.Proportion = value;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //TextDisplayFunction = () => Language.GetTextValue("Mods.Terrasweeper.ConfigTest.Text", GetValue());

            if (IsLocked)
            {
                backgroundColor = Color.Gray;
                TooltipFunction = () => "Locked. Turn on Custom Mine Spawn Chance to set this slider.";
            }
            else
            {
                backgroundColor = UICommon.DefaultUIBlue;
                TooltipFunction = () => Language.GetTextValue("Mods.Terrasweeper.ConfigTest.Tooltip");
            }
        }

        public override void DrawSelf(SpriteBatch sb)
        {
            if (IsLocked) return;

            base.DrawSelf(sb);
        }

        private void DrawLock(SpriteBatch sb)
        {
            // Draw lock
            CalculatedStyle dims = GetDimensions();
            Texture2D lockTex = Ass.Lock.Value;
            Vector2 lockPos = new Vector2(dims.X + dims.Width - lockTex.Width - 10f, dims.Y + (dims.Height - lockTex.Height) * 0.5f);
            sb.Draw(lockTex, lockPos, Color.White);
        }
    }
}