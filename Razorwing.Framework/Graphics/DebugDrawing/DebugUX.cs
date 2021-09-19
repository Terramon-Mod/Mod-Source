using Terraria.UI;

namespace Razorwing.Framework.Graphics.DebugDrawing
{
    public class DebugUX : UIState
    {
        protected DebugDrawings drawings = null;

        public UIElement DebugTarget
        {
            get => drawings.Target;
            set => drawings.Target = value;
        }

        public override void OnInitialize()
        {
            drawings = new DebugDrawings();
            Append(drawings);
        }
    }
}
