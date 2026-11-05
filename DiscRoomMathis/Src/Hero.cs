using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Lib;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace DiscRoom
{
    public class Hero : Spritesheet
    {

        private const float SPEED = 120;
        private const float FRICTION = 250;
        private const float START_X = 544/2; // Center of the grid
        private const float START_Y = 544/2;

        private const float BOOST_DISPLAY_X = 550;
        private const float BOOST_DISPLAY_Y = 110;



        // Concerns the dash
        private const float DASH_SPEED = 500f;
        private const float DASH_DURATION = 0.2f;
        private const float DASH_COOLDOWN = 1.5f;
        private const float DASH_ALPHA = 120f;
        private const float TRAIL_INTERVAL = 0.02f;

        private readonly AfterImageTrail _trail;
        private float _trailTimer = 0f;

        private bool _isDashing = false;
        private float _dashTimer = 0f;
        private float _cooldownTimer = 0f;
        private Vector2 _dashDirection = Vector2.Zero;
        private Vector2 _lastDirection = new Vector2(0, 1);

        private const float CHEAT_DURATION = 10;
        private const float CHEAT_ALPHA = 5f;

        private static readonly KeyboardKey[] CHEAT_SEQUENCE = [KeyboardKey.Up, KeyboardKey.Up, KeyboardKey.Down, KeyboardKey.Down, KeyboardKey.Left, KeyboardKey.Right, KeyboardKey.Left, KeyboardKey.Right, KeyboardKey.B, KeyboardKey.Q, KeyboardKey.Enter];

        private int _cheatIndex = 0;
        private bool _isCheatActive = false;
        private float _cheatTimer = 0f;

        /// <summary>
        /// The hero can't be killed while dashing or while the cheatCode is active
        /// </summary>
        public bool IsInvincible => _isDashing || _isCheatActive;



        public Hero() : base("assets/images/magician.png", 72 / 3, 192 / 8)
        {

            LoadAnimations();

            X = START_X;
            Y = START_Y;

            FrictionX = FRICTION;
            FrictionY = FRICTION;
            Scale = new System.Numerics.Vector2(2);
            _trail = new AfterImageTrail(this);
            HitboxOffset = new Rectangle(14, 20, -28, -24); // hitbox réduite, à ajuster avec IsHitboxVisible = true



        }



        /// <summary>
        /// Adds all the animations that the player needs
        /// </summary>
        private void LoadAnimations()
        {
            Animation.Add("WALK_DOWN", [0, 1, 2]);
            Animation.Add("WALK_UP", [13, 14, 15]);
            Animation.Add("WALK_RIGHT", [7, 8, 9]);
            Animation.Add("WALK_LEFT", [19, 20, 21]);
            Animation.Add("WALK_DIAG_RIGHT_DOWN", [4, 5, 6]);
            Animation.Add("WALK_DIAG_LEFT_DOWN", [22, 23, 24]);
            Animation.Add("WALK_DIAG_RIGHT_UP", [10, 11, 12]);
            Animation.Add("WALK_DIAG_LEFT_UP", [16, 17, 18]);
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            UpdateCooldown(dt);

            if (_isDashing)
            {
                UpdateDash(dt);
                UpdateTrail(dt);
            }
            else
            {
                ApplyMovement();
                TryStartDash();
            }

            CheckCheatCode();
            UpdateCheatBonus(dt);

            if (_isCheatActive) Alpha = CHEAT_ALPHA;

            _trail.Update(dt);
            HandleAnimations();
        }

        /// <summary>
        /// Returns the normalized direction asked by the player (zero if no input)
        /// </summary>
        private Vector2 GetInputDirection()
        {
            Vector2 direction = Vector2.Zero;

            if (IsKeyDown(KeyboardKey.Right)) direction.X += 1;
            if (IsKeyDown(KeyboardKey.Left)) direction.X -= 1;
            if (IsKeyDown(KeyboardKey.Down)) direction.Y += 1;
            if (IsKeyDown(KeyboardKey.Up)) direction.Y -= 1;

            if (direction != Vector2.Zero)
                direction = Vector2.Normalize(direction);

            return direction;
        }

        /// <summary>
        /// Reads and applies the directionals Inputs of the player 
        /// </summary>
        private void ApplyMovement()
        {
            Vector2 direction = GetInputDirection();

            if (direction == Vector2.Zero) return;

            _lastDirection = direction;
            Velocity = direction * SPEED;
        }

        /// <summary>
        /// Play the correct animation according to the direction of the player
        /// </summary>
        private void HandleAnimations()
        {
            // Plays the non-diagonals animations

            if (VelocityX > 0 && VelocityY == 0) Animation.Play("WALK_RIGHT", 5);
            if (VelocityX < 0 && VelocityY == 0) Animation.Play("WALK_LEFT", 5);
            if (VelocityY < 0 && VelocityX == 0) Animation.Play("WALK_UP", 5);
            if (VelocityY > 0 && VelocityX == 0) Animation.Play("WALK_DOWN", 5);

            // Plays the diagonals animations

            if (VelocityX > 0 && VelocityY < 0) Animation.Play("WALK_DIAG_RIGHT_UP", 5);
            if (VelocityX > 0 && VelocityY > 0) Animation.Play("WALK_DIAG_RIGHT_DOWN", 5);
            if (VelocityX < 0 && VelocityY < 0) Animation.Play("WALK_DIAG_LEFT_UP", 5);
            if (VelocityX < 0 && VelocityY > 0) Animation.Play("WALK_DIAG_LEFT_DOWN", 5);

            // Stops the animation if the player is static

            if(VelocityX == 0 && VelocityY == 0) Animation.Stop();
        }
        private void UpdateCooldown(float dt)
        {
            if (_cooldownTimer > 0f) _cooldownTimer -= dt;
        }

        /// <summary>
        /// Starts a dash if the player asks for it and the cooldown is over
        /// </summary>
        private void TryStartDash()
        {
            if (!IsKeyPressed(KeyboardKey.Space) || _cooldownTimer > 0f) return;

            _isDashing = true;
            _dashTimer = DASH_DURATION;
            _cooldownTimer = DASH_COOLDOWN;

            Vector2 inputDirection = GetInputDirection();
            _dashDirection = (inputDirection != Vector2.Zero) ? inputDirection : _lastDirection;

            Alpha = DASH_ALPHA;
        }

        /// <summary>
        /// Applies a constant force to the player until the dash ends
        /// </summary>
        private void UpdateDash(float dt)
        {
            Velocity = _dashDirection * DASH_SPEED;
            _dashTimer -= dt;

            if (_dashTimer > 0f) return;

            _isDashing = false;
            Alpha = 255f;
        }

        /// <summary>
        /// Leaves an afterimage at a regular rhythm while dashing
        /// </summary>
        private void UpdateTrail(float dt)
        {
            _trailTimer += dt;

            if (_trailTimer < TRAIL_INTERVAL) return;

            _trailTimer = 0f;
            _trail.Add();
        }

        /// <summary>
        /// Checks if the player is doing the cheat code sequence
        /// </summary>

        private void CheckCheatCode()
        {
            int pressedKey = 0;

            while ((pressedKey = (int)GetKeyPressed()) != 0)
            {
                if (pressedKey == (int)CHEAT_SEQUENCE[_cheatIndex]) _cheatIndex++;
                else _cheatIndex = (pressedKey == (int)CHEAT_SEQUENCE[0]) ? 1 : 0;

                if (_cheatIndex < CHEAT_SEQUENCE.Length) continue;

                _cheatIndex = 0;
                _isCheatActive = true;
                _cheatTimer = CHEAT_DURATION;
            }
        }

        /// <summary>
        /// Grants invicibility for CHEAT_DURATION seconds
        /// </summary>
        /// <param name="dt"></param>

        private void UpdateCheatBonus(float dt)
        {

            if (!_isCheatActive) return;

            _cheatTimer -= dt;

            if (_cheatTimer <= 0f)
            {
                _isCheatActive = false;
                Alpha = 255f;
            }


        }
        

        public override void Draw()
        {
            _trail.Draw();   // d'abord la traînée, puis le héros par-dessus
            DrawText("Boost", (int)BOOST_DISPLAY_X, (int)BOOST_DISPLAY_Y - 20, 20, Color.Red); // DrawText un peu au dessus du rectangle
            if (!_isDashing && _cooldownTimer <= 0) DrawRectangle((int)BOOST_DISPLAY_X, (int)BOOST_DISPLAY_Y, 50, 50, Color.Red);
            else DrawRectangle((int)BOOST_DISPLAY_X, (int)BOOST_DISPLAY_Y, 50, 50, Color.Black);

            if(_isCheatActive) DrawText("Cheat Activated for " + _cheatTimer.ToString("F0"), (int)BOOST_DISPLAY_X, (int)BOOST_DISPLAY_Y + 60, 20, Color.Yellow);
            base.Draw();
        }
    }
}
