using System;

public static class MathUtility
{
	static MathUtility ()
	{
		_random = new Random ();
	}

	public static int Random(int open, int close)
	{
		return _random.Next(open, close + 1);
	}

    public static int Random(int[] array)
    {
        if (array.IsNullOrEmptyEx())
        {
            Console.Error.WriteLine("[MathUtility.Random(int[] array)] array is null or empty!");
        }

        var index = _random.Next(array.Length);
        return array[index];
    }

    public static bool Approximately(float lhs, float rhs)
    {
        return Math.Abs(lhs - rhs) < 0.00001f && Math.Abs(lhs - rhs) < 0.00001f;
    }

	private static Random _random;
}

