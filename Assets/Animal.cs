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
    AnimalType m_animaltype;
    public AnimalType Animaltype { get; set; }
    Animator animator;
    Rigidbody rigid;
    bool isMoving;
    public bool IsMoveing => isMoving;
    void Start()
    {
        isMoving = true;
        rigid = GetComponent<Rigidbody>();
        Animaltype = (AnimalType)Random.Range(0, 6);
        transform.name = Animaltype.ToString();
        animator = GetComponent<Animator>();
        animator.Play(Animaltype.ToString());
    }
    void Update()
    {
        if (rigid.velocity.sqrMagnitude > 0.1f)
            isMoving = true;
        else
            isMoving = false;
    }
}
