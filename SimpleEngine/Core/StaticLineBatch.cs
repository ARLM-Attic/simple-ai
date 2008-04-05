using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SimpleEngine.Core
{
    public class StaticLineBatch : LineBatch
    {
        struct StaticLineBatchComponent
        {
            /// <summary>
            /// The vertex declaration which is defines the line vertices.
            /// </summary>
            public VertexDeclaration vertexDeclaration;

            /// <summary>
            /// The line vertices.
            /// </summary>
            public VertexPositionColor[] vertices;

            /// <summary>
            /// The current index being "drawn" into the array.
            /// </summary>
            public int currentIndex;

            /// <summary>
            /// The current number of lines to be draw.
            /// </summary>
            public int lineCount;
        }

        private bool batchDirty;

        Queue<StaticLineBatchComponent> qComponents;
        StaticLineBatchComponent lastComponent;

        #region Constants
        /// <summary>
        /// The maximum number of vertices in the LineBatch vertex array.
        /// </summary>
        const int maxVertexCount = 512;
        #endregion

        /// <summary>
        /// The graphics device that renders the lines.
        /// </summary>
        GraphicsDevice graphicsDevice;

        /// <summary>
        /// The effect applied to the lines.
        /// </summary>
        BasicEffect effect;

        public StaticLineBatch(GraphicsDevice graphicsDevice)
        {
            // assign the graphics device parameter after safety-checking
            if (graphicsDevice == null)
            {
                throw new ArgumentNullException("graphicsDevice");
            }
            this.graphicsDevice = graphicsDevice;
            batchDirty = false;

            // create and configure the effect
            this.effect = new BasicEffect(graphicsDevice, null);
            this.effect.VertexColorEnabled = true;
            this.effect.TextureEnabled = false;
            this.effect.LightingEnabled = false;
            // configure the effect
            this.effect.World = Matrix.Identity;
            this.effect.View = Matrix.CreateLookAt(Vector3.Zero, Vector3.Forward,
                Vector3.Up);

            this.qComponents = new Queue<StaticLineBatchComponent>();

            lastComponent = new StaticLineBatchComponent();
            //lastComponent = slbComponent;

            //qComponents.Enqueue(lastComponent);
            batchDirty = true;

            lastComponent.vertexDeclaration = new VertexDeclaration(graphicsDevice,
                VertexPositionColor.VertexElements);

            lastComponent.vertices = new VertexPositionColor[maxVertexCount];

            lastComponent.currentIndex = 0;
            lastComponent.lineCount = 0;

        }

        public void SetProjection(Matrix projection)
        {
            if (effect != null)
            {
                effect.Projection = projection;
            }
        }

        public void SetView(Matrix view)
        {
            if (effect != null)
            {
                effect.View = view;
            }
        }

        public void AddLine(Vector3 start, Vector3 end, Color color)
        {
            AddLine(
                new VertexPositionColor(start, color),
                new VertexPositionColor(end, color));

        }

        public void AddLine(Vector2 start, Vector2 end, Color color)
        {
            AddLine(
                new VertexPositionColor(new Vector3(start, 0f), color),
                new VertexPositionColor(new Vector3(end, 0f), color));
        }


        public void AddLine(Vector3 start, Vector3 end,
            Color startColor, Color endColor)
        {
            AddLine(
                new VertexPositionColor(start, startColor),
                new VertexPositionColor(end, endColor));
        }

        public void AddLine(Vector2 start, Vector2 end,
            Color startColor, Color endColor)
        {
            AddLine(
                new VertexPositionColor(new Vector3(start, 0f), startColor),
                new VertexPositionColor(new Vector3(end, 0f), endColor));
        }

        public void AddLine(VertexPositionColor start, VertexPositionColor end)
        {
            if (lastComponent.currentIndex >= (lastComponent.vertices.Length - 2))
            {
                qComponents.Enqueue(lastComponent);

                lastComponent = new StaticLineBatchComponent();
                //lastComponent = slbComponent;

                               
                lastComponent.vertexDeclaration = new VertexDeclaration(graphicsDevice,
                    VertexPositionColor.VertexElements);

                lastComponent.vertices = new VertexPositionColor[maxVertexCount];

                lastComponent.currentIndex = 0;
                lastComponent.lineCount = 0;
                batchDirty = false;

            }

            lastComponent.vertices[lastComponent.currentIndex++] = start;
            lastComponent.vertices[lastComponent.currentIndex++] = end;

            lastComponent.lineCount++;
            batchDirty = true;
        }


        public void Draw()
        {
            if (batchDirty)
            {
                qComponents.Enqueue(lastComponent);

                lastComponent = new StaticLineBatchComponent();
                //lastComponent = slbComponent;


                lastComponent.vertexDeclaration = new VertexDeclaration(graphicsDevice,
                    VertexPositionColor.VertexElements);

                lastComponent.vertices = new VertexPositionColor[maxVertexCount];

                lastComponent.currentIndex = 0;
                lastComponent.lineCount = 0;
                batchDirty = false;
            }
            // configure the graphics device to render our lines
            graphicsDevice.RenderState.AlphaBlendEnable = true;
            graphicsDevice.RenderState.SourceBlend = Blend.SourceAlpha;
            graphicsDevice.RenderState.DestinationBlend = Blend.InverseSourceAlpha;

            if (camera != null)
            {

                effect.World =
                    rotation * 
                Matrix.CreateTranslation(position);
                effect.View = Matrix.CreateLookAt(
                                                    camera.VectorPos, 
                                                    camera.VectorLook,
                                                    camera.VectorUp
                                                  );
                effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                                        MathHelper.ToRadians(45.0f),
                                        800.0f / 600.0f, 1.0f, 10000.0f);

            }
            

            foreach (StaticLineBatchComponent component in qComponents)
            {
                if (component.vertexDeclaration != null) // TODO: Investigate this
                {
                    //Console.WriteLine(number);
                    graphicsDevice.VertexDeclaration = component.vertexDeclaration;

                    effect.Begin();
                    for (int i = 0; i < effect.CurrentTechnique.Passes.Count; i++)
                    {
                        effect.CurrentTechnique.Passes[i].Begin();
                        graphicsDevice.DrawUserPrimitives<VertexPositionColor>(
                            PrimitiveType.LineList, component.vertices, 0, component.lineCount);
                        effect.CurrentTechnique.Passes[i].End();
                    }
                    effect.End();
                }
            }

        }

        public void Dispose()
        {
            if (effect != null)
            {
                effect.Dispose();
                effect = null;
            }

            while (qComponents.Count > 0)
            {
                StaticLineBatchComponent componentToRemove = qComponents.Dequeue();
                if (componentToRemove.vertexDeclaration != null)
                {
                    componentToRemove.vertexDeclaration.Dispose();
                    componentToRemove.vertexDeclaration = null;                    
                }
            }

        } //  public void Dispose()
    }
}
