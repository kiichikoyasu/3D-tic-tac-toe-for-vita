using System;

namespace Dtictactoe
{
	public class GameParameters
	{
		private static int gameSize = 3;
		private static float cubeSize = 1.2f;
		private static float cubeScale = 1.3f;
		private static float cubeInterval = 2.85f;
		private static int prevFrameSize = 10;
		
		public GameParameters ()
		{
		}
		
		public static int GameSize
		{
			get{return gameSize;}	
		}
		
		public static float CubeSize
		{
			get{return cubeSize;}
		}
		
		public static float CubeScale
		{
			get{return cubeScale;}
		}
		
		public static float CubeInterval
		{
			get{return cubeInterval;}
		}
		
		public static int PrevFrameSize
		{
			get{return prevFrameSize;}
		}
	}
}

