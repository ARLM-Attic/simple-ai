///////////////////////////////////////////////////////////
//  AIBehaviourFollowPath.cs
//  Implementation of the Class AIBehaviourFollowPath
//  Generated by Enterprise Architect
//  Created on:      20-mar-2008 20:30:13
//  Original author: piotrw
///////////////////////////////////////////////////////////




using SimpleAI;
using SimpleAI.Pathfinding;
using SimpleAI.Behaviours;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System;
namespace SimpleAI {
	public class AIBehaviourFollowPath : AIBehaviour {

        protected AIPathfinder pathFinder;
        protected int currentNode = 0;
        protected bool copiedPath = false;
        protected bool createdBiNormals = false;
        public AIFollowPathNode[] pathNodes;

        protected float tolerance;
        public float Tolerance
        {
            get { return tolerance; }
            set { tolerance = value; }
        }

        // taken from http://www.ziggyware.com/readarticle.php?article_id=78
        public bool IntersectionOfTwoLines(Vector3 a, Vector3 b, Vector3 c,
                                           Vector3 d, ref Vector3 result)
        {
            float r, s;

            float denominator = (b.X - a.X) * (d.Y - c.Y) - (b.Y - a.Y) * (d.X - c.X);

            // If the denominator in above is zero, AB & CD are colinear
            if (denominator == 0)
                return false;

            float numeratorR = (a.Y - c.Y) * (d.X - c.X) - (a.X - c.X) * (d.Y - c.Y);
            //  If the numerator above is also zero, AB & CD are collinear.
            //  If they are collinear, then the segments may be projected to the x- 
            //  or y-axis, and overlap of the projected intervals checked.

            r = numeratorR / denominator;

            float numeratorS = (a.Y - c.Y) * (b.X - a.X) - (a.X - c.X) * (b.Y - a.Y);

            s = numeratorS / denominator;

            //  If 0<=r<=1 & 0<=s<=1, intersection exists
            //  r<0 or r>1 or s<0 or s>1 line segments do not intersect
            if (r < 0 || r > 1 || s < 0 || s > 1)
                return false;

            ///*
            //    Note:
            //    If the intersection point of the 2 lines are needed (lines in this
            //    context mean infinite lines) regardless whether the two line
            //    segments intersect, then
            //
            //        If r>1, P is located on extension of AB
            //        If r<0, P is located on extension of BA
            //        If s>1, P is located on extension of CD
            //        If s<0, P is located on extension of DC
            //*/

            // Find intersection point
            result.X = (float)(a.X + (r * (b.X - a.X)));
            result.Y = (float)(a.Y + (r * (b.Y - a.Y)));

            return true;
        }

        

        public AIPathfinder PathFinder
        {
            get { return pathFinder; }
            set { pathFinder = value; }
        }
        
		public AIBehaviourFollowPath(){
            pathFinder = new AIPathfinder();
            tolerance = 2.0f;   // by default the tolerance when following path 
                                // is 1 meter each side

		}

		~AIBehaviourFollowPath(){

		}

		public override void Dispose(){

		}

