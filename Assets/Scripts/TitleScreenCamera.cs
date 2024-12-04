using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreenCamera : MonoBehaviour
{
    public float moveSpeed; // Speed at which the camera moves
    public float waitTime; // Initial movement direction
    Vector3[] targetPositions = new Vector3[3]; // List of 3 target positions for the camera to move towards

    
    

    

    private void Start()
    {   
        //Set the target positions for the camera to move towards
        targetPositions[0] = new Vector2(0, 7); // Start position
        targetPositions[1] = new Vector2(-7, -41);
        targetPositions[2] = new Vector2(22, -41); // End position, then Repeat
    }

    private void Update()
    {
        while(true){
            for (int i = 0; i < targetPositions.Length; i++)
            {
                if (transform.position != targetPositions[i])
                {
                    transform.position = Vector2.MoveTowards(transform.position, targetPositions[i], moveSpeed * Time.deltaTime);

                    if (transform.position == targetPositions[i])
                    {
                        StartCoroutine(Wait());
                    }
                }
                else
                {
                    if (i == targetPositions.Length - 1)
                    {
                        i = 0;
                    }
                    else
                    {
                        i++;
                    }
                }
            }
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(waitTime);
    }

    
}
