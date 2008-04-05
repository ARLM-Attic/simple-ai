using System;
using System.Collections.Generic;
using System.Text;
using SimpleAI.Pathfinding;
using Microsoft.Xna.Framework;

namespace SimpleAI.Behaviours
{
    public class AIBehaviourFindPath : AIBehaviour
    {

        protected AIPathfinder pathfinder;
        protected AINode startNode;
        protected AINode endNode;
        protected bool initialized = false;

        public AIPathfinder PathFinder
        {
            get { return pathfinder; }
        }

        public AINode StartNode
        {
            get { return startNode; }
            set
            {
                startNode = value;
            }
        }

        public AINode EndNode
        {
            get { return endNode; }
            set
            {
                endNode = value;
            }
        }

        public AIBehaviourFindPath()
        {
            pathfinder = new AIPathfinder();
        }

        public override void Iterate(GameTime gameTime)
        {
            if (!initialized)
            {
                pathfinder.Character = this.character;
                pathfinder.Map = this.map;
                pathfinder.StartNode = this.startNode;
                pathfinder.EndNode = this.endNode;
                pathfinder.Initilize();
                pathfinder.FindPath();
                initialized = true;
            }
            else
            {
                if (pathfinder.State == AIPathfinderState.Idle)
                {
                    pathfinder.FindPath();
                }
            }

            pathfinder.Iterate();

            switch (pathfinder.State)
            {
                case AIPathfinderState.Idle:
                    this.state = AIBehaviourState.Idle;
                    break;
                case AIPathfinderState.Working:
                    this.state = AIBehaviourState.Working;
                    break;
                case AIPathfinderState.Finished:
                    this.state = AIBehaviourState.Finished;
                    break;
                case AIPathfinderState.Failed:
                    this.state = AIBehaviourState.Failed;
                    break;
                default:
                    Exception err = new Exception("Unknown pathfinder state: " + pathfinder.State.ToString());
                    break;
            }

            // iterate subbehaviours
            base.Iterate(gameTime);
        }

        public override void Reset()
        {
            base.Reset();
            this.state = AIBehaviourState.Idle;
            pathfinder.Reset();
        }

        ~AIBehaviourFindPath()
        {
        }
    }
}
