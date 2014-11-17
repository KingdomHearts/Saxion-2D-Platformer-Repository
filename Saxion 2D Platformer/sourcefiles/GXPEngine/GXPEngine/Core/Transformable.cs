using System;
using GXPEngine.Core;

namespace GXPEngine
{
	/// <summary>
	/// The Transformable class contains all positional data of GameObjects.
	/// </summary>
	public class Transformable
	{
		protected float[] _matrix = new float[16] { 
			1.0f, 0.0f, 0.0f, 0.0f,
			0.0f, 1.0f, 0.0f, 0.0f,
			0.0f, 0.0f, 1.0f, 0.0f,
			0.0f, 0.0f, 0.0f, 1.0f };
			
		protected float _rotation = 0.0f;
		protected float _scaleX = 1.0f;
		protected float _scaleY = 1.0f;
		
		//------------------------------------------------------------------------------------------------------------------------
		//														Transform()
		//------------------------------------------------------------------------------------------------------------------------
		public Transformable () {
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														GetMatrix()
		//------------------------------------------------------------------------------------------------------------------------		
		/// <summary>
		/// Returns the gameobject's 4x4 matrix.
		/// </summary>
		/// <value>
		/// The matrix.
		/// </value>
		public float[] matrix {
			get {
				float[] matrix = (float[])_matrix.Clone();
				matrix[0] *= _scaleX;
				matrix[1] *= _scaleX;
				matrix[4] *= _scaleY;
				matrix[5] *= _scaleY;
				return matrix;
			}
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														x
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the x position.
		/// </summary>
		/// <value>
		/// The x.
		/// </value>
		public float x {
			get { return _matrix[12]; }
			set { _matrix[12] = (float)value; }
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														y
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the y position.
		/// </summary>
		/// <value>
		/// The y.
		/// </value>
		public float y {
			get { return _matrix[13]; }
			set { _matrix[13] = (float)value; }
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														SetXY
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the X and Y position.
		/// </summary>
		/// <param name='x'>
		/// The x coordinate.
		/// </param>
		/// <param name='y'>
		/// The y coordinate.
		/// </param>
		public void SetXY(double x, double y) {
			_matrix[12] = (float)x;
			_matrix[13] = (float)y;
		}
				
		//------------------------------------------------------------------------------------------------------------------------
		//														TransformPoint()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Transforms the point from this object's local space to the game's global space.
		/// </summary>
		/// <returns>
		/// The point.
		/// </returns>
		/// <param name='x'>
		/// The x coordinate.
		/// </param>
		/// <param name='y'>
		/// The y coordinate.
		/// </param>
		public virtual Vector2 TransformPoint(double x, double y) {
			Vector2 ret = new Vector2();
			ret.x = (float)(_matrix[0] * x * _scaleX + _matrix[4] * y * _scaleY + _matrix[12]);
			ret.y = (float)(_matrix[1] * x * _scaleX + _matrix[5] * y * _scaleY + _matrix[13]);
			return ret;
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														Rotation
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the object's rotation in degrees.
		/// </summary>
		/// <value>
		/// The rotation.
		/// </value>
		public float rotation {
			get { return _rotation; }
			set {
				_rotation = (float)value;
				double r = _rotation * Math.PI / 180.0;
				float cs = (float)Math.Cos (r);
				float sn = (float)Math.Sin (r);
				_matrix[0] = cs;
				_matrix[1] = sn;
				_matrix[4] = -sn;
				_matrix[5] = cs;
			}
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														Turn()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Turn the specified object with a certain angle in degrees.
		/// </summary>
		/// <param name='angle'>
		/// Angle.
		/// </param>
		public void Turn (double angle) {
			rotation = _rotation + (float)angle;
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														Move()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Move the object, based on it's current rotation.
		/// </summary>
		/// <param name='stepX'>
		/// Step x.
		/// </param>
		/// <param name='stepY'>
		/// Step y.
		/// </param>
		public void Move (double stepX, double stepY) {
			double r = _rotation * Math.PI / 180.0;
			float cs = (float)Math.Cos (r);
			float sn = (float)Math.Sin (r);
			_matrix[12] = (float)(_matrix[12] + cs * stepX - sn * stepY);
			_matrix[13] = (float)(_matrix[13] + sn * stepX + cs * stepY);
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														Translate()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Move the object, in world space. (Object rotation is ignored)
		/// </summary>
		/// <param name='stepX'>
		/// Step x.
		/// </param>
		/// <param name='stepY'>
		/// Step y.
		/// </param>
		public void Translate (double stepX, double stepY) {
			_matrix[12] += (float)stepX;
			_matrix[13] += (float)stepY;
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														SetScale()
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the object's scaling
		/// </summary>
		/// <param name='scaleX'>
		/// Scale x.
		/// </param>
		/// <param name='scaleY'>
		/// Scale y.
		/// </param>
		public void SetScaleXY(double scaleX, double scaleY) {
			_scaleX = (float)scaleX;
			_scaleY = (float)scaleY;
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														scaleX
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the object's x-axis scale
		/// </summary>
		/// <value>
		/// The scale x.
		/// </value>
		public double scaleX {
			get { return _scaleX; }
			set { _scaleX = (float)value; }
		}
		
		//------------------------------------------------------------------------------------------------------------------------
		//														scaleY
		//------------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the object's y-axis scale
		/// </summary>
		/// <value>
		/// The scale y.
		/// </value>
		public double scaleY {
			get { return _scaleY; }
			set { _scaleY = (float)value; }
		}
		
	}
}


