using UnityEngine.SceneManagement;
using UnityEngine;
using System;


public class Knife : MonoBehaviour
{
    [SerializeField] private float speed =50f;
   
    public Rigidbody2D myRigidbody2D;
    
    public bool IsReleased { get; set; }
    public bool Hit { get; set; }

    

    public void FireKnife()
    {
        if (!IsReleased && !GameManager.Instance.IsGameOver)
        {
            Debug.Log("FireKnife");
            IsReleased = true;
            myRigidbody2D.AddForce(new Vector2( 0f, speed), ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wheel") && !Hit && !GameManager.Instance.IsGameOver && IsReleased)
        {
            other.gameObject.GetComponent<Wheel>().KnifeHit(this);
            GameManager.Instance.Score++;
        }
          
        else if (other.gameObject.CompareTag("Knife") && !Hit && !GameManager.Instance.IsGameOver && IsReleased && other.gameObject.GetComponent<Knife>().IsReleased)
        {
            transform.SetParent(other.transform);
            myRigidbody2D.velocity = Vector2.zero;
            myRigidbody2D.isKinematic = true;

            GameManager.Instance.IsGameOver = true;
            Invoke(nameof(GameOver), 0.5f);
        }
        
        
    }

    private void GameOver()
    {
        UIManager.Instance.GameOver();
    }

    
}

