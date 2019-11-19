using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    public GameObject Bullet = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(Transform partenTransorm)
    {
        if (partenTransorm != null)
        {
            Vector3 position = partenTransorm.position + partenTransorm.forward * 2;
           GameObject myObject = Instantiate(Bullet, position, partenTransorm.rotation);
            myObject.GetComponent<BulletScript>().velocity = partenTransorm.forward *2f;
            myObject.GetComponent<BulletScript>().myParent = partenTransorm.gameObject.GetComponent<MyAgentsScript>();
        }
    }
}
