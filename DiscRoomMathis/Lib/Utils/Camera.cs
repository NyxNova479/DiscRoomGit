using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Lib
{
    public class Camera
    {

        private Camera2D _camera;

        public float Zoom { get; set; }

        public float Lerp { get; set; }

        public Vector2 Offset { get; set; }

        public Rectangle WorldBounds { get; set; }

        public Sprite? FollowingSprite { get; private set; }

        protected ShakeData ShakeData;

        private Vector2 _shakePosition;

        public Camera()
        {
            _camera = new Camera2D();
            WorldBounds = new Rectangle(0, 0, 0, 0);
            Zoom = 1f;
            Offset = Vector2.Zero;
            FollowingSprite = null;
            Lerp = 0f;

            ShakeData = new ShakeData();

            Anchor = Vector2.Zero;
            FollowStrength = 1f;

        }

        // Ajout
        public Vector2 Anchor { get; private set; }

        public float FollowStrength { get; set; }

        public void Update(float dt)
        {

            _camera.Zoom = Zoom;


            _camera.Offset = (FollowingSprite == null) ?Vector2.Zero + _shakePosition : new Vector2(GetScreenWidth() * 0.5f, GetScreenHeight() * 0.5f) + _shakePosition;

            if (FollowingSprite != null) UpdateFollow(dt);
            if (ShakeData.Duration > 0f) UpdateShake(dt);



        }

        public void Begin()
        {
            BeginMode2D(_camera);
        }

        public void End()
        {
            EndMode2D();
        }

        public void Follow(Sprite sprite)
        {
            FollowingSprite = sprite;
        }

        public void Shake(float min, float max, float duration)
        {
            ShakeData = new ShakeData(min, max, duration);
        }

        public void StopShake()
        {
            _shakePosition = Vector2.Zero;
            ShakeData = new ShakeData();
        }

        private void UpdateShake(float dt)
        {
            ShakeData.CurrentTimer += dt;

            _shakePosition = new Vector2(Utils.RandomInt((int)ShakeData.MinOffset.X,(int)ShakeData.MaxOffset.X), Utils.RandomInt((int)ShakeData.MinOffset.Y, (int)ShakeData.MaxOffset.Y));

            if (ShakeData.IsFinished) _shakePosition = Vector2.Zero;
        }

        private void UpdateFollow(float dt)
        {
            if (FollowingSprite == null) return;

            //Vector2 finalPos = FollowingSprite.Position + Offset;
            Vector2 targetPosition = FollowingSprite.Position + Offset;
            Vector2 finalPos = Anchor + (targetPosition - Anchor) * FollowStrength;

            if (WorldBounds.Width > 0f && WorldBounds.Height > 0f)
            {
                float halfViewWidth = GetScreenWidth() / (2f * _camera.Zoom);
                float halfViewHeight = GetScreenHeight() / (2f * _camera.Zoom);

                float minX = WorldBounds.X + halfViewWidth;
                float maxX = WorldBounds.X + WorldBounds.Width - halfViewWidth;

                float minY = WorldBounds.Y + halfViewHeight;
                float maxY = WorldBounds.Y + WorldBounds.Height - halfViewHeight;

                finalPos.X = Math.Clamp(finalPos.X, minX, maxX);
                finalPos.Y = Math.Clamp(finalPos.Y, minY, maxY);

            }

            if (Lerp <= 0f) _camera.Target = finalPos;

            else
            {
                float finalLerp = 1f - MathF.Exp(-Lerp * dt);

                _camera.Target = Vector2.Lerp(_camera.Target, finalPos, finalLerp);
            }



        }

        /// <summary>
        /// Defines the point the camera rests on and places the camera there immediately
        /// </summary>
        public void SetAnchor(Vector2 anchor)
        {
            Anchor = anchor;
            _camera.Target = anchor;
        }

    }
}
