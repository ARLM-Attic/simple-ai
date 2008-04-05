using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAI.Behaviours
{
    public class AISubbehaviours
    {
        private List<AIBehaviour> behaviours;


        public int Count
        {
            get { return behaviours.Count; }
        }


        public AISubbehaviours(int initialCapacity)
        {
            behaviours = new List<AIBehaviour>(initialCapacity);
        }


        public AISubbehaviours()
        {
            behaviours = new List<AIBehaviour>();
        }

        public AIBehaviour this[int index]
        {
            get
            {
                return behaviours[index];
            }
        }

        public void Add(ref AIBehaviour newBehaviour)
        {
            behaviours.Add(newBehaviour);
        }

        ~AISubbehaviours()
        {
        }
    }
}
