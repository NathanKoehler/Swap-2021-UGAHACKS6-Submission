using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeet_S : MonoBehaviour
{
    private Player_S player_S;
    private bool playerKillable = true;

    private void Start()
    {
        player_S = GetComponentInParent<Player_S>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Elusive"))
        {
            //rigid.velocity = new Vector2(rigid.velocity.x, 0);
            player_S.SetCanJump(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Elusive"))
        {
            player_S.SetCanJump(false);
        }
    }

    IEnumerator delayDying()
    {
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForEndOfFrame();
        playerKillable = true;
    }
}
