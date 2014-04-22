using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Decio.Animation
{
    class Sprite
    {
        public Texture2D Sheet;

        public Rectangle AnimationRectangle;
        
        int CurrentFrame , FramesNumber;
        
        float TimeOfTransition , ElapsedTime;
        
        SpriteSheetPacker Reading;
        
        string PreFix;

        public bool Ended;

        public Sprite(Texture2D sheet, float timeoftransition , string Path , string prefix)
        {
            Reading = new SpriteSheetPacker(Path);

            PreFix = prefix;
            
            Sheet = sheet;

            FramesNumber = Reading.SpriteSourceRectangles.Count;
            
            TimeOfTransition = timeoftransition;

            Ended = false;

            Restart();
        }

        public void Restart()
        {
            ElapsedTime = 0f;

            CurrentFrame = 0;

            AnimationRectangle = Reading.SpriteSourceRectangles[PreFix + CurrentFrame];
        }

        public void Update(GameTime gameTime)
        {
            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            
            if (ElapsedTime >= TimeOfTransition)
            {
                ElapsedTime = 0f;

                ++CurrentFrame;

                if (CurrentFrame >= FramesNumber)
                {
                    Ended = true;

                    CurrentFrame = 0;
                }
            }

            AnimationRectangle = Reading.SpriteSourceRectangles[PreFix + CurrentFrame];
        }

        public void Draw(SpriteBatch spriteBatch , Vector2 Position)
        {
            spriteBatch.Draw(Sheet, Position, AnimationRectangle, Color.White);
        }
    }
}