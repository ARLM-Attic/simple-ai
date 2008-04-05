///////////////////////////////////////////////////////////
//  AIEngine.cs
//  Implementation of the Class AIEngine
//  Generated by Enterprise Architect
//  Created on:      20-mar-2008 20:30:14
//  Original author: piotrw
///////////////////////////////////////////////////////////


using Microsoft.Xna.Framework;
namespace SimpleAI {
    public class AIEngine : DrawableGameComponent
    {

        private Game game;
        //public AIWorlds Worlds;
        private AIWorld world;

        private static float unitToMeterRatio = 1.0f;

        /// <summary>
        /// How many meters in a single unit in the SimpleAIEngine Universe.
        /// </summary>
        public float UnitToMeterRatio
        {
            get { return unitToMeterRatio; }
        }
		
        /// <summary>
        /// AIWorld contain the representation of a level in a game
        /// </summary>
        public AIWorld World
        {
            get { return world; }
        }

        /// <summary>
        /// TODO: Revise
        /// </summary>
        /// <param name="newWorld"></param>
        public void AddWorld(ref AIWorld newWorld)
        {
            world = newWorld;
        }

        public AIEngine(Game game) : base(game)
        {
            this.game = game;
            //Worlds = new AIWorlds();
        }

        public override void Initialize()
        {
        }

		~AIEngine(){

		}

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            world.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (this.world != null)
            {
                this.world.Draw();
            }
        }

        public virtual void Draw()
        {
            if (this.world != null)
            {
                this.world.Draw();
            }
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

	}//end AIEngine

}//end namespace SimpleAI