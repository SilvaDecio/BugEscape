using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Bug_Escape.Management;

namespace Bug_Escape.Objects
{
    class BugManager
    {
        public List<Bug> Bugs;

        public float EasyElapsedTime , MediumElapsedTime , HardElapsedTime;

        public float EasyInterval , MediumInterval , HardInterval;

        public float EasyElapsedTime2, MediumElapsedTime2, HardElapsedTime2;

        public float EasyChangeInterval, MediumChangeInterval, HardChangeInterval;

        ContentManager Content;

        Viewport Screen;

        Random Raffle;

        public int NivelEasy , NivelMedium , NivelHard , EasyCounter , MediumCounter , HardCounter;

        public bool TopHard, CanCreateEasy, CanCreateMedium, CanCreateHard;

        float TopHardElapsedTime, TopHardInterval, CanEasyElapsedTime, CanEasyInterval, CanMediumElapsedTime, CanMediumInterval,
            CanHardElapsedTime, CanHardInterval;

        public TipoBorda BordaAtual;

        public BugManager(ContentManager content , Viewport screen)
        {
            Content = content;

            Screen = screen;

            Raffle = new Random();

            EasyElapsedTime = 0f;
            MediumElapsedTime = 0f;
            HardElapsedTime = 0f;

            EasyInterval = 4000f;
            MediumInterval = 8000f;
            HardInterval = 16000f;

            EasyElapsedTime2 = 0f;
            MediumElapsedTime2 = 0f;
            HardElapsedTime2 = 0f;

            EasyChangeInterval = 15000f;
            MediumChangeInterval = 25000f;
            HardChangeInterval = 40000f;

            NivelEasy = 1;
            NivelMedium = 1;
            NivelHard = 1;

            TopHard = false;

            TopHardElapsedTime = 0f;
            TopHardInterval = 15000f;

            CanCreateEasy = false;
            CanCreateMedium = false;
            CanCreateHard = false;

            CanEasyElapsedTime = 0f;
            CanMediumElapsedTime = 0f;
            CanHardElapsedTime = 0f;

            CanEasyInterval = 264f;
            CanMediumInterval = 264f;
            CanHardInterval = 264f;

            EasyCounter = 0;
            MediumCounter = 0;
            HardCounter = 0;

            Bugs = new List<Bug>();
        }

        public void Update(GameTime gameTime , Vector2 TargetPosition , float OppositeAngle)
        {
            EasyElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            MediumElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            HardElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            EasyElapsedTime2 += gameTime.ElapsedGameTime.Milliseconds;
            MediumElapsedTime2 += gameTime.ElapsedGameTime.Milliseconds;
            HardElapsedTime2 += gameTime.ElapsedGameTime.Milliseconds;

            # region Borda Atual

            float OppositeAngle_Degrees = MathHelper.ToDegrees(OppositeAngle);

            if (OppositeAngle_Degrees >= 0 && OppositeAngle_Degrees <= 45)
            {
                BordaAtual = TipoBorda.Right;
            }
            else if (OppositeAngle_Degrees > 45 && OppositeAngle_Degrees < 135)
            {
                BordaAtual = TipoBorda.Top;
            }
            else if (OppositeAngle_Degrees >= 135 && OppositeAngle_Degrees <= 225)
            {
                BordaAtual = TipoBorda.Left;
            }
            else if (OppositeAngle_Degrees > 225 && OppositeAngle_Degrees < 315)
            {
                BordaAtual = TipoBorda.Bottom;
            }
            else if (OppositeAngle_Degrees >= 315)
            {
                BordaAtual = TipoBorda.Right;
            }

            # endregion

            for (int i = 0; i < Bugs.Count; i++)
            {
                Bugs[i].Update(gameTime, TargetPosition);
            }

            # region Criando Insetos

            if (EasyElapsedTime >= EasyInterval)
            {
                EasyElapsedTime = 0f;

                CanCreateEasy = true;
            }

            if (MediumElapsedTime >= MediumInterval)
            {
                MediumElapsedTime = 0f;

                CanCreateMedium = true;
            }

            if (HardElapsedTime >= HardInterval)
            {
                HardElapsedTime = 0f;

                CanCreateHard = true;
            }

            # endregion

            # region CanCreate

            # region Easy

            CanEasyElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (CanEasyElapsedTime >= CanEasyInterval)
            {
                CanEasyElapsedTime = 0f;

                if (CanCreateEasy)
                {
                    ++EasyCounter;

                    Bugs.Add(new Bug(Content, Screen, TipoInseto.Easy, BordaAtual, OppositeAngle));

                    if (EasyCounter > 6)
                    {
                        EasyCounter = 0;

                        CanCreateEasy = false;
                    }
                }
            }

            # endregion

            # region Medium

            CanMediumElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (CanMediumElapsedTime >= CanMediumInterval)
            {
                CanMediumElapsedTime = 0f;

                if (CanCreateMedium)
                {
                    ++MediumCounter;

                    Bugs.Add(new Bug(Content, Screen, TipoInseto.Medium, BordaAtual, OppositeAngle));

                    if (MediumCounter > 4)
                    {
                        MediumCounter = 0;

                        CanCreateMedium = false;
                    }
                }
            }

            # endregion

            # region Hard

            CanHardElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (CanHardElapsedTime >= CanHardInterval)
            {
                CanHardElapsedTime = 0f;

                if (CanCreateHard)
                {
                    ++HardCounter;

                    Bugs.Add(new Bug(Content, Screen, TipoInseto.Hard, BordaAtual, OppositeAngle));

                    if (HardCounter > 2)
                    {
                        HardCounter = 0;

                        CanCreateHard = false;
                    }
                }
            }

            # endregion

            # endregion

            # region Aumentando a Dificuldade

            # region Easy

            if (NivelEasy < 3)
            {
                if (EasyElapsedTime2 >= EasyChangeInterval)
                {
                    EasyElapsedTime2 = 0f;

                    EasyChangeInterval /= 2;

                    ++NivelEasy;
                }
            }

            # endregion

            # region Medium

            if (NivelMedium < 3)
            {
                if (MediumElapsedTime2 >= MediumChangeInterval)
                {
                    MediumElapsedTime2 = 0f;

                    MediumChangeInterval /= 2;

                    ++NivelMedium;
                }
            }

            # endregion

            # region Hard

            if (NivelHard < 3)
            {
                if (HardElapsedTime2 >= HardChangeInterval)
                {
                    HardElapsedTime2 = 0f;

                    HardChangeInterval /= 2;

                    ++NivelHard;
                }
            }

            # endregion

            # endregion

            if (NivelHard == 3)
	        {
                TopHardElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

                if (TopHardElapsedTime >= TopHardInterval)
                {
                    TopHard = true;
                }
	        }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < Bugs.Count; i++)
            {
                Bugs[i].Draw(spriteBatch);
            }
        }
    }
}