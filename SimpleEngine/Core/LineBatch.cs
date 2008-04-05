using System;
using System.Collections.Generic;
using System.Text;
using SimpleEngine.Camera;
using Microsoft.Xna.Framework;

namespace SimpleEngine.Core
{
    public class LineBatch
    {
        protected SimpleCamera camera;
        public SimpleCamera Camera
        {
            get { return camera; }
            set
            {
                camera = value;
            }
        }

        protected Vector3 position;
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        protected Matrix rotation;
        public Matrix Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public LineBatch()
        {
            camera = null;
            this.position = Vector3.Zero;
            this.rotation = Matrix.Identity;

        }

        ~LineBatch()
        {
        }
    }
}
