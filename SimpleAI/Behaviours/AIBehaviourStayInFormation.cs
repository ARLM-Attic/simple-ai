using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleAI.Behaviours
{
    public class AIBehaviourStayInFormation : AIBehaviour
    {

        protected AIActor leader;
        /// <summary>
        /// An actor that will lead the formation, creating path to follow.
        /// Leader has to use tail, as this will be used to generate positions
        /// for formation members
        /// </summary>
        public AIActor Leader
        {
            get { return leader; }
            set { leader = value; }
        }

        protected Vector3 offset;
        public Vector3 Offset
        {
            get { return offset; }
            set
            {
                offset = value;
                float behind = offset.Y;

                if (behind < 0.0f)
                {
                    behind = 0.0f;
                }

                if (behind > leader.Tail.MinDistance * (leader.Tail.Capacity - 1))
                {
                    behind = leader.Tail.MinDistance * (leader.Tail.Capacity - 1);
                }

                offset.Y = behind;

            }
        }

        public AIBehaviourStayInFormation()
        {
        }

        ~AIBehaviourStayInFormation()
        {
        }

        public override void Iterate(Microsoft.Xna.Framework.GameTime gameTime)
        {

            character.DesiredDirection = Vector3.Zero;

            base.Iterate(gameTime);

            // check if we have a leader first!
            if (leader == null)
            {
                this.state = AIBehaviourState.Failed;
                return;
            }

            // does the leader have tail?
            if (leader.UseTail == false)
            {
                this.state = AIBehaviourState.Failed;
            }

            // is the tail long enough that we one can join formation?
            float currentMaxBehind = leader.Tail.Count * leader.Tail.MinDistance;
            if (currentMaxBehind < offset.Y)
            {
                // still can't join the formation
                return;
            }

            // we have all stuff we need, calculate the desired
            // position in this formation

            // estimate tail element we are going to follow
            int estimatedIndex = (int)(offset.Y / leader.Tail.MinDistance);

            // reverse edtimated index:
            estimatedIndex = leader.Tail.Count - estimatedIndex;

            if (estimatedIndex < 1)
            {
                estimatedIndex = 1;
            }

            Vector3 segmentDirection = leader.Tail[estimatedIndex - 1] - leader.Tail[estimatedIndex];
            segmentDirection.Normalize();

            Vector3 segmentDirPerpendicular;

            if (offset.X >= 0.0)
            {
                segmentDirPerpendicular = Vector3.Cross(segmentDirection, new Vector3(0,0,1));
                    
            }
            else
            {
                segmentDirPerpendicular = Vector3.Cross(segmentDirection, new Vector3(0,0,-1));
            }

            Vector3 desiredPosition =
                leader.Tail[estimatedIndex] + segmentDirPerpendicular * Math.Abs(offset.X);

            character.DesiredDirection = desiredPosition - leader.Position;
            character.DesiredDirection.Normalize();
            character.DesiredPosition = desiredPosition;

        }

        public override void Reset()
        {
            base.Reset();
            leader = null; // do we really need to get rid of the leader?
        }
    }
}
