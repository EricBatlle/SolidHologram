using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableObject : MonoBehaviour {

	public virtual void StartBehaviour()
    {
        print("startBehaviour");
    }
    public virtual void EndBehaviour()
    {
        print("endBehaviour");
    }
}
