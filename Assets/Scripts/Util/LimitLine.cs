using System.Collections;

/// <summary>
/// スクロールの限界値
/// </summary>
public class LimitLine {

	float top;
	float bottom;
	float left;
	float right;

	public LimitLine(float top, float bottom, float left, float right) {
		this.top	= top;
		this.bottom = bottom;
		this.left   = left;
		this.right  = right;
	}
		
	public float Top {
		get{ return top; }
		set{ top = value; }
	}

	public float Bottom {
		get{ return bottom; }
		set{ bottom = value; }
	}

	public float Left {
		get{ return left; }
		set{ left = value; }
	}

	public float Right {
		get{ return right; }
		set{ right = value; }
	}
}
