using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

namespace Dtictactoe
{
	public class Cube
	{
		private float x;
		private float y;
		private float z;
		
		private GraphicsContext gc;
		private VertexBuffer vertexBuffer;
		private Texture2D texture;
		private int vertexCount;

		
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
		
		public void init()
		{
			vertexCount = 4;
			vertexBuffer = new VertexBuffer(vertexCount, VertexFormat.Float3,
			                                VertexFormat.Float2, VertexFormat.Float4);
			texture = new Texture2D("/Application/resources/test.png", false);
			
			vertexBuffer.SetVertices(0,
			                         new float[]{
				-1f, 1f, 0f,
				-1f, -1f, 0f,
				1f, 1f, 0f,
				1f, -1f, 0f,
			});
			
			vertexBuffer.SetVertices(1,
			                     new float[]{
				0.0f, 0.0f,
				0.0f, 1.0f,
				1.0f, 0.0f,
				1.0f, 1.0f,
			});
			
			vertexBuffer.SetVertices(2,
			                         new float[]{
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
			});
			
		}
		
		public void update()
		{
		}
		
		public void render(ShaderProgram program)
		{
			this.gc.SetVertexBuffer(0, vertexBuffer);
			this.gc.SetTexture(0,texture);
			this.gc.SetShaderProgram(program);
			
			
			this.gc.DrawArrays(DrawMode.TriangleStrip, 0, vertexBuffer.VertexCount);
		}
		
	}
}
