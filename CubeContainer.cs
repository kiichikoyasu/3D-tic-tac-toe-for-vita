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
		private int clickCount;
		private Cube clickingCube;
		private int clickingX, clickingY, clickingZ;

				
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
				for(int j = 0; j < gameSize; j++)
				{
					for(int k = 0; k < gameSize; k++)
					{
						cubes[i, j, k] = new Cube(gc, i, j, k);
						cubes[i, j, k].Initialize(textures[0], program);
					}
				}
			}
		}
			
		
		public void Update(GamePadData gamePadData)
		{
			/* cube自身のupdate */
			for(int i = 0; i < gameSize; i++)
			{
				for(int j = 0; j < gameSize; j++)
				{
					for(int k = 0; k < gameSize; k++)
					{
						cubes[i, j, k].Update(gamePadData);
					}
				}
			}
			
		}
		
		private bool XLineJudge(int y, int z)
		{
			if(cubes[0, y, z].SelectStatus != CubeSelectStatus.NotSelected)
			{
				return cubes[0, y, z].SelectStatus == cubes[1, y, z].SelectStatus &&
					cubes[1, y, z].SelectStatus == cubes[2, y, z].SelectStatus;
			}
			return false;
		}

		private bool YLineJudge(int x, int z)
		{
			if(cubes[x, 0, z].SelectStatus != CubeSelectStatus.NotSelected)
			{
				return cubes[x, 0, z].SelectStatus == cubes[x, 1, z].SelectStatus &&
					cubes[x, 1, z].SelectStatus == cubes[x, 2, z].SelectStatus;
			}
			return false;
		}

		private bool ZLineJudge(int x, int y)
		{
			if(cubes[x, y, 0].SelectStatus != CubeSelectStatus.NotSelected)
			{
				return cubes[x, y, 0].SelectStatus == cubes[x, y, 1].SelectStatus &&
					cubes[x, y, 1].SelectStatus == cubes[x, y, 2].SelectStatus;
			}
			return false;
		}
		
		private bool XYLineJudge(int z)
		{
			bool isBingo = false;
			if(cubes[0, 0, z].SelectStatus != CubeSelectStatus.NotSelected)
			{
				isBingo |= cubes[0, 0, z].SelectStatus == cubes[1, 1, z].SelectStatus &&
					cubes[1, 1, z].SelectStatus == cubes[2, 2, z].SelectStatus;
			}
			if(cubes[0, 2, z].SelectStatus != CubeSelectStatus.NotSelected)
			{
				isBingo |= cubes[0, 2, z].SelectStatus == cubes[1, 1, z].SelectStatus &&
					cubes[1, 1, z].SelectStatus == cubes[2, 0, z].SelectStatus;
			}
			return isBingo;
		}
		
		private bool YZLineJudge(int x)
		{
			bool isBingo = false;
			if(cubes[x, 0, 0].SelectStatus != CubeSelectStatus.NotSelected)
			{
				isBingo |= cubes[x, 0, 0].SelectStatus == cubes[x, 1, 1].SelectStatus &&
					cubes[x, 1, 1].SelectStatus == cubes[x, 2, 2].SelectStatus;
			}
			if(cubes[x, 2, 0].SelectStatus != CubeSelectStatus.NotSelected)
			{
				isBingo |= cubes[x, 2, 0].SelectStatus == cubes[x, 1, 1].SelectStatus &&
					cubes[x, 1, 1].SelectStatus == cubes[x, 0, 2].SelectStatus;
			}
			return isBingo;
		}
		
		private bool ZXLineJudge(int y)
		{
			bool isBingo = false;
			if(cubes[0, y, 0].SelectStatus != CubeSelectStatus.NotSelected)
			{
				isBingo |= cubes[0, y, 0].SelectStatus == cubes[1, y, 1].SelectStatus &&
					cubes[1, y, 1].SelectStatus == cubes[2, y, 2].SelectStatus;
			}
			if(cubes[2, y, 0].SelectStatus != CubeSelectStatus.NotSelected)
			{
				isBingo |= cubes[2, y, 0].SelectStatus == cubes[1, y, 1].SelectStatus &&
					cubes[1, y, 1].SelectStatus == cubes[0, y, 2].SelectStatus;
			}
			return isBingo;
		}
		
		private bool XYSpheareJugde(int z)
		{
			bool isBingo = false;
			for(int i = 0; i < gameSize; i++)
			{
				isBingo |= XLineJudge(i, z);
				isBingo |= YLineJudge(i, z);
			}
			isBingo |= XYLineJudge(z);
			return isBingo;
		}

		private bool YZSpheareJugde(int x)
		{
			bool isBingo = false;
			for(int i = 0; i < gameSize; i++)
			{
				isBingo |= YLineJudge(x, i);
				isBingo |= ZLineJudge(x, i);
			}
			isBingo |= YZLineJudge(x);
			return isBingo;
		}

		private bool ZXSpheareJugde(int y)
		{
			bool isBingo = false;
			for(int i = 0; i < gameSize; i++)
			{
				isBingo |= ZLineJudge(1, y);
				isBingo |= XLineJudge(y, 1);
			}
			isBingo |= ZXLineJudge(y);
			return isBingo;
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
			Logger.DebugLine("clickCube (" + clickingX + ", " + clickingY + ", " + clickingZ + ")");
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
				if(cubes[x, y, z].SelectStatus == CubeSelectStatus.NotSelected)
				{
					cubes[x, y, z].Clicked(textures[cpu.order],(CubeSelectStatus)cpu.order);
					clickCount++;
					clickingCube = cubes[x, y, z];
					clickingX = x; clickingY = y; clickingZ = z;
					return true;
				}
			}
			return false;
		}
		
		public void Reset(){
			clickCount = 0;
			clickingCube = null;
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
			bool isBingo = false;
/*			for(int i = 0; i < gameSize; i++)
			{
				isBingo |= XYSpheareJugde(i);
				isBingo |= YZSpheareJugde(i);
				isBingo |= ZXSpheareJugde(i);
			}*/
			/*ななめ*/
/*			if(cubes[1, 1, 1].SelectStatus != CubeSelectStatus.NotSelected)
			{
				isBingo |= (cubes[0, 0, 0].SelectStatus == cubes[1, 1, 1].SelectStatus && cubes[1, 1, 1].SelectStatus == cubes[2, 2, 2].SelectStatus)
					|| (cubes[2, 0, 0].SelectStatus == cubes[1, 1, 1].SelectStatus && cubes[1, 1, 1].SelectStatus == cubes[0, 2, 2].SelectStatus)
					|| (cubes[0, 2, 0].SelectStatus == cubes[1, 1, 1].SelectStatus && cubes[1, 1, 1].SelectStatus == cubes[2, 0, 2].SelectStatus)
					|| (cubes[0, 0, 2].SelectStatus == cubes[1, 1, 1].SelectStatus && cubes[1, 1, 1].SelectStatus == cubes[2, 2, 0].SelectStatus);
			}*/
			isBingo |= XLineJudge(clickingY, clickingZ);
			isBingo |= YLineJudge(clickingX, clickingZ);
			isBingo |= ZLineJudge(clickingX, clickingY);
			/* diagonal line on x,y-surface */
			if((clickingX + clickingY) % 2 == 0)
			{
				if(clickingX == 1 || clickingY == 1)
				{
					isBingo |= cubes[0, 0, clickingZ].SelectStatus == cubes[clickingX, clickingY, clickingZ].SelectStatus
						&& cubes[clickingX, clickingY, clickingZ].SelectStatus == cubes[2, 2, clickingZ].SelectStatus;
					isBingo |= cubes[2, 0, clickingZ].SelectStatus == cubes[clickingX, clickingY, clickingZ].SelectStatus
						&& cubes[clickingX, clickingY, clickingZ].SelectStatus == cubes[0, 2, clickingZ].SelectStatus;
				} else {
					/* f(x) = x * 2 + 2 mod 3を適用すると1はそのままで0が2に, 2が0になる */
					isBingo |= cubes[clickingX, clickingY, clickingZ].SelectStatus == cubes[1, 1, clickingZ].SelectStatus
						&& cubes[1, 1, clickingZ].SelectStatus == cubes[(clickingX * 2 + 2) % 3, (clickingY * 2 + 2) % 3, clickingZ].SelectStatus;
				}
			}
			/* diagonal line on y,z-surface */
			if((clickingY + clickingZ) % 2 == 0)
			{
				if(clickingY == 1 || clickingZ == 1)
				{
					isBingo |= cubes[clickingX, 0, 0].SelectStatus == cubes[clickingX, clickingY, clickingZ].SelectStatus
						&& cubes[clickingX, clickingY, clickingZ].SelectStatus == cubes[clickingX, 2, 2].SelectStatus;
					isBingo |= cubes[clickingX, 0, 2].SelectStatus == cubes[clickingX, clickingY, clickingZ].SelectStatus
						&& cubes[clickingX, clickingY, clickingZ].SelectStatus == cubes[clickingX, 2, 0].SelectStatus;
				} else {
					/* f(x) = x * 2 + 2 mod 3を適用すると1はそのままで0が2に, 2が0になる */
					isBingo |= cubes[clickingX, clickingY, clickingZ].SelectStatus == cubes[clickingX, 1, 1].SelectStatus
						&& cubes[clickingX, 1, 1].SelectStatus == cubes[clickingX, (clickingY * 2 + 2) % 3, (clickingZ * 2 + 2) % 3].SelectStatus;
				}
			}
			/* diagonal line on z,x-surface */
			if((clickingZ + clickingX) % 2 == 0)
			{
				if(clickingZ == 1 || clickingX == 1)
				{
					isBingo |= cubes[0, clickingY, 0].SelectStatus == cubes[clickingX, clickingY, clickingZ].SelectStatus
						&& cubes[clickingX, clickingY, clickingZ].SelectStatus == cubes[2, clickingY, 2].SelectStatus;
					isBingo |= cubes[0, clickingY, 2].SelectStatus == cubes[clickingX, clickingY, clickingZ].SelectStatus
						&& cubes[clickingX, clickingY, clickingZ].SelectStatus == cubes[2, clickingY, 0].SelectStatus;
				} else {
					/* f(x) = x * 2 + 2 mod 3を適用すると1はそのままで0が2に, 2が0になる */
					isBingo |= cubes[clickingX, clickingY, clickingZ].SelectStatus == cubes[1, clickingY, 1].SelectStatus
						&& cubes[1, clickingY, 1].SelectStatus == cubes[(clickingX * 2 + 2) % 3, clickingY, (clickingZ * 2 + 2) % 3].SelectStatus;
				}
			}
			
			/* diagonal line on cube */
			if(cubes[clickingX, clickingY, clickingZ].PositionStatus == CubePositionStatus.Core)
			{
				isBingo |= cubes[0, 0, 0].SelectStatus == cubes[1, 1, 1].SelectStatus && cubes[1, 1, 1].SelectStatus == cubes[2, 2, 2].SelectStatus;
				isBingo	|= cubes[2, 0, 0].SelectStatus == cubes[1, 1, 1].SelectStatus && cubes[1, 1, 1].SelectStatus == cubes[0, 2, 2].SelectStatus;
				isBingo	|= cubes[0, 2, 0].SelectStatus == cubes[1, 1, 1].SelectStatus && cubes[1, 1, 1].SelectStatus == cubes[2, 0, 2].SelectStatus;
				isBingo |= cubes[0, 0, 2].SelectStatus == cubes[1, 1, 1].SelectStatus && cubes[1, 1, 1].SelectStatus == cubes[2, 2, 0].SelectStatus;				
			}
			
			
			if(cubes[clickingX, clickingY, clickingZ].PositionStatus == CubePositionStatus.Vertex)
			{	
				/* f(x) = x * 2 + 2 mod 3を適用すると1はそのままで0が2に, 2が0になる */
				isBingo |= cubes[clickingX, clickingY, clickingZ].SelectStatus == cubes[1, 1, 1].SelectStatus &&
							cubes[1, 1, 1].SelectStatus == cubes[(clickingX * 2 + 2) % 3, (clickingY * 2 + 2) % 3, (clickingZ * 2 + 2) % 3].SelectStatus;
			}
			
			if(isBingo)
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
//			int clickedX = 0, clickedY = 0, clickedZ = 0;
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
								clickingX = i; clickingY = j; clickingZ = k;
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
				if(cubes[clickingX, clickingY, clickingZ].SelectStatus == CubeSelectStatus.NotSelected)
				{
					cubes[clickingX, clickingY, clickingZ].Clicked(textures[(int)AppMain.gameStatus], (CubeSelectStatus)((int)AppMain.gameStatus));
					clickCount++;
					clickingCube = cubes[clickingX, clickingY, clickingZ];
					return true;
				} else {
					/* クリックしたけどすでに選択済みのcubeだった場合 */
					return false;
				}
			}
		}
		
		public Texture2D GetTexture(int status)
		{
			return textures[status];
		}
		
	}
}

