using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ShipActions : MonoBehaviour {
  
    public Rigidbody2D ship;
    public Rigidbody2D bullet;

    private float _forwardThrust = 1.0f;
    private float _rotationalThrust = 0.05f;
    private float _bulletSpeed = 10.0f;
	
	// Update is called once per frame
	void Update ()
    {
        MoveShipForward();
        RotateShip();
        ShootBullets();
	}

    /// <summary>
    /// Moves the ship forward
    /// </summary>
    private void MoveShipForward()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            ship.AddRelativeForce(Vector2.up * _forwardThrust);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void RotateShip()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            ship.AddTorque(-_rotationalThrust);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            ship.AddTorque(_rotationalThrust);
        }
    }

    /// <summary>
    /// Instantiates a bullet clone that will travel in the direction the space ship is pointing
    /// </summary>
    private void ShootBullets()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rigidbody2D bulletClone;
            //Instantiate the bullet projectile at the tip of the player spaceship.
            bulletClone = (Rigidbody2D) Instantiate(bullet, ship.transform.TransformPoint(Vector2.up *2), ship.transform.rotation);
           
            bulletClone.velocity = transform.TransformDirection(Vector2.up * _bulletSpeed);

            Destroy(bulletClone.gameObject, 1); //Destroy the bullet after one second
        }
    }
}
