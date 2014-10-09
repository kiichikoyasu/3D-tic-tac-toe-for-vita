using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

namespace Dtictactoe
{
	public class AppMain
	{
		private static GraphicsContext graphics;
		private static ShaderProgram program;
		
		private static Cube cube, cube2;
		
		private static Camera camera;
		
		private static bool loop = true;
		
		public static void Main (string[] args)
		{
			Initialize ();

			while (loop) {
				SystemEvents.CheckEvents ();
				Update ();
				Render ();
			}
		}

		public static void Initialize ()
		{
			// Set up the graphics system
			graphics = new GraphicsContext ();
			cube = new Cube(graphics, 0.0f, 0.0f, 0.0f);
			cube.Initialize();
			
			cube2 = new Cube(graphics, 3.0f, 4.0f, -1.0f);
			cube2.Initialize();
			
			camera = new Camera(graphics);
			camera.Initialize();
			
			
			program = new ShaderProgram("/Application/shaders/VertexColor.cgx");
			program.SetUniformBinding(0, "WorldViewProj");			
			
		}

		public static void Update ()
		{
			// Query gamepad for current state
			var gamePadData = GamePad.GetData (0);

			//input処理
			
#if DEBUG			
			if((gamePadData.Buttons & GamePadButtons.Start) != 0 &&  (gamePadData.Buttons & GamePadButtons.Select) != 0)
			{
				Console.WriteLine("exit."); 
				loop = false;
				return;
			}
#endif			
			
			camera.Update();
			cube.Update();
			cube2.Update();
			
			/*本当はcamera.WorldViewProfをそのままrefしたい
			 * シェーダーの中で書き換わった値を後で使う場合に困る*/
//			Matrix4 worldViewProj = camera.WorldViewProj;
			/* プロパティは直接ref引数に渡せないので無理やりフィールドを読むようにした*/
			program.SetUniformValue(0, ref camera.worldViewProj);
			
			graphics.SetViewport(0, 0, graphics.Screen.Width, graphics.Screen.Height);
			
			
		}

		public static void Render ()
		{
			
			// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear ();
			
			cube.Render (program);
			cube2.Render (program);

			// Present the screen
			graphics.SwapBuffers ();
		}
		
	}
}
