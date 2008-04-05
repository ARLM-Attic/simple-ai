using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAI.Pathfinding
{
    public struct AIPathfinderNode
    {
        public int F;
        public int G;
        public int H;  // f = gone + heuristic
        public int X;
        public int Y;
        public int PX; // Parent
        public int PY;
    }
}
