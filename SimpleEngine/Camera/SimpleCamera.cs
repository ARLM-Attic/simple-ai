using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleEngine.Camera
{
    public class SimpleCamera : GameComponent
    {

        protected Matrix view;
        protected Matrix projection;

        protected bool projectionDirty;
        protected bool viewDirty;

        protected float aspect;
        public float Aspect
        {
            get { return Aspect; }
        }

        protected float fov;
        public float FOV
        {
            get { return this.fov; }
            set
            {
                this.fov = value;
                this.projectionDirty = true;
                viewDirty = true;
            }
        }

        protected float farPlane;
        public float FarPlane
        {
            get { return this.farPlane; }
            set
            {
                this.farPlane = value;
                this.projectionDirty = true;
                viewDirty = true;
            }
        }

        protected float nearPlane;
        public float NearPlane
        {
            get { return this.nearPlane; }
            set 
            {
                this.nearPlane = value;
                this.projectionDirty = true;
                viewDirty = true;
            }
        }

        protected Vector3 vectorPos;
        public Vector3 VectorPos
        {
            get { return this.vectorPos; }
            set
            {
                this.vectorPos = value;
                this.projectionDirty = true;
                viewDirty = true;
            }
        }


        public Vector3 Position
        {
            get { return this.vectorPos; }
            set
            {
                this.vectorPos = value;
                this.projectionDirty = true;
                viewDirty = true;
            }
        }


        protected Vector3 desiredPosition;
        public Vector3 DesiredPosition
        {
            get { return this.desiredPosition; }
            set
            {
                this.desiredPosition = value;
                this.projectionDirty = true;
                this.viewDirty = true;
            }
        }

        protected Vector3 vectorUp;
        public Vector3 VectorUp
        {
            get { return this.vectorUp; }
            set
            {
                this.vectorUp = value;
                this.projectionDirty = true;
                viewDirty = true;
            }
        }

        protected Vector3 vectorLook;
        public Vector3 VectorLook
        {
            get { return this.vectorLook; }
            set
            {
                this.vectorLook = value;
                this.projectionDirty = true;
                viewDirty = true;
            }
        }

        protected Vector3 vectorRight;
        public Vector3 VectorRight
        {
            get { return this.vectorRight; }
            set
            {
                this.vectorRight = value;
                this.projectionDirty = true;
                viewDirty = true;
            }
        }

        protected Vector3 lookAt;
        public Vector3 LookAt
        {
            get { return this.lookAt; }
            set
            {
                this.lookAt = value;
                this.projectionDirty = true;
                viewDirty = true;

            }
        }


        protected Viewport viewPort;
        public Viewport ViewPort
        {
            get { return this.viewPort; }
            set
            {
                this.viewPort = value;
                this.aspect = this.viewPort.Width / this.viewPort.Height;
                projectionDirty = true;
                viewDirty = true;
            }

        }

        public SimpleCamera(Game game) : base(game)
        {
            this.vectorRight = Vector3.Right; // X axis
            this.vectorUp = Vector3.Backward; // Z axis Vector3.Up;
            this.vectorLook = Vector3.Up;// Z axis 
            this.vectorPos = new Vector3(0.0f);
            this.fov = 1.0f;
            this.farPlane = 100.0f;
            this.nearPlane = 1.0f;
            this.viewPort = new Viewport();
                this.viewPort.X = 0;
                this.viewPort.Y = 0;
                this.viewPort.Width = 800;
                this.viewPort.Height = 600;
                this.viewPort.MinDepth = 0.0f;
                this.viewPort.MaxDepth = 1.0f;
            this.view = Matrix.Identity;
            this.projection = Matrix.Identity;
            this.projectionDirty = true;
            this.viewDirty = true;
            
        }

        ~SimpleCamera()
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (desiredPosition != vectorPos)
            {
                Vector3 vShift = desiredPosition - vectorPos;
                float shiftSize = vShift.Length();
                float movement = shiftSize * 0.011f;

                vShift.Normalize();
                vShift = vShift * movement;
                vectorPos = vectorPos + vShift;
            }

        }

        public virtual void Move(Vector3 translation)
        {
        }

        public virtual void Rotate(float x, float y, float z)
        {
        }

        public Matrix ViewMatrix
        {
            get
            {
                //if (this.viewDirty)
                {
                    viewDirty = false;

                    /*
                    vectorLook.Normalize();

                    vectorRight = Vector3.Cross(vectorUp, vectorLook);
                    vectorRight.Normalize();

                    vectorUp = Vector3.Cross(vectorLook, vectorRight);
                    vectorUp.Normalize();
                     * */

                    view.M11 = vectorRight.X;
                    view.M12 = vectorUp.X;
                    view.M13 = vectorLook.X;

                    view.M21 = vectorRight.Y;
                    view.M22 = vectorUp.Y;
                    view.M23 = vectorLook.Y;

                    view.M31 = vectorRight.Z;
                    view.M32 = vectorUp.Z;
                    view.M33 = vectorLook.Z;

                    view.M41 = -Vector3.Dot(vectorPos, vectorRight);// -D3DXVec3Dot(&m_vecPos, &m_vecRight);
                    view.M42 = -Vector3.Dot(vectorPos, vectorUp);// -D3DXVec3Dot(&m_vecPos, &m_vecUp);
                    view.M43 = -Vector3.Dot(vectorPos, vectorLook);
                    
                    view = Matrix.CreateLookAt(
                        vectorPos,
                        vectorLook,
                        vectorUp);

                }
                
                return this.view;
            }
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                //if (this.projectionDirty)
                {
                    this.projectionDirty = false;
                    this.aspect = this.viewPort.Width / this.viewPort.Height;
                    this.projection = Matrix.CreatePerspectiveFieldOfView(
                        this.fov,
                        this.aspect,
                        this.nearPlane,
                        this.farPlane);
                }

                return this.projection;
            }
        }
    }
}
