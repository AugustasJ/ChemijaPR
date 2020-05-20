
using UnityEngine;

public class ScaleRotate : MonoBehaviour {
	
	public float initialFingersDistance;
	public Vector3 initialScale;
	public static Transform ScaleTransform;
    public static Transform RotationTransform;
	
	void  Update ()
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
                Touch touche;
                touche = Input.GetTouch(0);

                if(Input.touchCount == 1 && touche.phase == TouchPhase.Moved)
                {
                    RotationTransform.transform.Rotate(Vector3.up * 8f * Time.deltaTime * touch.deltaPosition.x, Space.Self);
                    RotationTransform.transform.Rotate(Vector3.right * -8f * Time.deltaTime * touch.deltaPosition.y, Space.Self);
                }
            }
		}
	}
}