using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Lib
{
    public class Sprite
    {

        public Texture2D Texture { get; private set; }

        public string AssetPath { get; private set; } = "";

        public Vector2 Position { get; set; }

        public float X { get { return Position.X; } set { Position = new Vector2(value, Position.Y); } }
        public float Y
        { 
            get => Position.Y;  

            set => Position = new Vector2(Position.X,value);
        } 

        public Vector2 Velocity { get; set; }

        public float VelocityX { get { return Velocity.X; } set { Velocity = new Vector2(value, Velocity.Y); } }
        public float VelocityY
        {
            get => Velocity.Y;

            set => Velocity = new Vector2(Velocity.X, value);
        }

        public Vector2 Friction {  get; set; }

        public float FrictionX
        {
            get => Friction.X;

            set => Friction = new Vector2(value,Friction.Y);
        }
        public float FrictionY
        {
            get => Friction.Y;

            set => Friction = new Vector2(Friction.X, value);
        }

        public Vector2 Origin { get; set; }

        public float OriginX { get { return Origin.X; } set { Origin = new Vector2(value, Origin.Y); } }
        public float OriginY
        {
            get => Origin.Y;

            set => Origin = new Vector2(Origin.X, value);
        }

        public Vector2 Scale {  get; set; }

        public float ScaleX
        {
            get => Scale.X;

            set => Scale = new Vector2(value, Scale.Y);
        }
        public float ScaleY
        {
            get => Scale.Y;

            set => Scale = new Vector2(Scale.X, value);
        }

        public virtual float WidthScaled
        {
            get => Texture.Width * Scale.X;

        }
        public virtual float HeightScaled
        {
            get => Texture.Height * Scale.Y;

        }

        public float Angle {  get; set; }

        public Color Color { get; set; }

        public float Alpha {  get; set; }


        public bool FlipX {  get; set; }

        public bool FlipY {  get; set; }

        public bool IsVisible {  get; set; }

        public bool IsActive {  get; set; }


        public Rectangle Hitbox { get; private set; }

        public Rectangle HitboxOffset {  get; set; }

        public Color HitboxColor {  get; set; }

        public bool IsHitboxVisible{  get; set; }

        public Sprite(string asset)
        {
            Load(asset);
        }

        public void Load(string asset)
        {
            AssetPath = asset;
            Texture = LoadTexture(AssetPath);
            Reset();
        }

        public void Reset()
        {
            Position = Vector2.Zero;
            Velocity = Vector2.Zero;
            Friction = Vector2.Zero;
            Origin = Vector2.Zero;
            Scale = Vector2.One;
            Angle = 0f;
            Color = Color.White;
            Alpha = 255f;
            FlipX = FlipY = false;
            IsActive = true;
            IsVisible = true;
            Hitbox = new Rectangle();
            HitboxOffset = new Rectangle(0, 0, 0, 0);
            HitboxColor = new Color(255, 0, 0, 150);
            IsHitboxVisible = false;
        }

        public virtual void Update(float dt)
        {

            if (!IsActive) return;

            Position += Velocity * dt;
            UpdateFriction(dt);

            UpdateHitbox();

            if (Angle > 360f) Angle -= 360f;
            if (Angle < -360f) Angle += 360f;

        }


        public virtual void Draw()
        {

            if(!IsVisible || !IsActive) return;

            int flipX = (FlipX) ? -1 : 1;
            int flipY = (FlipY) ? -1 : 1;

            DrawTexturePro(Texture, new Rectangle(0,0,Texture.Width * flipX,Texture.Height * flipY), new Rectangle(Position, new Vector2(WidthScaled, HeightScaled)), new Vector2(WidthScaled * OriginX, HeightScaled * OriginY), Angle, new Color(Color.R,Color.G,Color.B,Alpha));

            if(!IsHitboxVisible ) return;

            DrawRectangle((int)Hitbox.X, (int)Hitbox.Y, (int)Hitbox.Width, (int)Hitbox.Height, HitboxColor);
        }

        public void UpdateFriction(float dt)
        {
            if(VelocityX > 0f)
            {
                VelocityX -= FrictionX * dt;
                if (VelocityX <= 0) VelocityX = 0;
            }
            if(VelocityX < 0f)
            {
                VelocityX += FrictionX * dt;
                if (VelocityX >= 0) VelocityX = 0;
            }

            if(VelocityY > 0f)
            {
                VelocityY -= FrictionY * dt;
                if (VelocityY <= 0) VelocityY = 0;
            }
            if(VelocityY < 0f)
            {
                VelocityY += FrictionY * dt;
                if (VelocityY >= 0) VelocityY = 0;
            }
        }
        private void UpdateHitbox()
        {
            Hitbox = new Rectangle(Position.X - (OriginX * WidthScaled) + HitboxOffset.X, Position.Y - (OriginY * HeightScaled) + HitboxOffset.Y, WidthScaled + HitboxOffset.Width, HeightScaled + HitboxOffset.Height);
        }

        public void Unload()
        {
            UnloadTexture(Texture);
        }

    }
}
