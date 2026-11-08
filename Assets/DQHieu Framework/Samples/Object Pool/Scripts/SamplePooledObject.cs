using System;
using DQHieu.Framework.DesignPattern;

namespace DQHieu.Framework.Sample
{
    using UnityEngine;

    public class SamplePooledObject : MonoBehaviour
    {
        public void Init()
        {
            Invoke("Release", 5f);
        }

        public void Release()
        {
            ObjectPoolManager.Instance.Release(this);
        }
    }

}