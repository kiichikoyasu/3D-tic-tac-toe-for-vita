using System;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Imaging;

namespace Dtictactoe
{
	public class Plane
	{
		private GraphicsContext gc;
		private ShaderProgram program;
		private VertexBuffer vertexBuffer;
		private Texture2D texture;
		private int vertexCount;
		private Vector3[] point = new Vector3[4];
		
		private float[] vertices, texcoords, colors;
		
		private Vector3 norm;
		
		public Plane (GraphicsContext graphics)
		{
			gc = graphics;
		}
		
		public void Initialize(Texture2D texture, ShaderProgram program)
		{
			vertexCount = 4;
			vertexBuffer = new VertexBuffer(vertexCount, VertexFormat.Float3,
			                                VertexFormat.Float2, VertexFormat.Float4);
			this.texture = texture;
			

//			program = new ShaderProgram("/Application/shaders/VertexColor.cgx");
			this.program = program;
			this.program.SetUniformBinding(0, "WorldViewProj");			
			
			vertices = new float[]{
				-1.0f, 1.0f, 0.0f,
				-1.0f, -1.0f, 0.0f,
				1.0f, 1.0f, 0.0f,
				1.0f, -1.0f, 0.0f,
			};
			
			point[0] = new Vector3(vertices[0], vertices[1], vertices[2]);
			point[1] = new Vector3(vertices[3], vertices[4], vertices[5]);
			point[2] = new Vector3(vertices[6], vertices[7], vertices[8]);
			point[3] = new Vector3(vertices[9], vertices[10], vertices[11]);
			
			norm = Vector3.Cross(Vector3.Subtract(point[3], point[1]), Vector3.Subtract(point[0], point[1])).Normalize();

			
			texcoords = new float[]{
				0.0f, 0.0f,
				0.0f, 1.0f,
				1.0f, 0.0f,
				1.0f, 1.0f,
			};
			
			colors = new float[]{
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
			};
			
			vertexBuffer.SetVertices(1, texcoords);
			vertexBuffer.SetVertices(2, colors);
			
		}
		
		public void Update()
		{
		}
		
		public void Render()
		{
			/* プロパティは直接ref引数に渡せないので無理やりフィールドを読むようにした*/
			program.SetUniformValue(0, ref Camera.worldViewProj);
			vertexBuffer.SetVertices(0, vertices);
			gc.SetVertexBuffer(0, vertexBuffer);
			gc.SetTexture(0,texture);
			gc.SetShaderProgram(program);
			
			if(!gc.IsEnabled(EnableMode.DepthTest))
			{
				gc.Enable(EnableMode.DepthTest);
			}
			
			if(!gc.IsEnabled(EnableMode.CullFace))
			{
				gc.Enable(EnableMode.CullFace);
			}
			
			gc.DrawArrays(DrawMode.TriangleStrip, 0, 4);
		}
		
		public float[] Vertices
		{
			set
			{
				var minLength = Math.Min(value.Length, vertices.Length);
				for (int i = 0; i < minLength; i++)
				{
					vertices[i] = value[i];
				}
				/* pointとverticesの整合性をとる */	
				point[0] = new Vector3(vertices[0], vertices[1], vertices[2]);
				point[1] = new Vector3(vertices[3], vertices[4], vertices[5]);
				point[2] = new Vector3(vertices[6], vertices[7], vertices[8]);
				point[3] = new Vector3(vertices[9], vertices[10], vertices[11]);
				
				/* 法線ベクトルも更新 */
				norm = Vector3.Cross(Vector3.Subtract(point[3], point[1]), Vector3.Subtract(point[0], point[1])).Normalize();

			}
		}
		
		public float[] Texcoords
		{
			set
			{
				var minLength = Math.Min(value.Length, texcoords.Length);
				for (int i = 0; i < minLength; i++)
				{
					texcoords[i] = value[i];
				}
			}
		}

		public float[] Colors
		{
			set
			{
				var minLength = Math.Min(value.Length, colors.Length);
				for (int i = 0; i < minLength; i++)
				{
					colors[i] = value[i];
				}
			}
		}
		
		public Texture2D Texture
		{
			set{texture = value;}
		}
		
		public bool IsCollision(Vector3 rayStart, Vector3 rayEnd)
		{
			
			/* rayはカメラからタッチした点までの方向ベクトル */
			var ray = Vector3.Subtract(rayEnd, rayStart);
			
			/* 法線ベクトルとrayが直角に交わるのであればrayは面と触れない */
			if(Vector3.Dot(ray, norm) == 0)
			{
				return false;
			}
			
			/* rayが面と触れる場所を計算 これはrayの始点と終点をa:1-aに内分する点として考える */
			
			var disWithCam = Math.Abs(Vector3.Dot(norm, Vector3.Subtract(rayStart, point[0])));
			var disWithTouchPos = Math.Abs(Vector3.Dot (norm, Vector3.Subtract(rayEnd, point[0])));
			var dividingRatio = disWithCam / (disWithCam + disWithTouchPos);
			var collisionPos = Vector3.Add(rayStart,
			                           ray.Multiply(dividingRatio));
			
			/* rayと面が触れる点がポリゴン内にあるか計算 */
			
			var c1 = Vector3.Cross(Vector3.Subtract(point[1], point[0]), Vector3.Subtract(collisionPos, point[1]));
			var c2 = Vector3.Cross(Vector3.Subtract(point[3], point[1]), Vector3.Subtract(collisionPos, point[3]));
			var c3 = Vector3.Cross(Vector3.Subtract(point[2], point[3]), Vector3.Subtract(collisionPos, point[2]));
			var c4 = Vector3.Cross(Vector3.Subtract(point[0], point[2]), Vector3.Subtract(collisionPos, point[0]));
			
			if( c1.Dot(c2) > 0 && c2.Dot(c3) > 0 && c3.Dot(c4) > 0 )
			{
				return true;
			}
			
			return false;
		}
	}
}

