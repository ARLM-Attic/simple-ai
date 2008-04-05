using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using SimpleEngine.Core;
using SimpleEngine.Graphs;

namespace SimpleEngine
{
    public class SEngine : DrawableGameComponent
    {
        protected Game game;
        protected SafeRegion safeRegion;
        protected GraphicsDeviceManager graphicsManager;

        protected bool gcGraph = false;
        protected Rectangle gcGraphWindow;
        protected DebugGraph debugGCGraph;
        protected DebugGraph debugFPSGraph;

        protected GraphManager debugGraphManager;


        /// <summary>
        /// Determines the GC graph position and size
        /// </summary>
        public Rectangle GCGraphWindow
        {
            get { return gcGraphWindow; }
            set { gcGraphWindow = value; }
        }

        /// <summary>
        /// Determines if GC graph is being rendered
        /// </summary>
        public bool RenderGCGraph
        {
            get { return gcGraph; }
            set { gcGraph = value; }
        }

        public void AddGraph(DebugGraph newGraph)
        {
            this.debugGraphManager.AddGraph(ref newGraph);
        }

        public SEngine(Game game, GraphicsDeviceManager graphicsManager)
            : base(game)
        {
            this.game = game;
            this.graphicsManager = graphicsManager;

            safeRegion = new SafeRegion(graphicsManager);
            gcGraphWindow = new Rectangle();
            debugGraphManager = new GraphManager(game, this);
            debugGraphManager.SafeDimensions = safeRegion.SafeDimensions;

            debugGCGraph = new GCGraph(ref debugGraphManager, 0.5f, 50);
            debugGCGraph.Position = new Vector2(0, 0);
            debugGraphManager.AddGraph(ref debugGCGraph);
            debugFPSGraph = new FPSGraph(ref debugGraphManager, 0.5f, 50);
            debugFPSGraph.Position = new Vector2(0, debugGCGraph.Height + 5);
            debugGraphManager.AddGraph(ref debugFPSGraph);

            game.Components.Add(debugGraphManager);
        }
     
        public override void Initialize()
        {
            //------------------------------------------------------------
            // Set up GC window stuff. GC window is the default feature of
            // the SimpleEngine.
            //
            gcGraphWindow.X = safeRegion.SafeDimensions.X;
            gcGraphWindow.Y = safeRegion.SafeDimensions.Y;
            gcGraphWindow.Width = (int)(safeRegion.SafeHeight * 1.0f);
            gcGraphWindow.Height = 140;

            debugGraphManager.Initialize();

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            debugGraphManager.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
