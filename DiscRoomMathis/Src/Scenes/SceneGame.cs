using System;
using System.Collections.Generic;
using System.Text;
using Lib;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace DiscRoom
{
    public class SceneGame : AScene
    {


        private Hero _player;

        private Camera _camera;

        private  Rectangle _arena = new Rectangle(0, 0, 17 * 32, 17 * 32); // même taille que la grille dessinée
        private SawManager _sawManager;
        private bool _isGameOver = false;

        private const float DEATH_DELAY = 1f;

        private float _gameTimer = 0f;
        private float _scoreMins = 0f;
        private float _scoreSecs = 0f;
        private const float SCORE_DISPLAY_X = 550;
        private const float SCORE_DISPLAY_Y= 20;
        private float _deathTimer = 0f;
        private bool _isPlayerDead = false;

        private const float CAMERA_FOLLOW_STRENGTH = 0.10f;

        public SceneGame()
        {
            _sawManager = new SawManager(_arena);
            _player = new Hero();
            _camera = new Camera();
            _camera.SetAnchor(new System.Numerics.Vector2(_arena.X + _arena.Width * 0.5f, _arena.Y + _arena.Height * 0.5f));
            _camera.FollowStrength = CAMERA_FOLLOW_STRENGTH;
            _camera.Follow(_player);
        }


        public override void Update(float dt)
        {
            _camera.Update(dt);

            if (_isPlayerDead)
            {
                UpdateDeath(dt);
                return;
            }

            _player.Update(dt);
            _sawManager.Update(dt);
            UpdateScore(dt);

            DefineBoundaries();
            CheckPlayerDeath();
        }

        /// <summary>
        /// Increase the score and translate it into  mins and secs
        /// </summary>
        /// <param name="dt"></param>
        private void UpdateScore(float dt)
        {
            _gameTimer += dt;

            _scoreMins = (int)(_gameTimer / 60);
            _scoreSecs = _gameTimer % 60;
        }

        public override void Draw()
        {
            _camera.Begin();
            DrawCheckerboard();




            DrawText("Score: " + _scoreMins + ":" + _scoreSecs.ToString("00.00"), (int)SCORE_DISPLAY_X, (int)SCORE_DISPLAY_Y, 30, Color.White);
            _sawManager.Draw();
            _player.Draw();

            _camera.End();
        }

        private  void DrawCheckerboard()
        {
            // Taille d'une case (en pixels)
            int tileSize = 32;
            int rows = 16;
            int cols = 16;

            for (int i = 0; i <= rows; i++)
            {
                for (int j = 0; j <= cols; j++)
                {
                    int x = j * tileSize;
                    int y = i * tileSize;

                    // alterne blanc / gris foncés
                    Color c = ((i + j) % 2 == 0) ? Color.White : new Color(150, 150, 150, 255);

                    DrawRectangle(x, y, tileSize, tileSize, c);
                }
            }
        }


        private void DefineBoundaries()
        {
            // Boundaries basées sur la grille utilisée dans Draw()
            int tileSize = 32;
            int rows = 16;
            int cols = 16;

            float worldLeft = -10f;
            float worldTop = -30f;
            float worldRight = (cols + 1.5f) * tileSize; // on dessine de 0 à cols inclus
            float worldBottom = (rows + 1) * tileSize;

            var hb = _player.Hitbox;

            // Gauche
            if (hb.X < worldLeft)
            {
                _player.X += worldLeft - hb.X;
                _player.VelocityX = 0f;
            }

            // Droite
            if (hb.X + hb.Width > worldRight)
            {
                _player.X -= (hb.X + hb.Width) - worldRight;
                _player.VelocityX = 0f;
            }

            // Haut
            if (hb.Y < worldTop)
            {
                _player.Y += worldTop - hb.Y;
                _player.VelocityY = 0f;
            }

            // Bas
            if (hb.Y + hb.Height > worldBottom)
            {
                _player.Y -= (hb.Y + hb.Height) - worldBottom;
                _player.VelocityY = 0f;
            }
        }

        private void CheckPlayerDeath()
        {
            if (_player.IsInvincible) return;
            if (!_sawManager.IsHeroHit(_player.Hitbox)) return;

            _isPlayerDead = true;
            _camera.Shake(-6, 6, 0.4f);
        }

        /// <summary>
        /// Leaves a short moment (with the screen shake) before showing the game over screen
        /// </summary>
        private void UpdateDeath(float dt)
        {
            _deathTimer += dt;

            if (_deathTimer < DEATH_DELAY) return;

            ChangeScene(new SceneGameOver(_gameTimer));
        }

        public override void Unload()
        {
            _player.Unload();   
            _sawManager.Reset(); // libère les textures des scies
        }

    }
}
