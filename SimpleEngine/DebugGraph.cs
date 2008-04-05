using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections;
using Microsoft.Xna.Framework.Graphics;

using SimpleEngine.Graphs;

namespace SimpleEngine
{
    /// <summary>
    /// A base class for all graph-like object renderd by engine.
    /// </summary>
    public class DebugGraph
    {
        protected string name = "";
        /// <summary>
        /// Name to be rendered
        /// </summary>
        public virtual string Name
        {
            get { return name; }
        }

        protected Vector2 position;
        protected int width;
        protected int height;
        protected bool enabled = true;
        private GraphManager graphManager;

        protected float accumulator = 0.0f;
        protected double lastTotalSec = 0;
        protected float frequency = 0.0f;
        private int capacity = 0;
        protected ArrayList probesContainer;
        protected string outputExpression = "";
        //protected StringBuilder outputExpression = new StringBuilder(64);
        protected float maxSampleValue;

        protected bool capSet = false;
        protected float capValue = 0.0f;

        public float Cap
        {
            get { return capValue; }
            set { capValue = value; capSet = true; }
        }

        /// <summary>
        /// Left upper corner of the graph. Please note that position (0, 0) could 
        /// actually be something else in case we run this on XBOX360. System adds
        /// SafeDimensions value to this position during draw phase.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Width of the graph window (in pixels).
        /// Default value is 200.
        /// </summary>
        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        /// <summary>
        /// Height of the graph window (in pixels).
        /// Default value is 75
        /// </summary>
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        /// <summary>
        /// Denotes the state of a given graph. Enabled == true mean, this graph will be:
        /// updated and rendered. If Enabled == false - no Update method will be called!
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graphManager">reference to the graph manager</param>
        /// <param name="frequency">how often should it probe for a new value</param>
        /// <param name="capacity">how many probes store in memory and draw in graph window</param>
        public DebugGraph(ref GraphManager graphManager, float frequency, int capacity)
        {
            position = new Vector2(0.0f);
            width = 200;
            height = 75;
            this.frequency = frequency;
            this.capacity = capacity;
            probesContainer = new ArrayList(capacity);
            this.graphManager = graphManager;
            maxSampleValue = 1;

            for (int index = 0; index < capacity; index++)
            {
                probesContainer.Add(0.0f);
            }

        }

        ~DebugGraph()
        {
        }

        /// <summary>
        /// Creates a string that represents final text to be rendered by graph.
        /// Eg. "FPS: 60.0"...
        /// This has to be overwritten by a derived class
        /// </summary>
        public virtual void FormatOutputExpression()
        {
            //outputExpression = name + ":" + probesContainer[probesContainer.Capacity - 1];
        }


        /// <summary>
        /// This has to be overwritten by a derived class
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
        }

        protected virtual void Update(GameTime gameTime, float newValue)
        {
            accumulator += newValue;
            float timePassed = (float)(gameTime.TotalRealTime.TotalSeconds - lastTotalSec);

            if (timePassed >= frequency)
            {
                float newVal = accumulator / timePassed;
                accumulator = 0.0f;
                lastTotalSec = gameTime.TotalRealTime.TotalSeconds;
                probesContainer.RemoveAt(0);
                probesContainer.Add(newVal);
                FormatOutputExpression();

                maxSampleValue = 1;

                for (int index = 0; index < probesContainer.Count; index++)
                {
                    float fSample = (float)probesContainer[index];
                    if (fSample > maxSampleValue)
                    {
                        maxSampleValue = fSample;
                    }
                }

            }

        }

        /// <summary>
        /// Draws text related to a given graph. Usually it's
        /// graph's name and current value.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void DrawText(GameTime gameTime)
        {

            // render graph's name
            graphManager.SpriteBatch.DrawString(
                graphManager.SpriteFont, outputExpression,
                new Vector2(
                    graphManager.SafeDimensions.Left + position.X,
                    graphManager.SafeDimensions.Top + position.Y),
                Color.Yellow);

        }

        private void FromSampleToVector(ref Vector2 vPos, int index)
        {
            if (probesContainer.Count < 1)
            {
                return;
            }

            // calculate Y val

            float desiredX = width * index / (probesContainer.Count - 1) + graphManager.SafeDimensions.X + position.X;
            float desiredY = (float)probesContainer[index];
            if (capSet)
            {
                if (desiredY > capValue)
                {
                    desiredY = capValue;
                }

                //if (maxSampleValue > capValue)
                {
                    maxSampleValue = capValue;
                }
            }

            desiredY = height - height * desiredY / maxSampleValue;
            desiredY += graphManager.SafeDimensions.Y + position.Y;

            vPos.Y = desiredY;
            vPos.X = desiredX;

        }

        /// <summary>
        /// Draws outlining box and then the graph itself
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void DrawLines(GameTime gameTime)
        {
            // render outlining box
            Vector2 vFrom = new Vector2(0);
            Vector2 vTo = new Vector2(0);

            vFrom.X = graphManager.SafeDimensions.X + position.X - 1;
            vFrom.Y = graphManager.SafeDimensions.Y + position.Y - 1;
            vTo.X = vFrom.X + width + 2;
            vTo.Y = vFrom.Y;
            graphManager.LineBatch.DrawLine(vFrom, vTo, Color.Gray);
            vFrom.X = vTo.X;
            vFrom.Y = vTo.Y + height + 2;
            graphManager.LineBatch.DrawLine(vFrom, vTo, Color.Gray);
            vTo.Y = vFrom.Y;
            vTo.X = vFrom.X - width - 2;
            graphManager.LineBatch.DrawLine(vFrom, vTo, Color.Gray);
            vFrom.X = graphManager.SafeDimensions.X + position.X - 1;
            vFrom.Y = graphManager.SafeDimensions.Y + position.Y - 1;
            graphManager.LineBatch.DrawLine(vFrom, vTo, Color.Gray);

            if (probesContainer.Count > 0)
            {
                FromSampleToVector(ref vFrom, 0);
                for (int index = 1; index < probesContainer.Count; index++)
                {
                    FromSampleToVector(ref vTo, index);
                    graphManager.LineBatch.DrawLine(vFrom, vTo, Color.White);
                    vFrom = vTo;
                }
            }
        }
    }
}

