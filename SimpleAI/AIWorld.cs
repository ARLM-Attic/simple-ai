///////////////////////////////////////////////////////////
//  AIWorld.cs
//  Implementation of the Class AIWorld
//  Generated by Enterprise Architect
//  Created on:      20-mar-2008 21:01:51
//  Original author: piotrw
///////////////////////////////////////////////////////////


using System;
using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SimpleAI {
	public class AIWorld : IAINameOwner {

        protected AIMaps maps;
        protected AIActors actors;

        public AIActors Actors
        {
            get { return actors; }
        }


        public AIMaps Maps
        {
            get { return maps; }
        }

        protected string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Denotes the current SimpleAIEngine version
        /// </summary>
        protected AIVersion systemVersion;

        /// <summary>
        /// Denotes the minimal version of data, that this engine can handle.
        /// Data saved with version earlier then minimal won't be deserialized
        /// </summary>
        protected AIVersion minimalVersion;

        /// <summary>
        /// Stores the version of data deserialized - in case we need to serialize
        /// it again. If you want to upgrade data version - write a separate serialization
        /// - deserialization code.
        /// </summary>
        protected AIVersion dataVersion;
        

        /// <summary>
        /// Serialization routine for the AIWorld object.
        /// Serialization order:
        /// - world's data version
        /// - world's name
        /// - [...]
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        /*public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            //You can use any custom name for your name-value pair. But make sure you
            // read the values with the same name. For ex:- If you write EmpId as "EmployeeId"
            // then you should read the same with "EmployeeId"
            //info.AddValue("EmployeeId", EmpId);
            //info.AddValue("EmployeeName", EmpName);
            //info.AddValue("Version", dataVersion);
            dataVersion.GetObjectData(info, ctxt);
            info.AddValue("Name", this.name);
        }*/

        /// <summary>
        /// Deserialization constructor.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        /*public AIWorld(SerializationInfo info, StreamingContext ctxt)
        {
            systemVersion.Major = 0;
            systemVersion.Minor = 1;

            minimalVersion.Major = 0;
            minimalVersion.Minor = 1;

            this.Deserialize(info, ctxt);


        }*/


        /// <summary>
        /// Deserializes AIWorld's data for a stream. If checks agains data, system and
        /// minimal version raising an exception in case requirements are not meet.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        /*public virtual void Deserialize(SerializationInfo info, StreamingContext ctxt)
        {
            dataVersion.Deserialize(info, ctxt);
            if (dataVersion < minimalVersion)
            {
                Exception err = new Exception("Deserialization error. Minimal supperted version is " + minimalVersion.ToString() + ", your data version is" + dataVersion.ToString());
            }

            name = (string)info.GetValue("Name", typeof(string));
        }*/


		public AIWorld()
        {

            dataVersion = systemVersion;
            maps = new AIMaps();
            actors = new AIActors();
		}

		~AIWorld(){

		}

		public virtual void Dispose(){

		}

        public virtual void Update(GameTime gameTime)
        {
            if (actors != null)
            {
                for (int index = 0; index < actors.Count; index++)
                {
                    actors[index].Update(gameTime);
                }
            }
        }

        public virtual void Draw()
        {
            if (actors != null)
            {
                for (int index = 0; index < actors.Count; index++)
                {
                    actors[index].Draw();
                }
            }
        }

    }//end AIWorld

}//end namespace SimpleAI