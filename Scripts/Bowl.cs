using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowl : MonoBehaviour
{
    public static bool bowlSelection;

    private void OnMouseDown()
    {
        if (!bowlSelection && !GameController.endGame && !GameController.isPlaying)
        {
            StartCoroutine(DisplayResult());
            bowlSelection = true;
        }
    }

    private IEnumerator DisplayResult()
    {
        if (transform.childCount > 0) // If she had a child, the ball is under this bowl.
        {
            Transform sp = transform.GetChild(0);
            sp.parent = null;

            int level = PlayerPrefs.GetInt("level") + 1;
            PlayerPrefs.SetInt("level", level);

            float time = PlayerPrefs.GetFloat("time") + 0.5f;
            PlayerPrefs.SetFloat("time", time);

            float speed = PlayerPrefs.GetFloat("speed") + 20f;
            PlayerPrefs.SetFloat("speed", speed);
        }

        bool isMove = true;
        while (isMove)
        {
            isMove = MoveToTarget(1f);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        isMove = true;
        while (isMove)
        {
            isMove = MoveToTarget(0.855f);
            yield return null;
        }

        GameController.endGame = true;
    }

    public bool MoveToTarget(float targetY)
    {
        Vector3 targetPos = transform.position;
        targetPos.y = targetY;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 0.5f);

        if (transform.position.y == targetY)
        {
            return false;
        }

        return true;
    }
}
