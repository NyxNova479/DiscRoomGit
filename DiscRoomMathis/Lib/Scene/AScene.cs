using System;
using System.Collections.Generic;
using System.Text;

namespace Lib
{ 
    public abstract class AScene
    {

        private SceneManager? _manager;

        public virtual void Load()
        {

        }

        public virtual void Update(float dt)
        {

        }

        public virtual void Draw()
        {

        }

        public virtual void Unload()
        {

        }

        public void SetManager(SceneManager manager)
        {
            _manager = manager;
        }

        protected virtual void ChangeScene(AScene scene)
        {
            _manager?.ChangeScene(scene);
        }

    }
}
