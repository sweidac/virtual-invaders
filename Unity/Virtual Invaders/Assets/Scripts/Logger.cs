using System.Diagnostics;
using System.IO;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts
{
    /// <summary>
    /// Bietet Methoden zum einheitlichen Logging
    /// </summary>
    public class Logger
    {
        private const string FILENAME = "logger.txt";

        private const string LEVEL_DEBUG = "DEBUG";
        private const string COLOR_DEBUG = "#000000ff";

        private const string LEVEL_INFO = "INFO";
        private const string COLOR_INFO = "#008000ff";

        private const string LEVEL_WARN = "WARN";
        private const string COLOR_WARN = "#ff9000ff";

        private const string LEVEL_FEHLER = "FEHLER";
        private const string COLOR_FEHLER = "#ff0000ff";

        private static StreamWriter outputStream = null;

        /// <summary>
        /// Öffnet die Log-Datei.
        /// </summary>
        public static void OpenFile()
        {
			// TODO: Fix fuer Smartphones analog zum Highscore
            //CloseFile();
            //outputStream = new StreamWriter(FILENAME, true);
        }

        /// <summary>
        /// Schließt die Log-Datei.
        /// </summary>
        public static void CloseFile()
        {
			// TODO: Fix fuer Smartphones analog zum Highscore
            //if (outputStream != null)
            //{
            //    outputStream.Close();
            //    outputStream = null;
            //}
        }

        /// <summary>
        /// Loggt eine Meldung mit Level DEBUG.
        /// </summary>
        public static void D(string text)
        {
            Log(LEVEL_DEBUG, COLOR_DEBUG, text);
        }

        /// <summary>
        /// Loggt eine Meldung mit Level INFO.
        /// </summary>
        public static void I(string text)
        {
            Log(LEVEL_INFO, COLOR_INFO, text);
        }

        /// <summary>
        /// Loggt eine Meldung mit Level WARN.
        /// </summary>
        public static void W(string text)
        {
            Log(LEVEL_WARN, COLOR_WARN, text);
        }

        /// <summary>
        /// Loggt eine Meldung mit Level FEHLER.
        /// </summary>
        public static void F(string text)
        {
            Log(LEVEL_FEHLER, COLOR_FEHLER, text);
        }

        private static void Log(string level, string color, string text)
        {
            StackFrame frame = new StackTrace().GetFrame(2);
            string header = level + "\t" + System.DateTime.Now + "\t" + frame.GetMethod().Name + ": ";

            // Ausgabe in Konsole
            Debug.Log("<color=" + color + "><b>" + header + "</b></color>" + text);

            // Ausgabe in Datei
            if (outputStream != null)
            {
                outputStream.WriteLine(header + text);
                outputStream.Flush();
            }
        }
    }
}
