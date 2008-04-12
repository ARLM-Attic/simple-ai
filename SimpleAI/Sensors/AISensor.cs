using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleAI.Sensors
{
    public class AISensor
    {
        protected AIActor owner;
        /// <summary>
        /// AIActor to whom this sensor is attached
        /// </summary>
        public AIActor Owner
        {
            get { return owner; }
            set { owner = value; }
        }

        protected Vector3 orientation;
        /// <summary>
        /// Orientation in actor's coordinates
        /// </summary>
        public Vector3 Orientation
        {
            get { return orientation; }
            set { orientation = value; }
        }


        protected Vector3 leftHandSideVector;
        public Vector3 LeftHandSideVector
        {
            get { return leftHandSideVector; }
        }


        protected Vector3 rightHandSideVector;
        public Vector3 RightHandSideVector
        {
            get { return rightHandSideVector; }
        }

        protected float range;
        /// <summary>
        /// How far can it sense (in AI units - meters)
        /// </summary>
        public float Range
        {
            get { return range; }
            set { range = value; }
        }

        protected float arc;
        /// <summary>
        /// angular range of this sensor in radians. This will constitute two
        /// halfs ranges.
        /// </summary>
        public float Arc
        {
            get { return arc; }
            set { arc = value; }
        }

        public AISensor()
        {
        }

        ~AISensor()
        {
        }

        public void Update(GameTime gameTime)
        {
            Matrix rotation = Matrix.Identity;

            rotation = Matrix.CreateRotationZ(this.arc * 0.5f);

            Vector3 vLeftFromCharsOrientation = Vector3.Transform(
                owner.Orientation, rotation);

            vLeftFromCharsOrientation.Normalize();
            this.leftHandSideVector = vLeftFromCharsOrientation * this.range + owner.Position;

            rotation = Matrix.CreateRotationZ(this.arc * -0.5f);
            Vector3 vRightFromCharsOrinetation = Vector3.Transform(
                owner.Orientation, rotation);

            vRightFromCharsOrinetation.Normalize();
            this.rightHandSideVector = vRightFromCharsOrinetation * this.range + owner.Position;

        }
    }
}
