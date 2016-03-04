using UnityEngine;
using System.Collections;

public class AsteroidMovement : MonoBehaviour {

    public Rigidbody2D asteroid;

    private float _speed = 50.0f;

	// Use this for initialization
	void Start () {
        InitiateMovement();
	}

    /// <summary>
    /// When the asteroid is spawn it will move in a random direction
    /// </summary>
    void InitiateMovement()
    {
        asteroid.AddForce(Random.insideUnitCircle * _speed);
    }

}
