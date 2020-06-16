using System;

public class Counter
{
	public Counter(float total=float.PositiveInfinity)
	{
		Redefine(total);
	}

	public void Reset()
	{
		Current = 0.0f;
	}

	public void Redefine(float total)
	{
		if (total < 0.0f)
		{
			Total = float.PositiveInfinity;
		}
		else
		{
			Total = total;
		}
		Reset();
	}

	public void Zero()
	{
		Total = 0.0f;
		Reset();
	}

	public void Infinity()
	{
		Total = float.PositiveInfinity;
		Reset();
	}

	public void Exceed()
	{
		Current = Total;
	}

	public bool IsZero()
	{
		return Total == 0.0f;
	}

	public bool IsInfinity()
	{
		return float.IsPositiveInfinity(Total);
	}

	public bool IsFinity()
	{
		return Total < float.PositiveInfinity;
	}

	public bool IsPositiveFinity()
	{
		return 0.0f < Total && Total < float.PositiveInfinity;
	}

	public bool IsExceed()
	{
		return Current >= Total;
	}

	public bool IsNotExceed()
	{
		return Current < Total;
	}

	public bool Increase(float delta)
	{
		if (Current + delta < Total)
		{
			Current += delta;
		}
		else
		{
			Current = Total;
		}
		return IsExceed();
	}

	public float Current { get; private set; }

	public float CurrentNormalized
	{
		get
		{
			if (IsPositiveFinity())
			{
				return Math.Min(Total, Current) / Total;
			}
			return 1.0f;
		}
	}

	public float Remain
	{
		get
		{
			if (IsPositiveFinity())
			{
				return Math.Max(0.0f, Total - Current);
			}
			return 0.0f;
		}
	}

	public float RemainNormalized
	{
		get
		{
			if (IsPositiveFinity())
			{
				return Remain / Total;
			}
			return 0.0f;
		}
	}

	public float Total { get; private set; }
}