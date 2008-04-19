using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAI.Actors
{
    public class AIGroup
    {

        protected AIActors actors;
        public AIActors Actors
        {
            get { return actors; }
        }

        public AIGroup()
        {
            actors = new AIActors();
            this.FillList();
        }

        public AIGroup(int count)
        {
            actors = new AIActors(count);
            this.FillList();
        }

        protected AIMap map;
        public AIMap Map
        {
            get { return map; }
            set
            {
                map = value;

                for (int index = 0; index < this.actors.Count; index++)
                {
                    this.actors[index].Map = this.map;
                    // leader could be without behaviour... e.g. player
                    if (this.actors[index].CurrentBehaviour != null)
                    {
                        this.actors[index].CurrentBehaviour.Map = this.map;
                    }
                }
            }
        }

        protected void FillList()
        {
            for (int index = 0; index < actors.Capacity; index++)
            {
                this.CreateGroupMember(index);
            }
        }

        /// <summary>
        /// Overwrite this method for creating actors.
        /// Create actor of your chosen type,
        /// create all additional components needed by actor (motion controler,
        ///  behaviours perhaps)
        /// and set up all actor parameters (like initial position).
        /// Don't call base class!
        /// </summary>
        /// <param name="index"></param>
        protected virtual void CreateGroupMember(int index)
        {
            
        }

        public virtual void SetMembersDistribution(int rowsNumber, int colsNumber, float rowsSeparation, float colsSeparation)
        {
        }

        ~AIGroup()
        {
        }

        public void RegisterActors(ref AIEngine engine)
        {
            for (int index = 0; index < actors.Count; index++)
            {
                AIActor actorToRegister = actors[index];
                engine.World.Actors.Add(ref actorToRegister);
            }
        }
    }
}
