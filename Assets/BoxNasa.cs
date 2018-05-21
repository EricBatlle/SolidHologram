using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BoxNasa : NetworkBehaviour {

    public Sprite collisionSprite;

    public void Start()
    {
        //StartCoroutine(DestroyAfterTime(5f));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.CompareTag("Pusher")))
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = collisionSprite;
            //Auto-Destroy after few time (to allow the players to see the collision sprite)
            StartCoroutine(DestroyAfterTime(0.1f));
        }
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        NetworkServer.Destroy(this.gameObject);
    }
}
