using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Ascension.Engine.Editor
{
    public class EditorMouseState
    {
        public ButtonState LeftButton = ButtonState.Released;
        public ButtonState RightButton = ButtonState.Released;
        public ButtonState MiddleButton = ButtonState.Released;
        public int ScrollValue = 0;
        public Vector2 Position = Vector2.Zero;
        public bool onControl = false;

        private System.Windows.Forms.Control control;
        private bool releaseLeftButton = false;

        public EditorMouseState(System.Windows.Forms.Control control)
        {
            this.control = control;

            control.MouseWheel += (s, e) => { ScrollValue += e.Delta; };

            control.MouseClick += (s, e) =>
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    this.LeftButton = ButtonState.Pressed;
                }

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    this.RightButton = ButtonState.Pressed;
                }

                if (e.Button == System.Windows.Forms.MouseButtons.Middle)
                {
                    this.MiddleButton = ButtonState.Pressed;
                }
            };

 

            control.MouseEnter += (s, e) => { onControl = true; };
           
            control.MouseLeave += (s, e) => { onControl = false; };
        }

        public void Update()
        {


            if (onControl)
            {
                System.Drawing.Point p = control.PointToClient(System.Windows.Forms.Control.MousePosition);
                Position = new Vector2(p.X, p.Y);
                var buttons = System.Windows.Forms.Control.MouseButtons;
                LeftButton = (buttons & System.Windows.Forms.MouseButtons.Left) == System.Windows.Forms.MouseButtons.Left ? ButtonState.Pressed : ButtonState.Released;
                MiddleButton = (buttons & System.Windows.Forms.MouseButtons.Middle) == System.Windows.Forms.MouseButtons.Middle ? ButtonState.Pressed : ButtonState.Released;
                RightButton = (buttons & System.Windows.Forms.MouseButtons.Right) == System.Windows.Forms.MouseButtons.Right ? ButtonState.Pressed : ButtonState.Released;
            } else
            {
                LeftButton = RightButton = MiddleButton = ButtonState.Released;
            }
           
        }
    } 
}