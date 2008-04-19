using System;
using System.Collections.Generic;
using System.Text;
using SimpleAI.Actors;
using SimpleAI;
using SimpleEngine.Core;
using SimpleAI.Behaviours;
using Microsoft.Xna.Framework;

namespace TestFramework.SimpleAIEngineToGame
{
    public class DrawableAIGroupFormationAlpha : AIGroup
    {
        protected DynamicLineBatch lineBatch;
        public DynamicLineBatch LineBatch
        {
            get { return lineBatch; }
            set 
            { 
                lineBatch = value;

                for (int index = 0; index < actors.Count; index++)
                {
                    ((DrawableAICharacter)(actors[index])).DynamicLineBatch = lineBatch;
                    actors[index].Position = new Vector3(0, 0, 0);
                    
                }
            }
        }

            public DrawableAIGroupFormationAlpha()
                : base()
        {
        }

        public DrawableAIGroupFormationAlpha(int numberOfMembers)
            : base(numberOfMembers)
        {
        }

        ~DrawableAIGroupFormationAlpha()
        {
        }

        public override void SetMembersDistribution(int rowsNumber, int colsNumber, float rowsSeparation, float colsSeparation)
        {
            //base.SetMembersDistribution(rowsNumber, colsNumber, rowsSeparation, colsSeparation);

            Vector3 baseOffset = new Vector3(-((float)(rowsNumber) - 1.0f) * 0.5f * rowsSeparation, 2.0f, 0.0f);

            int actorIndex = 1; // first actor is always leader

            for (int col = 0; col < colsNumber; col++)
            {
                for (int row = 0; row < rowsNumber; row++)
                {
                    if (actorIndex < actors.Count)
                    {
                        ((AIBehaviourStayInFormation)(actors[actorIndex].CurrentBehaviour)).Offset =
                            actors[0].Position + baseOffset +
                                new Vector3(row * rowsSeparation, col * colsSeparation, 0);
                        
                    }
                    actorIndex++;
                }
            }
        }

        protected override void CreateGroupMember(int index)
        {
            // no need to call base method
            //base.CreateGroupMember(index);
            
            //1. Create an actor of your desired type...
            AIActor newMember = new DrawableAICharacter();
            this.actors.Add(ref newMember);
            if (index == 0)
            {
                newMember.UseTail = true;
            }
            newMember.Radius = 0.5f;

            //2. Create motion controller for this actor, and attach it 
            AIMotionController newMotionControler = new AIMotionControllerToPosition();
            newMember.MotionController = newMotionControler;

            //3. Set up motion controller parameters
            newMotionControler.MaxRotation = 720.0f; // rotate no faster then 90 degrees per second
            newMotionControler.MaxSpeed = 3.5f;     // max speed = 3 m /s

            // 3. Create and attach apropriate behaviour
            AIBehaviour newBehaviour = new AIBehaviourStayInFormation();
            
            // 4. Set formation leader
            if (index > 0)
            {
                ((AIBehaviourStayInFormation)(newBehaviour)).Leader = actors[0];
                newMember.CurrentBehaviour = newBehaviour;
            }

            newMember.DesiredOrientation = new Vector3(1, 0, 0);
            newMember.Orientation = newMember.DesiredOrientation;


        }
    }
}
