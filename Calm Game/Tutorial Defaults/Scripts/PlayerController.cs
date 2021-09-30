using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    private Vector2 moveDirection;
    void Update()
    {
        moveInputs();
        Interact();
    }
    void FixedUpdate()
    {
        Move();
    }
    void moveInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY);
    }
    void Move()
    {
        rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
    }
    void Interact()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float xDistance = Mathf.Abs(GameObject.Find("Interactable placeholder").transform.position.x - GameObject.Find("Main_Character").transform.position.x);
            float yDistance = Mathf.Abs(GameObject.Find("Interactable placeholder").transform.position.y - GameObject.Find("Main_Character").transform.position.y);

            if (Mathf.Sqrt(Mathf.Pow(xDistance, 2) + Mathf.Pow(yDistance, 2)) <= 5f)
            {
                Debug.Log("hi");
            }
        }
    }
}
