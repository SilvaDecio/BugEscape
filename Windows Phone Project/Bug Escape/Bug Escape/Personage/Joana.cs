using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Decio.Animation;
using Decio.Collision;

using Bug_Escape.Objects;
using Bug_Escape.Management;

namespace Bug_Escape.Personage
{
    class Joana
    {
        Sprite Animation;

        Vector2 Position , Speed , MaximumSpeed , TargetPosition , DistanceToTarget , Direction;

        public Rectangle BoundingRectangle;

        float Angle , TargetAngle;

        public AngulatedRectangle BoundingRectangle_Angulated;

        ContentManager Content;

        Viewport Screen;

        Random Raffle;

        public BugManager Enemies;

        public int Score;

        List<Goo> Goos;

        public Joana(ContentManager content , Viewport screen)
        {
            Content = content;

            Screen = screen;

            Animation = new Sprite(Content.Load<Texture2D>("Personage/Joaninha"), 33f, "Personage/Joaninha.txt", "Joaninha");

            Position = new Vector2(Screen.Bounds.Center.X - Animation.AnimationRectangle.Width / 2,
                Screen.Bounds.Center.Y - Animation.AnimationRectangle.Height / 2);

            Direction = new Vector2();
            TargetPosition = new Vector2();
            DistanceToTarget = new Vector2();

            BoundingRectangle = new Rectangle((int)Position.X, (int)Position.Y,
                Animation.AnimationRectangle.Width , Animation.AnimationRectangle.Height);

            Angle = 0f;
            TargetAngle = 0f;

            BoundingRectangle_Angulated = new AngulatedRectangle(BoundingRectangle , Angle);

            Speed = new Vector2(4,4);

            MaximumSpeed = new Vector2(4, 4);

            Score = 0;

            Raffle = new Random();

            Enemies = new BugManager(Content, Screen);

            Goos = new List<Goo>();

            CreateNewTarget();
        }

        void CreateNewTarget()
        {
            TargetPosition = new Vector2(Raffle.Next(200 , 601) , Raffle.Next(100 , 381));
        }

        public void Update(GameTime gameTime)
        {
            if (BoundingRectangle.Contains(new Point((int)TargetPosition.X , (int)TargetPosition.Y)))
            {
                CreateNewTarget();
            }

            DistanceToTarget = Position - TargetPosition;

            TargetAngle = (float)Math.Atan2(DistanceToTarget.Y, DistanceToTarget.X);

            # region Goos

            for (int i = 0; i < Goos.Count; i++)
            {
                Goos[i].Update(gameTime);

                if (!Goos[i].Ative)
                {
                    Goos.RemoveAt(i);
                }
            }

            # endregion

            //# region Acceleration

            //if (Accelerate)
            //{
            //    Speed += new Vector2(0.1f, 0.1f);
            //}
            //else
            //{
            //    Speed -= new Vector2(0.05f, 0.05f);
            //}

            //Speed.X = MathHelper.Clamp(Speed.X, 0.0f, MaximumSpeed.X);
            //Speed.Y = MathHelper.Clamp(Speed.Y, 0.0f, MaximumSpeed.Y);

            //# endregion

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

            Position.X = MathHelper.Clamp(Position.X , 0 , Screen.Width - Animation.AnimationRectangle.Width);
            Position.Y = MathHelper.Clamp(Position.Y , 0 , Screen.Height - Animation.AnimationRectangle.Height);

            BoundingRectangle.X = (int)Position.X;
            BoundingRectangle.Y = (int)Position.Y;
            BoundingRectangle.Width = Animation.AnimationRectangle.Width;
            BoundingRectangle.Height = Animation.AnimationRectangle.Height;

            BoundingRectangle_Angulated.X = BoundingRectangle.X;
            BoundingRectangle_Angulated.Y = BoundingRectangle.Y;

            BoundingRectangle_Angulated.Rotation = Angle - MathHelper.PiOver2;
            BoundingRectangle_Angulated.CalculateOrigin();
        }

        public void LateUpdate(GameTime gameTime , List<Vector2> Positions)
        {
            float OppositeAngle = Angle;

            # region Enemies

            Enemies.Update(gameTime, Position, OppositeAngle);

            for (int i = 0; i < Enemies.Bugs.Count; i++)
            {
                if (BoundingRectangle_Angulated.Intersects(Enemies.Bugs[i].BoundingRectangle_Angulated))
                {
                    --GamePlay.Lifes;

                    Enemies.Bugs.RemoveAt(i);
                }
            }

            # endregion

            # region Colisão Toque - Inseto

            if (Positions.Count > 0)
            {
                for (int i = Enemies.Bugs.Count - 1; i >= 0; i--)
                {
                    for (int j = Positions.Count - 1; j >= 0; j--)
                    {
                        if (Enemies.Bugs[i].BoundingRectangle.Contains(new Point((int)Positions[j].X, (int)Positions[j].Y)))
                        {
                            Score += Enemies.Bugs[i].Score;

                            Goos.Add(new Goo(Content, Enemies.Bugs[i].Position));

                            Enemies.Bugs.RemoveAt(i);
                        }
                    }
                }
            }

            # endregion
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle abc = new Rectangle(BoundingRectangle.X + BoundingRectangle.Width / 2, BoundingRectangle.Y + BoundingRectangle.Height / 2,
                           BoundingRectangle.Width, BoundingRectangle.Height);

            for (int i = 0; i < Goos.Count; i++)
            {
                Goos[i].Draw(spriteBatch);
            }
            
            spriteBatch.Draw(Animation.Sheet, abc, Animation.AnimationRectangle, Color.White, BoundingRectangle_Angulated.Rotation,
                new Vector2(Animation.AnimationRectangle.Width / 2, Animation.AnimationRectangle.Height / 2),
                SpriteEffects.None, 0f);

            Enemies.Draw(spriteBatch);
        }
    }
}