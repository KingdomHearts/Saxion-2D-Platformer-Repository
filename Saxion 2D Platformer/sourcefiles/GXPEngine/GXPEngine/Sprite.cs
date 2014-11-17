using System;
using GXPEngine.Core;

namespace GXPEngine
{
	/// <summary>
	/// The Sprite class holds 2D images that can be used as objects in your game.
	/// </summary>
	public class Sprite : GameObject
	{
		protected Texture2D _texture;
		protected Rectangle _bounds;
		protected float[] _uvs;
		
		private uint _color = 0xFFFFFF;
		private float _alpha = 1.0f;
		
		protected bool _mirrorX = false;
		protected bool _mirrorY = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="GXPEngine.Sprite"/> class.
		/// Specify a System.Drawing.Bitmap to use. Bitmaps will not be cached.
		/// </summary>
		/// <param name='bitmap'>
		/// Bitmap.
		/// </param>
		public Sprite (System.Drawing.Bitmap bitmap)
		{
			name = "BMP" + bitmap.Width + "x" + bitmap.Height;
			initializeFromTexture(new Texture2D(bitmap));
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														Sprite()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="GXPEngine.Sprite"/> class.
		/// Specify an image file to load. Please use a full filename. Initial path is the application folder.
		/// Images will be cached internally. That means once it is loaded, the same data will be used when
		/// you load the file again.
		/// </summary>
		/// <param name='filename'>
		/// The name of the file that should be loaded.
		/// </param>
		public Sprite (string filename)
		{
			name = filename;
			initializeFromTexture(Texture2D.GetInstance(filename));
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														initializeFromTexture()
		//------------------------------------------------------------------------------------------------------------------------
		private void initializeFromTexture (Texture2D texture) {
			_texture = texture;
			_bounds = new Rectangle(0, 0, _texture.width, _texture.height);
			setUVs();
			_collider = new BoxCollider(this);
			
			if (Game.main != null) Game.main.Add(this);
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														setUVs
		//------------------------------------------------------------------------------------------------------------------------
		protected virtual void setUVs() {
			float left = _mirrorX?1.0f:0.0f;
			float right = _mirrorX?0.0f:1.0f;
			float top = _mirrorY?1.0f:0.0f;
			float bottom = _mirrorY?0.0f:1.0f;
			_uvs = new float[8] { left, top, right, top, right, bottom, left, bottom };
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														texture
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns the texture that is used to create this sprite.
		/// If no texture is used, null will be returned.
		/// Use this to retreive the original width/height or filename of the texture.
		/// </summary>
		public Texture2D texture {
			get { return _texture; }
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														width
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the sprite's width in pixels.
		/// </summary>
		virtual public int width {
			get { 
				if (_texture != null) return (int)Math.Abs(_texture.width * _scaleX);
				return 0;
			}
			set {
				if (_texture != null) 
					if (_texture.width != 0) scaleX = value / (float)_texture.width;
			}
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														height
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the sprite's height in pixels.
		/// </summary>
		virtual public int height {
			get { 
				if (_texture != null) return (int)Math.Abs(_texture.height * _scaleY);
				return 0;
			}
			set {
				if (_texture != null) 
					if (_texture.height != 0) scaleY = value / (float)_texture.height;
			}
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														RenderSelf()
		//------------------------------------------------------------------------------------------------------------------------
		override protected void RenderSelf(GLContext glContext) {
			if (game != null) {
				Vector2[] bounds = GetExtents();
				float maxX = float.MinValue;
				float maxY = float.MinValue;
				float minX = float.MaxValue;
				float minY = float.MaxValue;
				for (int i=0; i<4; i++) {
					if (bounds[i].x > maxX) maxX = bounds[i].x;
					if (bounds[i].x < minX) minX = bounds[i].x;
					if (bounds[i].y > maxY) maxY = bounds[i].y;
					if (bounds[i].y < minY) minY = bounds[i].y;
				}
				bool test = (maxX < 0) || (maxY < 0) || (minX >= game.width) || (minY >= game.height);
				if (test == false) {
					_texture.Bind();
					glContext.SetColor((byte)((_color >> 16) & 0xFF), 
					                   (byte)((_color >> 8) & 0xFF), 
					                   (byte)(_color & 0xFF), 
					                   (byte)(_alpha * 0xFF));
					glContext.DrawQuad(GetArea(), _uvs);
					glContext.SetColor(1, 1, 1, 1);
					_texture.Unbind();
				}
			}
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														GetArea()
		//------------------------------------------------------------------------------------------------------------------------
		internal float[] GetArea() {
			return new float[8] {
				_bounds.left, _bounds.top,
				_bounds.right, _bounds.top,
				_bounds.right, _bounds.bottom,
				_bounds.left, _bounds.bottom
			};
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														GetExtents()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the four corners of this object as a set of 4 Vector2s.
		/// </summary>
		/// <returns>
		/// The extents.
		/// </returns>
		public Vector2[] GetExtents() {
			Vector2[] ret = new Vector2[4];
			ret[0] = TransformPoint(_bounds.left, _bounds.top);
			ret[1] = TransformPoint(_bounds.right-1, _bounds.top);
			ret[2] = TransformPoint(_bounds.right-1, _bounds.bottom-1);
			ret[3] = TransformPoint(_bounds.left, _bounds.bottom-1);
			return ret;			
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														SetOrigin()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the origin, the pivot point of this Sprite in pixels.
		/// </summary>
		/// <param name='x'>
		/// The x coordinate.
		/// </param>
		/// <param name='y'>
		/// The y coordinate.
		/// </param>
		public void SetOrigin(float x, float y) {
			_bounds.x = -x;
			_bounds.y = -y;
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														Mirror
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// This function can be used to enable mirroring for the sprite in either x or y direction.
		/// </summary>
		/// <param name='mirrorX'>
		/// If set to <c>true</c> to enable mirroring in x direction.
		/// </param>
		/// <param name='mirrorY'>
		/// If set to <c>true</c> to enable mirroring in y direction.
		/// </param>
		public void Mirror(bool mirrorX, bool mirrorY) {
			_mirrorX = mirrorX;
			_mirrorY = mirrorY;
			setUVs();
		}
				
		//------------------------------------------------------------------------------------------------------------------------
		//														HitTest()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Tests if this object overlaps the one specified. An oriented bounding box will be used for testing.
		/// </summary>
		/// <returns>
		/// <c>true</c>, if test was hit, <c>false</c> otherwise.
		/// </returns>
		/// <param name='other'>
		/// Other.
		/// </param>
		override public bool HitTest(GameObject other) {
			if (other is Sprite) {
				return _collider.HitTest(((Sprite)other)._collider);
			} else {
				return false;
			}
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														HitTestPoint()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Returns 'true' if a 2D point overlaps this object. An oriented bounding box will be used for testing, else
		/// it will return 'false'.
		/// You could use this for instance to check if the mouse (Input.mouseX, Input.mouseY) is over the object.
		/// </summary>
		/// <param name='x'>
		/// The x coordinate to test.
		/// </param>
		/// <param name='y'>
		/// The y coordinate to test.
		/// </param>
		override public bool HitTestPoint(float x, float y) {
			return _collider.HitTestPoint(x, y);
		}		
		
		//------------------------------------------------------------------------------------------------------------------------
		//														color
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the color filter for this sprite.
		/// This can be any value between 0x000000 and 0xFFFFFF.
		/// </summary>
		public uint color {
			get { return _color; }
			set { _color = value & 0xFFFFFF; }
		}

		//------------------------------------------------------------------------------------------------------------------------
		//														color
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the color of the sprite.
		/// </summary>
		/// <param name='r'>
		/// The red component, range 0..1
		/// </param>
		/// <param name='g'>
		/// The green component, range 0..1
		/// </param>
		/// <param name='b'>
		/// The blue component, range 0..1
		/// </param>
		public void SetColor(float r, float g, float b) {
			r = Utils.Clamp(r, 0, 1);
			g = Utils.Clamp(g, 0, 1);
			b = Utils.Clamp(b, 0, 1);
			byte rr = (byte)Math.Floor((r * 255));
			byte rg = (byte)Math.Floor((g * 255));
			byte rb = (byte)Math.Floor((b * 255));
			color = (uint)rb + (uint)(rg << 8) + (uint)(rr << 16);
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														alpha
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the alpha value of the sprite. 
		/// Setting this value allows you to make the sprite (semi-)transparent.
		/// The alpha value should be in the range 0...1, where 0 is fully transparent and 1 is fully opaque.
		/// </summary>
		public float alpha {
			get { return _alpha; }
			set { _alpha = value; }
		}
		
	}
}

