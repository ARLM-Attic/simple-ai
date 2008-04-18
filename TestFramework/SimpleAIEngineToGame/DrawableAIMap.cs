using System;
using System.Collections.Generic;
using System.Text;
using SimpleAI;
using SimpleEngine.Core;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SimpleEngine;
using SimpleEngine.Camera;

namespace TestFramework.SimpleAIEngineToGame
{
    class DrawableAIMap : AIMap
    {
        private StaticLineBatch staticLineBatch;
        private GraphicsDevice graphicsDevice;
        private SimpleCamera camera;

        protected Vector3 shiftDown = new Vector3(0, 0, 0.1f);

        public SimpleCamera Camera
        {
            get { return camera; }
            set 
            {
                camera = value;
                staticLineBatch.Camera = camera;
            }
        }
        
        /// <summary>
        /// Drawable map derived from AIMap. Uses StaticLineBatch to create a map grid. 
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="horizontalSpan"></param>
        /// <param name="verticalSpan"></param>
        public DrawableAIMap(GraphicsDevice graphicsDevice, int width, int height, float horizontalSpan, float verticalSpan)
            : base(width, height, horizontalSpan, verticalSpan)
        {
            this.graphicsDevice = graphicsDevice;
            staticLineBatch = new StaticLineBatch(graphicsDevice);
            
            Vector3 startEdge = new Vector3();
            startEdge.X = this.HorizontalSpan *-0.5f; // graphicsDevice.Viewport.Width * 0.5f;
            startEdge.Y = this.VerticalSpan *-0.5f;// graphicsDevice.Viewport.Height * 0.5f;
            Vector3 tempEdge = new Vector3();

            float horizontalStep = (float)(this.HorizontalSpan / width );
            float verticalStep = (float)(this.VerticalSpan / height );

            startEdge.X = startEdge.X;// -horizontalStep * 0.5f;
            startEdge.Y = startEdge.Y;// -verticalStep * 0.5f;

            /*
            for (int indexWidth = 0; indexWidth <= width ; indexWidth++)
            {
                tempEdge = tempEdge = startEdge + new Vector3(horizontalStep * (float)indexWidth, 0.0f, 0.0f);
                staticLineBatch.AddLine(
                    shiftDown + tempEdge,
                    shiftDown + tempEdge + new Vector3(0.0f, verticalSpan, 0.0f),
                    Color.SlateGray);
            }

            for (int indexHeight = 0; indexHeight <= height ; indexHeight++)
            {
                tempEdge = tempEdge = startEdge + new Vector3(0.0f, verticalStep * (float)indexHeight, 0.0f);
                staticLineBatch.AddLine(
                    shiftDown +tempEdge,
                    shiftDown+ tempEdge + new Vector3(horizontalSpan, 0.0f, 0.0f),
                    Color.SlateGray);
            }
             */

            Vector3 vShiftLeftTop = new Vector3(-horizontalStep * 0.5f, -horizontalStep * 0.5f, 0.0f);
            Vector3 vShiftRightTop = new Vector3(horizontalStep * 0.5f, -horizontalStep * 0.5f, 0.0f);
            Vector3 vShiftRightBottom = new Vector3(horizontalStep * 0.5f, horizontalStep * 0.5f, 0.0f);
            Vector3 vShiftLeftBottom = new Vector3(-horizontalStep * 0.5f, horizontalStep * 0.5f, 0.0f);

            for (int indexWidth = 0; indexWidth < width; indexWidth++)
            {
                for (int indexHeight = 0; indexHeight < height; indexHeight++)
                {
                    staticLineBatch.AddLine(
                        this.Node(indexWidth, indexHeight).Position + vShiftLeftTop,
                        this.Node(indexWidth, indexHeight).Position + vShiftRightTop,
                        Color.SlateGray);

                    staticLineBatch.AddLine(
                        this.Node(indexWidth, indexHeight).Position + vShiftRightTop,
                        this.Node(indexWidth, indexHeight).Position + vShiftRightBottom,
                        Color.SlateGray);

                    staticLineBatch.AddLine(
                        this.Node(indexWidth, indexHeight).Position + vShiftRightBottom,
                        this.Node(indexWidth, indexHeight).Position + vShiftLeftBottom,
                        Color.SlateGray);

                    staticLineBatch.AddLine(
                        this.Node(indexWidth, indexHeight).Position + vShiftLeftTop,
                        this.Node(indexWidth, indexHeight).Position + vShiftLeftBottom,
                        Color.SlateGray);




                } // next indexHeight;
            } // next indexWidth
        } // DrawableAIMap

        ~DrawableAIMap()
        {
        }

        public override void SetProjection(Matrix projection)
        {
            staticLineBatch.SetProjection(projection);
        }

        public override void Draw()
        {
            
            this.staticLineBatch.Draw();
        }

        public override void Dispose()
        {
            this.staticLineBatch.Dispose();
        }
    }
}
