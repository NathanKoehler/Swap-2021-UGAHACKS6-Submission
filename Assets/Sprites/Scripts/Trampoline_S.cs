using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline_S : MonoBehaviour
{
    public bool isRed;

    void Start() {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isRed && collision.gameObject.GetComponent<Player_S>().isRed)
            {
                StartCoroutine(bounce(collision.GetComponent<Rigidbody2D>()));
            } else if (!isRed && !collision.gameObject.GetComponent<Player_S>().isRed)
            {
                StartCoroutine(bounce(collision.GetComponent<Rigidbody2D>()));
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isRed && collision.gameObject.GetComponent<Player_S>().isRed)
            {
                StartCoroutine(bounce(collision.GetComponent<Rigidbody2D>()));
            }
            else if (!isRed && !collision.gameObject.GetComponent<Player_S>().isRed)
            {
                StartCoroutine(bounce(collision.GetComponent<Rigidbody2D>()));
            }
        }
    }

    IEnumerator bounce(Rigidbody2D rigid)
    {

        yield return new WaitForSeconds(0.05f);
        rigid.AddForce(transform.up * 5f, ForceMode2D.Impulse);
    }

    
}
