using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.Core.Graphics;

using System.Collections.Generic;

namespace Dtictactoe
{
	public class Cube
	{
		private float x;
		private float y;
		private float z;
		
		/* cube立方体1辺の長さ */
		private float size;
		private float scale;
		private CubeStatus status;
		
		private GraphicsContext gc;
				
		private Plane front, left, back, right, top, bottom;

		
		public Cube (GraphicsContext graphics, float x, float y, float z)
		{
			this.gc = graphics;
			this.x = x;
			this.y = y;
			this.z = z;
		}
		
		public Cube (GraphicsContext graphics, Vector3 position)
		{
			this.gc = graphics;
			this.x = position.X;
			this.y = position.Y;
			this.z = position.Z;
		}
		
/*		public enum SelectStatus {
			None,
			Circle,
			Cross,
			Square,
			Triangle
		}*/		
		
		public void Initialize(Texture2D texture, ShaderProgram program)
		{
			size = GameParameters.CubeSize;
			scale = 1.0f;
			status = CubeStatus.NotSelected;
//			selectStatus = SelectStatus.None;
			
			front = new Plane(gc);
			front.Initialize(texture, program);
			
			left = new Plane(gc);
			left.Initialize(texture, program);
			
			back = new Plane(gc);
			back.Initialize(texture, program);
			
			right = new Plane(gc);
			right.Initialize(texture, program);
			
			top = new Plane(gc);
			top.Initialize(texture, program);		
			
			bottom = new Plane(gc);
			bottom.Initialize(texture, program);
		}
		
		public void Update(GamePadData gamePadData)
		{			
			scale = 1.0f;
			
			if((gamePadData.Buttons & GamePadButtons.Circle) != 0)
			{
				if(status == CubeStatus.Circle){
					scale = GameParameters.CubeScale;
				}
			}
			
			if((gamePadData.Buttons & GamePadButtons.Cross) != 0)
			{
				if(status == CubeStatus.Cross){
					scale = GameParameters.CubeScale;
				}
			}
			
			if((gamePadData.Buttons & GamePadButtons.Square) != 0)
			{
				if(status == CubeStatus.Square){
					scale = GameParameters.CubeScale;
				}
			}
			
			if((gamePadData.Buttons & GamePadButtons.Triangle) != 0)
			{
				if(status == CubeStatus.Triangle){
					scale = GameParameters.CubeScale;
				}
			}
			
			
			front.Update();
			left.Update();
			back.Update();
			right.Update();
			top.Update();
			bottom.Update();
			
		}
		
