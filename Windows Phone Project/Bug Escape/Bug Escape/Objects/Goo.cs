using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Bug_Escape.Objects
{
    class Goo
    {
        Texture2D Image;

        Vector2 Position;

        public bool Ative;

        float LifeTime;

        public Goo(ContentManager Content , Vector2 position)
        {
            Image = Content.Load<Texture2D>("Goo");

            Position = position;

            Ative = true;

            LifeTime = 0f;
        }

        public void Update(GameTime gameTime)
        {
            LifeTime += gameTime.ElapsedGameTime.Milliseconds;

            if (LifeTime >= 1000f)
            {
                Ative = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, Color.White);
        }
    }
}