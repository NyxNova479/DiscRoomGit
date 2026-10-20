using Lib;
using System;
using System.Collections.Generic;
using System.Text;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Lib
{
    public class Spritesheet : Sprite
    {

        public int SizeX {  get; private set; }
        public int SizeY {  get; private set; }

        public override float WidthScaled
        {
            get => SizeX * Scale.X;
        }
        public override float HeightScaled
        {
            get => SizeY * Scale.Y;
        }
        public Animation Animation {  get; protected set; }

        public Spritesheet(string asset, int sizeX, int sizeY) : base(asset)
        {
            Load(asset, sizeX, sizeY);
        }


        public void Load(string asset, int sizeX, int sizeY)
        {
            base.Load(asset);
            SizeX = sizeX;
            SizeY = sizeY;
            Animation = new Animation();
        }

        public override void Update(float dt)
        {
            Animation.Update(dt);
            base.Update(dt);
        }

        public override void Draw()
        {
            if (!IsVisible || !IsActive) return;

            int flipX = (FlipX) ? -1 : 1;
            int flipY = (FlipY) ? -1 : 1;

            int frameID = Animation.CurrentFrame;
            int columns = Texture.Width / SizeX;
            if (columns <= 0) return;

            int frameCol = frameID % columns;
            int frameRow = frameID / columns;

            float sourceX = frameCol * SizeX;
            float sourceY = frameRow * SizeY;


            Rectangle source = new Rectangle(sourceX, sourceY, SizeX, SizeY);

            if (FlipX) source.Width *= -1;

            if(FlipY) source.Height *= -1;

            DrawTexturePro(Texture, source, new Rectangle(Position, WidthScaled, HeightScaled), new System.Numerics.Vector2(WidthScaled * OriginX, HeightScaled * OriginY), Angle, new Color(Color.R, Color.G, Color.B, Alpha));
        }




    }
}
