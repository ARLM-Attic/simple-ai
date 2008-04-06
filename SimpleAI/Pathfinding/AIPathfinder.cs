using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAI.Pathfinding
{
    public class AIPathfinder
    {

        protected PriorityQueueB<AIPathfinderNode> openQueue;
        public List<AIPathfinderNode> closeList;

        protected bool initialized = false;
        protected byte[,] grid;

        protected bool forceStop = false;
        protected bool stopped = false;
        protected bool found = false;

        protected static sbyte[,] direction = new sbyte[8,2]
            { 
                {0,-1} , {1,0}, {0,1}, {-1,0}, {1,-1}, {1,1}, {-1,1}, {-1,-1}
            };

        /// <summary>
        /// Character that provides cost calculating routine
        /// </summary>
        protected AIActor character;
        public AIActor Character
        {
            get { return character; }
            set { character = value; }
        }
        
        protected AIMap map;
        public AIMap Map
        {
            get { return map; }
            set { map = value; }
        }

        protected AINode startNode;
        public AINode StartNode
        {
            get { return startNode; }
            set { startNode = value; }
        }
        
        protected AINode endNode;
        public AINode EndNode
        {
            get { return endNode; }
            set { endNode = value; }
        }

        protected AIPathfinderNode parentNode;


        protected int heuristicEstimateValue = 3;
        public int HeuristicEstimateValue
        {
            get { return heuristicEstimateValue; }
            set { heuristicEstimateValue = value; }
        }

        protected AIPathfinderState state;
        public AIPathfinderState State
        {
            get { return state; }
        }
        
        
        public AIPathfinder()
        {
        }

        ~AIPathfinder()
        {
        }

        public virtual void Initilize()
        {
            if (initialized)
            {
                Exception err = new Exception("ERROR: AIPathfinder.Initialize() - Can't initialize more then once!");
            }

            if (map == null)
            {
                Exception err = new Exception("ERROR: AIPathfinder.Initialize() - Set map before calling this method!");
            }

            if (character == null)
            {
                Exception err = new Exception("ERROR: AIPathfinder.Initilize() - Set character before calling this method!");
            }
            
            openQueue = new PriorityQueueB<AIPathfinderNode>(new AINodeComparer());
            closeList = new List<AIPathfinderNode>();
            if (grid == null)
            {
                grid = new byte[map.Width, map.Height];


                // get the cost of each node as seend from character's perspective
                for (int w = 0; w < map.Width; w++)
                {
                    for (int h = 0; h < map.Height; h++)
                    {
                        grid[w, h] = character.TypeToCost(
                                map.Node(w, h).Type
                            );
                    }
                }
            }

            parentNode = new AIPathfinderNode();
            parentNode.G = 0;
            parentNode.H = heuristicEstimateValue;
            parentNode.F = parentNode.G + parentNode.H;
            parentNode.X = startNode.X;
            parentNode.Y = startNode.Y;
            parentNode.PX = parentNode.X;
            parentNode.PY = parentNode.Y;

            initialized = true;

            this.state = AIPathfinderState.Idle;
        }

        public void FindPath()
        {
            if (!initialized)
            {
                Exception err = new Exception("ERROR: AIPathfinder.FindPath() - Class not initilized yet!");
            }

            forceStop = false;
            stopped = false;
            found = false;
            openQueue.Clear();
            closeList.Clear();

            openQueue.Push(parentNode);
            this.state = AIPathfinderState.Working;

            //Console.WriteLine("Pathfinding from:[" + this.startNode.X + "-" + this.startNode.Y+ "] to: [" + this.endNode.X + " - " + this.endNode.Y + "]");

        }

        public virtual void Reset()
        {
            this.found = false;
            this.closeList.Clear();
            this.openQueue.Clear();
            this.stopped = false;
            this.state = AIPathfinderState.Idle;
            this.initialized = false;
            this.Initilize();
            this.FindPath();
        }

        public virtual void Iterate()
        {
            if (found)
            {
                return;
            }

            //for (int iteration = 0; iteration < 100; iteration++)
            {

                while (openQueue.Count > 0 && !forceStop)
                {
                    parentNode = openQueue.Pop();

                    if (parentNode.X == endNode.X && parentNode.Y == endNode.Y)
                    {
                        closeList.Add(parentNode);
                        found = true;
                        break;
                    }

                    for (int index = 0; index < 8; index++)
                    {
                        int newX = parentNode.X + direction[index, 0];
                        int newY = parentNode.Y + direction[index, 1];

                        if (newX < 0 || newY < 0 || newX >= map.Width || newY >= map.Height)
                        {
                            // probing out of map
                            continue;
                        }

                        AIPathfinderNode newNode = new AIPathfinderNode();
                        newNode.X = newX;
                        newNode.Y = newY;
                        newNode.G = (map.Node(newNode.X, newNode.Y)).Type;

                        int newG = parentNode.G + grid[newNode.X, newNode.Y];

                        if (newG == parentNode.G)
                        {
                            continue;
                        }

                        int foundInOpenIndex = -1;

                        for (int j = 0; j < openQueue.Count; j++)
                        {
                            if (openQueue[j].X == newNode.X && openQueue[j].Y == newNode.Y)
                            {
                                foundInOpenIndex = j;
                                break;
                            }
                        }
                        if (foundInOpenIndex != -1 && openQueue[foundInOpenIndex].G <= newG)
                        {
                            continue;
                        }

                        newNode.PX = parentNode.X;
                        newNode.PY = parentNode.Y;
                        newNode.G = newG;

                        newNode.H = heuristicEstimateValue * (
                            Math.Abs(newNode.X - endNode.X) +
                            Math.Abs(newNode.Y - endNode.Y));

                        newNode.F = newNode.G + newNode.H;

                        openQueue.Push(newNode);
                    }
                    closeList.Add(parentNode);
                    return;
                    //break;
                }

            }

            if (found)
            {
                AIPathfinderNode fNode = closeList[closeList.Count - 1];

                for(int i=closeList.Count - 1; i>=0; i--)
                {
                    if (fNode.PX == closeList[i].X && fNode.PY == closeList[i].Y || i == closeList.Count - 1)
                    {
                        fNode = closeList[i];
                    }
                    else
                    {
                        closeList.RemoveAt(i);
                    }
                }
                this.stopped = true;
                this.state = AIPathfinderState.Finished;
                return;
                
                //return mClose;
            }
            stopped= true;
            this.state = AIPathfinderState.Failed;
            return;

        }

    }
}
