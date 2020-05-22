
using UnityEngine;

public class ScaleRotate : MonoBehaviour {

    public Vector3 initialScale;
    public float initialFingersDistance;
	public static Transform ScaleTransform;
    public static Transform RotationTransform;
	
	public void Update ()
    {
		int fingersOnScreen = 0;
		
		foreach(Touch touch in Input.touches)
        {
			fingersOnScreen++; 
            
			if(fingersOnScreen == 2)
            {
				if(touch.phase == TouchPhase.Began)
                {
					initialFingersDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
					initialScale = ScaleTransform.localScale;
				}
				else
                {
					float currentFingersDistance = Vector2.Distance(Input.touches[0].position, Input.touches[1].position);
					float scaleFactor = currentFingersDistance / initialFingersDistance;
					ScaleTransform.localScale = initialScale * scaleFactor; 
				}
			}
            else if (fingersOnScreen == 1)
            {
                if(Input.touchCount == 1 && touch.phase == TouchPhase.Moved)
                {
                    RotationTransform.transform.Rotate
                        (Vector3.up * 8f * Time.deltaTime * touch.deltaPosition.x, Space.Self);
                    RotationTransform.transform.Rotate
                        (Vector3.right * -8f * Time.deltaTime * touch.deltaPosition.y, Space.Self);
                }
            }
		}
	}
}