using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Lib
{
    public class SceneManager
    {

        public AScene? CurrentScene { get; private set; }


        public SceneManager()
        {
            CurrentScene = null;
        }

        public void ChangeScene(AScene scene)
        {
            CurrentScene?.Unload();
            CurrentScene = scene;
            CurrentScene.SetManager(this);
            CurrentScene.Load();
        }

        public void Update(float dt)
        {
            if(CurrentScene != null)
                CurrentScene.Update(dt);
        }

        public void Draw()
        {
            CurrentScene?.Draw();
        }

    }
}
