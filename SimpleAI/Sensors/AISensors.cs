using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleAI.Sensors
{
    public class AISensors : IAISequencer{

        protected static int iSequNumber = 0;
        private List<AISensor> sensors;
        private int initialCapacity = 25;
       
        public int NextSequenceNumber
        {
            get
            {
                return ++iSequNumber;
            }
        }

        public AISensor this[int index]
        {
            get
            {
                return sensors[index];
            }
        }

		public AISensors(){
            sensors = new List<AISensor>(initialCapacity);
		}

        public int Count
        {
            get { return sensors.Count; }
        }

        public void Add(ref AISensor newSensor)
        {
            sensors.Add(newSensor);
        }

        ~AISensors()
        {
        }
    
    }
}
