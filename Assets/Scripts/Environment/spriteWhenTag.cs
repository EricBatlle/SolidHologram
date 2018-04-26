using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spriteWhenTag : MonoBehaviour {

    public Sprite sprite;
    public string[] triggerTags;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (string tag in triggerTags)
        {
            if ((collision.gameObject.CompareTag(tag)))
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
                StartCoroutine(DestroyAfterTime(0.1f));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (string tag in triggerTags)
        {
            if ((collision.gameObject.CompareTag(tag)))
            {
                this.gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
                StartCoroutine(DestroyAfterTime(0.1f));

            }
        }
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
