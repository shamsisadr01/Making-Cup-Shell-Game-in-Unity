using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    [SerializeField] private Text guide;
    [SerializeField] private Text level;
    [SerializeField] private Text time;
    [SerializeField] private Text speed;

    public string Guide { set { guide.text = value; } }
    public string Level { set { level.text = value; } }
    public string Time { set { time.text = value; } }
    public string Speed { set { speed.text = value; } }
}
