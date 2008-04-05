using System;
using System.Collections.Generic;
using System.Text;
using SimpleAI.Behaviours;
using SimpleAI;
using Microsoft.Xna.Framework.Graphics;
using SimpleEngine.Core;

namespace TestFramework.SimpleAIEngineToGame
{
    public class DrawableAIBehaviourGoTo : AIBehaviourGoTo
    {

        DynamicLineBatch dynamicLineBatch;
        public DynamicLineBatch LineBatch
        {
            get { return dynamicLineBatch; }
            set { dynamicLineBatch = value; }
        }

        public DrawableAIBehaviourGoTo()
        {
        }

        ~DrawableAIBehaviourGoTo()
        {
        }

        public override void Draw()
        {
            // this behaviour has 2 subbehaviour
            // - FindPath
            // - FollowPath
            DrawPath();
            DrawProgress();
        }

        private void DrawPath()
        {
        }

        private void DrawProgress()
        {
            AIBehaviourFollowPath followBehaviour = (AIBehaviourFollowPath)this.subbehaviours[1];

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

            /*
            if (((AIBehaviourFollowPath)(currentBehaviour.SubBehaviours[1])).pathNodes != null)
            {
                for (int index = 0;
                    index < ((AIBehaviourFollowPath)(currentBehaviour.SubBehaviours[1])).pathNodes.Length;
                    index++)
                {
                    Color newColor;
                    if (((AIBehaviourFollowPath)(currentBehaviour.SubBehaviours[1])).pathNodes[index].nodeVisited == true)
                    {
                        newColor = Color.Yellow;
                    }
                    else
                    {
                        newColor = Color.White;
                    }
                    dynamicLineBatch.DrawLine
                    (
                        ((AIBehaviourFollowPath)(currentBehaviour.SubBehaviours[1])).pathNodes[index].biNormalStart + new Vector3(0, 0, 0.1f),
                        ((AIBehaviourFollowPath)(currentBehaviour.SubBehaviours[1])).pathNodes[index].biNormalEnd + new Vector3(0, 0, 0.1f),
                        newColor
                    );

                }
            }
             */


        }
    }
}
