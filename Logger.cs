using System;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace Dtictactoe
{
	public class Logger
	{
		private static string gameInfo = String.Empty;
		
		public static void GameInfo(bool value)
		{
#if DEBUG			
			gameInfo = gameInfo + value.ToString();
#endif		
		}
		
		public static void GameInfo(int value)
		{
#if DEBUG			
			gameInfo = gameInfo + value.ToString();
#endif		
		}
		
		public static void GameInfo(uint value)
		{
#if DEBUG			
			gameInfo = gameInfo + value.ToString();
#endif		
		}
		
		public static void GameInfo(long value)
		{
#if DEBUG			
			gameInfo = gameInfo + value.ToString();
#endif		
		}
		
		public static void GameInfo(ulong value)
		{
#if DEBUG			
			gameInfo = gameInfo + value.ToString();
#endif		
		}
		
		public static void GameInfo(float value)
		{
#if DEBUG			
			gameInfo = gameInfo + value.ToString();
#endif		
		}
		
		public static void GameInfo(double value)
		{
#if DEBUG			
			gameInfo = gameInfo + value.ToString();
#endif		
		}
		
		public static void GameInfo(char value)
		{
#if DEBUG			
			gameInfo = gameInfo + value.ToString();
#endif		
		}
		
		public static void GameInfo(char[] value)
		{
#if DEBUG
			for(int i = 0; i < value.Length; i++){
				gameInfo = gameInfo + value[i].ToString();
			}
#endif		
		}
		
		public static void GameInfo(string value)
		{
#if DEBUG
			gameInfo = gameInfo + value;
#endif		
		}
		
		public static void GameInfo(object value)
		{
#if DEBUG			
			gameInfo = gameInfo + value.ToString();
#endif		
		}
		

		public static void GameInfoLine()
		{
#if DEBUG			
			gameInfo = gameInfo + "\n";
#endif		
		}
		
		public static void GameInfoLine(bool value)
		{
#if DEBUG			
			gameInfo = gameInfo + value.ToString() + "\n";
#endif		
		}
		
		public static void GameInfoLine(int value)
		{
#if DEBUG			
			gameInfo = gameInfo + value.ToString() + "\n";
#endif		
		}
		
		public static void GameInfoLine(uint value)
		{
#if DEBUG			
			gameInfo = gameInfo + value.ToString() + "\n";
#endif		
		}
		
		public static void GameInfoLine(long value)
		{
#if DEBUG			
			gameInfo = gameInfo + value.ToString() + "\n";
#endif		
		}
		
		public static void GameInfoLine(ulong value)
		{
#if DEBUG			
			gameInfo = gameInfo + value.ToString() + "\n";
#endif		
		}

		public static void GameInfoLine(float value)
		{
#if DEBUG			
			gameInfo = gameInfo + value.ToString() + "\n";
#endif		
		}
		
		public static void GameInfoLine(double value)
		{
#if DEBUG			
			gameInfo = gameInfo + value.ToString() + "\n";
#endif		
		}
		
		public static void GameInfoLine(char value)
		{
#if DEBUG			
			gameInfo = gameInfo + value.ToString() + "\n";
#endif		
		}
		
		public static void GameInfoLine(char[] value)
		{
#if DEBUG
			for(int i = 0; i < value.Length; i++)
			{
				gameInfo = gameInfo + value[i].ToString();
			}
			gameInfo = gameInfo + "\n";
#endif		
		}
		
		public static void GameInfoLine(string value)
		{
#if DEBUG			
			gameInfo = gameInfo + value + "\n";
#endif		
		}
		
		public static void GameInfoLine(object value)
		{
#if DEBUG			
			gameInfo = gameInfo + value + "\n";
#endif		
		}
		
		public static void Display()
		{
#if DEBUG
			if(gameInfo.Length > 0){
				Console.Write(gameInfo);
				gameInfo = String.Empty;
			}
#endif		
		}
		
		public static void ProgramInfo()
		{
#if DEBUG
			/* 行数を読むためには引数にtrueを与える必要があるが、これをすると落ちる */
			StackTrace st = new StackTrace();
			StackFrame sf1 = st.GetFrame(1);
			MethodBase mb1 = sf1.GetMethod();
			
			Console.WriteLine("[" + mb1.DeclaringType + "." + mb1.Name + "]");
//			Console.WriteLine(sf1.GetFileLineNumber());
#endif		
		}
	}
}

