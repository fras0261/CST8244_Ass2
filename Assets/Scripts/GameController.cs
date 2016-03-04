using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    //Prefabs of the asteroid types
    public Rigidbody2D asteroid1;
    public Rigidbody2D asteroid2;
    public Rigidbody2D asteroid3;

    //References to the images used as lives
    public Image life1;
    public Image life2;
    public Image life3;

    public Text scoreText;

    public AudioClip crashSound;
    public AudioClip explosionSound;

    private Rigidbody2D[] _asteroidBelt = new Rigidbody2D[3];

    private AudioSource _audio;

    private const int MAX_SCORE = 999999999;
    private int _score = 0;
    private int _lives = 3;
    private int _numOfAsteroidTypes = 3;
    private int _asteroidSelector; //A random number that will be used to select which asteroid will be spawned next.
    private int _numToSpawn = 5; //Number of asteroids to spawn initially

    private float _spawnTime = 10.0f; //The delay between spawns of asteroids
    private float _scanRadius = 1.0f;

    private bool _isSpawnPointFree = false;


	// Use this for initialization
	void Start () {
        _asteroidBelt[0] = asteroid1;
        _asteroidBelt[1] = asteroid2;
        _asteroidBelt[2] = asteroid3;

        _audio = GetComponent<AudioSource>();

        UpdateScore(_score);

        //S
        for (int i = 0; i < _numToSpawn; i++)
        {
            Spawn();
        }

        InvokeRepeating("Spawn", 5.0f, _spawnTime);
	}

    /// <summary>
    /// Selects a random location within the Camera's view to spawn a randomly selected asteroid
    /// </summary>
    private void Spawn()
    {
        Vector2 spawnPoint; //Coordinates for where the next asteroid will spawn
           
        spawnPoint = new Vector2(Random.value, Random.value);
        spawnPoint = Camera.main.ViewportToWorldPoint(spawnPoint);
        _isSpawnPointFree = false;

        //If the spawn point is found to not overlap with any other objects then the 
        if (Physics2D.OverlapCircle(spawnPoint, _scanRadius) == null)
        {
            _asteroidSelector = (int)(Random.value * 1000) % _numOfAsteroidTypes;
            Rigidbody2D asteroid = (Rigidbody2D)Instantiate(_asteroidBelt[_asteroidSelector], spawnPoint, Quaternion.identity);
            asteroid.transform.localScale = new Vector3(1.0f, 1.0f);
        }
        else
        {
            //The system will continue to generate a random spawn location until one is found that is free of obstructions
            while (_isSpawnPointFree == false)
            {
                spawnPoint = new Vector2(Random.value, Random.value);
                spawnPoint = Camera.main.ViewportToWorldPoint(spawnPoint);

                if (Physics2D.OverlapCircle(spawnPoint, _scanRadius) == null)
                {
                    _asteroidSelector = (int)(Random.value * 1000) % _numOfAsteroidTypes;
                    Rigidbody2D asteroid = (Rigidbody2D) Instantiate(_asteroidBelt[_asteroidSelector], spawnPoint, Quaternion.identity);
                    asteroid.transform.localScale = new Vector3(1.0f, 1.0f);
                    _isSpawnPointFree = true;
                } //End of if
            } //End of while
        } //End of else   
    }

    /// <summary>
    /// Spawns two smaller asteroid when the smaller one is destroyed
    /// </summary>
    public void SpawnAsteroidChild(GameObject asteroidParent)
    {
        Vector2 spawnPoint;
        Vector2 pointOfDestruction = asteroidParent.transform.position; //Get point where the asteroid was destroyed
        Rigidbody2D asteroidTemp;
        float xScale = asteroidParent.gameObject.transform.localScale.x; //Get the x scale of the parent asteroid
        float yScale = asteroidParent.gameObject.transform.localScale.y; //Get the y scale of the parent asteroid

        //As long as the asteroid scale is greater then 25% the asteroid will split into smaller pieces.
        if (asteroidParent.gameObject.transform.localScale.x > 0.25f)
        {

            //Spawn the first asteroid child to the lower right of the parent asteroid
            spawnPoint = new Vector2(pointOfDestruction.x + 0.5f, pointOfDestruction.y - 0.5f);
            _asteroidSelector = (int)(Random.value * 1000) % _numOfAsteroidTypes;
            asteroidTemp = _asteroidBelt[_asteroidSelector];
            asteroidTemp.gameObject.transform.localScale = new Vector3(xScale * 0.5f, yScale * 0.5f, 0);
            Instantiate(asteroidTemp, spawnPoint, Quaternion.identity);

            //Spawn the second asteroid child to the upper left of the parent asteroid
            spawnPoint = new Vector2(pointOfDestruction.x - 0.5f, pointOfDestruction.y + 0.5f);
            _asteroidSelector = (int)(Random.value * 1000) % _numOfAsteroidTypes;
            asteroidTemp = _asteroidBelt[_asteroidSelector];
            asteroidTemp.gameObject.transform.localScale = new Vector3(xScale * 0.5f, yScale * 0.5f, 0);
            Instantiate(asteroidTemp, spawnPoint, Quaternion.identity);
        }
    }

    /// <summary>
    /// Updates the user's score
    /// </summary>
    /// <param name="pointsToAdd">The number of points to add to the player's score</param>
    public void UpdateScore(int pointsToAdd)
    {
        _score += pointsToAdd; 
        if (_score > MAX_SCORE)
            _score = MAX_SCORE;
        scoreText.text = _score.ToString("D9");
    }

    /// <summary>
    /// Reduce the number of lives, and removes the life images depending on the number of lives left
    /// If the player runs out of lives then a sound plays and the game restarts
    /// </summary>
    public void DecreaseLives()
    {
        _lives--;

        switch(_lives)
        {
            case 2:
                life3.enabled = false; //Remove a life image
                break;
            case 1:
                life2.enabled = false; //Remove a life image
                break;
            case 0:
                life1.enabled = false; //Remove a life image
                break;
            default:
                _audio.Play(); //Play an explosion noise
                Destroy(GameObject.FindGameObjectWithTag("Player").gameObject);
                StartCoroutine("WaitBeforeLoad");
                break;
        }
    }
    
    /// <summary>
    /// Get the audio clip and play it
    /// </summary>
    /// <param name="audioClip">AudioClip to play</param>
    public void PlayAudioClip(AudioClip audioClip)
    {
        //_audio.clip = audioClip;
        _audio.PlayOneShot(audioClip);
    }

   

    /// <summary>
    /// Let the game wait for a few seconds before reloading so that the sound effect can finish.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitBeforeLoad()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("MainGame");
    }
}
