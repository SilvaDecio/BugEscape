using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

using Bug_Escape.BaseClasses;
using Bug_Escape.DataBase;

using Bug_Escape;

namespace Bug_Escape.Management
{
    class Pause : State
    {
        Button ResumeButton, RestartButton, MenuButton , SoundButton;

        public Pause(StateManager Father)
        {
            Manager = Father;

            BackGroundImage = Manager.Game.Content.Load<Texture2D>("Pause");

            # region Buttons

            ResumeButton = new Button(Manager.Game.Content.Load<Texture2D>
                ("Buttons/Pause/Resume"), new Vector2(75 , 210));
            RestartButton = new Button(Manager.Game.Content.Load<Texture2D>
                ("Buttons/Pause/Restart"), new Vector2(10 , 10));
            MenuButton = new Button(Manager.Game.Content.Load<Texture2D>
                ("Buttons/Pause/Menu"), new Vector2(90 , 10));

            if (MediaPlayer.IsMuted)
            {
                SoundButton = new Button(Manager.Game.Content.Load<Texture2D>
                ("Buttons/Pause/SoundOff"), new Vector2(20, 410));
            }
            else
            {
                SoundButton = new Button(Manager.Game.Content.Load<Texture2D>
                ("Buttons/Pause/SoundOn"), new Vector2(20, 410));
            }

            # endregion

            if (StateManager.HasAudioControl)
            {
                MediaPlayer.Pause();
            }
        }

        public override void Update(GameTime gameTime)
        {
            # region Device's BackButton

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                if (StateManager.HasAudioControl)
                {
                    MediaPlayer.Resume();
                }

                Manager.ResumeGamePlay();
            }

            # endregion

            # region Buttons

            if (Manager.Touched)
            {
                if (ResumeButton.Rectangle.Contains(Manager.TouchedPlace))
                {
                    if (StateManager.HasAudioControl)
                    {
                        MediaPlayer.Resume();
                    }

                    Manager.ResumeGamePlay();
                }

                else if (RestartButton.Rectangle.Contains(Manager.TouchedPlace))
                {
                    Manager.RestartGamePlay();
                }

                else if (MenuButton.Rectangle.Contains(Manager.TouchedPlace))
                {
                    if (StateManager.HasAudioControl)
                    {
                        MediaPlayer.Stop();
                    }

                    Manager.GoToMenu();
                }

                else if (SoundButton.Rectangle.Contains(Manager.TouchedPlace))
                {
                    if (MediaPlayer.IsMuted)
                    {
                        SoundButton.Image = Manager.Game.Content.Load<Texture2D>
                            ("Buttons/Pause/SoundOn");

                        MediaPlayer.IsMuted = false;
                    }
                    else
                    {
                        SoundButton.Image = Manager.Game.Content.Load<Texture2D>
                            ("Buttons/Pause/SoundOff");

                        MediaPlayer.IsMuted = true;
                    }
                }
            }

            # endregion

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Manager.spriteBatch.Draw(BackGroundImage, Vector2.Zero ,Color.White);

            ResumeButton.Draw(Manager.spriteBatch);
            RestartButton.Draw(Manager.spriteBatch);
            MenuButton.Draw(Manager.spriteBatch);
            SoundButton.Draw(Manager.spriteBatch);

            base.Draw(gameTime);
        }
    }
}