        public override void Reset()
        {
            currentNode = 0;
            copiedPath = false;
            createdBiNormals = false;
            pathNodes = null;
            this.state = AIBehaviourState.Idle;
            
        }

        
        public override void Iterate(GameTime gameTime)
        {
            // make sure everything is ok.
            if (pathFinder != null)
            {
                if (pathFinder.State == AIPathfinderState.Finished)
                {
                    this.state = AIBehaviourState.Working;

                    if (copiedPath)
                    {
                        if (createdBiNormals)
                        {
                            // do the follow path thing
                            
                            // Get node we are going towards
                            //currentNode

                            if (currentNode >= pathNodes.Length)
                            {
                                // end!
                                this.state = AIBehaviourState.Finished;
                                return;
                            }

                            Vector3 vDistance = character.Position - pathNodes[currentNode].nodePosition;
                            float fDistance = vDistance.Length();

                            if (fDistance <= character.Radius)
                            {
                                pathNodes[currentNode].nodeVisited = true;
                                //Console.WriteLine("Visited node: [" +pathNodes[currentNode].X + "-" + pathNodes[currentNode].Y + "]");
                                currentNode++;

                                if (currentNode >= pathNodes.Length)
                                {
                                    // end!
                                    this.state = AIBehaviourState.Finished;
                                    return;
                                }

                                return;
                            }

                            //Vector3 registeredMotion =
                            //    character.Position - character.PreviousPosition;

                            // did we cross next bi-normal?
                            Vector3 dummyVector = new Vector3();

                            if (IntersectionOfTwoLines(
                                character.Position,
                                character.PreviousPosition,
                                pathNodes[currentNode].biNormalStart,
                                pathNodes[currentNode].biNormalEnd,
                                ref dummyVector))
                            {
                                pathNodes[currentNode].nodeVisited = true;
                                //Console.WriteLine("Visited node: [" + pathNodes[currentNode].X + "-" + pathNodes[currentNode].Y + "]");
                                currentNode++;
                            }

                            if (currentNode < pathNodes.Length)
                            {
                                Vector3 desiredDirection =
                                    pathNodes[currentNode].nodePosition -
                                    character.Position;

                                desiredDirection.Normalize();

                                character.DesiredDirection = desiredDirection;
                                character.DesiredOrientation = desiredDirection;
                                
                                }
                            else
                            {
                                character.DesiredDirection = new Vector3(0);
                                this.state = AIBehaviourState.Finished;
                            }
                                                        
                        }
                        else
                        {
                            // create bi-normals to help with path following.
                            for (int index = 0; index < pathNodes.Length - 1; index++)
                            {
                                pathNodes[index].nodeDirection =
                                    pathNodes[index + 1].nodePosition - pathNodes[index].nodePosition;

                                // create direction vector to the next node
                                pathNodes[index].nodeDirection.Normalize();
                            }

                            Vector3 vR = new Vector3(0, 0, 1);
                            Vector3 vB = new Vector3();
                            for (int index = 0; index < pathNodes.Length - 1; index++)
                            {
                                  //pathNodes[index].nodeDirection;

                                vB = Vector3.Cross(pathNodes[index].nodeDirection, vR);
                                vB.Normalize();
                                pathNodes[index].biNormalGenerated = true;
                                pathNodes[index].biNormalStart =
                                    pathNodes[index].nodePosition - vB * tolerance;

                                pathNodes[index].biNormalEnd =
                                    pathNodes[index].nodePosition + vB * tolerance;

                                pathNodes[index].nodeVisited = false;

                            }

                            // no direction for the last node, use bi-normal data
                            // from the previous one

                            pathNodes[pathNodes.Length - 1].biNormalGenerated = true;
                            pathNodes[pathNodes.Length - 1].biNormalStart = pathNodes[pathNodes.Length - 2].biNormalStart;
                            pathNodes[pathNodes.Length - 1].biNormalEnd = pathNodes[pathNodes.Length - 2].biNormalEnd;
                            pathNodes[pathNodes.Length - 1].nodeVisited = false;

                            // now shift binormal data accordingly.
                            Vector3 shiftVector = pathNodes[pathNodes.Length - 1].nodePosition -
                                pathNodes[pathNodes.Length - 2].nodePosition;

                            pathNodes[pathNodes.Length - 1].biNormalStart += shiftVector;
                            pathNodes[pathNodes.Length - 1].biNormalEnd += shiftVector;


                            createdBiNormals = true;

                        }
                    }
                    else
                    {
                        // copy path to the local container, reverse the nodes
                        // order
                        if (pathNodes != null)
                        {
                            //TODO:pathNodes.Clear();
                        }
                        else
                        {
                            // copying nodes
                            pathNodes = new AIFollowPathNode[pathFinder.closeList.Count];
                            int counter = 0;
                            //for (int index = pathFinder.closeList.Count - 1; index >= 0; index--)
                            for (int index = 0; index <= pathFinder.closeList.Count - 1; index++)
                            {
                                AIFollowPathNode newNode = new AIFollowPathNode();
                                newNode.nodePosition.X = map.Node(pathFinder.closeList[index].X,
                                                        pathFinder.closeList[index].Y).Position.X;
                                newNode.nodePosition.Y = map.Node(pathFinder.closeList[index].X,
                                                        pathFinder.closeList[index].Y).Position.Y;
                                newNode.X = pathFinder.closeList[index].X;
                                newNode.Y = pathFinder.closeList[index].Y;

                                pathNodes[counter] = newNode;
                                counter++;
                            }

                            copiedPath = true;

                        }
                    }
                   
                }
                else
                {
                    this.state = AIBehaviourState.Failed;
                    
                }
            }
            else
            {
                this.state = AIBehaviourState.Failed;
            }

            base.Iterate(gameTime);
        }

	}//end namespace Behaviours//end AIBehaviourFollowPath

}//end namespace SimpleAI