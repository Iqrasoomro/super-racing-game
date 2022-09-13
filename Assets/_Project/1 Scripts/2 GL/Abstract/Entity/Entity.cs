using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ArcadianLab.SimFramework.GL.Data;
using ArcadianLab.SimFramework.GL.Systems;
using UnityEngine.Assertions;
using ArcadianLab.SimFramework.GL;

namespace ArcadianLab.SimFramework.GL.Abstract
{
    public abstract class Entity : MonoBehaviour, IEntity
    {
        protected bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (_isActive == value) return;
                _isActive = value;
            }
        }

        protected EntityType _type;
        public EntityType Type => _type;

        protected Vector3 _position;
        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                gameObject.transform.localPosition = _position;
                Despawn(); // Temp
            }
        }

        public virtual void Init() { }

        public virtual void Spawn()
        {
            gameObject.SetActive(true);
            _isActive = true;
        }

        public virtual void Despawn()
        {
            gameObject.SetActive(false);
            _isActive = false;
        }

        public virtual void Dispose() => Despawn();
    }
}
