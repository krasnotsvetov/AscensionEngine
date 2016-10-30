using System;

namespace AscensionEditor
{
#if WINDOWS
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var form = new EditorForm())
            {
                using (var game = new GameEditor(form, form.drawingSurface))
                {
                    form.Show();
                    game.Run();
                }
            }
        }
    }
#endif
}
