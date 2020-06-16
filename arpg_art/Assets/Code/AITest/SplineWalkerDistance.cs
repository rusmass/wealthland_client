using UnityEngine;
using System.Collections;

public class SplineWalkerDistance : MonoBehaviour
{
    public CurvySpline Spline;
    public CurvyClamping Clamping = CurvyClamping.Clamp;
    public bool SetOrientation = true;
    public bool FastInterpolation;
    public float InitialDistance;
    public float Speed;

    public float Distance 
    {
        get { return mDistance; }
        set { mDistance = value; }
    }

    float mDistance;
    int mDir;
    Transform mTransform;

    IEnumerator Start()
    {
        mDistance = InitialDistance;
        mDir = (Speed >= 0) ? 1 : -1;
        Speed = Mathf.Abs(Speed);
        mTransform = transform;

        if (Spline) 
		{
			while (!Spline.IsInitialized) 
			{
				yield return null;
			}
            InitPosAndRot();
        }
    }

    void Update()
    {
		if (!Spline || !Spline.IsInitialized) 
		{
			return;
		}
		if (Application.isPlaying && mDistance < 2) 
		{
            float tf = Spline.DistanceToTF(mDistance);

			mTransform.position = (FastInterpolation) ?
            Spline.MoveByFast(ref tf, ref mDir, Speed * Time.deltaTime, Clamping) :
            Spline.MoveBy(ref tf, ref mDir, Speed * Time.deltaTime, Clamping);
            mDistance = Spline.TFToDistance(tf);
            
            if (SetOrientation) 
			{
            	transform.rotation = Spline.GetOrientationFast(tf);
            }
        }
    }

    void InitPosAndRot()
	{
		if (!Spline) 
		{
			return;
		}
		float tf = Spline.DistanceToTF (InitialDistance);
		mTransform.position = Spline.Interpolate (tf);
		if (SetOrientation) 
		{
			mTransform.rotation = Spline.GetOrientationFast (tf);
		}
	}
}