		public void Render()
		{
			var dispSize = size * scale;
			
			front.Vertices = new float[]{
				this.x - dispSize / 2.0f , this.y + dispSize / 2.0f, this.z + dispSize / 2.0f,//手前左上
				this.x - dispSize / 2.0f , this.y - dispSize / 2.0f, this.z + dispSize / 2.0f,//手前左下
				this.x + dispSize / 2.0f , this.y + dispSize / 2.0f, this.z + dispSize / 2.0f,//手前右上
				this.x + dispSize / 2.0f , this.y - dispSize / 2.0f, this.z + dispSize / 2.0f,//手前右下
			};
			
			left.Vertices = new float[]{
				this.x - dispSize / 2.0f , this.y + dispSize / 2.0f, this.z - dispSize / 2.0f,//奥左上
				this.x - dispSize / 2.0f , this.y - dispSize / 2.0f, this.z - dispSize / 2.0f,//奥左下
				this.x - dispSize / 2.0f , this.y + dispSize / 2.0f, this.z + dispSize / 2.0f,//手前左上
				this.x - dispSize / 2.0f , this.y - dispSize / 2.0f, this.z + dispSize / 2.0f,//手前左下
			};				
			
			back.Vertices = new float[]{
				this.x + dispSize / 2.0f , this.y + dispSize / 2.0f, this.z - dispSize / 2.0f,//奥右上
				this.x + dispSize / 2.0f , this.y - dispSize / 2.0f, this.z - dispSize / 2.0f,//奥右下
				this.x - dispSize / 2.0f , this.y + dispSize / 2.0f, this.z - dispSize / 2.0f,//奥左上
				this.x - dispSize / 2.0f , this.y - dispSize / 2.0f, this.z - dispSize / 2.0f,//奥左下
			};
			
			right.Vertices = new float[]{
				this.x + dispSize / 2.0f , this.y + dispSize / 2.0f, this.z + dispSize / 2.0f,//手前右上
				this.x + dispSize / 2.0f , this.y - dispSize / 2.0f, this.z + dispSize / 2.0f,//手前右下
				this.x + dispSize / 2.0f , this.y + dispSize / 2.0f, this.z - dispSize / 2.0f,//奥右上
				this.x + dispSize / 2.0f , this.y - dispSize / 2.0f, this.z - dispSize / 2.0f,//奥右下
			};
			
			top.Vertices = new float[]{
				this.x - dispSize / 2.0f , this.y + dispSize / 2.0f, this.z - dispSize / 2.0f,//奥左上
				this.x - dispSize / 2.0f , this.y + dispSize / 2.0f, this.z + dispSize / 2.0f,//手前左上
				this.x + dispSize / 2.0f , this.y + dispSize / 2.0f, this.z - dispSize / 2.0f,//奥右上
				this.x + dispSize / 2.0f , this.y + dispSize / 2.0f, this.z + dispSize / 2.0f,//手前右上
			};
			
			bottom.Vertices = new float[]{
				this.x - dispSize / 2.0f , this.y - dispSize / 2.0f, this.z + dispSize / 2.0f,//手前左下
				this.x - dispSize / 2.0f , this.y - dispSize / 2.0f, this.z - dispSize / 2.0f,//奥左下
				this.x + dispSize / 2.0f , this.y - dispSize / 2.0f, this.z + dispSize / 2.0f,//手前右下
				this.x + dispSize / 2.0f , this.y - dispSize / 2.0f, this.z - dispSize / 2.0f,//奥右下
			};
				
			
			front.Render();
			left.Render();
			back.Render();
			right.Render();
			top.Render();
			bottom.Render();
			
		}
		
		/**
		 * cubeがrayと当たっているか調べる
		 * 当たっていればrayの始点との距離を返す
		 * 当たっていなければ距離0を返す
		 */

		public float DistWithRayStartClicked (Vector3 rayStart, Vector3 rayEnd)
		{
			/* rayと何枚衝突しているか */
			var collisionPlane = 0;
			
			if(front.IsCollision(rayStart, rayEnd))
			{
				collisionPlane++;
			}
			
			if(left.IsCollision(rayStart, rayEnd))
			{
				collisionPlane++;
			}
			
			if(back.IsCollision(rayStart, rayEnd))
			{
				collisionPlane++;
			}
			
			if(right.IsCollision(rayStart, rayEnd))
			{
				collisionPlane++;
			}
			
			if(top.IsCollision(rayStart, rayEnd))
			{
				collisionPlane++;
			}
			
			if(bottom.IsCollision(rayStart, rayEnd))
			{
				collisionPlane++;
			}
			
			
			if (collisionPlane >= 2)
			{
				return new Vector3(x, y, z).Distance(rayStart);
			}
			else
				return 0f;
		}
		
		/**
		 * cubeがクリックされたか調べる
		 * クリックされていればカメラとの距離を返す
		 * クリックされていなければ距離0を返す
		 */
		public float DistWithCamClicked(Vector3 touchPos)
		{	
			return DistWithRayStartClicked (Camera.Eye, touchPos);
		}
		
		public void Clicked(Texture2D texture, CubeStatus status)
		{
			if(this.status == CubeStatus.NotSelected)
			{
				/* 新たに状態が変わるときのみ */
				this.status = status;
				
				front.Texture = texture;
				left.Texture = texture;
				back.Texture = texture;
				right.Texture = texture;
				top.Texture = texture;
				bottom.Texture = texture;
			}
				
		}
		
		public void Reset(Texture2D texture)
		{
			status = CubeStatus.NotSelected;
			front.Texture = texture;
			front.Update();
			left.Texture = texture;
			left.Update();
			back.Texture = texture;
			back.Update();
			right.Texture = texture;
			right.Update();
			top.Texture = texture;
			top.Update();
			bottom.Texture = texture;
			bottom.Update();
		}
		
		public float Scale
		{
			set{scale = value;}
		}
		
		public CubeStatus Status
		{
			set{status = value;}
			get{return status;}
		}
			
	}
}

