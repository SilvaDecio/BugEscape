using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;

using Bug_Escape.BaseClasses;
using Bug_Escape.DataBase;
using Bug_Escape.Objects;
using Bug_Escape.Personage;

namespace Bug_Escape.Management
{
    enum TipoBorda
    {
        Top , Bottom , Right , Left
    }

    class GamePlay : State
    {
        float ElapsedTime;

        Joana Jogador;

        Random Raffle;

        public static int Score , Lifes;

        TouchCollection Touches;

        HUD LocalHUD;

        SoundEffect GameOverEffect;

        public GamePlay(StateManager Father)
        {
            Manager = Father;

            Raffle = new Random();

            BackGroundImage = Manager.Game.Content.Load<Texture2D>("GamePlay");

            LocalHUD = new HUD(Manager.Game.Content);

            GameOverEffect = Manager.Game.Content.Load<SoundEffect>("Audio/SoundEffects/GameOver");

            Restart();
        }

        public override void Restart()
        {
            ElapsedTime = 0f;

            Jogador = new Joana(Manager.Game.Content, Manager.Game.GraphicsDevice.Viewport);
            
            Score = 0;

            Lifes = 3;

            Touches = new TouchCollection();

            if (StateManager.HasAudioControl)
            {
                MediaPlayer.Play(Manager.GamePlaySong);
            }

            base.Restart();
        }

        public override void Update(GameTime gameTime)
        {
            # region Device's BackButton

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Manager.GoToPause();
            }

            # endregion

            ElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            
            # region Input

            Touches = TouchPanel.GetState();

            List<Vector2> Positions = new List<Vector2>();

            for (int i = 0; i < Touches.Count; i++)
            {
                if (Touches[i].State == TouchLocationState.Moved || Touches[i].State == TouchLocationState.Pressed)
                {
                    Positions.Add(Touches[i].Position);
                }
            }

            # endregion

            # region Pause Button

            for (int i = 0; i < Positions.Count; i++)
            {
                if (PauseButton.Rectangle.Contains(new Point((int)Positions[i].X , (int)Positions[i].Y)))
                {
                    Manager.GoToPause();
                }
            }
            # endregion

            # region Joaninha

            Jogador.Update(gameTime);

            Jogador.LateUpdate(gameTime, Positions);

            # endregion
            
            # region Score

            Score = Jogador.Score;

            # endregion

            # region GameOver

            if (Lifes <= 0)
            {
                GameOverEffect.Play();

                Manager.Vibrate.Start(new TimeSpan(0, 0, 0, 0, 500));

                Manager.GoToGameOver(Score);
            }

            # endregion

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Manager.spriteBatch.Draw(BackGroundImage , Vector2.Zero , Color.White);

            LocalHUD.Draw(Manager.spriteBatch , Lifes , Manager.Font , Score);
            
            # region Joaninha

            Jogador.Draw(Manager.spriteBatch);

            # endregion

            base.Draw(gameTime);
        }
    }
}