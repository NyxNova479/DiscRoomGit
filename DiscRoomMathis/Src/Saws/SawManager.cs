using System;
using System.Collections.Generic;
using System.Numerics;
using DiscRoom;
using Lib;
using Raylib_cs;

namespace DiscRoom
{
    public class SawManager
    {
        private const float START_SPAWN_INTERVAL = 3f;
        private const float MIN_SPAWN_INTERVAL = 0.8f;
        private const float SPAWN_INTERVAL_DECREASE = 0.1f;
        private const int MAX_SAWS = 25;

        private const float MIN_DIAMETER = 40f;
        private const float MAX_DIAMETER = 120f;
        private const float MIN_SPEED = 100f;
        private const float MAX_SPEED = 300f;

        private readonly List<Saw> _saws = new List<Saw>();
        private readonly Rectangle _arena;

        private float _spawnTimer;
        private float _spawnInterval;

        public int SawCount => _saws.Count;

        public SawManager(Rectangle arena)
        {
            _arena = arena;
            Reset();
        }

        public void Update(float dt)
        {
            UpdateSpawn(dt);

            foreach (Saw saw in _saws)
            {
                saw.Update(dt);
            }
        }

        public void Draw()
        {
            foreach (Saw saw in _saws)
            {
                saw.Draw();
            }
        }

        /// <summary>
        /// True if at least one deadly saw touches the given hitbox
        /// </summary>
        public bool IsHeroHit(Rectangle heroHitbox)
        {
            foreach (Saw saw in _saws)
            {
                if (saw.CollidesWith(heroHitbox)) return true;
            }

            return false;
        }

        /// <summary>
        /// Removes every saw and restarts the difficulty (the first saw spawns immediately)
        /// </summary>
        public void Reset()
        {
            foreach (Saw saw in _saws)
            {
                saw.Unload();
            }

            _saws.Clear();
            _spawnInterval = START_SPAWN_INTERVAL;
            _spawnTimer = _spawnInterval;
        }

        /// <summary>
        /// Spawns a saw at a regular rhythm, a bit faster each time
        /// </summary>
        private void UpdateSpawn(float dt)
        {
            _spawnTimer += dt;

            if (_spawnTimer < _spawnInterval) return;

            _spawnTimer = 0f;
            _spawnInterval = MathF.Max(MIN_SPAWN_INTERVAL, _spawnInterval - SPAWN_INTERVAL_DECREASE);

            if (_saws.Count >= MAX_SAWS) return;

            SpawnSaw();
        }

        private void SpawnSaw()
        {
            float diameter = Utils.RandomFloat(MIN_DIAMETER, MAX_DIAMETER);
            float speed = Utils.RandomFloat(MIN_SPEED, MAX_SPEED);
            float directionAngle = Utils.RandomFloat(0f, MathF.Tau);

            // The margin keeps the whole saw inside the arena
            float margin = diameter * 0.5f;
            Vector2 position = new Vector2(
                Utils.RandomFloat(_arena.X + margin, _arena.X + _arena.Width - margin),
                Utils.RandomFloat(_arena.Y + margin, _arena.Y + _arena.Height - margin));

            _saws.Add(new Saw(position, diameter, speed, directionAngle, _arena));
        }
    }
}
