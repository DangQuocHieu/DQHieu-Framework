using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DQHieu.Framework.Sample
{
    using DQHieu.Framework.Data;
    public class DataManagerSample : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _sampleStringField;

        private void Start()
        {
            DataManager.Instance.LoadData();
            _sampleStringField.text = DataManager.Instance.SampleData.SampleString;  
        }

        private void OnEnable()
        {
            _sampleStringField.onValueChanged.AddListener(OnSampleStringChanged);
        }

        private void OnDisable()
        {
            _sampleStringField.onValueChanged.RemoveListener(OnSampleStringChanged);
        }
        
        private void OnSampleStringChanged(string value)
        {
            DataManager.Instance.SampleData.SampleString = value;
        }
        
    }
}