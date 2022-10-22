using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class WoonyMethods
{
    [System.Serializable]
    public class ShakeScaleInfo
    {
        public float animateDuration = 0.5f;
        public float shakeScaleDuration = 0.3f;
        public float shakeScaleStrength = 0.3f;
        public int shakeScaleVibrato = 5;
        public float shakeScaleRandomness = 45;
    }

    [System.Serializable]
    public class CustomEase
    {
        public float duration;
        public AnimationCurve customEase = AnimationCurve.Linear(0, 0, 1, 1);

        public CustomEase(float duration)
        {
            this.duration = duration;
            customEase = AnimationCurve.Linear(0, 0, 1, 1);
        }
    }

    public static Vector3 GetDirection(this Vector3 from, Vector3 to)
    {
        return (to - from).normalized;
    }

    public static Vector3 GetDirectionWithNoHeight(this Vector3 from, Vector3 to)
    {
        var direction = from.GetDirection(to);
        direction.y = 0;
        return direction;
    }

    public static Vector3 RandomVector3(float min, float max)
    {
        return new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
    }

    public static Vector3 RandomVector3(float value)
    {
        return new Vector3(Random.Range(-value, value), Random.Range(-value, value), Random.Range(-value, value));
    }

    public static Vector3 RandomVector3XZ(float value)
    {
        return new Vector3(Random.Range(-value, value), 0, Random.Range(-value, value));
    }

    public static Vector3 IncreasedVector3(Vector3 vec3)
    {
        return new Vector3(SetValue(vec3.x), SetValue(vec3.y), SetValue(vec3.z));

        float SetValue(float value)
        {
            if (value == 0)
                return 0;
            else
                return value > 0 ? value + 1 : value - 1;
        }
    }

    public static void StopCo<T>(T t, Coroutine handle) where T : MonoBehaviour
    {
        if (handle != null)
            t.StopCoroutine(handle);
    }

    public static Coroutine StopAndStartCo<T>(T t, Coroutine handle, IEnumerator CoroutineFn) where T : MonoBehaviour
    {
        StopCo(t, handle);
        return t.StartCoroutine(CoroutineFn);
    }

    /// <summary>
    /// 예시 : WoonyMethods.Asserts(this, (variable, nameof(variable)));
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="objs"></param>
    public static void Asserts<T>(T t, params (UnityEngine.Object, string)[] objs) where T : MonoBehaviour
    {
#if UNITY_EDITOR
        foreach (var obj in objs)
        {
            if (obj.Item1 == null)
            {
                AlertError($"{t.transform.name} : {obj.Item2} is null", t.transform);
            }
        }
#else
        foreach (var obj in objs)
        {
            Debug.Assert(obj.Item1 != null, $"{t.transform.name} : {obj.Item2} is null", t.transform);
        }
#endif
    }

    /// <summary>
    /// bool 조건이 false라면 메시지를 띄웁니다
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <param name="objs"></param>
    public static void Asserts<T>(T t, params (bool expressionResult, string message)[] objs) where T : MonoBehaviour
    {
#if UNITY_EDITOR
        foreach (var obj in objs)
        {
            if (obj.expressionResult == false)
            {
                AlertError($"{t.transform.name} : {obj.message}", t.transform);
            }
        }
#else
        foreach (var obj in objs)
        {
            if (obj.expressionResult == false)
            {
                Debug.Assert(obj.expressionResult, $"{t.transform.name} : {obj.message}", t.transform);
            }
        }
#endif
    }

    public static void Asserts(string caller, params (UnityEngine.Object, string)[] objs)
    {
#if UNITY_EDITOR
        foreach (var obj in objs)
        {
            if (obj.Item1 == null)
            {
                AlertError($"{caller} : {obj.Item2} is null");
            }
        }
#else
        foreach (var obj in objs)
        {
            Debug.Assert(obj.Item1 != null, $"{caller} : {obj.Item2} is null");
        }
#endif
    }

    public static void AlertError(string v)
    {
        Debug.LogError($"우니 : {v}");
#if UNITY_EDITOR
        EditorUtility.DisplayDialog("우니 알림", $"우니 : {v}", "오케이 ~");
#endif
    }

    public static void AlertError(string v, Transform transform)
    {
        Debug.LogError($"우니 : {v}", transform);
#if UNITY_EDITOR
        EditorUtility.DisplayDialog("우니 알림", $"우니 : {v}", "오케이 ~");
#endif
    }

    public static T GetRandomItem<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static T GetLastItem<T>(this List<T> list)
    {
        return list[list.Count - 1];
    }

    public static void Suffle<T>(this List<T> list)
    {
        int n = list.Count;
        int m = 0;
        T tempItem;

        while (n > 1)
        {
            n--;
            m = Random.Range(0, n + 1);
            tempItem = list[m];
            list[m] = list[n];
            list[n] = tempItem;
        }
    }

    public static string GetPath(this Transform current)
    {
        if (current.parent == null)
        {
            return current.name;
        }

        return current.parent.GetPath() + "/" + current.name;
    }

    public static Tween AppearLikeFountain(this GameObject childObject,
                                           GameObject targetParent,
                                           Vector3 destPosition,
                                           float duration,
                                           float jumpHeight,
                                           AnimationCurve jumpEase)
    {
        bool isAutoAttacted = false;
        var distance = destPosition - childObject.transform.position;
        var agent = targetParent.GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            agent = targetParent.AddComponent<NavMeshAgent>();
            isAutoAttacted = true;
        }
        var moveFactor = distance / duration;

        var sequence = DOTween.Sequence();
        sequence.Append(childObject.transform.DOLocalMoveY(jumpHeight, duration)
                                        .SetLink(childObject)
                                        .SetEase(jumpEase)
                                        .OnUpdate(() => agent.Move(moveFactor * Time.deltaTime))
                                        .OnComplete(() =>
                                        {
                                            if (isAutoAttacted)
                                            {
                                                GameObject.Destroy(agent);
                                            }
                                        }));
        return sequence;
    }

    public static List<T1> GetAllObjectsOnlyInScene<T1>() where T1 : Component
    {
        List<T1> result = new List<T1>();
        var components = Resources.FindObjectsOfTypeAll(typeof(T1));
        foreach (UnityEngine.Object co in components)
        {
            Component component = co as Component;
            GameObject go = component.gameObject;
            if (go.scene.name == null) // 씬에 있는 오브젝트가 아니므로 제외한다.
                continue;

            // HideFlags 이용하여 씬에 있는 오브젝트가 아닌경우 제외
            if (go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave || go.hideFlags == HideFlags.HideInHierarchy)
                continue;

            result.Add(component as T1);
        }

        return result;
    }
}
