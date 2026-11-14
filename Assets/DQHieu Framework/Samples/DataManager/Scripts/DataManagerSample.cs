using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DQHieu.Framework.Sample
{
    using DQHieu.Framework.Data;
    public class DataManagerSample : MonoBehaviour
    {
        [SerializeField] private TMP_InputField sampleStringField;

        private void Start()
        {
            DataManager.Instance.LoadData();
            sampleStringField.text = DataManager.Instance.sampleData.sampleString;  
        }

        private void OnEnable()
        {
            sampleStringField.onValueChanged.AddListener(OnSampleStringChanged);
        }

        private void OnDisable()
        {
            sampleStringField.onValueChanged.RemoveListener(OnSampleStringChanged);
        }
        
        private void OnSampleStringChanged(string value)
        {
            DataManager.Instance.sampleData.sampleString = value;
        }
        
    }
}