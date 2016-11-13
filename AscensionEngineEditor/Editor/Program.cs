using Ascension;
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
                using (var game = new GameEx(form.constructEditor, form.startEditor, form.loadEditor, form.updateEditor, form.drawEditor))
                {
                    form.Show();
                    game.Run();
                }
            }
        }
    }
#endif
}
