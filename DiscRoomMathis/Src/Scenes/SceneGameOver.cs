using System;
using Lib;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace DiscRoom
{
    public class SceneGameOver : AScene
    {

        private Music _gameOverMusic;

        private readonly float _score;

        private float _bestScore;
        private bool _isNewBestScore;

        public SceneGameOver(float score)
        {
            _score = score;
        }

        public override void Load()
        {
            _isNewBestScore = ScoreStorage.TrySaveBestScore(_score);
            _bestScore = ScoreStorage.LoadBestScore();

            _gameOverMusic = LoadMusicStream("assets/sounds/gameOverMusic.mp3");
            PlayMusicStream(_gameOverMusic);
        }

        public override void Update(float dt)
        {
            if (IsKeyPressed(KeyboardKey.Enter))
            {
                ChangeScene(new SceneGame());
                return;
            }

            if (IsKeyPressed(KeyboardKey.M))
            {
                ChangeScene(new SceneMenu());
                return;
            }

            UpdateMusicStream(_gameOverMusic);
        }

        public override void Draw()
        {
            float centerX = GetScreenWidth() * 0.5f;

            TextUtils.DrawTextCentered("GAME OVER", centerX, 120f, 72, Color.Red);
            TextUtils.DrawTextCentered("SCORE : " + _score.ToString("F2") + " s", centerX, 240f, 40, Color.White);

            if (_isNewBestScore)
            {
                TextUtils.DrawTextCentered("NEW BEST SCORE !", centerX, 300f, 32, Color.Gold);
            }
            else
            {
                TextUtils.DrawTextCentered("BEST : " + _bestScore.ToString("F2") + " s", centerX, 300f, 32, Color.Gray);
            }

            TextUtils.DrawTextCentered("ENTER : restart", centerX, 400f, 32, Color.White);
            TextUtils.DrawTextCentered("M : main menu", centerX, 450f, 32, Color.White);
        }

        public override void Unload()
        {
            UnloadMusicStream(_gameOverMusic);
            base.Unload();
        }
    }
}
