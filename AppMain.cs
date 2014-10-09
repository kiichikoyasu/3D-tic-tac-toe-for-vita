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
//		private static VertexBuffer vertexBuffer;
		private static ShaderProgram program;
//		private static Texture2D texture;
//		private static int vertexCount;
		
//		private static Matrix4 view, proj, world;
//		private static Vector3 eyePosition, centerPosition, cameraUpPosition;
		
		private static Cube cube, cube2;
		
		private static Camera camera;
		
		private static float epsilon = 0.01f;
		
		public static void Main (string[] args)
		{
			Initialize ();

			while (true) {
				SystemEvents.CheckEvents ();
				Update ();
				Render ();
			}
		}

		public static void Initialize ()
		{
			// Set up the graphics system
			graphics = new GraphicsContext ();
			cube = new Cube(graphics, 0.0f, 0.0f, 0.0f);
			cube.Initialize();
			
			cube2 = new Cube(graphics, 3.0f, 4.0f, -1.0f);
			cube2.Initialize();
			
			camera = new Camera(graphics);
			camera.Initialize();
			
			/*
			vertexCount = 4;
			vertexBuffer = new VertexBuffer(vertexCount, VertexFormat.Float3,
			                                VertexFormat.Float2, VertexFormat.Float4);
			texture = new Texture2D("/Application/resources/test.png", false);
			
			vertexBuffer.SetVertices(0,
			                         new float[]{
				-1f, 1f, 0f,
				-1f, -1f, 0f,
				1f, 1f, 0f,
				1f, -1f, 0f,
			});
			
			vertexBuffer.SetVertices(1,
			                     new float[]{
				0.0f, 0.0f,
				0.0f, 1.0f,
				1.0f, 0.0f,
				1.0f, 1.0f,
			});
			
			vertexBuffer.SetVertices(2,
			                         new float[]{
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
				1f, 1f, 1f, 1f,
			});
			
			*/
			
			program = new ShaderProgram("/Application/shaders/VertexColor.cgx");
			program.SetUniformBinding(0, "WorldViewProj");
			
			/*
			float aspect = graphics.Screen.AspectRatio;
			float fov = FMath.Radians(45.0f);
			
			proj = Matrix4.Perspective(fov, aspect, 1.0f, 1000.0f);
			
			eyePosition = new Vector3(0.0f, 0.0f, 10.0f);
			centerPosition = new Vector3(0.0f, 0.0f, 0.0f);
			cameraUpPosition = Vector3.UnitY;
			*/
			
			/*CalcCameraPosメソッドにおいて注視点が原点じゃない時でも動くかテスト用*/
/*			eyePosition = new Vector3(0.0f, 0.0f, 10.0f);
			centerPosition = new Vector3(2.0f, 0.0f, 0.0f);
			cameraUpPosition = new Vector3(0.0f, 1.0f, 0.0f);*/
			
//			world = Matrix4.Identity;
			
			
		}

		public static void Update ()
		{
			// Query gamepad for current state
			var gamePadData = GamePad.GetData (0);
			
			//input処理
			
			Vector2 inputVector;
			
			if((gamePadData.ButtonsDown & GamePadButtons.Right) != 0){
//				CalcCameraPos(ref eyePosition, ref cameraUpPosition, centerPosition, new Vector2(1.0f, 0.0f));
				camera.CalcPos(new Vector2(1.0f, 0.0f));
			}
			
			if((gamePadData.ButtonsDown & GamePadButtons.Left) != 0){
//				CalcCameraPos(ref eyePosition, ref cameraUpPosition, centerPosition, new Vector2(-1.0f, 0.0f));
				camera.CalcPos(new Vector2(-1.0f, 0.0f));
			}
			
			if((gamePadData.ButtonsDown & GamePadButtons.Up) != 0){
//				CalcCameraPos(ref eyePosition, ref cameraUpPosition, centerPosition, new Vector2(0.0f, 1.0f));
				camera.CalcPos(new Vector2(0.0f, 1.0f));
			}

			if((gamePadData.ButtonsDown & GamePadButtons.Down) != 0){
//				CalcCameraPos(ref eyePosition, ref cameraUpPosition, centerPosition, new Vector2(0.0f, -1.0f));
				camera.CalcPos(new Vector2(0.0f, -1.0f));
			}
			
			inputVector = new Vector2(gamePadData.AnalogLeftX, -gamePadData.AnalogLeftY);
			if(inputVector.Length() > epsilon){
//				CalcCameraPos(ref eyePosition, ref cameraUpPosition, centerPosition, inputVector);
				camera.CalcPos(inputVector);
			}
			
/*			view = Matrix4.LookAt(
				eyePosition,
				centerPosition,
				cameraUpPosition);
						
			Matrix4 worldViewProj = proj * view * world;
			*/
			
			camera.Update();
			
			/*本当はcamera.WorldViewProfをそのままrefしたい
			 * シェーダーの中で書き換わった値を後で使う場合に困る*/
			Matrix4 worldViewProj = camera.WorldViewProj;
			program.SetUniformValue(0, ref worldViewProj);
			
			graphics.SetViewport(0, 0, graphics.Screen.Width, graphics.Screen.Height);
			
			
		}

		public static void Render ()
		{
			
			// Clear the screen
			graphics.SetClearColor (0.0f, 0.0f, 0.0f, 0.0f);
			graphics.Clear ();
			
			cube.Render (program);
			cube2.Render (program);
			/*
			graphics.SetVertexBuffer(0, vertexBuffer);
			graphics.SetTexture(0,texture);
			graphics.SetShaderProgram(program);
			
			
			graphics.DrawArrays(DrawMode.TriangleStrip, 0, vertexBuffer.VertexCount);*/

			// Present the screen
			graphics.SwapBuffers ();
		}
		
		/* カメラの注視点と注視点との距離を変えずにカメラの位置を移動するメソッド 
		 * 画面上の入力と直観的に画面が動く
		 * カメラは注視点を中心とした球面上を動く*/
		private static void CalcCameraPos(ref Vector3 eyePosition, ref Vector3 cameraUpVector,
		                                  Vector3 centerPositon, Vector2 input2D){
			
			/*
			 * カメラと注視点の距離
			 * 計算誤差によって距離が変わらないように正規化してdistanceをかける*/
			float distance = (eyePosition - centerPositon).Length();
			
			//画面の入力ベクトルと画面の上ベクトルとの角度を出す
			Vector2 upScreen = new Vector2(0.0f, 1.0f);
			float inputAngle = upScreen.Angle(input2D);
			Console.WriteLine("angle : " + inputAngle);
			
			//その角度分upベクトルを目線ベクトルを軸に回転させる（これが画面の中の世界での入力ベクトルになる）
			Quaternion upQ = new Quaternion (cameraUpVector, 0.0f);
			Quaternion rotationQ = Quaternion.RotationAxis(eyePosition.Negate(), inputAngle);
			Quaternion inputQ = rotationQ.Conjugate().Multiply(upQ).Multiply(rotationQ);
			Console.WriteLine("inputVector in 3D : " + inputQ);
			
			float rotateAngle = input2D.Length() / 10.0f;
			//入力ベクトルと目線ベクトルの法線を軸にカメラ位置とupベクトルを回転させる
			Vector3 input3D = new Vector3(inputQ.X, inputQ.Y, inputQ.Z);
			Vector3 rotationAxis = input3D.Cross(eyePosition.Negate());
			Console.WriteLine("camera rotation axis : " + rotationAxis);
			rotationQ = Quaternion.RotationAxis(rotationAxis, rotateAngle);
			
			//カメラの注視点が原点でない場合は回転させてから戻す
			eyePosition = eyePosition.Subtract(centerPositon);
//			cameraUpVector = cameraUpPosition.Subtract(centerPositon);
			Quaternion eyePositionQ = new Quaternion(eyePosition, 0.0f);
			upQ = new Quaternion(cameraUpVector, 0.0f);
			eyePositionQ = rotationQ.Conjugate().Multiply(eyePositionQ).Multiply(rotationQ);
			upQ = rotationQ.Conjugate().Multiply(upQ).Multiply(rotationQ);
			
			eyePosition = new Vector3(eyePositionQ.X, eyePositionQ.Y, eyePositionQ.Z);
			cameraUpVector = new Vector3(upQ.X, upQ.Y, upQ.Z);
			
			eyePosition = eyePosition.Add (centerPositon);
			
			//距離調整
			eyePosition = eyePosition.Normalize().Multiply(distance);
//			cameraUpVector = cameraUpPosition.Add(centerPositon);
		}
	}
}
