using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleAI
{
    public class AIMotionControllerToPosition : AIMotionController
    {
        public AIMotionControllerToPosition()
        {
        }

        ~AIMotionControllerToPosition()
        {
        }

        public override void Update(GameTime gameTime)
        {
            // calculate the angle between current orientation and
            // desired orientation
            Vector3 desiredOrientation = owner.DesiredPosition - owner.Position;
            desiredOrientation.Z = 0;
            desiredOrientation.Normalize();
            owner.DesiredOrientation = desiredOrientation;

            float desiredChangeRaw = Vector3.Dot(
                owner.Orientation,
                owner.DesiredOrientation);
            float desiredChange = (float)Math.Acos(desiredChangeRaw);

            Console.WriteLine(desiredChangeRaw);

            float timeFactor = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

            if (timeFactor == 0.0f)
            {
                return;
            }

            if (desiredChange > maxRotationRad * timeFactor && Math.Abs(desiredChange) > 0.0f)
            {
                // determine if we rotating clock or counter-clock wise,
                // then aply maxRotation only

                // create vector perpendicular to the desired direction;
                Vector3 vPerpendicular = new Vector3();
                Vector3 vUp = new Vector3(0, 0, 1);
                Vector3 vDes = owner.DesiredOrientation;
                vDes.Normalize();
                Vector3.Cross(ref vDes, ref vUp, out vPerpendicular);
                bool clockWise = (Vector3.Dot(owner.Orientation, vPerpendicular) >= 0.0f);

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
                
            }
            else
            {
                owner.Orientation = owner.DesiredOrientation;
            }

            if (desiredChangeRaw < 0.0f)
            {
                desiredChangeRaw = 0.0f;
            }


            float distanceToDesired = ((Vector3)(owner.DesiredPosition - owner.Position)).Length();

            float newSpeed = (1 + desiredChangeRaw) * 0.5f * maxSpeed;
            float distanceThisStep = newSpeed * timeFactor * distanceToDesired * 0.25f;


            if (distanceToDesired < distanceThisStep)
            {
                distanceThisStep = distanceToDesired;
            }




            if (distanceToDesired < 0.11f)
            {
                return;
            }


            owner.Position += owner.Orientation * distanceThisStep;//newSpeed * timeFactor;

        }

    }
}
