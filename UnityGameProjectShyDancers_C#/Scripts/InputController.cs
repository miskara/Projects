using UnityEngine;
using System.Collections;
public class InputController : MonoBehaviour
{
	[HideInInspector]
	public bool inputEnabled = true;

#if UNITY_ANDROID
    private static bool touchHolding;
#endif

	public virtual void Update(){
		if (Input.GetMouseButtonDown(0) || Input.touchCount == 1){
			OnTouchBegan();
		}
	}

    public static bool HasTouchBegan ()
    {
	bool touchBegan = false;
		
	#if UNITY_EDITOR

	touchBegan = Input.GetMouseButtonDown (0);
	#else
	if (Input.touchCount == 1)
	{
	    touchBegan = (Input.GetTouch(0).phase == TouchPhase.Began);
		
	    if (touchBegan)
	        touchHolding = true;
	}
	#endif
		
	return touchBegan;
    }
    
    public static bool HasTouchHold ()
    {
        bool touchHold = false;

        #if UNITY_EDITOR
        touchHold = Input.GetMouseButton (0);
        #else
        if (Input.touchCount == 1)
        {
            touchHold = touchHolding;
        }
        #endif

        return touchHold;
    }
    
    public static bool HasTouchEnd ()
    {
        bool touchEnd = false;

        #if UNITY_EDITOR
        touchEnd = Input.GetMouseButtonUp (0);
        #else
        if (Input.touchCount == 1)
        {
            touchEnd = (Input.GetTouch(0).phase == TouchPhase.Ended);

            if (touchEnd)
                touchHolding = false;
        }
        #endif

        return touchEnd;
    }

    public static Vector3 GetTouchPosition ()
    {
        Vector3 touchPosition = new Vector3 ();

        #if UNITY_EDITOR
        touchPosition = Input.mousePosition;
        #else
        if (Input.touchCount == 1)
        {
            touchPosition = Input.GetTouch(0).position;
        }
        #endif

        return touchPosition;
    }

	public virtual void OnTouchBegan(){
	}
}
