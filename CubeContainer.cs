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
			var start = DateTime.Now;
			program = new ShaderProgram("/Application/shaders/VertexColor.cgx");
			var end = DateTime.Now;
			Console.WriteLine((end - start).TotalSeconds);
			
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

