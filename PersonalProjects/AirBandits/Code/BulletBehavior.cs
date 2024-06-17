using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BulletBehavior : NetworkBehaviour
{
    public float bulletSpeed;
    public float lifeTime;
    public int damage;

    [SyncVar]
    public uint fromPlayerId;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += transform.up * bulletSpeed * Time.deltaTime;
    }

    
    public void OnTriggerEnter2D(Collider2D collision)
    {
        //hit enemy player
        if (collision.tag == "Player" && fromPlayerId != collision.GetComponent<playerBehavior>().playerId && collision.GetComponent<playerBehavior>().airplaneMode == true)
        {
            //remove health
            collision.GetComponent<playerBehavior>().airplaneHealth -= damage;


            //if this shot kills the player, give the shooting player 75% of their gold
            if (collision.GetComponent<playerBehavior>().airplaneHealth <= 0)
            {
                int rewardGold = (int)Mathf.Round(collision.GetComponent<playerBehavior>().myGold * .75f);
                GameObject ace = NetworkIdentity.spawned[fromPlayerId].gameObject;
                ace.GetComponent<playerBehavior>().myGold += rewardGold;
            }

            Destroy(this.gameObject);
        }
    }
}
