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
		
		private static Camera camera;
		
		private static bool loop = true;
		
		/* 直近フレームの指id = 0のTouchDataのリスト */
		private static List<TouchData> prevTouchDataList;
		
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

			cubes = new CubeContainer(graphics);
			cubes.Initialize();
			
			camera = new Camera(graphics);
			camera.Initialize();
			
			prevTouchDataList = new List<TouchData>();
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
			
			if(touchDataList.Count == 0)
			{
				/* タッチがなかったとき */
				var nonTouchData = new TouchData();
				nonTouchData.Status = TouchStatus.None;
				prevTouchDataList.Add(nonTouchData);
			}
			
			foreach(TouchData touchData in touchDataList)
			{
				if(touchData.ID != 0)
				{
					/* 指1本だけ対応 */
					continue;
				}
				prevTouchDataList.Add(touchData);
			}
			
			if(prevTouchDataList.Count > GameParameters.PrevFrameSize)
			{
				prevTouchDataList.RemoveAt(0);
			}
			
/*			for(int i = 0; i < prevTouchDataList.Count; i++)
			{
				Console.Write(prevTouchDataList[i].Status);
			}
			Console.WriteLine("");*/
			
			camera.Update(gamePadData, prevTouchDataList);
			
			cubes.Update(gamePadData, prevTouchDataList);
						
			graphics.SetViewport(0, 0, graphics.Screen.Width, graphics.Screen.Height);
			
			
		}

		public static void Render ()
		{
			
			// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear ();
			
			cubes.Render();

			// Present the screen
			graphics.SwapBuffers ();
		}
		
	}
}
