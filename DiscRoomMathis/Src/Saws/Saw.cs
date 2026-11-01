using System;
using System.Numerics;
using Lib;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace DiscRoom
{
    public class Saw : Sprite
    {
        private const string ASSET_PATH = "assets/images/saw.png";

        private const float SPAWN_DURATION = 1.5f;
        private const float MIN_SPAWN_ALPHA = 80f;
        private const float ROTATION_SPEED = 720f;
        private const float HITBOX_RATIO = 0.8f;
        private const float WARNING_BLINK_SPEED = 12f;

        private readonly Rectangle _arena;
        private readonly float _targetScale;
        private readonly Vector2 _activeVelocity;

        private float _spawnTimer = 0f;

        /// <summary>
        /// True while the saw is appearing : it can't kill anybody yet
        /// </summary>
        public bool IsSpawning => _spawnTimer < SPAWN_DURATION;

        public bool IsDeadly => !IsSpawning;

        /// <summary>
        /// Radius used for the collisions (a bit smaller than the visual one)
        /// </summary>
        public float HitRadius => WidthScaled * 0.5f * HITBOX_RATIO;

        /// <param name="position">Center of the saw</param>
        /// <param name="diameter">Final diameter in pixels</param>
        /// <param name="speed">Speed once the saw is active</param>
        /// <param name="directionAngle">Direction in radians</param>
        /// <param name="arena">Rectangle in which the saw bounces</param>
        public Saw(Vector2 position, float diameter, float speed, float directionAngle, Rectangle arena) : base(ASSET_PATH)
        {
            _arena = arena;
            _targetScale = diameter / Texture.Width;
            _activeVelocity = new Vector2(MathF.Cos(directionAngle), MathF.Sin(directionAngle)) * speed;

            Position = position;
            Origin = new Vector2(0.5f);
            Scale = Vector2.Zero;
            Alpha = MIN_SPAWN_ALPHA;
        }

        public override void Update(float dt)
        {
            if (IsSpawning) UpdateSpawn(dt);

            Angle += ROTATION_SPEED * dt;

            base.Update(dt);

            BounceOnArena();
        }

        public override void Draw()
        {
            if (IsSpawning) DrawSpawnWarning();

            base.Draw();
        }

        /// <summary>
        /// Circle (saw) versus rectangle (hero hitbox). A saw that is spawning never collides
        /// </summary>
        public bool CollidesWith(Rectangle rectangle)
        {
            if (!IsDeadly) return false;

            return CheckCollisionCircleRec(Position, HitRadius, rectangle);
        }

        /// <summary>
        /// Grows and fades in the saw until it becomes deadly
        /// </summary>
        private void UpdateSpawn(float dt)
        {
            _spawnTimer += dt;

            if (!IsSpawning)
            {
                Activate();
                return;
            }

            float progress = _spawnTimer / SPAWN_DURATION;

            Scale = new Vector2(_targetScale * progress);
            Alpha = MIN_SPAWN_ALPHA + (255f - MIN_SPAWN_ALPHA) * progress;
        }

        private void Activate()
        {
            Scale = new Vector2(_targetScale);
            Alpha = 255f;
            Velocity = _activeVelocity;
        }

        /// <summary>
        /// Keeps the saw inside the arena and reflects its velocity on the edges
        /// </summary>
        private void BounceOnArena()
        {
            float radius = WidthScaled * 0.5f;

            float left = _arena.X + radius;
            float right = _arena.X + _arena.Width - radius;
            float top = _arena.Y + radius;
            float bottom = _arena.Y + _arena.Height - radius;

            if (X < left)
            {
                X = left;
                VelocityX = MathF.Abs(VelocityX);
            }
            else if (X > right)
            {
                X = right;
                VelocityX = -MathF.Abs(VelocityX);
            }

            if (Y < top)
            {
                Y = top;
                VelocityY = MathF.Abs(VelocityY);
            }
            else if (Y > bottom)
            {
                Y = bottom;
                VelocityY = -MathF.Abs(VelocityY);
            }
        }

        /// <summary>
        /// Blinking red circle showing where (and how big) the saw will be
        /// </summary>
        private void DrawSpawnWarning()
        {
            float targetRadius = Texture.Width * _targetScale * 0.5f;
            float blink = (MathF.Sin(_spawnTimer * WARNING_BLINK_SPEED) + 1f) * 0.5f;

            Color fillColor = new Color(255, 60, 60, 100);
            Color lineColor = new Color(255, 60, 60, (int)(80 + 150 * blink));

            DrawCircleV(Position, targetRadius, fillColor);
            DrawCircleLines((int)X, (int)Y, targetRadius, lineColor);
        }
    }
}
