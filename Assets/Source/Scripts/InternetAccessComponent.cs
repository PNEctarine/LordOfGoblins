using System;
using System.Collections;
using System.Threading.Tasks;
using Kuhpik;
using UnityEngine;
using UnityEngine.Networking;

namespace InternetCheck
{
    public class InternetAccessComponent : Singleton<InternetAccessComponent>
    {
        [Header("Parameters")]
        [Tooltip("Uri's List")]
        [SerializeField] private string[] urls;

        public IEnumerator CoroutineTestConnection(Action<bool> callback)
        {
            foreach (string url in urls)
            {
                UnityWebRequest request = UnityWebRequest.Get(url);
                yield return request.SendWebRequest();

                Debug.Log("{GameLog} => [InternetAccess] - TestConnection \n URI: " + url + "\n Network Error: " + request.result);

                if (request.result == UnityWebRequest.Result.Success)
                {
                    callback(true);
                    yield break;
                }
            }

            callback(false);
        }

        public async void AsyncTestConnection(Action<bool> callback)
        {
            foreach (string url in urls)
            {
                UnityWebRequest request = UnityWebRequest.Get(url);

                request.SendWebRequest();

                while (!request.isDone)
                {
                    await Task.Yield();
                }

                Debug.Log("{GameLog} => [InternetAccess] - TestConnection \n URI: " + url + "\n Network Error: " + request.result);

                if (request.result == UnityWebRequest.Result.Success)
                {
                    callback(true);
                    return;
                }
            }

            callback(false);
        }
    }
}