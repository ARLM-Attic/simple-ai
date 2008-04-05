using System;
using System.Collections.Generic;
using System.Text;
using SimpleAI;
using SimpleEngine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestFramework.SimpleAIEngineToGame
{
    public class DrawableAICharacter : AIActor
    {

        protected DynamicLineBatch dynamicLineBatch;

        public DynamicLineBatch DynamicLineBatch
        {
            get { return dynamicLineBatch; }
            set { dynamicLineBatch = value; }
        }

        public DrawableAICharacter()
        {
        }

        ~DrawableAICharacter()
        {
        }

        public override void Draw()
        {
            base.Draw();    // make sure we draw behaviour(s) attached 
                            // to this character.

            dynamicLineBatch.DrawCylinder(position, 0.5f, 2.0f, 10, Color.White);            

            // draw desired orientation
            Vector3 halfSize = new Vector3(0, 0, 1);
            dynamicLineBatch.DrawLine(position + halfSize,
                                      position + halfSize + desiredOrientation * 4.0f,
                                      Color.GreenYellow);
        }
    }
}
