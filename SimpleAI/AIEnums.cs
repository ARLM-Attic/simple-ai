using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAI
{
    public enum AIEnumEdge
    {
        Pending,    // this edge is pending creation
        Created,    // this edge has been created, but not yet visited
        Blocked,    // this edge had no end node
        Visited     // this edge has beed visited
    }
}
