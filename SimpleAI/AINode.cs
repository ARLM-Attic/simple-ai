///////////////////////////////////////////////////////////
//  AICell.cs
//  Implementation of the Class AICell
//  Generated by Enterprise Architect
//  Created on:      21-mar-2008 20:44:42
//  Original author: piotrw
///////////////////////////////////////////////////////////




using Microsoft.Xna.Framework;
namespace SimpleAI {

    /// <summary>
    /// A node is an element of AIMap. It holds infomation about terrain type (node type)
    /// position of the node's centre.
    /// </summary>
	public class AINode {


		private AIMap parent;
		private int type;
        private int x;
        private int y;
        private Vector3 position;

        /// <summary>
        /// Center position of a node
        /// </summary>
        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// X index of a node
        /// </summary>
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// Y index of a node.
        /// </summary>
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// Type of a node
        /// </summary>
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        /// Reference to parent AIMap, which this node belongs to.
        /// </summary>
        public AIMap Parent
        {
            get { return parent; }
            set { parent = value; }
        }


		public AINode(){

		}

		~AINode(){

		}

		public virtual void Dispose(){

		}

	}//end AICell

}//end namespace SimpleAI