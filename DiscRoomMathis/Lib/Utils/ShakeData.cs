using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Lib
{
    public struct ShakeData
    {

        public Vector2 MinOffset {  get; private set; }

        public Vector2 MaxOffset { get; private set;  }

        public float Duration {  get; private set; }

        public float CurrentTimer { get; set; }

        public bool IsFinished => CurrentTimer > Duration;

        public ShakeData()
        {
            MinOffset = Vector2.Zero;
            MaxOffset = Vector2.Zero;
            Duration = 0f;
            CurrentTimer = 0f;
        }

        public ShakeData(Vector2 min, Vector2 max, float duration)
        {
            MinOffset = min;
            MaxOffset = max;
            Duration = duration;
            CurrentTimer = 0f;
        }

        public ShakeData(float min, float max, float duration)
        {
            MinOffset = new Vector2(min);
            MaxOffset = new Vector2(max);
            Duration = duration;
            CurrentTimer = 0f;
        }

    }
}
