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
		
		private static CubeContainer cubes;
		
//		private static Cube cube, cube2;
		
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
//			cube = new Cube(graphics, 0.0f, 0.0f, 0.0f);
//			cube.Initialize();
			
//			cube2 = new Cube(graphics, 3.0f, 4.0f, -1.0f);
//			cube2.Initialize();
			
			cubes = new CubeContainer(graphics);
			cubes.Initialize();
			
			camera = new Camera(graphics);
			camera.Initialize();
		}

		public static void Update ()
		{
			// Query gamepad for current state
			//inputの取得は1フレームに1回のみ
			var gamePadData = GamePad.GetData(0);
			var touchDataList = Touch.GetData(0);

			//input処理
			
#if DEBUG			
			if((gamePadData.Buttons & GamePadButtons.Start) != 0 &&
			   (gamePadData.Buttons & GamePadButtons.Select) != 0)
			{
				Console.WriteLine("exit."); 
				loop = false;
				return;
			}
#endif		
			
			camera.Update(gamePadData, touchDataList);
			foreach(TouchData touchData in touchDataList)
			{
				if(touchData.Status == TouchStatus.Up)
				{
					/* スクリーン上のタッチした点から画面に映し出されている範囲で一番奥の点を計算 */
					var screenPos = new Vector4(touchData.X * 2, -touchData.Y * 2, 1.0f, 1.0f);
					var touchLocalPos = Camera.worldViewProj.Inverse().Transform(screenPos);
					touchLocalPos = touchLocalPos.Divide(touchLocalPos.W);
//					cube.isTouch(touchLocalPos.Xyz);
//					cube2.isTouch(touchLocalPos.Xyz);
				}
			}
			
//			cube.Update(gamePadData, touchDataList);
//			cube2.Update(gamePadData, touchDataList);
			cubes.Update(gamePadData, touchDataList);
						
			graphics.SetViewport(0, 0, graphics.Screen.Width, graphics.Screen.Height);
			
			
		}

		public static void Render ()
		{
			
			// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear ();
			
//			cube.Render ();
//			cube2.Render ();
			cubes.Render();

			// Present the screen
			graphics.SwapBuffers ();
		}
		
	}
}
