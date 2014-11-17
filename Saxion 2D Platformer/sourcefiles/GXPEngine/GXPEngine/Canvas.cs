using System;
using System.Drawing;
using GXPEngine.Core;

namespace GXPEngine
{
	/// <summary>
	/// The Canvas object can be used for drawing 2D visuals at runtime.
	/// </summary>
	public class Canvas : GameObject
	{
		private Texture2D _texture;
		private Graphics _graphics;
		private float[] _bounds;
		private float[] _uvs;
		
		private uint _color = 0xFFFFFF;
		private byte _alpha = 0xFF;
		
		//------------------------------------------------------------------------------------------------------------------------
		//														Canvas()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="GXPEngine.Canvas"/> class.
		/// It is a regular GameObject that can be added to any displaylist via commands such as AddChild.
		/// It contains a <a href="http://msdn.microsoft.com/en-us/library/system.drawing.graphics(v=vs.110).aspx">System.Drawing.Graphics</a> component.
		/// </summary>
		/// <param name='width'>
		/// Width of the canvas in pixels.
		/// </param>
		/// <param name='height'>
		/// Height of the canvas in pixels.
		/// </param>
		public Canvas (int width, int height) : base()
		{
			name = width + "x" + height;
			
			_texture = new Texture2D(width, height);
			_graphics = Graphics.FromImage(_texture.bitmap);
			_bounds = new float[8] { 0.0f, 0.0f, width, 0.0f, width, height, 0.0f, height };
			_uvs = new float[8] { 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f };
			_invalidate = true;
			
			if (Game.main != null) Game.main.Add(this);
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														width
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the canvas width.
		/// </summary>
		public int width {
			get { return _texture.width; }
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														height
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the canvas height.
		/// </summary>
		public int height {
			get { return _texture.height; }
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														Destroy()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Destroys this canvas.
		/// </summary>
		override public void Destroy() {
			_graphics.Dispose();
			base.Destroy();
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														graphics
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the graphics component. This interface provides tools to draw on the canvas.
		/// See: <a href="http://msdn.microsoft.com/en-us/library/system.drawing.graphics(v=vs.110).aspx">System.Drawing.Graphics</a>
		/// </summary>
		public Graphics graphics {
			get { 
				_invalidate = true;
				return _graphics; 
			}
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														Render()
		//------------------------------------------------------------------------------------------------------------------------
		override protected void RenderSelf(GLContext glContext) {
			if (_invalidate) {
				_texture.UpdateGLTexture ();
				_invalidate = false;
			}
			_texture.Bind();
			glContext.SetColor((byte)((_color >> 16) & 0xFF), 
			                   (byte)((_color >> 8) & 0xFF), 
			                   (byte)(_color & 0xFF), 
			                   _alpha);
			glContext.DrawQuad(_bounds, _uvs);
			_texture.Unbind();
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														color
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the color filter for this canvas. 
		/// This can be any value between 0x000000 and 0xFFFFFF.
		/// </summary>
		public uint color {
			get { return _color; }
			set { _color = value & 0xFFFFFF; }
		}
		
		
		//------------------------------------------------------------------------------------------------------------------------
		//														alpha
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the alpha value of the canvas. 
		/// Setting this value allows you to make the canvas (semi-)transparent.
		/// The alpha value should be in the range 0...1, where 0 is fully transparent and 1 is fully opaque.
		/// </summary>
		public float alpha {
			get { return _alpha / (float)0xFF; }
			set { _alpha = (byte)Math.Floor(value * 0xFF); }
		}		

		//------------------------------------------------------------------------------------------------------------------------
		//														alpha
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Draws a Sprite onto the canvas.
		/// It will ignore Sprite properties, such as color and animation.
		/// </summary>
		/// <param name='sprite'>
		/// The Sprite that should be drawn.
		/// </param>
		private PointF[] destPoints = new PointF[3];
		public void DrawSprite(Sprite sprite) {
			float halfWidth = sprite.texture.width / 2.0f;
			float halfHeight = sprite.texture.height / 2.0f;
			Vector2 p0 = sprite.TransformPoint(-halfWidth, -halfHeight);
			Vector2 p1 = sprite.TransformPoint(halfWidth, -halfHeight);
			Vector2 p2 = sprite.TransformPoint(-halfWidth, halfHeight);
			destPoints[0] = new PointF(p0.x, p0.y);
			destPoints[1] = new PointF(p1.x, p1.y);
			destPoints[2] = new PointF(p2.x, p2.y);
			graphics.DrawImage(sprite.texture.bitmap, destPoints);
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														ToString()
		//------------------------------------------------------------------------------------------------------------------------
		public override string ToString() {
			return "[" + name + "]";
		}		
		
	}
}

