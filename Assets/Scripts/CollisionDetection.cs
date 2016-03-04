using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour
{
    public AudioClip crashSound;
       
    private GameController _gameController;
    private int _points = 100;
   
    // Use this for initialization
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("GameController") != null)
        {
            _gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        }
    }

    /// <summary>
    /// When the bullet collides with an asteroid, then the asteroid and the bullet will be destroyed. 
    /// Points will  be added to the players score
    /// </summary>
    /// <param name="obj"></param>
    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.tag == "Player")
        {
            _gameController.DecreaseLives();
            _gameController.PlayAudioClip(crashSound);
            Destroy(this.gameObject); //Destroy the asteroid
        }
        else if (obj.tag == "Projectile")
        {       
            _gameController.UpdateScore(_points);
            _gameController.SpawnAsteroidChild(this.gameObject);
            Destroy(obj.gameObject);
            Destroy(this.gameObject);
        }
        else
            return;
    }

    
}
    
