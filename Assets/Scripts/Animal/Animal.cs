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
    public int Index { get; set; } // as GameManager.x, column 0 ~ 6
    public bool IsMoveing => isMoving;

    Animator animator;
    Rigidbody rigid;
    bool isMoving;
    bool isAlive = true;

    void Start()
    {
        isMoving = true;
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(0, -100f, 0, ForceMode.Force);
        Animaltype = (AnimalType)Random.Range(0, 6);
        transform.name = Animaltype.ToString();
        animator = GetComponent<Animator>();
        animator.Play(Animaltype.ToString());
    }

    void Update()
    {
        if (isAlive == false)
            return;
        CheckMoving();
        CheckSwipping();
        CheckUPForce();
    }

    void CheckUPForce()
    {
        // velocity가 0보다 크면 0으로 초기화
        if (rigid.velocity.y > 0)
            rigid.Sleep();
    }

    void CheckMoving()
    {
        if (rigid.velocity.sqrMagnitude > 0.1f)
            isMoving = true;
        else
            isMoving = false;
    }

    void CheckSwipping()
    {
        rigid.useGravity = !GameManager.Instance.IsSwipping;
    }

    public void Destroy()
    {
        StartCoroutine(Co());

        IEnumerator Co()
        {
            animator.Play("DestroyEffect");
            // Wait 0.4f, cuz DestroyAnimation Lengh = 0.5f
            yield return new WaitForSeconds(0.4f);
            GameManager.Instance.OnCompleteDestroyAnimal(gameObject, Index);
            Destroy(gameObject);
        }
    }

    void OnMouseDown()
    {
        if (GameManager.Instance.PlayMode == PlayModeType.None)
            return;

        GameManager.Instance.pressedAnimal = transform;
    }

    private void OnMouseOver()
    {
        if (GameManager.Instance.PlayMode == PlayModeType.None)
            return;

        if (Input.GetMouseButtonUp(0))
            GameManager.Instance.releasedAnimal = transform;
    }
}
