using System.Collections;
using System.Collections.Generic;
using Kuhpik;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Reset : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(() =>
        {
            PlayerPrefs.DeleteAll();
            GameEvents.ClearEvents();
            SceneManager.LoadScene(0);
        });
    }
}
