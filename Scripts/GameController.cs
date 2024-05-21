using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    [SerializeField] private Transform rotate;
    [SerializeField] private Transform sphere;
    [SerializeField] private Bowl[] bowls;
    [SerializeField] private UIGame uiGame;
    [SerializeField] private GameObject uiMenu;
    [SerializeField] private CameraController cam;

    public static bool isPlaying;
    public static bool endGame;
    private bool isRotate;
    private bool isAnim = true;
    private float rotateSpeed;

    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey("level"))
        {
            PlayerPrefs.SetInt("level", 1);
        }

        if (!PlayerPrefs.HasKey("time"))
        {
            PlayerPrefs.SetFloat("time", 5f);
        }

        if (!PlayerPrefs.HasKey("speed"))
        {
            PlayerPrefs.SetFloat("speed", 100f);
        }
    }

    private IEnumerator Start()
    {
        isPlaying = true;
        endGame = false;
        Bowl.bowlSelection = false;

        if (uiGame.gameObject.activeSelf)
        {
            uiGame.Level = "Level : " + PlayerPrefs.GetInt("level");
            uiGame.Time = "Time : " + PlayerPrefs.GetFloat("time");
            rotateSpeed = PlayerPrefs.GetFloat("speed");
            uiGame.Speed = "Speed : " + rotateSpeed;

            uiGame.Guide = "Restart Game...";
            yield return new WaitForSeconds(1.5f);
        }
        else
        {
            rotateSpeed = Random.Range(100f, 200f);
        }

        uiGame.Guide = "Choose a bowl at random";

        int indexRand = Random.Range(0, bowls.Length);

        bool isMove = true;
        while (isMove)
        {
            isMove = bowls[indexRand].MoveToTarget(1f);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        Transform sp = Instantiate(sphere);
        Vector3 newPos = bowls[indexRand].transform.position;
        newPos.y = 0.77f;
        sp.position = newPos;

        isMove = true;
        yield return new WaitForSeconds(1f);

        while (isMove)
        {
            isMove = bowls[indexRand].MoveToTarget(0.855f);
            yield return null;
        }

        sp.parent = bowls[indexRand].transform;

        if (isAnim)
        {
            while (isAnim)
            {
                if (!isRotate)
                    StartCoroutine(StartRotation());
                yield return null;
            }
        }
        else if (!isAnim && !endGame)
        {
            float time = PlayerPrefs.GetFloat("time");
            uiGame.Guide = "Bowl Rotation Time : " + time;
            while (time > 0 && !isAnim)
            {
                if (!isRotate)
                {
                    StartCoroutine(StartRotation());
                }
                time -= Time.deltaTime;
                uiGame.Guide = "Bowl Rotation Time : " + time.ToString("F2");
                yield return null;
            }
        }

        uiGame.Guide = "Please Select A Bowl";

        isPlaying = false;

        while (!endGame || isRotate)
        {
            yield return null;
        }

        Destroy(sp.gameObject);

        uiGame.Guide = "";
        if (!uiGame.gameObject.activeSelf)
        {
            uiGame.Level = "";
            uiGame.Time = "";
            uiGame.Speed = "";
        }

        uiGame.gameObject.SetActive(!isAnim);

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(Start());
    }

    private IEnumerator StartRotation()
    {
        isRotate = true;

        List<int> items = new List<int>(); // To choose a random index bowl
        items.Add(0);
        items.Add(1);
        items.Add(2); 

        int r1 = items[Random.Range(0, items.Count)];
        items.RemoveAt(r1);
        int r2 = items[Random.Range(0, items.Count)];

        rotate.position = (bowls[r1].transform.position + bowls[r2].transform.position) / 2f;

        bowls[r1].transform.parent = rotate;
        bowls[r2].transform.parent = rotate;

        float angle = 0f;

        while (angle < 180f)
        {
            rotate.rotation = Quaternion.Euler(0, angle, 0);
            angle += Time.deltaTime * rotateSpeed;
            yield return null;
        }
        rotate.rotation = Quaternion.Euler(0, 180f, 0);

        bowls[r1].transform.parent = null;
        bowls[r2].transform.parent = null;

        rotate.rotation = Quaternion.Euler(0, 0, 0);

        isRotate = false;
    }

    public void Play()
    {
        isAnim = false;
        endGame = true;
        cam.StartGame();
        uiMenu.SetActive(false);
    }

    public void Menu()
    {
        isAnim = true;
        endGame = true;
        cam.StartGame();
        uiMenu.SetActive(true);
        uiGame.gameObject.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
