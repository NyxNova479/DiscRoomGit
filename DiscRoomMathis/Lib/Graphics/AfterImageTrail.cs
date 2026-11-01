using System;
using System.Collections.Generic;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Lib
{
    /// <summary>
    /// Leaves fading copies of a spritesheet behind it (used for the dash)
    /// </summary>
    public class AfterImageTrail
    {
        private struct AfterImage
        {
            public Vector2 Position;
            public int FrameId;
            public bool FlipX;
            public float Timer;
        }

        private readonly Spritesheet _owner;
        private readonly List<AfterImage> _afterImages = new List<AfterImage>();

        public float Lifetime { get; set; } = 0.3f;

        public float MaxAlpha { get; set; } = 160f;

        public Color Tint { get; set; } = new Color(60, 160, 255, 255);

        public AfterImageTrail(Spritesheet owner)
        {
            _owner = owner;
        }

        /// <summary>
        /// Records the current look of the owner (position + animation frame)
        /// </summary>
        public void Add()
        {
            AfterImage afterImage = new AfterImage
            {
                Position = _owner.Position,
                FrameId = _owner.Animation.CurrentFrame,
                FlipX = _owner.FlipX,
                Timer = 0f
            };

            _afterImages.Add(afterImage);
        }

        public void Update(float dt)
        {
            for (int i = _afterImages.Count - 1; i >= 0; i--)
            {
                AfterImage afterImage = _afterImages[i];
                afterImage.Timer += dt;

                if (afterImage.Timer >= Lifetime)
                {
                    _afterImages.RemoveAt(i);
                    continue;
                }

                _afterImages[i] = afterImage;
            }
        }

        /// <summary>
        /// Must be called BEFORE the owner's Draw so the hero stays on top
        /// </summary>
        public void Draw()
        {
            int columns = _owner.Texture.Width / _owner.SizeX;
            if (columns <= 0) return;

            Vector2 origin = new Vector2(_owner.WidthScaled * _owner.OriginX, _owner.HeightScaled * _owner.OriginY);

            foreach (AfterImage afterImage in _afterImages)
            {
                float lifeRatio = 1f - afterImage.Timer / Lifetime;
                Color color = new Color(Tint.R, Tint.G, Tint.B, (int)(MaxAlpha * lifeRatio));

                Rectangle source = new Rectangle(
                    (afterImage.FrameId % columns) * _owner.SizeX,
                    (afterImage.FrameId / columns) * _owner.SizeY,
                    _owner.SizeX,
                    _owner.SizeY);

                if (afterImage.FlipX) source.Width *= -1;

                Rectangle destination = new Rectangle(afterImage.Position, _owner.WidthScaled, _owner.HeightScaled);

                DrawTexturePro(_owner.Texture, source, destination, origin, _owner.Angle, color);
            }
        }
    }
}
