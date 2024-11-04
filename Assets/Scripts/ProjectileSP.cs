using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSP : MonoBehaviour
{

    public Projectile projectile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void shoot(){
        GameObject go = Instantiate(projectile.gameObject, transform.position, Quaternion.identity);
    }
}
