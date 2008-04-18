using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SimpleAI;
using SimpleEngine;

namespace TestFramework
{
    /// <summary>
    /// Class translating controller input data info AIActor movement
    /// </summary>
    public class UserMotionController : AIMotionController
    {

        protected Game game;
        public Game Game
        {
            get { return game; }
            set { game = value; }
        }

        protected KeyState keyState;
        public KeyState KeyState
        {
            set { keyState = value; }
        }

        public UserMotionController()
        {
        }

        ~UserMotionController()
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {

            float timeFactor = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            if (timeFactor == 0.0f)
            {
                return;
            }

            float desiredChange = MathHelper.ToRadians(maxRotation) * timeFactor * -GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;

            // rotate Orientation vector by desiredCharge
            Matrix rotation = Matrix.Identity;
            Matrix.CreateRotationZ(desiredChange, out rotation);

            Vector3 vDes = Vector3.Transform(owner.Orientation, rotation);
            vDes.Normalize();

            owner.Orientation = vDes;
            owner.Position = owner.Position +
                owner.Orientation * maxSpeed * GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y * timeFactor;

        }
    }
}
