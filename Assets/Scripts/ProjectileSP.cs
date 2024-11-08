using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSP : MonoBehaviour
{

    public Projectile projectile;
    Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        direction = (transform.localRotation * Vector2.right).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void shoot(){
        GameObject go = Instantiate(projectile.gameObject, transform.position, Quaternion.identity);
        Projectile goProjectile = go.GetComponent<Projectile>();
        goProjectile.direction = direction;
    }
}
