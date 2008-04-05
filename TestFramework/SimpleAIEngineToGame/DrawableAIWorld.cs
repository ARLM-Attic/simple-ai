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

            if (characters != null)
            {
                for (int index = 0; index < characters.Count; index++)
                {
                    characters[index].Draw();
                }
            }
        }
    }
}
