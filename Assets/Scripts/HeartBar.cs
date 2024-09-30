using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBar : MonoBehaviour
{
    public GameObject heartPrefab;
    public float health, maxHealth;

    List<HealthHeart> hearts = new List<HealthHeart>();

    public void ClearHearts() {
        foreach (Transform t in transform) {
            Destroy(t.gameObject);
        }
        hearts = new List<HealthHeart>();
    }

    public void createEmptyHeart() {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform);

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.setHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);
    }

    // public void drawHearts() {
    //     ClearHearts();
    //     float maxHealthRemainder = player
    // }
}
