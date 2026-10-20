using System;
using System.Collections.Generic;
using System.Text;
using Lib;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace DiscRoom
{
    public class Hero : Spritesheet
    {

        private const float SPEED = 150;
        private const float FRICTION = 100;

        // Concerns the dash
        private  float _dashForce = 50;
        private bool _dashed = false;
        private float _timer = 0f;
        private const float DASH_COOLDOWN = 5f;

        

        public Hero() : base("assets/images/magician.png", 72 / 3, 192 / 8)
        {

            LoadAnimations();

            FrictionX = FRICTION;
            FrictionY = FRICTION;
            Scale = new System.Numerics.Vector2(2);
        }

        /// <summary>
        /// Adds all the animations that the player needs
        /// </summary>
        private void LoadAnimations()
        {
            Animation.Add("WALK_DOWN", [0, 1, 2]);
            Animation.Add("WALK_UP", [12, 13, 14]);
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

            ApplyMovement();

            if (IsKeyPressed(KeyboardKey.Space) &&_timer >= DASH_COOLDOWN)
            {
                UpdateDash(dt);
            }

            HandleAnimations();


        }

        /// <summary>
        /// Reads and applies the directionals Inputs of the player 
        /// </summary>
        private void ApplyMovement()
        {
            if (IsKeyDown(KeyboardKey.Right))
            {
                VelocityX = SPEED;

            }
            if (IsKeyDown(KeyboardKey.Left))
            {
                VelocityX = -SPEED;

            }
            if (IsKeyDown(KeyboardKey.Up))
            {
                VelocityY = -SPEED;


            }
            if (IsKeyDown(KeyboardKey.Down))
            {
                VelocityY = SPEED;

            }
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

        /// <summary>
        /// Applies a force to the player's movement
        /// </summary>
        /// <param name="dt"></param>
        private void UpdateDash(float dt)
        {
            if (_dashed) return;

            _timer += dt;



        }
    }
}
