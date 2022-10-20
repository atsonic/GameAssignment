using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View.UI{
    public class Score : MonoBehaviour
    {
        public void UpdateText(int point)
        {
            this.gameObject.GetComponent<Text>().text = "Score: " + point.ToString();
        }
    }
}