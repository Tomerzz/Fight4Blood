using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFreezer : MonoBehaviour
{
    bool isFrozen = false;
    public float duration = 1.0f;
    bool canFreeze = false;

    private void Update()
    {
        if (canFreeze && !isFrozen)
        {
            StartCoroutine(DoFreeze());
        }
    }

    public void Freeze()
    {
        canFreeze = true;
    }

    IEnumerator DoFreeze()
    {
        isFrozen = true;
        float original = Time.timeScale;
        Time.timeScale = 0.0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = original;
        canFreeze = false;
        isFrozen = false;
    }
}
