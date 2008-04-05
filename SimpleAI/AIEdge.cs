using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAI
{
    public class AIEdge
    {
        private AINode startNode;
        private AINode endNode;
        private float cost;
        private float pathCost;
        
        /*private AIEnumEdge state = AIEnumEdge.Pending;

        public AIEnumEdge State
        {
            get { return state; }
        }
          */

        public float PathCost
        {
            get { return pathCost; }
            set { pathCost = value; }
        }


        public float Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        public AIEdge(AINode startNode, AINode endNode)
        {
            this.startNode = startNode;
            this.endNode = endNode;
        }

        public AINode StartNode
        {
            get { return startNode; }
            set { startNode = value; }
        }

        public AINode EndNode
        {
            get { return endNode; }
            set { endNode = value; }
        }

        public AIEdge()
        {
        }

        ~AIEdge()
        {
        }
    }
}
