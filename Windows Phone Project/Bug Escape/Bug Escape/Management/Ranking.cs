using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Bug_Escape.BaseClasses;
using Bug_Escape.DataBase;
using Bug_Escape;

namespace Bug_Escape.Management
{
    class Ranking : State
    {   
        Vector2 Name0Position, Name1Position, Name2Position , 
            Record0Position, Record1Position, Record2Position;
        
        List<Player> Players;

        public Ranking(StateManager Father)
        {
            Manager = Father;

            # region Language

            switch (StateManager.CurrentLanguage)
            {
                case GameLanguage.English:

                    BackGroundImage = Manager.Game.Content.Load<Texture2D>
                        ("English/BackGroundImages/Ranking");

                    break;

                case GameLanguage.Portugues:

                    BackGroundImage = Manager.Game.Content.Load<Texture2D>
                        ("Portugues/Telas/Recordes");

                    break;
            }

            # endregion

            # region Positions

            Name0Position = new Vector2(300,170);
            Record0Position = new Vector2(500,170);
            Name1Position = new Vector2(305,265);
            Record1Position = new Vector2(500,265);
            Name2Position = new Vector2(310,365);
            Record2Position = new Vector2(500,365);

            # endregion

            Players = Player.Load().OrderByDescending(x => x.Record).ToList();
        }

        public override void Update(GameTime gameTime)
        {
            # region Device's BackButton

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                Manager.GoToMenu();
            }

            # endregion

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Manager.spriteBatch.Draw(BackGroundImage, Vector2.Zero, Color.White);

            # region Records

            if (Players != null)
            {
                if (Players.Count == 1)
                {
                    Manager.spriteBatch.DrawString(Manager.Font, 
                        Players[0].Name,Name0Position, Color.White);
                    Manager.spriteBatch.DrawString(Manager.Font,
                        Players[0].Record.ToString(),
                    Record0Position, Color.White);
                }
                else if (Players.Count == 2)
                {
                    Manager.spriteBatch.DrawString(Manager.Font,
                        Players[0].Name,Name0Position, Color.White);
                    Manager.spriteBatch.DrawString(Manager.Font,
                        Players[0].Record.ToString(),
                    Record0Position, Color.White);

                    Manager.spriteBatch.DrawString(Manager.Font,
                        Players[1].Name,Name1Position, Color.White);
                    Manager.spriteBatch.DrawString(Manager.Font,
                        Players[1].Record.ToString(),
                    Record1Position, Color.White);
                }
                else if (Players.Count >= 3)
                {
                    Manager.spriteBatch.DrawString(Manager.Font,
                        Players[0].Name,Name0Position, Color.White);
                    Manager.spriteBatch.DrawString(Manager.Font,
                        Players[0].Record.ToString(),
                    Record0Position, Color.White);

                    Manager.spriteBatch.DrawString(Manager.Font,
                        Players[1].Name,Name1Position, Color.White);
                    Manager.spriteBatch.DrawString(Manager.Font,
                        Players[1].Record.ToString(),
                    Record1Position, Color.White);

                    Manager.spriteBatch.DrawString(Manager.Font,
                        Players[2].Name,Name2Position, Color.White);
                    Manager.spriteBatch.DrawString(Manager.Font,
                        Players[2].Record.ToString(),
                    Record2Position, Color.White);
                }
            }
            
            #endregion

            base.Draw(gameTime);
        }
    }
}