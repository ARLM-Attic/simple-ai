using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleEngine.Graphs
{
    public class FPSGraph : DebugGraph
    {
        public FPSGraph(ref GraphManager graphManager, float frequency, int capacity)
            : base(ref graphManager, frequency, capacity)
        {
            this.name = "FPS";
        }

        ~FPSGraph()
        {
        }

        public override void Update(GameTime gameTime)
        {
            // count number of calls, this will give an estimated FPS
            //Console.WriteLine((float)(GC.GetTotalMemory(false) / 1024.0f / 1024.0f));
            //base.Update(gameTime, (float)(GC.GetTotalMemory(false) / 1024.0f / 1024.0f) );

            accumulator++;
            float timePassed = (float)(gameTime.TotalRealTime.TotalSeconds - lastTotalSec);

            if (timePassed >= frequency)
            {
                float newVal = (float)(accumulator / timePassed);
                //float newVal = (float)(GC.GetTotalMemory(false));
                accumulator = 0.0f;
                lastTotalSec = gameTime.TotalRealTime.TotalSeconds;
                probesContainer.RemoveAt(0);
                probesContainer.Add(newVal);
                FormatOutputExpression();

                maxSampleValue = 1;

                for (int index = 0; index < probesContainer.Count; index++)
                {
                    float fSample = (float)probesContainer[index];
                    if (fSample > maxSampleValue)
                    {
                        maxSampleValue = fSample;
                    }
                }

            }

        }

        public override void FormatOutputExpression()
        {
            //base.FormatOutputExpression();
            this.outputExpression = this.name + ": " + (((float)(probesContainer[probesContainer.Capacity - 1]))).ToString();
        }
    } // end FPSGraph
}
