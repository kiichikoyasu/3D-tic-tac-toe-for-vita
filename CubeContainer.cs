using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using System.Collections.Generic;

namespace Dtictactoe
{
	public class CubeContainer
	{
		private Cube[,,] cubes;
		private float interval;
		private int gameSize;
		private GraphicsContext gc;
		private Texture2D texture;
		private ShaderProgram program;
		
		public CubeContainer (GraphicsContext graphics)
		{
			gc = graphics;
		}
		
		public void Initialize()
		{
			
			interval = GameParameters.CubeInterval;
			var cubeSize = GameParameters.CubeSize;
			gameSize = GameParameters.GameSize;
			texture = new Texture2D("/Application/resources/test.png", false);
//			var start = DateTime.Now;
			program = new ShaderProgram("/Application/shaders/VertexColor.cgx");
//			var end = DateTime.Now;
//			Console.WriteLine((end - start).TotalSeconds);
			
			cubes = new Cube[gameSize, gameSize, gameSize];
			
			for(int i = 0; i < gameSize; i++)
			{
				var z = (float)(i - 1) * interval;
				for(int j = 0; j < gameSize; j++)
				{
					var y = (float)(j - 1) * interval;
					for(int k = 0; k < gameSize; k++)
					{
						var x = (float)(k - 1) * interval;
						cubes[i, j, k] = new Cube(gc, x, y, z);
						cubes[i, j, k].Initialize(texture, program);
					}
				}
			}
		}
			
		
		public void Update(GamePadData gamePadData, List<TouchData> touchDataList)
		{
			
			foreach(TouchData touchData in touchDataList)
			{
				if(touchData.Status == TouchStatus.Up)
				{
					if(touchData.ID != 0)
					{
						/* 指1本だけ対応 */
						continue;
					}
					
					float minDist = float.PositiveInfinity;
					int clickedX = 0, clickedY = 0, clickedZ = 0;
					/* スクリーン上のタッチした点から画面に映し出されている範囲で一番奥の点を計算 */
					var screenPos = new Vector4(touchData.X * 2, -touchData.Y * 2, 1.0f, 1.0f);
					var touchLocalPos = Camera.worldViewProj.Inverse().Transform(screenPos);
					touchLocalPos = touchLocalPos.Divide(touchLocalPos.W);
					for(int i = 0; i < gameSize; i++)
					{
						for(int j = 0; j < gameSize; j++)
						{
							for(int k = 0; k < gameSize; k++)
							{
								float dist;
								if((dist = cubes[i, j, k].DistWithCamClicked(touchLocalPos.Xyz)) > 0f)
								{
									/* cubeが触った直線状にいる場合 */
									if(dist < minDist){
										minDist = dist;
										clickedX = i; clickedY = j; clickedZ = k;
									}
								}
							}
						}
					}
					
					if(float.IsPositiveInfinity(minDist))
					{
						/* 触ったcubeなし */
					} else {
						cubes[clickedX, clickedY, clickedZ].Clicked();
						Console.WriteLine(clickedX * 9 + clickedY * 3 + clickedZ);
					}
				}

			}
			
			
			/* cube自身のupdate */
			for(int i = 0; i < gameSize; i++)
			{
				for(int j = 0; j < gameSize; j++)
				{
					for(int k = 0; k < gameSize; k++)
					{
						cubes[i, j, k].Update(gamePadData, touchDataList);
					}
				}
			}
			
			/* この辺で勝敗判定したい */
			
		}

		public void Render()
		{
			
			/* cube自身のrender */
			for(int i = 0; i < gameSize; i++)
			{
				for(int j = 0; j < gameSize; j++)
				{
					for(int k = 0; k < gameSize; k++)
					{
						cubes[i, j, k].Render();
					}
				}
			}
		}
		
		
	}
}

