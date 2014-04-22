using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using Bug_Escape.BaseClasses;
using Bug_Escape.DataBase;
using Bug_Escape;

using Decio.Animation;

namespace Bug_Escape.Management
{
    class Directions : State
    {
        Sprite Animation;

        public Directions(StateManager Father)
        {
            Manager = Father;

            # region Language

            switch (StateManager.CurrentLanguage)
            {
                case GameLanguage.English:

                    //BackGroundImage = Manager.Game.Content.Load<Texture2D>
                    //    ("English/BackGroundImages/Directions");

                    break;

                case GameLanguage.Portugues:

                    //BackGroundImage = Manager.Game.Content.Load<Texture2D>
                    //    ("Portugues/Telas/Instrucoes");

                    break;
            }

            # endregion

            Animation = new Sprite(Manager.Game.Content.Load<Texture2D>("Directions"), 250f, "Directions.txt", "Directions");
        }

        public override void Update(GameTime gameTime)
        {
            # region Device's BackButton

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Manager.GoToMenu();
            }

            # endregion

            Animation.Update(gameTime);

            if (Animation.Ended)
            {
                if (StateManager.HasAudioControl)
                {
                    MediaPlayer.Stop();
                }

                Manager.GoToGamePlay();
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //Manager.spriteBatch.Draw(BackGroundImage, Vector2.Zero, Color.White);

            Animation.Draw(Manager.spriteBatch , Vector2.Zero);

            base.Draw(gameTime);
        }
    }
}