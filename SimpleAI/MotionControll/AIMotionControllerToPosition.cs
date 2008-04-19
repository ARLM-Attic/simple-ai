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

            if (owner.DesiredPosition == owner.Position)
            {
                return;
            }

            Vector3 desiredOrientation = owner.DesiredPosition - owner.Position;
            desiredOrientation.Z = 0;
            desiredOrientation.Normalize();
            owner.DesiredOrientation = desiredOrientation;

            float desiredChangeRaw = 0.0f;

            if (owner.Orientation != owner.DesiredOrientation)
            {
                desiredChangeRaw = Vector3.Dot(
                    owner.Orientation,
                    owner.DesiredOrientation);
            }

            float desiredChange = (float)Math.Acos(desiredChangeRaw);

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
            float distanceThisStep = newSpeed * timeFactor;// *distanceToDesired * 0.25f;

            if (distanceToDesired < owner.Radius * 4)
            {
                distanceThisStep = distanceThisStep * distanceToDesired * 0.25f;
            }


            if (distanceToDesired < distanceThisStep)
            {
                distanceThisStep = distanceToDesired;
            }

            //Console.WriteLine(distanceToDesired);

            if (distanceToDesired < 0.11f)
            {
                return;
            }

            //
            //
            // now calculate and apply flocking 

            //owner.NodesIamIn[0].;
            List<AINode> nodesToConsider = new List<AINode>(9);

            //nodesToConsider.Add(owner.NodesIamIn[0]);
            int xIndex = owner.NodesIamIn[0].X;
            int yIndex = owner.NodesIamIn[0].Y;

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    int nodeX = xIndex + x;
                    int nodeY = yIndex + y;

                    if (nodeX > 0 && nodeY > 0 && nodeX < owner.Map.Width && nodeY < owner.Map.Height)
                    {
                        nodesToConsider.Add(owner.Map.Node(nodeX, nodeY));
                    }

                }
            }

            Vector3 flockingVector = new Vector3(0);

            for (int nodeIndex = 0; nodeIndex < nodesToConsider.Count; nodeIndex++)
            {
                for (int actorIndex = 0; actorIndex < nodesToConsider[nodeIndex].Actors.Count; actorIndex++)
                {
                    if (nodesToConsider[nodeIndex].Actors[actorIndex] != owner)
                    {
                        Vector3 vDistance = owner.Position - nodesToConsider[nodeIndex].Actors[actorIndex].Position;
                        float fDistance = vDistance.Length();
                        if (fDistance <  4.0f * owner.Radius)
                        {
                            // this is too close! I want to avoid this
                            if (fDistance != 0.0f)
                            {
                                vDistance.Normalize();
                            }
                            else
                            {
                                // a way to generate random vector. ish.
                                vDistance = new Vector3(nodeIndex + 1, actorIndex + 1, 0);
                                vDistance.Normalize();
                            }
                            float force = 4.0f * owner.Radius - fDistance;
                            flockingVector = flockingVector + vDistance * force;
                        }
                    }
                }
            }

            flockingVector = flockingVector + owner.DesiredPosition - owner.Position;
            flockingVector.Z = 0.0f;

            if (flockingVector.Length() > 0.0f)
            {
                flockingVector.Normalize();
            }

            owner.Position += flockingVector * distanceThisStep;//newSpeed * timeFactor;

        }

    }
}
