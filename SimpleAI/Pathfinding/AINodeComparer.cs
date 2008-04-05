using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAI.Pathfinding
{
    public class AINodeComparer : IComparer<AIPathfinderNode>
    {
        public int Compare(AIPathfinderNode nodeA, AIPathfinderNode nodeB)
        {
            if (nodeA.F > nodeB.F)
                return 1;
            else if (nodeA.F < nodeB.F)
                return -1;
            return 0;
        }

    }
}
