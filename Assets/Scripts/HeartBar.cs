using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBar : MonoBehaviour
{
    public GameObject heartPrefab;
    public Player player;

    private float previousHealth;

    List<HealthHeart> hearts = new List<HealthHeart>();

    private void OnEnable() {
        player.OnPlayerDamaged += drawHearts;
    }

    private void OnDisable() {
        player.OnPlayerDamaged -= drawHearts;
    }

    private void Start() {
         previousHealth = player.health;
        drawHearts();
    }

    private void Update() {
    if (player.health != previousHealth) {
        drawHearts();  // Update hearts if health changes
        previousHealth = player.health;  // Update the stored health
    }
}

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

    public void drawHearts() {
        ClearHearts();
        float maxHealthRemainder = player.maxHealth % 2;
        int heartsToMake = (int)((player.maxHealth / 20) + maxHealthRemainder);
        for(int i = 0; i < heartsToMake; i++) {
            createEmptyHeart();
        }

        for (int i = 0; i < hearts.Count; i++) {
            int HeartStatusRemainder = (int)Mathf.Clamp(player.health - (i * 20), 0, 1);
            hearts[i].setHeartImage((HeartStatus)HeartStatusRemainder);
        }
    }
}
