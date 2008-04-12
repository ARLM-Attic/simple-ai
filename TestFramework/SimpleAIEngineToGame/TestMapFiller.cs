using System;
using System.Collections.Generic;
using System.Text;
using SimpleAI;

namespace TestFramework.SimpleAIEngineToGame
{
    // Just to help create a decent map, since there is
    // no serialization/deserialization implemented.
    public class TestMapFiller
    {
        public TestMapFiller()
        {
        }

        ~TestMapFiller()
        {
        }


        public void FillMapForAvoidDemo(ref AIMap mapToFill)
        {

            if (mapToFill == null)
            {
                return;
            }

            int X = mapToFill.Width / 2;
            int Y = mapToFill.Height / 2;

            mapToFill.Node(X, Y).Type = 0;
            mapToFill.Node(X + 1, Y).Type = 0;
            mapToFill.Node(X - 1, Y).Type = 0;
            mapToFill.Node(X, Y + 1).Type = 0;
            mapToFill.Node(X, Y - 1).Type = 0;

        }

        public void FillMap(ref AIMap mapToFill)
        {
            if (mapToFill == null)
            {
                return;
            }

            // node types:
            // 0 - can't cross
            // 1 - crossabe

            Random rnd = new Random();

            for (int index = 0; index < 1000; index++)
            {
                int X = (int)rnd.Next(mapToFill.Width - 1);
                int Y = (int)rnd.Next(mapToFill.Height - 1);

                mapToFill.Node(X, Y).Type = 0;
            }


        }
    }
}
