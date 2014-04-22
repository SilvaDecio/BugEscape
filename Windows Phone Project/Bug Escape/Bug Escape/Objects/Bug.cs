using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using Decio.Animation;
using Decio.Collision;

using Bug_Escape.Management;

namespace Bug_Escape.Objects
{
    enum TipoInseto
    {
        Easy , Medium , Hard
    }

    class Bug
    {
        Sprite Animation;

        public Vector2 Position;
        
        Vector2 Speed , TargetPosition, DistanceToTarget, Direction;

        public Rectangle BoundingRectangle;

        public AngulatedRectangle BoundingRectangle_Angulated;

        float Angle, TargetAngle;

        public int Score;

        public TipoInseto Type;

        Viewport Screen;

        Random Raffle;

        public Bug(ContentManager Content, Viewport screen , TipoInseto type , TipoBorda local , float angle)
        {
            Screen = screen;

            Raffle = new Random();

            Direction = new Vector2();
            TargetPosition = new Vector2();
            DistanceToTarget = new Vector2();

            Type = type;

            # region Type

            switch (Type)
            {
                case TipoInseto.Easy:

                    Animation = new Sprite(Content.Load<Texture2D>("Insetos/BrownBug"), 33f, "Insetos/BrownBug.txt", "BrownBug");

                    Speed = new Vector2(2 , 2);

                    Score = 10;

                    break;

                case TipoInseto.Medium:

                    Animation = new Sprite(Content.Load<Texture2D>("Insetos/BlackBug"), 33f, "Insetos/BlackBug.txt", "BlackBug");

                    Speed = new Vector2(3 , 3);

                    Score = 20;

                    break;

                case TipoInseto.Hard:

                    Animation = new Sprite(Content.Load<Texture2D>("Insetos/CockRoach"), 33f, "Insetos/CockRoach.txt", "CockRoach");

                    Speed = new Vector2(4 , 4);

                    Score = 30;

                    break;
            }

            # endregion

            switch (local)
            {
                case TipoBorda.Top:

                    Position = new Vector2(Raffle.Next(0 , Screen.Width - Animation.AnimationRectangle.Width) , -Animation.AnimationRectangle.Height);

                    break;

                case TipoBorda.Bottom:

                    Position = new Vector2(Raffle.Next(0, Screen.Width - Animation.AnimationRectangle.Width), Screen.Height + Animation.AnimationRectangle.Height);

                    break;

                case TipoBorda.Right:

                    Position = new Vector2(Screen.Width + Animation.AnimationRectangle.Width , Raffle.Next(0 , Screen.Height - Animation.AnimationRectangle.Height));

                    break;

                case TipoBorda.Left:

                    Position = new Vector2(-Animation.AnimationRectangle.Width, Raffle.Next(0, Screen.Height - Animation.AnimationRectangle.Height));

                    break;
            }

            BoundingRectangle = new Rectangle((int)Position.X, (int)Position.Y, Animation.AnimationRectangle.Width,
                Animation.AnimationRectangle.Height);

            Angle = angle;
            TargetAngle = angle;

            BoundingRectangle_Angulated = new AngulatedRectangle(BoundingRectangle, Angle);
        }

        public virtual void Update(GameTime gameTime , Vector2 Target)
        {
            TargetPosition = Target;

            DistanceToTarget = Position - TargetPosition;

            TargetAngle = (float)Math.Atan2(DistanceToTarget.Y, DistanceToTarget.X);

            # region Getting Angle to Move

            float Distance = Angle - TargetAngle;

            if (Distance > Math.PI)
            {
                Angle -= MathHelper.TwoPi;
            }
            if (Distance < -Math.PI)
            {
                Angle += MathHelper.TwoPi;
            }
            if (Angle < TargetAngle)
            {
                Angle += 0.125f;
            }

            if (Angle > TargetAngle)
            {
                Angle -= 0.125f;
            }

            # endregion

            Animation.Update(gameTime);

            Direction = new Vector2((float)-Math.Cos(Angle), (float)-Math.Sin(Angle));

            Position += Speed * Direction;

            Position.X = MathHelper.Clamp(Position.X, 0, Screen.Width - Animation.AnimationRectangle.Width);
            Position.Y = MathHelper.Clamp(Position.Y, 0, Screen.Height - Animation.AnimationRectangle.Height);

            BoundingRectangle.X = (int)Position.X;
            BoundingRectangle.Y = (int)Position.Y;
            BoundingRectangle.Width = Animation.AnimationRectangle.Width;
            BoundingRectangle.Height = Animation.AnimationRectangle.Height;

            BoundingRectangle_Angulated.X = BoundingRectangle.X;
            BoundingRectangle_Angulated.Y = BoundingRectangle.Y;

            BoundingRectangle_Angulated.Rotation = Angle - MathHelper.PiOver2;
            BoundingRectangle_Angulated.CalculateOrigin();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle abc = new Rectangle(BoundingRectangle.X + BoundingRectangle.Width / 2,
                BoundingRectangle.Y + BoundingRectangle.Height / 2,
                BoundingRectangle.Width, BoundingRectangle.Height);

            spriteBatch.Draw(Animation.Sheet, abc, Animation.AnimationRectangle, Color.White, BoundingRectangle_Angulated.Rotation,
                new Vector2(Animation.AnimationRectangle.Width / 2, Animation.AnimationRectangle.Height / 2),
                SpriteEffects.None, 0f);
        }
    }
}