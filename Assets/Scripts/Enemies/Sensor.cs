using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : MonoBehaviour
{

    //Matar lo que esté en su rango (en su defecto, las cosas mueren al entrar en contacto con él)
    //Cambiar de sprite cada vez que lo haga

    [SerializeField] private Sprite spriteOn;
    [SerializeField] private Sprite spriteOff;

    private void OnCollisionEnter2D(Collision2D collision)
    {        
        this.gameObject.GetComponent<SpriteRenderer>().sprite = spriteOn;
        StartCoroutine(SetInitialSpriteAfter(0.1f));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {        
        this.gameObject.GetComponent<SpriteRenderer>().sprite = spriteOn;
        StartCoroutine(SetInitialSpriteAfter(0.1f));    
    }

    IEnumerator SetInitialSpriteAfter(float time)
    {
        yield return new WaitForSeconds(time);
        this.gameObject.GetComponent<SpriteRenderer>().sprite = spriteOff;
    }
}
