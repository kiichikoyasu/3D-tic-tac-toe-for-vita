using System;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Imaging;

namespace Dtictactoe
{
	public class Dialog
	{
		private GraphicsContext gc;
		private ShaderProgram program;
		private VertexBuffer vertexBuffer;
		private Texture2D texture;
		private int vertexCount;
		private Vector3[] point = new Vector3[4];
		
		private float[] vertices, texcoords, colors;
		
		private Vector3 norm;
		
		public Dialog (GraphicsContext graphics)
		{
			gc = graphics;
		}
		
		public void Initialize(Texture2D texture)
		{
			vertexCount = 4;
			vertexBuffer = new VertexBuffer(vertexCount, VertexFormat.Float3,
			                                VertexFormat.Float2, VertexFormat.Float4);
			this.texture = texture;
			
//			Font font = new Font(FontAlias.System, 16, FontStyle.Regular);
//			texture = createTexture("あいうえお", font, 0xffffffff);
						
			program = new ShaderProgram("/Application/shaders/Simple.cgx");
			
			vertices = new float[]{
				-1.0f / gc.Screen.AspectRatio, 1.0f * texture.Height / texture.Width, -0.9f,
				-1.0f / gc.Screen.AspectRatio, -1.0f * texture.Height / texture.Width, -0.9f,
				1.0f / gc.Screen.AspectRatio, 1.0f * texture.Height / texture.Width, -0.9f,
				1.0f / gc.Screen.AspectRatio, -1.0f * texture.Height / texture.Width, -0.9f,
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

		}
		
		public void Update()
		{
		}
		
		public void Render()
		{
			vertexBuffer.SetVertices(0, vertices);
			vertexBuffer.SetVertices(1, texcoords);
			vertexBuffer.SetVertices(2, colors);

			
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

    /// Given a string, create a texture
    public static Texture2D createTexture(string text, Font font, uint argb)
    {
        int width = font.GetTextWidth(text, 0, text.Length);
        int height = font.Metrics.Height;

        var image = new Image(ImageMode.Rgba,
                              new ImageSize(width, height),
                              new ImageColor(0, 0, 0, 0));

        image.DrawText(text,
                       new ImageColor((int)((argb >> 16) & 0xff),
                                      (int)((argb >> 8) & 0xff),
                                      (int)((argb >> 0) & 0xff),
                                      (int)((argb >> 24) & 0xff)),
                       font, new ImagePosition(0, 0));

        var texture = new Texture2D(width, height, false, PixelFormat.Rgba);
        texture.SetPixels(0, image.ToBuffer());
        image.Dispose();

        return texture;
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
		
/*		public Texture2D Texture
		{
			set{texture = value;}
		}*/
		
		public bool IsCollision(Vector3 touchPos)
		{
			var cameraPos = Camera.Eye;
			
			/* rayはカメラからタッチした点までの方向ベクトル */
			var ray = Vector3.Subtract(touchPos, cameraPos);
			
			/* 法線ベクトルとrayが直角に交わるのであればrayは面と触れない */
			if(Vector3.Dot(ray, norm) == 0)
			{
				return false;
			}
			
			/* rayが面と触れる場所を計算 これはrayの始点と終点をa:1-aに内分する点として考える */
			
			var disWithCam = Math.Abs(Vector3.Dot(norm, Vector3.Subtract(cameraPos, point[0])));
			var disWithTouchPos = Math.Abs(Vector3.Dot (norm, Vector3.Subtract(touchPos, point[0])));
			var dividingRatio = disWithCam / (disWithCam + disWithTouchPos);
			var collisionPos = Vector3.Add(cameraPos,
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
