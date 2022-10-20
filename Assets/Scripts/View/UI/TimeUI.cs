using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View.UI{
    public class TimeUI : MonoBehaviour
    {
        public void UpdateText(int time)
        {
            this.gameObject.GetComponent<Text>().text = "Time : " + time.ToString();
        }
    }
}