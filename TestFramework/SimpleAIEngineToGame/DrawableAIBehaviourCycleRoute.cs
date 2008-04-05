using System;
using System.Collections.Generic;
using System.Text;
using SimpleAI.Behaviours;
using SimpleEngine.Core;
using SimpleAI;
using Microsoft.Xna.Framework.Graphics;

namespace TestFramework.SimpleAIEngineToGame
{
    public class DrawableAIBehaviourCycleRoute : AIBehaviourCyclicRoute
    {

        DynamicLineBatch dynamicLineBatch;
        public DynamicLineBatch LineBatch
        {
            get { return dynamicLineBatch; }
            set { dynamicLineBatch = value; }
        }


        public DrawableAIBehaviourCycleRoute()
        {
        }

        ~DrawableAIBehaviourCycleRoute()
        {
        }

        protected void DrawSubbehaviour(ref AIBehaviour behaviourToDraw)
        {
            // behaviourToDraw contains actually 2 subbehaviours
            // find path and follow path, so we have to draw
            // them separately

            AIBehaviourFollowPath followBehaviour = (AIBehaviourFollowPath)(behaviourToDraw.SubBehaviours[1]);

            if (followBehaviour == null)
            {
                return;
            }

            if (followBehaviour.pathNodes == null)
            {
                return;
            }

            Color pathColor;
            Color bisectorsColor;
            for (int index = 0; index < followBehaviour.pathNodes.Length; index++)
            {
                if (followBehaviour.pathNodes[index].nodeVisited)
                {
                    pathColor = Color.Gray;
                    bisectorsColor = Color.Gray;
                }
                else
                {
                    pathColor = Color.Gold;
                    bisectorsColor = Color.Honeydew;
                }

                if (index < followBehaviour.pathNodes.Length - 1)
                {
                    dynamicLineBatch.DrawLine(
                        followBehaviour.pathNodes[index].nodePosition,
                        followBehaviour.pathNodes[index + 1].nodePosition,
                        pathColor
                    );
                }

                dynamicLineBatch.DrawLine(
                    followBehaviour.pathNodes[index].biNormalStart,
                    followBehaviour.pathNodes[index].biNormalEnd,
                    bisectorsColor
                    );
            }


        }

        public override void Draw()
        {
            base.Draw();

            AIBehaviour behaviourToDraw = (AIBehaviour)this.subbehaviours[this.currentSubBehaviour];

            if (behaviourToDraw == null)
            {
                return;
            }

            this.DrawSubbehaviour(ref behaviourToDraw);

        }

    }
}
