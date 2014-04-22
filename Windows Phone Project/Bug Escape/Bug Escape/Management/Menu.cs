using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.GamerServices;

using System.IO.IsolatedStorage;
using System.Xml.Serialization;

using Microsoft.Phone.Tasks;
using Microsoft.Phone.Net.NetworkInformation;

using Bug_Escape.BaseClasses;
using Bug_Escape.Management;
using Bug_Escape.DataBase;
using Bug_Escape;

namespace Bug_Escape.Management
{
    class Menu : State
    {
        Button PlayButton, DirectionsButton , CreditsButton, RankingButton;

        public Menu(StateManager Father)
        {
            Manager = Father;

            BackGroundImage = Manager.Game.Content.Load<Texture2D>("Menu");

            # region Buttons

            PlayButton = new Button(Manager.Game.Content.Load<Texture2D>
                ("Buttons/Menu/Play"), new Vector2(335, 325));
            CreditsButton = new Button(Manager.Game.Content.Load<Texture2D>
                ("Buttons/Menu/Credits"), new Vector2(525, 375));
            RankingButton = new Button(Manager.Game.Content.Load<Texture2D>
                ("Buttons/Menu/Ranking"), new Vector2(200, 375));

            # endregion

            if (StateManager.HasAudioControl)
            {
                if (MediaPlayer.State == MediaState.Stopped)
                {
                    MediaPlayer.Play(Manager.MenuSong);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            # region Device's BackButton

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Manager.Game.Exit();
            }

            # endregion

            # region Buttons

            if (Manager.Touched)
            {
                if (PlayButton.Rectangle.Contains(Manager.TouchedPlace))
                {
                    Manager.GoToDirections();
                }

                else if (CreditsButton.Rectangle.Contains(Manager.TouchedPlace))
                {
                    Manager.GoToCredits();
                }

                else if (RankingButton.Rectangle.Contains(Manager.TouchedPlace))
                {
                    Manager.GoToRanking();
                }
            }

            # endregion

            base.Update(gameTime);
        }

        private void OnEndShowMessageBox(IAsyncResult result) {}

        public override void Draw(GameTime gameTime)
        {
            Manager.spriteBatch.Draw(BackGroundImage, Vector2.Zero, Color.White);

            PlayButton.Draw(Manager.spriteBatch);
            CreditsButton.Draw(Manager.spriteBatch);
            RankingButton.Draw(Manager.spriteBatch);

            base.Draw(gameTime);
        }
    }
}