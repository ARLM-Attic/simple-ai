///////////////////////////////////////////////////////////
//  AIActor.cs
//  Implementation of the Class AIActor
//  Generated by Enterprise Architect
//  Created on:      20-mar-2008 20:30:14
//  Original author: piotrw
///////////////////////////////////////////////////////////




using Microsoft.Xna.Framework;
using System.Collections;
using SimpleAI.Behaviours;
using SimpleAI.Actors;
using System;
using SimpleAI.Sensors;
namespace SimpleAI {
	public class AIActor : IAINameOwner {

        /*
         * 
         *  TODO: implement
        private Vector3 lastGoodPosition;

        /// <summary>
        /// Last known good position. In case character gets out of map, this could
        /// be used as a point of return to the map. All in SimpleAIEngine units
        /// </summary>
        public Vector3 LastGoodPosition
        {
            get { return lastGoodPosition; }
        }
         */

        protected AISensors sensors;
        public void AddSensor(ref AISensor newSensor)
        {
            newSensor.Owner = this;
            sensors.Add(ref newSensor);
        }

        protected int tailCapacity = 500;
        public int TailCapacity
        {
            get { return tailCapacity; }
            set { tailCapacity = value; }
        }

        protected bool useTail;
        public bool UseTail
        {
            get { return useTail; }
            set 
            { 
                useTail = value;
                if (useTail == true)
                {
                    if (tail == null)
                    {
                        tail = new AITail(tailCapacity);

                    }
                    else
                    {
                        tail.Resize(tailCapacity);
                    }
                }
                else
                {
                    tail.Reset();
                    tail = null;
                }
            }
        }

        protected AITail tail;

        protected AIMotionController motionController;
        public AIMotionController MotionController
        {
            get { return motionController; }
            set 
            {
                motionController = value;
                motionController.Owner = this;
            }
        }

        protected bool collidable;
        public bool Collidable
        {
            get { return collidable; }
            set { collidable = value; }
        }

        protected AIMap map;
        public AIMap Map
        {
            get { return this.map; }
            set
            {
                this.map = value;
                this.isDirty = true;    // a new map is being set.
            }
        }

        protected bool isActive;
        public bool IsActive
        {
            get { return this.isActive; }
            set { this.isActive = value; }
        }

        protected Vector3 position;
        
        /// <summary>
        /// Position of the character. All in SimpleAIEngine units. Setting this property
        /// externally could trigger data reset as character will be marks as dirty
        /// </summary>
        public Vector3 Position
        {
            get { return this.position; }
            set 
            { 
                this.position = value;
                this.isDirty = true;
            }
        }

        protected Vector3 desiredDirection;
        /// <summary>
        /// Sets a direction you want this character to move, when called update
        /// character's code should deal with this request adequatly
        /// </summary>
        public Vector3 DesiredDirection
        {
            get { return desiredDirection; }
            set { desiredDirection = value; }
        }

        protected Vector3 desiredOrientation;
        /// <summary>
        /// Sets an orientation which you want this character to face, when called
        /// Update character's code should deal with this request adequatly
        /// </summary>
        public Vector3 DesiredOrientation
        {
            get { return desiredOrientation; }
            set { desiredOrientation = value; }
        }

        /// <summary>
        /// Stores position ocuppied by this character before Update()
        /// </summary>
        protected Vector3 previousPosition;
        public Vector3 PreviousPosition
        {
            get { return previousPosition; }
            //set { previousPosition = value; }
        }

        private Vector3 orientation;

        /// <summary>
        /// Orientation of the character.
        /// </summary>
        public Vector3 Orientation
        {
            get { return this.orientation; }
            set { this.orientation = value; }
        }

        private bool isDirty;

        /// <summary>
        /// Em... not that kind of dirtynes.
        /// </summary>
        public bool IsDirty
        {
            get { return this.isDirty; }
            set { this.isDirty = value; }
        }

        private string name;

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        protected float radius;

        /// <summary>
        /// Radius of the character. In SimpleAIEngine units.
        /// </summary>
        public float Radius
        {
            get { return this.radius; }
            set
            {
                this.radius = value;
                this.isDirty = true;
            }
        }

        protected AIBehaviour currentBehaviour;
        public AIBehaviour CurrentBehaviour
        {
            get { return currentBehaviour; }
            set 
            { 
                currentBehaviour = value;
                currentBehaviour.Character = this;
                currentBehaviour.Map = map;
            }
        }

		public AIActor(){
            orientation = new Vector3(1, 1, 0);
            orientation.Normalize();

            desiredDirection = orientation;
            sensors = new AISensors();
		}

		~AIActor(){

		}

        protected void UpdateBehaviours(GameTime gameTime)
        {
            // execute current behaviour
            if (currentBehaviour != null)
            {
                if (currentBehaviour.State != AIBehaviourState.Failed &&
                    currentBehaviour.State != AIBehaviourState.Finished)
                {
                    currentBehaviour.Iterate(gameTime);
                }
            }

        }

        public void Update(GameTime gameTime)
        {

            // store current position
            if (PreviousPosition == new Vector3(0))
            {
                previousPosition = Position;
            }

            this.UpdateBehaviours(gameTime);
            this.UpdatePosition(gameTime);
            if (useTail)
            {
                this.tail.Add(ref position);
            }

            this.UpdateSensors(gameTime);

        }


        public void UpdateSensors(GameTime gameTime)
        {
            for (int index = 0; index < sensors.Count; index++)
            {
                sensors[index].Update(gameTime);
            }

        }

        public void UpdatePosition(GameTime gameTime)
        {
            
            if (desiredDirection.Length() > 0.0f)
            {
                previousPosition = position;
            }

            motionController.Update(gameTime);

        }

		public virtual void Dispose(){

		}

        /// <summary>
        /// Defines type to cost conversion routine for a given character.
        /// Intended to be overwritte by a class derived from AIActor.
        /// </summary>
        /// <returns></returns>
        public virtual byte TypeToCost(int nodeType)
        {
            if (nodeType == 0)
            {
                // very difficult to cross, pathfinding will try to 
                // avoid this node if possible
                return 255;
            }
            else
            {
                // easy to cross
                return 1;
            }
        }

        public virtual void Draw()
        {
            if (currentBehaviour != null)
            {
                currentBehaviour.Draw();
            }
        }

    }//end AIActor

}//end namespace SimpleAI