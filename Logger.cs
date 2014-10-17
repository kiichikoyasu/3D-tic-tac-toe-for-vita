using System;
using System.Reflection;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;

using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Graphics;

namespace Dtictactoe
{
	public class Logger
	{
		private static string debugString = String.Empty;
		private static GraphicsContext gc;
		private static ShaderProgram shader;
		private static Font font;
		private static List<Object2D> debugStringList;
		private static int height;
		
		public static void Initialize(GraphicsContext graphics, ShaderProgram program)
		{
#if DEBUG
			gc = graphics;
			shader = program;
			debugStringList = new List<Object2D>();
			font = new Font(FontAlias.System, 14, FontStyle.Regular);
			height = 0;
#endif
		}
		
		public static void Debug(bool value)
		{
#if DEBUG			
			debugString = debugString + value.ToString();
#endif		
		}
		
		public static void Debug(int value)
		{
#if DEBUG			
			debugString = debugString + value.ToString();
#endif		
		}
		
		public static void Debug(uint value)
		{
#if DEBUG			
			debugString = debugString + value.ToString();
#endif		
		}
		
		public static void Debug(long value)
		{
#if DEBUG			
			debugString = debugString + value.ToString();
#endif		
		}
		
		public static void Debug(ulong value)
		{
#if DEBUG			
			debugString = debugString + value.ToString();
#endif		
		}
		
		public static void Debug(float value)
		{
#if DEBUG			
			debugString = debugString + value.ToString();
#endif		
		}
		
		public static void Debug(double value)
		{
#if DEBUG			
			debugString = debugString + value.ToString();
#endif		
		}
		
		public static void Debug(char value)
		{
#if DEBUG			
			debugString = debugString + value.ToString();
#endif		
		}
		
		public static void Debug(char[] value)
		{
#if DEBUG
			for(int i = 0; i < value.Length; i++){
				debugString = debugString + value[i].ToString();
			}
#endif		
		}
		
		public static void Debug(string value)
		{
#if DEBUG
			debugString = debugString + value;
#endif		
		}
		
		public static void Debug(object value)
		{
#if DEBUG			
			debugString = debugString + value.ToString();
#endif		
		}
		

		public static void DebugLine()
		{
#if DEBUG			
			debugString = debugString + "\n";
			SetTextTexture ();
#endif		
		}
		
		public static void DebugLine(bool value)
		{
#if DEBUG			
			debugString = debugString + value.ToString() + "\n";
			SetTextTexture ();
#endif		
		}
		
		public static void DebugLine(int value)
		{
#if DEBUG			
			debugString = debugString + value.ToString() + "\n";
			SetTextTexture ();
#endif		
		}
		
		public static void DebugLine(uint value)
		{
#if DEBUG			
			debugString = debugString + value.ToString() + "\n";
			SetTextTexture ();
#endif		
		}
		
		public static void DebugLine(long value)
		{
#if DEBUG			
			debugString = debugString + value.ToString() + "\n";
			SetTextTexture ();
#endif		
		}
		
		public static void DebugLine(ulong value)
		{
#if DEBUG			
			debugString = debugString + value.ToString() + "\n";
			SetTextTexture ();
#endif		
		}

		public static void DebugLine(float value)
		{
#if DEBUG			
			debugString = debugString + value.ToString() + "\n";
			SetTextTexture ();
#endif		
		}
		
		public static void DebugLine(double value)
		{
#if DEBUG			
			debugString = debugString + value.ToString() + "\n";
			SetTextTexture ();
#endif		
		}
		
		public static void DebugLine(char value)
		{
#if DEBUG			
			debugString = debugString + value.ToString() + "\n";
			SetTextTexture ();
#endif		
		}
		
		public static void DebugLine(char[] value)
		{
#if DEBUG
			for(int i = 0; i < value.Length; i++)
			{
				debugString = debugString + value[i].ToString();
			}
			debugString = debugString + "\n";
			SetTextTexture ();
#endif		
		}
		
		public static void DebugLine(string value)
		{
#if DEBUG			
			debugString = debugString + value + "\n";
			SetTextTexture ();
#endif		
		}
		
		public static void DebugLine(object value)
		{
#if DEBUG			
			debugString = debugString + value + "\n";
			SetTextTexture ();
#endif		
		}
		
		public static void Display()
		{
#if DEBUG
			if(debugString.Length > 0){
				SetTextTexture();
				debugString = String.Empty;
			}
			
			foreach(Object2D obj in debugStringList)
			{
				obj.Render();
			}
			height = 0;
			debugStringList.Clear();
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

		static void SetTextTexture ()
		{
			Object2D obj = new Object2D(gc);
			obj.Initialize(shader);
			obj.Visible = true;
			obj.Texture = Object2D.createTexture(debugString, font, 0xffffffff);
			obj.SetLeftTop(0, height);
			debugStringList.Add(obj);
			height += font.Size;
			Console.Write(debugString);
			debugString = String.Empty;
		}
	}
}

