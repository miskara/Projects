using UnityEngine;
using System.Collections;
public class InputController : MonoBehaviour
{
	
	#if UNITY_ANDROID
	private static bool touchHolding;
	#endif



	public void Update(){
	
		if (Input.GetMouseButtonDown (0))
			OnTouchBegan ();
	
	}
	public static bool HasTouchBegan ()
	{
		bool touchBegan = false;
		
		#if UNITY_EDITOR
		touchBegan = Input.GetMouseButtonDown (0);
		//if(touchBegan) OnTouchBegan();
		#else
		if (Input.touchCount > 0)
		{
			foreach(Touch touch in Input.touches){
			touchBegan = (touch.phase == TouchPhase.Began);
			
			if (touchBegan)
				touchHolding = true;
				//OnTouchBegan();
			}
			return touchBegan;
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
		if (Input.touchCount > 0)
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
		if (Input.touchCount > 0)
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
		if (Input.touchCount > 0)
		{
			touchPosition = Input.GetTouch(0).position;
		}
		#endif
		
		return touchPosition;
	}



	public virtual void OnNoTouches(){}
	public virtual void OnTouchBegan(){print (name + " is not using OnTouchBegan");}
	public virtual void OnTouchEnded(){}
	public virtual void OnTouchMoved(){}
	public virtual void OnTouchStayed(){}
	public virtual void OnTouchBeganAnywhere(){}
	public virtual void OnTouchEndedAnywhere(){}
	public virtual void OnTouchMovedAnywhere(){}
	public virtual void OnTouchStayedAnywhere(){}
}
