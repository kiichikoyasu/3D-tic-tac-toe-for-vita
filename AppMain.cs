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
		
		public static int count;
		
		private static CubeContainer cubes;
		
		private static Camera camera;
		
		public static GameStatus gameStatus;
		
	 	private static Player[] players;
		private static Player currentPlayre;
		
		private static Object2D dialog;
		
		private static bool loop = true;
		private static bool isCubeUpdate = true;
		
		private static TouchDataList touchDataList;

		private static float epsilon = 0.01f;
		
		private static ShaderProgram simpleShader;
		
		public static void Main (string[] args)
		{
			Initialize ();

			while (loop) {
				SystemEvents.CheckEvents ();
				Update ();
				Render ();
			}
		}

		static void MakeOrder ()
		{
			var random = new Random();
			int order = 1;
			while(order < 5)
			{
				var rdm = random.Next(4);
				if(players[rdm].order == 0)
				{
					players[rdm].order = order;
					order++;
				}
			}
			for(int i = 0; i < 4; i++)
			{
				Logger.GameInfoLine(players[i].order);
			}
		}

		public static void Initialize ()
		{
			// Set up the graphics system
			graphics = new GraphicsContext ();
			
			count = 0;

			cubes = new CubeContainer(graphics);
			cubes.Initialize();
			
			camera = new Camera(graphics);
			camera.Initialize();
			
			gameStatus = GameStatus.Start;
			
			players = new Player[4];
			for(int i = 0; i < 4 ; i++)
			{
				players[i] = new Player();
				players[i].id = i;
			}
			players[0].isHuman = true;
			players[1].isHuman = false;
			players[2].isHuman = false;
			players[3].isHuman = false;
			
			/* 順番決め */
//			MakeOrder ();
			
			/* object2D用のシェーダー */
			simpleShader = new ShaderProgram("/Application/shaders/Simple.cgx");

			
			dialog = new Object2D(graphics);
			dialog.Initialize(simpleShader);
			dialog.Visible = false;
			
			touchDataList = new TouchDataList();
		}

		public static void Update ()
		{
			// Query gamepad for current state
			//inputの取得は1フレームに1回のみ
			var gamePadData = GamePad.GetData(0);
//			var touchDataList = Touch.GetData(0);
			touchDataList.Update(Touch.GetData(0));
			
			MyTouchData currentTouchData = touchDataList.getCurrentFrameData();

			count++;
//			isCameraUpdate = true;
			isCubeUpdate = true;

			//input処理			
						
/*			if(count % 6 == 0){
				for(int i = 0; i < touchDataL.Count; i++)
				{
					Logger.GameInfo(touchDataL[i].Status);
				}
				Logger.GameInfoLine();
			}*/
			
#if DEBUG			
			if((gamePadData.Buttons & GamePadButtons.Start) != 0 &&
			   (gamePadData.Buttons & GamePadButtons.Select) != 0)
			{
				Logger.GameInfoLine("exit."); 
				loop = false;
				return;
			}
			
			if((gamePadData.Buttons & GamePadButtons.L) != 0 &&
			   (gamePadData.Buttons & GamePadButtons.R) != 0)
			{
				gameStatus = GameStatus.Finish;
			}

#endif		
			
/*			if(count % 60 == 0)
			{
				Logger.GameInfoLine("Game Status:" + gameStatus.ToString());
				Logger.Info();
			}*/			
			switch(gameStatus)
			{
			case GameStatus.Start:
//				isCameraUpdate = false;
				isCubeUpdate = false;
				/* タッチ */
				if(currentTouchData.Status == MyTouchStatus.Clicked)
				{
//					gameStatus = GameStatus.Message;
					gameStatus = GameStatus.First;
					MakeOrder();
					dialog.Texture = cubes.GetTexture(players[0].order);
					dialog.Scale = (0.25f);
					dialog.SetLeftTop(0, 0);
					dialog.Visible = true;
					count = 0;
//					isCubeUpdate = true;
				}
				break;
				
			case GameStatus.Message:
				/* タッチ */
				if(currentTouchData.Status == MyTouchStatus.Down)
				{
//					isCameraUpdate = false;
					gameStatus = GameStatus.First;
				}
				break;
			case GameStatus.First: 
			case GameStatus.Second:
			case GameStatus.Third:
			case GameStatus.Forth:
				
				if(count < 10)
				{
					isCubeUpdate = false;
				}
				for(int i = 0; i < 4; i++)
				{
					if(players[i].order == (int)gameStatus)
					{
						currentPlayre = players[i];
						break;
					}
				}
				
				bool isCubeSelected = false;
				if(currentPlayre.isHuman)
				{
					/* 人間の入力待ち */
					/* スティック */
					Vector2 inputVector;			
					inputVector = new Vector2(gamePadData.AnalogLeftX, -gamePadData.AnalogLeftY);
					if(inputVector.Length() > epsilon)
					{
						inputVector = inputVector.Normalize();
						camera.Move(inputVector);
					}
					
					/* タッチ */
					if(currentTouchData.Status == MyTouchStatus.Clicked)
					{
						isCubeSelected = cubes.IsCubeSelected(currentTouchData.X, -currentTouchData.Y);
						//						Logger.GameInfoLine("Clicked!");
					}
					
					if(currentTouchData.Status == MyTouchStatus.Move)
					{
						var currentPoint = new Vector2(currentTouchData.X, -currentTouchData.Y);
						var prevPoint = new Vector2(touchDataList.getPrevFrameData().X,
						                            -touchDataList.getPrevFrameData().Y);
						inputVector = Vector2.Subtract(currentPoint, prevPoint);
						/* touchはスティックに比べて値が小さいのでepsilonの調整余地あり */
						if(inputVector.Length() > epsilon)
						{
							inputVector = inputVector.Normalize();
							camera.Move(inputVector);
						}
					}
				}else {
					/* コンピュータの計算待ち */
					cubes.CpuClick(currentPlayre);
					isCubeSelected = true;
//					isCameraUpdate = false;
					isCubeUpdate = false;
				}
				/* 勝利判定 */
				if(isCubeSelected)
				{
					int judge = cubes.JudgeGame();
					if(judge == 0)
					{
						/* ゲーム続行 */
						gameStatus = (GameStatus)((int)gameStatus % 4 + 1);
					} else if(judge == 1){
						Logger.GameInfoLine("Player" + currentPlayre.id + "win !");
						gameStatus = GameStatus.Finish;
						count = 0;
					} else {
						Logger.GameInfoLine("Draw");
						gameStatus = GameStatus.Finish;
						count = 0;
					}
				}
				break;
			case GameStatus.Finish:
				dialog.Visible = false;
				isCubeUpdate = false;
				/* スティック */
				Vector2 input;			
				input = new Vector2(gamePadData.AnalogLeftX, -gamePadData.AnalogLeftY);
				if(input.Length() > epsilon)
				{
					input = input.Normalize();
					camera.Move(input);
				}
				/* タッチ */
				if(currentTouchData.Status == MyTouchStatus.Move)
				{
					var currentPoint = new Vector2(currentTouchData.X, -currentTouchData.Y);
					var prevPoint = new Vector2(touchDataList.getPrevFrameData().X,
					                            -touchDataList.getPrevFrameData().Y);
					input = Vector2.Subtract(currentPoint, prevPoint);
					/* touchはスティックに比べて値が小さいのでepsilonの調整余地あり */
					if(input.Length() > epsilon)
					{
						input = input.Normalize();
						camera.Move(input);
					}
				}
				
				if(currentTouchData.Status == MyTouchStatus.Clicked)
				{
					gameStatus = GameStatus.Start;
					/* いろいろ元の状態に戻してスタート */
					/* カメラ（視点）を先に戻さないとスタート時に画面が元の位置に戻らない */
					camera.Reset();
					cubes.Reset();
					for(int i = 0; i < 4; i++)
					{
						players[i].order = 0;
					}
					count = 0;
				}
				break;
				
			default:
				break;
			}
			
			camera.Update();
			dialog.Update();
			
			if(isCubeUpdate) cubes.Update(gamePadData);
						
			graphics.SetViewport(0, 0, graphics.Screen.Width, graphics.Screen.Height);
			
			Logger.Display();
//			SystemMemory.Dump();
			
		}

		public static void Render ()
		{
			
			// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear ();
			
			
			dialog.Render();
			cubes.Render();

			// Present the screen
			graphics.SwapBuffers ();
		}
	
		private static bool ContainStatusPrev (List<TouchData> list, TouchStatus queryStatus)
		{
			bool isContain = false;
			foreach(TouchData data in list)
			{
				isContain |= data.Status == queryStatus;
			}
			return isContain;
		}
		
		
	}
}
