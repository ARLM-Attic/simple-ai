using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAI.Behaviours
{
    public class AIBehaviourCyclicRoute : AIBehaviour
    {

        protected List<AINode> nodesToVisit;
        protected bool initialized;
        protected int sequenceCounter;

        public AIBehaviourCyclicRoute()
        {
            initialized = false;
            nodesToVisit = new List<AINode>();
            
        }

        ~AIBehaviourCyclicRoute()
        {
        }

        public void AddPoint(ref AINode intermediateNode)
        {
            nodesToVisit.Add(intermediateNode);
        }

        public override void Reset()
        {
            sequenceCounter = 0;
            this.state = AIBehaviourState.Idle;
            base.Reset();
        }

        public override void OnBehaviourFinish(int finishedBehaviour)
        {
            base.OnBehaviourFinish(finishedBehaviour);
            sequenceCounter++;
            if (sequenceCounter > nodesToVisit.Count - 1)
            {
                this.Reset();
            }
            else
            {
                //currentSubBehaviour++;
            }
        }

        protected void OnSubbehaviourFinish(int finishedSubBehaviour)
        {
            if (finishedSubBehaviour >= nodesToVisit.Count)
            {
                this.Reset();
            }
        }

        public override void Iterate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!initialized)
            {
                subbehaviours = new AISubbehaviours(nodesToVisit.Count);
                if (character != null)
                {
                    // 1 add goto behaviour from character position to first node
                    AIBehaviour newBehaviour = new AIBehaviourGoTo();
                    newBehaviour.Character = this.character;
                    newBehaviour.Map = this.map;
                    /*((AIBehaviourGoTo)(newBehaviour)).StartNode = map.Node(7, 7);
                    ((AIBehaviourGoTo)(newBehaviour)).EndNode = nodesToVisit[0];
                    subbehaviours.Add(ref newBehaviour);
                    */

                    // loop
                    for (int index = 0; index < nodesToVisit.Count - 1; index++)
                    {
                        AIBehaviour newIBehaviour = new AIBehaviourGoTo();
                        newIBehaviour.Character = this.character;
                        newIBehaviour.Map = this.map;
                        ((AIBehaviourGoTo)(newIBehaviour)).StartNode = nodesToVisit[index];
                       ((AIBehaviourGoTo)(newIBehaviour)).EndNode = nodesToVisit[index + 1];
                       subbehaviours.Add(ref newIBehaviour);

                    }

                    newBehaviour = new AIBehaviourGoTo();
                    newBehaviour.Character = this.character;
                    newBehaviour.Map = this.map;
                    ((AIBehaviourGoTo)(newBehaviour)).StartNode = nodesToVisit[nodesToVisit.Count - 1];
                    ((AIBehaviourGoTo)(newBehaviour)).EndNode = nodesToVisit[0];
                    subbehaviours.Add(ref newBehaviour);

                    // add goto behaviour from last position to first position
                }
                initialized = true;
            }
            base.Iterate(gameTime);
        }

    }
}
