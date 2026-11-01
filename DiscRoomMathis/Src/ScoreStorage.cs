using System;
using System.Globalization;
using System.IO;

namespace DiscRoom
{
    public static class ScoreStorage
    {
        private const string FILE_PATH = "bestscore.txt";

        /// <summary>
        /// Returns the saved best score (0 if there is none or if the file is unreadable)
        /// </summary>
        public static float LoadBestScore()
        {
            try
            {
                if (!File.Exists(FILE_PATH)) return 0f;

                string content = File.ReadAllText(FILE_PATH);

                bool isParsed = float.TryParse(content, NumberStyles.Float, CultureInfo.InvariantCulture, out float bestScore);

                return isParsed ? bestScore : 0f;
            }
            catch (Exception exception) when (exception is IOException || exception is UnauthorizedAccessException)
            {
                return 0f;
            }
        }

        /// <summary>
        /// Saves the score only if it beats the current best. Returns true if it's a new record
        /// </summary>
        public static bool TrySaveBestScore(float score)
        {
            if (score <= LoadBestScore()) return false;

            try
            {
                File.WriteAllText(FILE_PATH, score.ToString(CultureInfo.InvariantCulture));
            }
            catch (Exception exception) when (exception is IOException || exception is UnauthorizedAccessException)
            {
                return false;
            }

            return true;
        }
    }
}