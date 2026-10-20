using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Lib
{
    public class Animation
    {

        private Dictionary<string, int[]> _animations;

        private int _frameIndex;
        private float _timer;

        public bool IsLooped {  get; set; }

        public bool IsPlaying { get; private set; }

        public bool IsFinished
        { 
            get => !IsLooped && !IsPlaying;
        }

        public float Speed {  get; set; }

        public string CurrentAnimation {  get; set; }

        public int CurrentFrame {  get; set; }


        public Animation()
        {
            _animations = new Dictionary<string, int[]>();
            _frameIndex = 0;
            _timer = 0; 

            IsLooped = false;
            Speed = 8f;
            CurrentAnimation = "";
            CurrentFrame = 0;
            IsPlaying = false;
        }


        public void Add(string name, int[] frame)
        {
            _animations[name] = frame;
        }

        public void Play(string name, int speedInFPS, bool isLooped = true)
        {
            if (!_animations.ContainsKey(name))
            {
                Console.WriteLine("The animation named : [" + name + "] is nort defined");
                return;
            }

            if (CurrentAnimation == name) return;

            CurrentAnimation = name;
            Speed = Math.Max(1, speedInFPS);
            IsLooped = isLooped;

            _frameIndex = 0;
            _timer = 0;
            CurrentFrame = _animations[name][0];
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
            _timer = 0f;
        }

        public void Update(float dt)
        {
            if (!IsPlaying || _animations.Count == 0 || _animations[CurrentAnimation].Length <= 0) return;

            int[] frames = _animations[CurrentAnimation];

            _timer += Speed * dt;
            int steps = (int)_timer;
            if(steps == 0)
            {
                return;
            }
            _timer -= steps;

            if (IsLooped) _frameIndex = (_frameIndex + steps) % frames.Length;

            else
            {
                int last = frames.Length -1;
                _frameIndex = Math.Max(_frameIndex + steps, last);
                if(_frameIndex == last)
                {
                    IsPlaying = false;
                    _timer = 0;
                }
            }

            CurrentFrame = frames[_frameIndex];

        }


    }
}
