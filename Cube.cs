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
		private SelectStatus selectStatus;
		
		private GraphicsContext gc;
		private VertexBuffer vertexBuffer;
		private Texture2D texture;
		private int vertexCount;
		
		private float[] vertices, texcoords, colors;

		
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
		
		public enum SelectStatus {
			None,
			Circle,
			Cross,
			Square,
			Triangle
		}		
		
		public void Initialize()
		{
			size = 1.0f;
			scale = 1.0f;
			vertexCount = 24;
			vertexBuffer = new VertexBuffer(vertexCount, VertexFormat.Float3,
			                                VertexFormat.Float2, VertexFormat.Float4);
			selectStatus = SelectStatus.None;
			texture = new Texture2D("/Application/resources/test.png", false);
			
			
			texcoords = new float[]{
				//front
				0.0f, 0.0f,
				0.0f, 1.0f,
				1.0f, 0.0f,
				1.0f, 1.0f,
				
				//left
				0.0f, 0.0f,
				0.0f, 1.0f,
				1.0f, 0.0f,
				1.0f, 1.0f,
				
				//back
				0.0f, 0.0f,
				0.0f, 1.0f,
				1.0f, 0.0f,
				1.0f, 1.0f,
				
				//right
				0.0f, 0.0f,
				0.0f, 1.0f,
				1.0f, 0.0f,
				1.0f, 1.0f,
				
				//top
				0.0f, 0.0f,
				0.0f, 1.0f,
				1.0f, 0.0f,
				1.0f, 1.0f,
				
				//bottom
				0.0f, 0.0f,
				0.0f, 1.0f,
				1.0f, 0.0f,
				1.0f, 1.0f,

			};
			
			colors =  new float[]{
				//front
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				
				//left
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				
				//back
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				
				//right
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				
				//top
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				
				//bottom
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
			};
						
		}
		
		public void Update(GamePadData gamePadData, List<TouchData> touchDataList)
		{			
			scale = 1.0f;
			
			if((gamePadData.Buttons & GamePadButtons.Circle) != 0)
			{
				scale = 1.3f;
			}
			
			foreach(TouchData touchData in touchDataList)
			{
				if((touchData.Status & TouchStatus.Down) != 0)
				{
				}
			}
			 
		}
		
		public void Render(ShaderProgram program)
		{
			var dispSize = size * scale;
			vertices = new float[]{
				//front
				this.x - dispSize / 2.0f , this.y + dispSize / 2.0f, this.z + dispSize / 2.0f,//手前左上
				this.x - dispSize / 2.0f , this.y - dispSize / 2.0f, this.z + dispSize / 2.0f,//手前左下
				this.x + dispSize / 2.0f , this.y + dispSize / 2.0f, this.z + dispSize / 2.0f,//手前右上
				this.x + dispSize / 2.0f , this.y - dispSize / 2.0f, this.z + dispSize / 2.0f,//手前右下
				
				//left
				this.x - dispSize / 2.0f , this.y + dispSize / 2.0f, this.z - dispSize / 2.0f,//奥左上
				this.x - dispSize / 2.0f , this.y - dispSize / 2.0f, this.z - dispSize / 2.0f,//奥左下
				this.x - dispSize / 2.0f , this.y + dispSize / 2.0f, this.z + dispSize / 2.0f,//手前左上
				this.x - dispSize / 2.0f , this.y - dispSize / 2.0f, this.z + dispSize / 2.0f,//手前左下
				
				//back
				this.x + dispSize / 2.0f , this.y + dispSize / 2.0f, this.z - dispSize / 2.0f,//奥右上
				this.x + dispSize / 2.0f , this.y - dispSize / 2.0f, this.z - dispSize / 2.0f,//奥右下
				this.x - dispSize / 2.0f , this.y + dispSize / 2.0f, this.z - dispSize / 2.0f,//奥左上
				this.x - dispSize / 2.0f , this.y - dispSize / 2.0f, this.z - dispSize / 2.0f,//奥左下
				
				//right
				this.x + dispSize / 2.0f , this.y + dispSize / 2.0f, this.z + dispSize / 2.0f,//手前右上
				this.x + dispSize / 2.0f , this.y - dispSize / 2.0f, this.z + dispSize / 2.0f,//手前右下
				this.x + dispSize / 2.0f , this.y + dispSize / 2.0f, this.z - dispSize / 2.0f,//奥右上
				this.x + dispSize / 2.0f , this.y - dispSize / 2.0f, this.z - dispSize / 2.0f,//奥右下
				
				//top
				this.x - dispSize / 2.0f , this.y + dispSize / 2.0f, this.z - dispSize / 2.0f,//奥左上
				this.x - dispSize / 2.0f , this.y + dispSize / 2.0f, this.z + dispSize / 2.0f,//手前左上
				this.x + dispSize / 2.0f , this.y + dispSize / 2.0f, this.z - dispSize / 2.0f,//奥右上
				this.x + dispSize / 2.0f , this.y + dispSize / 2.0f, this.z + dispSize / 2.0f,//手前右上
				
				//bottom
				this.x - dispSize / 2.0f , this.y - dispSize / 2.0f, this.z + dispSize / 2.0f,//手前左下
				this.x - dispSize / 2.0f , this.y - dispSize / 2.0f, this.z - dispSize / 2.0f,//奥左下
				this.x + dispSize / 2.0f , this.y - dispSize / 2.0f, this.z + dispSize / 2.0f,//手前右下
				this.x + dispSize / 2.0f , this.y - dispSize / 2.0f, this.z - dispSize / 2.0f,//奥右下
			};
			
			
			vertexBuffer.SetVertices(0, vertices);
			vertexBuffer.SetVertices(1, texcoords);
			vertexBuffer.SetVertices(2, colors);

			
			this.gc.SetVertexBuffer(0, vertexBuffer);
			this.gc.SetTexture(0,texture);
			this.gc.SetShaderProgram(program);
			
			this.gc.Enable(EnableMode.DepthTest);
			
			this.gc.DrawArrays(DrawMode.TriangleStrip, 0, 4);
			this.gc.DrawArrays(DrawMode.TriangleStrip, 4, 4);
			this.gc.DrawArrays(DrawMode.TriangleStrip, 8, 4);
			this.gc.DrawArrays(DrawMode.TriangleStrip, 12, 4);
			this.gc.DrawArrays(DrawMode.TriangleStrip, 16, 4);
			this.gc.DrawArrays(DrawMode.TriangleStrip, 20, 4);
		}
		
	}
}

