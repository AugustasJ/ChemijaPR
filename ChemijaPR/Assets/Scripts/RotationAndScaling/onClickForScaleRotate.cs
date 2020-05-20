using UnityEngine;
using System.Collections;

public class onClickForScaleRotate : MonoBehaviour
{
	void OnMouseDown()
    {
		ScaleRotate.ScaleTransform = this.transform;
        ScaleRotate.RotationTransform = this.transform;
	}
}