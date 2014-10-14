using System;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Imaging;

namespace Dtictactoe
{
	/* カメラの移動に関わらず画像を画面に対して正面に描画するときに使うobject（メッセージなど） */
	public class Object2D
	{
		private GraphicsContext gc;
		private ShaderProgram program;
		private VertexBuffer vertexBuffer;
		private Texture2D texture;
		private int vertexCount;
		/* 位置はスクリーンのピクセルをイメージ */
		private int left, right, top, bottom;
		private int centerX, centerY;
		private float scale;
		
		private bool visible;
	
		private float[] vertices, texcoords, colors;
		
		public Object2D (GraphicsContext graphics)
		{
			gc = graphics;
		}
		
		public void Initialize(ShaderProgram program)
		{
			vertexCount = 4;
			vertexBuffer = new VertexBuffer(vertexCount, VertexFormat.Float3,
			                                VertexFormat.Float2, VertexFormat.Float4);
			
			centerX = 0;
			centerY = 0;
			scale = 1.0f;
			
/*			Font font = new Font(FontAlias.System, 60, FontStyle.Italic);
			texture = createTexture(text, font, 0xffffff00);*/
			
			this.program = program;			
//			program = new ShaderProgram("/Application/shaders/Simple.cgx");
			
				
			
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
			if(visible)
			{
/*			vertices = new float[]{
				(float)-texture.Width / gc.Screen.Width, (float)texture.Height / gc.Screen.Height , -1.0f,
				(float)-texture.Width / gc.Screen.Width, (float)-texture.Height / gc.Screen.Height , -1.0f,
				(float)texture.Width / gc.Screen.Width, (float)texture.Height / gc.Screen.Height , -1.0f,
				(float)texture.Width / gc.Screen.Width, (float)-texture.Height / gc.Screen.Height , -1.0f,
			};*/
				
				vertices = new float[]{
					(float)left / gc.Screen.Width * 2.0f - 1.0f, -((float)top / gc.Screen.Height * 2.0f - 1.0f), -1.0f,
					(float)left / gc.Screen.Width * 2.0f - 1.0f, -((float)bottom / gc.Screen.Height * 2.0f - 1.0f), -1.0f,
					(float)right / gc.Screen.Width * 2.0f - 1.0f, -((float)top / gc.Screen.Height * 2.0f - 1.0f), -1.0f,
					(float)right / gc.Screen.Width * 2.0f - 1.0f, -((float)bottom / gc.Screen.Height * 2.0f - 1.0f), -1.0f,
				};
				
//				Logger.GameInfoLine((float)top / gc.Screen.Height * 2.0f - 1.0f);
			
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
			
				/* 文字の背景が透明？なのかよくわからないが、このブレンドを設定しないと文字がでない */
				gc.Enable(EnableMode.Blend);
				gc.SetBlendFunc(BlendFuncMode.Add, BlendFuncFactor.SrcAlpha, BlendFuncFactor.OneMinusSrcAlpha);

			
				gc.DrawArrays(DrawMode.TriangleStrip, 0, 4);
			}
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
		
		public void SetCenterPos(int centerX, int centerY)
		{
			this.centerX = centerX;
			this.centerY = centerY;
			left = (int)(centerX - scale * texture.Width / 2.0f);
			right = (int)(centerX + scale * texture.Width / 2.0f);
			top = (int)(centerY - scale * texture.Height / 2.0f);
			bottom = (int)(centerY + scale * texture.Height / 2.0f);
		}
		
		public void SetLeftTop(int left, int top)
		{
			this.left = left;
			this.top = top;
			right = left + (int)(texture.Width * scale);
			bottom = top + (int)(texture.Height * scale);
			centerX = (left + right) / 2;
			centerY = (top + bottom) / 2;
		}
		
		public void SetLeftBottom(int left, int bottom)
		{
			this.left = left;
			this.bottom = bottom;
			right = left + (int)(texture.Width * scale);
			top = bottom - (int)(texture.Height * scale);
			centerX = (left + right) / 2;
			centerY = (top + bottom) / 2;
		}
		
		public void SetRightTop(int right, int top)
		{
			this.right = right;
			this.top = top;
			left = right - (int)(texture.Width * scale);
			bottom = top + (int)(texture.Height * scale);
			centerX = (left + right) / 2;
			centerY = (top + bottom) / 2;
		}
		
		public void SetRightBottom(int right, int bottom)
		{
			this.right = right;
			this.bottom = bottom;
			left = right - (int)(texture.Width * scale);
			top = bottom - (int)(texture.Width * scale);
			centerX = (left + right) / 2;
			centerY = (top + bottom) / 2;
		}
		
		public Texture2D Texture
		{
			set
			{
				texture = value;
				left = centerX - (int)(texture.Width * scale);
				right = centerX + (int)(texture.Width * scale);
				top = centerY - (int)(texture.Height * scale);
				bottom = centerY + (int)(texture.Height * scale);
			}
		}
		
		
		public float Scale
		{
			set
			{
				scale = value;
				left = centerX - (int)(texture.Width * scale);
				right = centerX + (int)(texture.Width * scale);
				top = centerY - (int)(texture.Height * scale);
				bottom = centerY + (int)(texture.Height * scale);
			}
		}
		
		public bool Visible
		{
			set{visible = value;}
		}
		
	}
}

