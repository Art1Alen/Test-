using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = System.Random;

public class Wheel : MonoBehaviour
{
    [SerializeField] private int availableKnifes;

    [SerializeField] private Sprite firstWheel;
    [SerializeField] private Sprite secondWheel;
    [SerializeField] private Sprite thirdWheel;

    [SerializeField] private bool isBoos;
  
    [Header(header: "Prefabs")]
    [SerializeField] private GameObject applePrefab;
    [SerializeField] private GameObject knifePrefab;

    [Header(header: "Settings")]
    [SerializeField] private float rotationZ;

    public List<Level> levels;

    [HideInInspector]
    public List<Knife> Knifes;

    private int levelIndex;

    public int AvailableKnifes => availableKnifes;

    private void Start()
    {
        if (isBoos)
        {
           if (GameManager.Instance.Stage < 5)
            {
                GetComponent<SpriteRenderer>().sprite = firstWheel;
            }
           else if (GameManager.Instance.Stage > 5 && GameManager.Instance.Stage < 10)
            {
                GetComponent<SpriteRenderer>().sprite = secondWheel;
            }
           else if (GameManager.Instance.Stage > 10)
            {
                GetComponent<SpriteRenderer>().sprite = thirdWheel;
            }
        }

        levelIndex = UnityEngine.Random.Range(0, levels.Count);

        if (levels[levelIndex].appleChance > UnityEngine.Random.value)
        {
            SpawnApple();
        }

        SpawnKnifes();
    }

    private void RotateWheel()
    {
        transform.Rotate(0f, 0f, rotationZ * Time.deltaTime);
    }

    private void Update()
    {
        RotateWheel();
    }

    private void SpawnKnifes()
    {
        foreach (float knifeAngle in levels[levelIndex].knifeAngleFromWheel)
        {
            GameObject knifeTmp = Instantiate(knifePrefab);
            knifeTmp.transform.SetParent(transform);

            SetRotationFromWheel(wheel: transform, objetcToPlace: knifeTmp.transform, knifeAngle, spaceFromObject: 0.20f, objectRotation: 180f);
            knifeTmp.transform.localScale = new Vector3( 0.8f, 0.8f, 0.8f);
        }
    }

    private void SpawnApple()
    {
        foreach (float appleAngle in levels[levelIndex].appleAngleFromWheel)
        {
            GameObject appleTmp = Instantiate(applePrefab);
            appleTmp.transform.SetParent(transform);

            SetRotationFromWheel(wheel: transform, objetcToPlace: appleTmp.transform, appleAngle, spaceFromObject: 0.25f, objectRotation: 0f);
            appleTmp.transform.localScale = Vector2.one;
        }
    }

    public void SetRotationFromWheel(Transform wheel, Transform objetcToPlace, float angle, float spaceFromObject, float objectRotation)
    {
        Vector2 offset = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad)) * (wheel.GetComponent<CircleCollider2D>().radius + spaceFromObject);
        objetcToPlace.localPosition = (Vector2)wheel.localPosition + offset;
        objetcToPlace.localRotation = Quaternion.Euler(0, 0, -angle + objectRotation);

    }
    
    public void KnifeHit(Knife knife)
    {
        knife.myRigidbody2D.isKinematic = true;
        knife.myRigidbody2D.velocity = Vector2.zero;
        knife.transform.SetParent(transform);
        knife.Hit = true;

        Knifes.Add(knife);
          
        if (Knifes.Count >= availableKnifes)
        {
            LevelManager.Instance.NexеtLevel();
        }
        GameManager.Instance.Score++;
        
    }
    
    public void DestroyKnife()
    {
        foreach (var knife in Knifes)
        {
            Destroy(knife.gameObject);
        }

        Destroy(gameObject);
    }





   // [System.Serializable]
   //private class RotationElement
   // {
   //     public float Speed;
   //     public float Duration;
   // }

   // [SerializeField]
   // private RotationElement[] rotationPattern;

   // private WheelJoint2D WheelJoint;
   // private JointMotor2D motor;

   // private void Awake()
   // {
   //     WheelJoint = GetComponent<WheelJoint2D>();
   //     motor = new JointMotor2D();
   //     StartCoroutine("PlayRotationPattern");
   // }

   // private IEnumerator PlayRotationPattern()
   // {
   //     int rotationIndex = 0;
   //     while (true)
   //     {
   //         yield return new WaitForFixedUpdate();

   //         motor.motorSpeed = rotationPattern[rotationIndex].Speed;
   //         motor.maxMotorTorque = 10000;
   //         WheelJoint.motor = motor;

   //         yield return new WaitForSecondsRealtime(rotationPattern[rotationIndex].Duration);
   //         rotationIndex++;
   //         rotationIndex = rotationIndex < rotationPattern.Length ? rotationIndex : 0;
   //     }
   // }

}

[Serializable]
public class Level
{
    [Range(0, 1)] [SerializeField] public float appleChance;

    public List<float> appleAngleFromWheel = new List<float>();
    public List<float> knifeAngleFromWheel = new List<float>();
}
