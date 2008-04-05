using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using SimpleEngine.Core;

namespace SimpleEngine
{
    public class GraphManager : DrawableGameComponent
    {
        private List<DebugGraph> graphs = new List<DebugGraph>();
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private DynamicLineBatch lineBatch;
        private Rectangle safeDimensions;
        private SEngine engine;

        public SEngine Engine
        {
            get { return engine; }
        }

        public Rectangle SafeDimensions
        {
            get { return safeDimensions; }
            set { safeDimensions = value; }
        }

        public SpriteBatch SpriteBatch
        {
            get { return spriteBatch; }
        }

        public SpriteFont SpriteFont
        {
            get { return font; }
        }

        public DynamicLineBatch LineBatch
        {
            get { return lineBatch; }
        }

        public int GraphCount
        {
            get { return graphs.Count; }
        }

        public GraphManager(Game game, SEngine engine)
            : base(game)
        {
            this.engine = engine;
        }

        ~GraphManager()
        {

        }

        public override void Initialize()
        {
            
            base.Initialize();
            
            
        }

        protected override void LoadContent()
        {

            lineBatch = new DynamicLineBatch(GraphicsDevice);

            // Load content belonging to the screen manager.
            ContentManager content = Game.Content;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = content.Load<SpriteFont>("Fonts/debugSmall");
            
            lineBatch.SetProjection(Matrix.CreateOrthographicOffCenter(0.0f,
                GraphicsDevice.Viewport.Width,
                GraphicsDevice.Viewport.Height, 0.0f, 0.0f, 1.0f));

        }

        protected override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
            for (int index = 0; index < graphs.Count; index++)
            {
                graphs[index].Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            LineBatch.Begin();
            //foreach (DebugGraph graph in graphs)
            for (int index = 0; index < graphs.Count; index++)
            {
                graphs[index].DrawLines(gameTime);
            }
            LineBatch.End();

            SpriteBatch.Begin();
            //foreach (DebugGraph graph in graphs)
            for (int index = 0; index < graphs.Count; index++)
            {
                graphs[index].DrawText(gameTime);
            }
            SpriteBatch.End();

        }

        public void AddGraph(ref DebugGraph newGraph)
        {
            graphs.Add(newGraph);
        }


    }
}
