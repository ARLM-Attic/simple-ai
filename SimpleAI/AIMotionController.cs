using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleAI
{
    // Controls the way an AIActor moves.
    public class AIMotionController
    {
        protected float maxSpeed;
        protected float maxRotation;
        protected float maxRotationRad;
        protected AIActor owner;

        // Owner of this MotionController
        public AIActor Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        // Determines the maximum rotation rate that can be achibed by a character.
        // Units degrees per second
        public float MaxRotation
        {
            get { return maxRotation; }
            set 
            { 
                maxRotation = value;
                maxRotationRad = MathHelper.ToRadians(maxRotation);
            }
        }

        // Determines the maximum speed that can be achived by a character.
        // Units: meters per second
        public float MaxSpeed
        {
            get { return maxSpeed; }
            set { maxSpeed = value; }
        }

        public AIMotionController()
        {

        }

        ~AIMotionController()
        {
        }

        public virtual void Update(GameTime gameTime)
        {
            // calculate the angle between current orientation and
            // desired orientation
            float desiredChangeRaw = Vector3.Dot(
                owner.Orientation,
                owner.DesiredDirection);
            float desiredChange = (float)Math.Acos(desiredChangeRaw);

            float timeFactor = gameTime.ElapsedRealTime.Milliseconds / 1000.0f;

            if (timeFactor == 0.0f)
            {
                return;
            }

            if (desiredChange > maxRotationRad * timeFactor)
            {
                // determine if we rotating clock or counter-clock wise,
                // then aply maxRotation only

                // create vector perpendicular to the desired direction;
                Vector3 vPerpendicular = new Vector3();
                Vector3 vUp = new Vector3(0, 0, 1);
                Vector3 vDes = owner.DesiredDirection;
                vDes.Normalize();
                Vector3.Cross(ref vDes, ref vUp, out vPerpendicular);
                bool clockWise = (Vector3.Dot(owner.Orientation, vPerpendicular) > 0.0f);

/*                if (clockWise)
                {
                    vDes.X = vDes.X * (float)Math.Cos(-maxRotationRad * timeFactor);
                    vDes.Y = vDes.Y * (float)Math.Sin(-maxRotationRad * timeFactor);
                }
                else
                {
                    vDes.X = vDes.X * (float)Math.Cos(maxRotationRad * timeFactor);
                    vDes.Y = vDes.Y * (float)Math.Sin(maxRotationRad * timeFactor);
                }
 */

                Matrix rotation = Matrix.Identity;
                if (clockWise)
                {
                    Matrix.CreateRotationZ(maxRotationRad * timeFactor, out rotation);
                }
                else
                {
                    Matrix.CreateRotationZ(-maxRotationRad * timeFactor, out rotation);
                }


                vDes = Vector3.Transform(owner.Orientation, rotation);
                vDes.Normalize();
  

                owner.Orientation = vDes;
                //owner.DesiredDirection = vDes;

            }
            else
            {
                owner.Orientation = owner.DesiredDirection;
            }

            float newSpeed = ((desiredChangeRaw + 1.0f) * 0.5f) * maxSpeed;

            owner.Position += owner.Orientation * newSpeed * timeFactor;

            // set a new orientation
                



        }
    }
}
