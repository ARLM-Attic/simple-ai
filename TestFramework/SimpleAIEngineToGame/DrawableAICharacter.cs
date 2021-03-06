using System;
using System.Collections.Generic;
using System.Text;
using SimpleAI;
using SimpleEngine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TestFramework.SimpleAIEngineToGame
{
    public class DrawableAICharacter : AIActor
    {

        /// <summary>
        /// Colour used for rendering this characted debug represenation (e.g. a cylinder).
        /// Default value set to Color.White
        /// </summary>
        protected Color debugColor;
        public Color DebugColor
        {
            get { return debugColor; }
            set { debugColor = value; }
        }

        protected DynamicLineBatch dynamicLineBatch;

        public DynamicLineBatch DynamicLineBatch
        {
            get { return dynamicLineBatch; }
            set { dynamicLineBatch = value; }
        }

        public DrawableAICharacter()
        {
            debugColor = Color.White;
        }

        protected bool drawDesiredPosition = false;
        public bool DrawDesiredPosition
        {
            get { return drawDesiredPosition; }
            set { drawDesiredPosition = value; }
        }
     
        protected bool drawDesiredOrientation = false;
        public bool DrawDesiredOrientation
        {
            get { return drawDesiredOrientation; }
            set { drawDesiredOrientation = value; }
        }

        protected bool drawOrientation = false;
        public bool DrawOrientation
        {
            get { return drawOrientation; }
            set { drawOrientation = value; }
        }

        protected bool drawNodeAtPosition = false;
        public bool DrawNodeAtPosition
        {
            get { return drawNodeAtPosition; }
            set { drawNodeAtPosition = value; }
        }

        ~DrawableAICharacter()
        {
        }

        public override void Draw()
        {
            base.Draw();    // make sure we draw behaviour(s) attached 
                            // to this character.

            if (drawNodeAtPosition)
            {
                dynamicLineBatch.DrawLine(
                    map.Node(debugX, debugY).Position,
                    map.Node(debugX, debugY).Position + new Vector3(0, 0, 15),
                    Color.Green);
            }

            dynamicLineBatch.DrawCylinder(position, 0.5f, 0.0f, 10, debugColor);

            Vector3 halfSize = new Vector3(0, 0, 1);
            if (drawOrientation)
            {
                dynamicLineBatch.DrawLine(position + halfSize,
                                          position + halfSize + this.Orientation * 4.0f,
                                          Color.GreenYellow);
            }

            if (drawDesiredOrientation)
            {
                dynamicLineBatch.DrawLine(position + halfSize * 0.5f,
                              position + halfSize * 0.5f + this.desiredOrientation * 4.0f,
                              Color.Purple);
            }

            if (useTail)
            {
                for (int index = 0; index < tail.Count - 1; index++)
                {
                    dynamicLineBatch.DrawLine(
                        tail[index],
                        tail[index + 1],
                        Color.Blue);
                }
            }

            if (drawDesiredPosition)
            {
                dynamicLineBatch.DrawLine(
                    desiredPosition,
                    desiredPosition + new Vector3(0, 0, 5),
                    Color.Yellow);
            }

            Matrix rotation = Matrix.Identity;
                      
            for (int index = 0; index < sensors.Count; index++)
            {
                rotation = Matrix.CreateRotationZ(sensors[index].Arc * 0.5f);

                Vector3 vLeftFromCharsOrientation = Vector3.Transform(
                    this.Orientation, rotation);

                vLeftFromCharsOrientation.Normalize();
                vLeftFromCharsOrientation = vLeftFromCharsOrientation * sensors[index].Range + this.Position;

                rotation = Matrix.CreateRotationZ(sensors[index].Arc * -0.5f);
                Vector3 vRightFromCharsOrinetation = Vector3.Transform(
                    this.Orientation, rotation);

                vRightFromCharsOrinetation.Normalize();
                vRightFromCharsOrinetation = vRightFromCharsOrinetation * sensors[index].Range + this.Position;

                dynamicLineBatch.DrawLine(this.Position, vLeftFromCharsOrientation, Color.Gray);
                dynamicLineBatch.DrawLine(vLeftFromCharsOrientation, vRightFromCharsOrinetation, Color.Gray);
                dynamicLineBatch.DrawLine(vRightFromCharsOrinetation, this.Position, Color.Gray);
            }

        }
    }
}
