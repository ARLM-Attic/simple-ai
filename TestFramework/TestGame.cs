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
using SimpleAI;
using SimpleEngine;
using SimpleEngine.Camera;
using TestFramework.SimpleAIEngineToGame;
using SimpleAI.Pathfinding;
using SimpleEngine.Core;
using SimpleAI.Behaviours;

namespace TestFramework
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class TestGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        AIEngine aiEngine;
        SEngine sEngine;
        AIPathfinder pathFinder;
        DynamicLineBatch lineBatch;
        AIActor character ;
        ThirdPersonCamera cam;
        
        public TestGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.graphics.PreferredBackBufferWidth = 640;
            this.graphics.PreferredBackBufferHeight = 480;
     
            // Game should run as fast as possible.
            IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;
                      
            aiEngine = new AIEngine((Game)this);
            this.Components.Add(aiEngine);

            AIWorld newWorld = new DrawableAIWorld();
            aiEngine.AddWorld(ref newWorld);

            pathFinder = new AIPathfinder();


            sEngine = new SEngine((Game)this, graphics);
            this.Components.Add(sEngine);

           
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            lineBatch = new DynamicLineBatch(GraphicsDevice);
                        
            AIMap newMap = new DrawableAIMap(GraphicsDevice, 55, 55, 100.0f, 100.0f);
            newMap.Position = new Vector3(
                GraphicsDevice.Viewport.Width * 0.5f,
                GraphicsDevice.Viewport.Height * 0.5f,
                .0f);

            aiEngine.World.Maps.Add(ref newMap);

            cam = new ThirdPersonCamera((Game)this);

            cam.DesiredPosition = new Vector3(0.0f, 50.0f, 50.0f);
            cam.Position = new Vector3(0.0f, 50.0f, 50.0f);
            cam.LookAt = new Vector3(0.0f, 0.0f, 0.0f);
            
            ((DrawableAIMap)(newMap)).Camera = cam;
            this.Components.Add(cam);
          
            // TODO: use this.Content to load your game content here
            aiEngine.Start();

            // Create and set up character
            character = new DrawableAICharacter();
            cam.Character = character;

            // Attach character to a map
            character.Map = newMap;

            // Reposition character
            character.Position = newMap.Node(7, 7).Position;
            character.Radius = 0.5f;

            AIBehaviourCyclicRoute bCycle = new DrawableAIBehaviourCycleRoute();
            
            AINode tmpNode = newMap.Node(5, 5);
            bCycle.AddPoint(ref tmpNode);

            tmpNode = newMap.Node(12, 10);
            bCycle.AddPoint(ref tmpNode);

            tmpNode = newMap.Node(40, 40);
            bCycle.AddPoint(ref tmpNode);

            tmpNode = newMap.Node(40, 22);
            bCycle.AddPoint(ref tmpNode);

            tmpNode = newMap.Node(5, 44);
            bCycle.AddPoint(ref tmpNode);

            ((DrawableAIBehaviourCycleRoute)(bCycle)).LineBatch = this.lineBatch;




            ///////
            //((DrawableAIBehaviourGoTo)(bGoto)).LineBatch = this.lineBatch;

            // Attach behaviour to character
            character.CurrentBehaviour = (AIBehaviour)bCycle;
            
            
            //bGoto.Character = character;
            //bGoto.Map = newMap;
            
            // Instruct Drawable character what to use for rendering
            ((DrawableAICharacter)(character)).DynamicLineBatch = this.lineBatch;
            
            // Attach character to world
            aiEngine.World.Characters.Add(ref character);

            lineBatch.Camera = cam;
            
            this.IsFixedTimeStep = false;
            graphics.SynchronizeWithVerticalRetrace = false;
            
#if XBOX
#else
            this.IsMouseVisible = true;
#endif 
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
            //pathFinder.Iterate();
            //character.Update(gameTime);
            //cam.LookAt = character.Position;
            aiEngine.Update(gameTime);

            //Console.WriteLine(character.CurrentBehaviour.State.ToString());
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            lineBatch.Begin();
            
                aiEngine.Draw();
            
            lineBatch.End();

            base.Draw(gameTime);
        }
    }
}
