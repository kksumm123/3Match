using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AnimalType
{
    Animal1 = 0,
    Animal2,
    Animal3,
    Animal4,
    Animal5,
    Animal6,
}
public class Animal : MonoBehaviour
{
    public AnimalType Animaltype { get; set; }
    Animator animator;
    Rigidbody rigid;
    bool isMoving;
    public bool IsMoveing => isMoving;
    int index;
    public int Index { get; set; } // as GameManager.x, column 0 ~ 6
    void Start()
    {
        isMoving = true;
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(0, -1f, 0, ForceMode.Force);
        Animaltype = (AnimalType)Random.Range(0, 6);
        transform.name = Animaltype.ToString();
        animator = GetComponent<Animator>();
        animator.Play(Animaltype.ToString());
    }
    bool isAlive = true;
    void Update()
    {
        if (isAlive == false)
            return;
        CheckMoving();
        if (Input.GetMouseButtonDown(0))
        {
            // 터치 구현 시작
        }
    }

    void CheckMoving()
    {
        if (rigid.velocity.sqrMagnitude > 0.1f)
            isMoving = true;
        else
            isMoving = false;
    }

    public IEnumerator Destroy()
    {
        animator.Play("DestroyEffect");
        // Wait 0.5f, cuz DestroyAnimation Lengh = 0.5f
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.Remove(gameObject, Index);
        GameManager.instance.Reborn(Index);
        Destroy(gameObject);
    }
}
