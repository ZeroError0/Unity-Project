using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer;
    public LayerMask grassLayer;

    private bool isMoving;
    private Vector2 input;

    private Animator animator;

    private void Awake()
    {
        animator=GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;
            //remove diagonal movement.

            if (input != Vector2.zero)
            {   
                animator.SetFloat("moveX",input.x);
                animator.SetFloat("moveY",input.y); 

                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if(IsWalkable(targetPos))
                StartCoroutine(Move(targetPos));

            }
        }
        animator.SetBool("isMoving",isMoving);
    }
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;

        CheckForEncounters();
    }
    private bool IsWalkable(Vector3 targetPos)
    {
        if(Physics2D.OverlapCircle(targetPos,0.03f,solidObjectsLayer)!=null)
        {
            return false;
        }
        return true;
           
    }
    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position,0.2f,grassLayer) !=null )
        {
            if (Random.Range(1, 101) <=10 )
            {
                Debug.Log("Encounter a Wild Pokemon");            }
            
        }
    }
    
}



