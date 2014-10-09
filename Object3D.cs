using System;
using Sce.PlayStation.Core;

namespace Dtictactoe
{
	abstract class Object3D
	{
		protected Vector3 position;
		
		public Object3D (float x, float y, float z)
		{
			this.position.X = x;
			this.position.Y = y;
			this.position.Z = z;
		}
		
		public Object3D (Vector3 position)
		{
			this.position.X = position.X;
			this.position.Y = position.Y;
			this.position.Z = position.Z;
		}
		
		public Vector3 Position
		{
			set{this.position = value;}
			get{return position;}
		}
		
		public float X
		{
			set{this.position.X = value;}
			get{return position.X;}
		}

		public float Y
		{
			set{this.position.Y = value;}
			get{return position.Y;}
		}
		
		public float Z
		{
			set{this.position.Z = value;}
			get{return position.Z;}
		}
		
		public abstract void Initialize();
		
		
		public abstract void Render();
		
	}
}

