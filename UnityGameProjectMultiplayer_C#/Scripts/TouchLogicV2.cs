using UnityEngine;
using System.Collections;

public class TouchLogicV2 : MonoBehaviour
{

	#if UNITY_ANDROID
	private static bool touchHolding;
	#endif

	public static int currTouch = 0;//so other scripts can know what touch is currently on screen
	[HideInInspector]
	public int touch2Watch = 64;
	public Vector2 currPos;
	public Touch virtualTouch;
	public TouchPhase virtualPhase;

	public virtual void Update()//If your child class uses Update, you must call base.Update(); to get this functionality
	{

		#if UNITY_EDITOR
		currPos = Input.mousePosition;
		if(Input.GetMouseButtonDown(0)){
			if(this.guiTexture != null && (this.guiTexture.HitTest(currPos)))
			{
				virtualPhase = TouchPhase.Began;
				OnTouchBegan();
			}
		}
		if(Input.GetMouseButtonUp (0)){
			if(this.guiTexture != null && (this.guiTexture.HitTest(currPos)))
			{
				virtualPhase = TouchPhase.Ended;
				OnTouchEnded();
			}
		}
		if(Input.GetAxisRaw("Mouse X") == 0 && Input.GetAxisRaw("Mouse Y") == 0){
			virtualPhase=TouchPhase.Stationary;
			OnTouchStayed();	
		}
		
		if(Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0){
			virtualPhase = TouchPhase.Moved;
			OnTouchMoved();
		}
		#else
		//is there a touch on screen?
		if(Input.touches.Length <= 0)
		{
			OnNoTouches();
		}
		else //if there is a touch
		{
			//loop through all the the touches on screen
			foreach(Touch touch in Input.touches)
			{	
				virtualTouch = touch;
				virtualPhase = touch.phase;
				currTouch = touch.fingerId;
				currPos = touch.position;
				//executes this code for current touch (i) on screen
				if(this.guiTexture != null && (this.guiTexture.HitTest(touch.position)))
				{
					//if current touch hits our guitexture, run this code
					if(touch.phase == TouchPhase.Began)
					{
						OnTouchBegan();
						touch2Watch = currTouch;
					}
					if(touch.phase == TouchPhase.Ended)
					{
						OnTouchEnded();
					}
					if(touch.phase == TouchPhase.Moved)
					{
						OnTouchMoved();
					}
					if(touch.phase == TouchPhase.Stationary)
					{
						OnTouchStayed();
					}
				}
				//outside so it doesn't require the touch to be over the guitexture
				switch(touch.phase)
				{
				case TouchPhase.Began:
					OnTouchBeganAnywhere();
					break;
				case TouchPhase.Ended:
					OnTouchEndedAnywhere();
					break;
				case TouchPhase.Moved:
					OnTouchMovedAnywhere();
					break;
				case TouchPhase.Stationary:
					OnTouchStayedAnywhere();
					break;
				}
			}
		}
		#endif
	}
	//the default functions, define what will happen if the child doesn't override these functions
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