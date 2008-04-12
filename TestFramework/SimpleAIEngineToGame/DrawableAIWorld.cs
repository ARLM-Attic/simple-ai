using System;
using System.Collections.Generic;
using System.Text;
using SimpleAI;

namespace TestFramework.SimpleAIEngineToGame
{
    public class DrawableAIWorld : AIWorld
    {
        public DrawableAIWorld()
        {
        }

        ~DrawableAIWorld()
        {
        }

        public override void Draw()
        {
            if (maps != null)
            {
                for (int index = 0; index < maps.Count; index++)
                {
                    maps[index].Draw();
                }
            }

            if (actors != null)
            {
                for (int index = 0; index < actors.Count; index++)
                {
                    actors[index].Draw();
                }
            }
        }
    }
}
