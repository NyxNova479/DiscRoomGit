using System;
using System.Collections.Generic;
using System.Text;

namespace Lib
{
    public static class Utils
    {

        private static Random _rand = new Random();

        public static void SetRandom(int seed)
        {
            _rand = new Random(seed);
        }

        /// <summary>
        /// Generates a random number between min  and max 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="excluded"></param>
        /// <returns></returns>
        public static int RandomInt(int min, int max, int[]? excluded = null)
        {

            if (min > max) (min, max) = (max, min);

            int randInt = _rand.Next(min, max + 1);

            if (excluded == null)
                return randInt;
            
            while(excluded.Contains(randInt))
            {
                randInt = _rand.Next(min,max + 1);
            }
            return randInt;
        }

        /// <summary>
        /// Generates a random float between min  and max 
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="excluded"></param>
        /// <returns></returns>
        public static float RandomFloat(float min, float max, float[]? excluded = null)
        {
            if (min > max) (min, max) = (max, min);



            float randFloat = (max-min) * (float)_rand.NextDouble() +min;


            if(excluded == null) return randFloat;

            while( excluded.Contains(randFloat))
            {
                randFloat = (max - min) * (float)_rand.NextDouble() + min;
            }

            return randFloat;
        }

    }
}
