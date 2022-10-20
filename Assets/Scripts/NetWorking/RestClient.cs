/*
* https://qiita.com/satotin/items/579fa3b9da0ad0d899e8
*/
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Data;

namespace NetWorking
{
    public class RestClient : MonoBehaviour
    {
        [SerializeField] string _getHostName;
        [SerializeField] string _PostHostName;
        public async Task<string> Get()
        {
            UnityWebRequest www = UnityWebRequest.Get(_getHostName);
            await www.SendWebRequest();
            string response = www.downloadHandler.text;
            // Score[] array = response.FromJsonArray<Score>();

            return response;
        }
        // Score Post
        public async Task<string> Post(int score)
        {
            Score scoreData = new Score();
            WWWForm form = new WWWForm();
            form.AddField("score", score);

            UnityWebRequest www = UnityWebRequest.Post(_PostHostName, form);
            await www.SendWebRequest();
            var response = www.downloadHandler.text;

            return response;
        }
    }
}

