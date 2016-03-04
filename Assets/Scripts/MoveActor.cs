using UnityEngine;
using System.Collections;

public class MoveActor : MonoBehaviour {

    public Rigidbody2D actor;

	// Use this for initialization
	void Start () {
        MoveSpaceCraft();
	}
	
    private void MoveSpaceCraft()
    {

        actor.AddForce(Vector2.right * 100);
    }
}
