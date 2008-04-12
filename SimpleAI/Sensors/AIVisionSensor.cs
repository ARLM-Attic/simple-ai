using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SimpleAI.Sensors
{
    public class AIVisionSensor : AISensor
    {
        public AIVisionSensor()
        {
            this.orientation = new Vector3(1, 0, 0);
            this.range = 50.0f;
            this.arc = MathHelper.ToRadians(45.0f);
        }

        ~AIVisionSensor()
        {
        }

    }
}
