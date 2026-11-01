using System;
using Lib;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace DiscRoom
{
    public class SceneMenu : AScene
    {
        private float _bestScore;

        public override void Load()
        {
            _bestScore = ScoreStorage.LoadBestScore();
        }

        public override void Update(float dt)
        {
            if (IsKeyPressed(KeyboardKey.Enter))
            {
                ChangeScene(new SceneGame());
            }
        }

        public override void Draw()
        {
            float centerX = GetScreenWidth() * 0.5f;

            TextUtils.DrawTextCentered("MINI DISC ROOM", centerX, 150f, 72, Color.White);
            TextUtils.DrawTextCentered("BEST SCORE : " + _bestScore.ToString("F2") + " s", centerX, 270f, 32, Color.Gold);

            TextUtils.DrawTextCentered("Press ENTER to play", centerX, 400f, 36, Color.White);
            TextUtils.DrawTextCentered("Arrows : move   -   Space : dash", centerX, GetScreenHeight() - 60f, 20, Color.Gray);
        }
    }
}
