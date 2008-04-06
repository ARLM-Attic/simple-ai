using System;
using System.Collections.Generic;
using System.Text;
using SimpleAI.Pathfinding;
using Microsoft.Xna.Framework;

namespace SimpleAI.Behaviours
{
    public class AIBehaviourGoTo : AIBehaviour
    {
        protected AIBehaviour findPath = new AIBehaviourFindPath();
        protected AIBehaviour followPath = new AIBehaviourFollowPath();
        protected float tolerance;

        // How far from the desiret path a character can deviate and still
        // be recognised as if following path exactly.
        public float Tolarance
        {
            get { return tolerance; }
            set 
            {
                tolerance = value;
                // propagate new tolerance value to the follow path behaviour
                if (followPath != null)
                {
                    ((AIBehaviourFollowPath)(followPath)).Tolerance = value;
                }
            }
        }

        public AINode StartNode
        {
            get { return ((AIBehaviourFindPath)findPath).StartNode; }
            set 
            { 
                ((AIBehaviourFindPath)findPath).StartNode = value;
                value.Type = 1;
            
            }
        }

        public AINode EndNode
        {
            get { return ((AIBehaviourFindPath)findPath).EndNode; }
            set 
            { 
                ((AIBehaviourFindPath)findPath).EndNode = value;
                value.Type = 1;
            }
        }

        public AIBehaviourGoTo()
        {
            subbehaviours = new AISubbehaviours(2);
            subbehaviours.Add(ref findPath);            
            subbehaviours.Add(ref followPath);
        }

        ~AIBehaviourGoTo()
        {
        }


        public override void Iterate(GameTime gameTime)
        {
            if (((AIBehaviourFindPath)findPath).StartNode != null && 
                ((AIBehaviourFindPath)findPath).EndNode != null)
            {
                base.Iterate(gameTime);
            }
        }

        public override void OnBehaviourFinish(int finishedBehaviour)
        {
            
            if (finishedBehaviour == 0)
            {
                ((AIBehaviourFollowPath)(followPath)).PathFinder =
                    ((AIBehaviourFindPath)(findPath)).PathFinder;
            }

            base.OnBehaviourFinish(finishedBehaviour);
            
        }
        
    }
}
