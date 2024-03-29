﻿using System.Collections;
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
    public int Index { get; set; } //  column 0 ~ 6
    public bool IsMoveing => isMoving;

    Animator animator;
    Rigidbody rigid;
    bool isMoving;
    bool isAlive = true;

    private void Start()
    {
        isMoving = true;
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(0, -100f, 0, ForceMode.Force);
        Animaltype = (AnimalType)Random.Range(0, 6);
        transform.name = Animaltype.ToString();
        animator = GetComponent<Animator>();
        animator.Play(Animaltype.ToString());

        StartCheckState();
    }

    private void StartCheckState()
    {
        StartCoroutine(CheckStateCo());

        IEnumerator CheckStateCo()
        {
            while (isAlive)
            {
                CheckMoving();
                CheckSwitching();
                CheckUPForce();
                yield return null;
            }
        }
    }

    private void CheckMoving()
    {
        isMoving = rigid.velocity.sqrMagnitude > 0.1f;
    }

    private void CheckSwitching()
    {
        rigid.useGravity = !GameManager.Instance.IsSwitching;
    }

    private void CheckUPForce()
    {
        // velocity가 0보다 크면 0으로 초기화
        if (rigid.velocity.y > 0)
        {
            rigid.Sleep();
        }
    }

    public void DestroyAnimal()
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

    private void OnMouseDown()
    {
        if (GameManager.Instance.PlayMode == PlayModeType.None) return;

        GameManager.Instance.OnMouseDown(transform);
    }

    private void OnMouseOver()
    {
        if (GameManager.Instance.PlayMode == PlayModeType.None) return;

        if (Input.GetMouseButtonUp(0))
        {
            GameManager.Instance.OnMouseOver(transform);
        }
    }
}
