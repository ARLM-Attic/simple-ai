using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleAI.Actors
{
    public class AITail
    {
        protected List<Vector3> tail;
        protected bool initilized = false;
        protected int capacity = 0;
        protected float minDistance = 0.3f;

        public float MinDistance
        {
            get { return minDistance; }
            set { minDistance = value; }
        }

        public int Capacity
        {
            get { return capacity; }
        }

        public int Count
        {
            get { return tail.Count; }
        }

        public Vector3 this[int index]
        {
            get 
            {
                if (index < tail.Count)
                {
                    return tail[index];
                 
                }
                else
                {
                    return new Vector3(0);
                }
            }
        }

        public AITail()
        {
        }

        public AITail(int capacity)
        {
            if (capacity > 0)
            {
                tail = new List<Vector3>(capacity);
                this.capacity = capacity;
            }
            else
            {
                tail = new List<Vector3>();
            }

            initilized = true;

        }

        public void Initilize(int capacity)
        {
            if (initilized == false)
            {
                tail = new List<Vector3>(capacity);
                this.capacity = capacity;
            }
        }

        public void Add(ref Vector3 newPoint)
        {
            // check if we have enought room left in out list.
            // if not, remove last element;

            if (!initilized)
            {
                return;
            }

            bool distanceOK = false;

            if (tail.Count == 0)
            {
                distanceOK = true;
            }
            else
            {
                // get distance between last point in the tail and
                // one from the argument. If the distance is >= minDistance,
                // add this point, else - ignore
                float currentDistance = ((Vector3)(tail[tail.Count - 1] - newPoint)).Length();
                if (currentDistance >= minDistance)
                {
                    distanceOK = true;
                }
            }

            if (distanceOK)
            {
                if (tail.Count >= tail.Capacity - 1)
                {
                    tail.RemoveAt(0);
                }
                Vector3 shiftedPoint = newPoint;
                shiftedPoint.Z += 0.1f;
                tail.Add(shiftedPoint);
            }
        }

        public void Reset()
        {
            tail.Clear();
        }

        public void Resize(int newCapacity)
        {
            if (newCapacity < 1)
            {
                return;
            }
            tail.Clear();
            tail = null;
            tail = new List<Vector3>(newCapacity);
        }

        ~AITail()
        {
        }
    }
}
