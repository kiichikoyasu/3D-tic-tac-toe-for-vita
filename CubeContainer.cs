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
		private Texture2D[] textures;
		private ShaderProgram program;
		private int gameStatus;
		private int clickCount;

				
		public CubeContainer (GraphicsContext graphics)
		{
			gc = graphics;
		}
		
		public void Initialize()
		{
			
			interval = GameParameters.CubeInterval;
			var cubeSize = GameParameters.CubeSize;
			gameSize = GameParameters.GameSize;
			textures = new Texture2D[5];
			textures[0] = new Texture2D("/Application/resources/test.png", false);
			textures[1] = new Texture2D("/Application/resources/circle.png", false);
			textures[2] = new Texture2D("/Application/resources/cross.png", false);
			textures[3] = new Texture2D("/Application/resources/triangle.png", false);
			textures[4] = new Texture2D("/Application/resources/square.png", false);
//			var start = DateTime.Now;
			program = new ShaderProgram("/Application/shaders/VertexColor.cgx");
//			var end = DateTime.Now;
//			Log.DebugLine((end - start).TotalSeconds);
			
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
						cubes[i, j, k].Initialize(textures[0], program);
					}
				}
			}
		}
			
		
		public void Update(GamePadData gamePadData, TouchDataList touchDataList)
		{
			gameStatus = (int)AppMain.gameStatus;

			
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
			
		}

/*		private bool LineJudge (Vector3 rayStart, Vector3 rayEnd)
		{
			List<CubeStatus> hitCubeIndex = new List<CubeStatus>();
			for(int i = 0; i < gameSize; i++)
			{
				for(int j = 0; j < gameSize; j++)
				{
					for(int k = 0; k < gameSize; k++)
					{
						if(cubes[i, j, k].DistWithRayStartClicked(rayStart, rayEnd) > 0)
						{
							hitCubeIndex.Add(cubes[i, j, k].Status);
						}
					}
				}
			}
			
			if(hitCubeIndex.Count == 3)
			{
				if(hitCubeIndex[0] != CubeStatus.NotSelected)
				{
					if(hitCubeIndex[0] == hitCubeIndex[1] && hitCubeIndex[1] == hitCubeIndex[2])
					{*/
						/*ダイアログで誰が勝ったか表示*/
						/* finish状態に遷移 */
/*						AppMain.gameStatus = GameStatus.Finish;
						return true;
					}
				}
			}
			return false;
		}*/
		
		private bool XLineJudge (int y, int z)
		{
			if(cubes[0, y, z].Status != CubeStatus.NotSelected)
			{
				return cubes[0, y, z].Status == cubes[1, y, z].Status && cubes[1, y, z].Status == cubes[2, y, z].Status;
			}
			return false;
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
		
		private bool ContainStatusPrev (List<TouchData> list, TouchStatus queryStatus)
		{
			bool isContain = false;
			foreach(TouchData data in list)
			{
				isContain |= data.Status == queryStatus;
			}
			return isContain;
		}
		
		/* コンピュータがますを更新するための関数 */
		public bool CpuClick(Player cpu)
		{
			Random random = new Random();
			while(cpu.order == (int)AppMain.gameStatus)
			{
				var x = random.Next(3);
				var y = random.Next(3);
				var z = random.Next(3);
				if(cubes[x,y,z].Status == CubeStatus.NotSelected)
				{
					cubes[x,y,z].Clicked(textures[cpu.order],(CubeStatus)cpu.order);
					clickCount++;
					return true;
				}
			}
			return false;
		}
		
		public void Reset(){
			clickCount = 0;
			for(int i = 0; i < gameSize; i++)
			{
				for(int j = 0; j < gameSize; j++)
				{
					for(int k = 0; k < gameSize; k++)
					{
						cubes[i, j, k].Reset(textures[0]);
					}
				}
			}
		}
		
		/* 
		 * 勝敗判定をするメソッド
		 * 
		 * 勝敗が決まった場合は1を返す
		 * 勝負が引き分けに終わった場合は2
		 * まだゲーム続行の場合は0を返す
		 * 
		 * */		
		public int JudgeGame()	
		{
			bool b = false;
			if(b)
			{
				return 1;
			}else if(clickCount >= gameSize * gameSize * gameSize)
			{
				return 2;
			} else {
				return 0;
			}	
		}
		
		public bool IsCubeSelected(float touchX, float touchY)
		{
			float minDist = float.PositiveInfinity;
			int clickedX = 0, clickedY = 0, clickedZ = 0;
			/* スクリーン上のタッチした点から画面に映し出されている範囲で一番奥の点を計算 */
			var screenPos = new Vector4(touchX * 2, touchY * 2, 1.0f, 1.0f);
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
				return false;
			} else {
				if(cubes[clickedX, clickedY, clickedZ].Status == CubeStatus.NotSelected)
				{
					cubes[clickedX, clickedY, clickedZ].Clicked(textures[(int)AppMain.gameStatus], (CubeStatus)(gameStatus));
					clickCount++;
					return true;
				} else {
					/* クリックしたけどすでに選択済みのcubeだった場合 */
					return false;
				}
			}
		}
	}
}

