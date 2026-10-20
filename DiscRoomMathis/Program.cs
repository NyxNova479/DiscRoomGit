
using Lib;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace DiscRoom
{
    public class DiscRoom
    {

        public static int Main()
        {


            InitWindow(1280, 720, "Mini Disc Room");
            SetTargetFPS(60);
            InitAudioDevice();

            SceneManager sceneManager = new SceneManager();
            sceneManager.ChangeScene(new SceneGame());


            while (!WindowShouldClose())
            {
                BeginDrawing();
                ClearBackground(Color.Black);
                float dt = GetFrameTime();

                sceneManager.Update(dt);
                sceneManager.Draw();

                EndDrawing();
            }

            CloseAudioDevice();
            CloseWindow();

            return 0;
        }



    }
}
