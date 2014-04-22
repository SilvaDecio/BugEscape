using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using Bug_Escape.BaseClasses;

namespace Bug_Escape.Objects
{
    class HUD
    {
        Texture2D LifeImage;

        Vector2 LifePosition , ScorePosition;

        Button PauseButton;

        public HUD(ContentManager Content)
        {
            LifeImage = Content.Load<Texture2D>("Life");

            LifePosition = new Vector2(685, 20);

            ScorePosition = new Vector2();

            PauseButton = new Button(Content.Load<Texture2D>("Buttons/Pause"), new Vector2(15, 15));
        }

        public void Draw(SpriteBatch spriteBatch , int Lifes , SpriteFont Font , float Score)
        {
            spriteBatch.Draw(LifeImage, LifePosition, new Rectangle(0, 0, (int)(LifeImage.Width / 3) * Lifes, LifeImage.Height), Color.White);

            spriteBatch.DrawString(Font, Score.ToString(), ScorePosition, Color.White);

            PauseButton.Draw(spriteBatch);
        }
    }
}