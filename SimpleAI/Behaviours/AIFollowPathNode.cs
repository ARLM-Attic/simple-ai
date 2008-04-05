using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleAI.Behaviours
{
    public struct AIFollowPathNode
    {
        public Vector3 nodePosition;
        public Vector3 nodeDirection;
        public Vector3 biNormalStart;
        public Vector3 biNormalEnd;
        public bool biNormalGenerated;
        public bool nodeVisited;
        public int X;
        public int Y;
    }
}
