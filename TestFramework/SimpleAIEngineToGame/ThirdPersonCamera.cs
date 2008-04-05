using System;
using System.Collections.Generic;
using System.Text;
using SimpleEngine.Camera;
using Microsoft.Xna.Framework;
using SimpleAI;

namespace TestFramework.SimpleAIEngineToGame
{
    public class ThirdPersonCamera : SimpleCamera
    {

        protected AIActor character;
        public AIActor Character
        {
            get { return character; }
            set { character = value; }
        }

        protected Vector3 offset;
        public Vector3 Offset
        {
            get { return offset; }
            set { offset = value; }
        }

        public ThirdPersonCamera(Game game)
            : base(game)
        {
            // set up some default offset
            offset = new Vector3(0, 0, 10);
        }

        public override void Update(GameTime gameTime)
        {
            // fiddle with parameters and let the base class do
            // the rest
            if (character != null)
            {
                this.desiredPosition = character.Position + offset -character.DesiredOrientation * 25.0f;
                this.vectorLook = character.Position + new Vector3(0, 0, 0);
            }

            base.Update(gameTime);
        }
    }
}
