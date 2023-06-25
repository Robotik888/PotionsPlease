using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PotionsPlease.InGame
{
    public class Revertor : MonoBehaviour
    {
        public static Revertor Instance { get; private set; }
        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void Start()
        {
            RevertorCollider = GetComponent<Collider2D>();
            RevertorRigidbody = GetComponent<Rigidbody2D>();
        }


        [SerializeField] public SpriteRenderer RevertorSprite;
        [SerializeField] public Collider2D RevertorCollider;
        [SerializeField] public Rigidbody2D RevertorRigidbody;

        public void enableMe()
        {
            RevertorSprite.enabled = true;
            RevertorCollider.enabled = true;
            RevertorRigidbody.isKinematic = false;
        }

        public void disableMe()
        {
            RevertorSprite.enabled = false;
            RevertorCollider.enabled = false;
            RevertorRigidbody.isKinematic = true;
        }
    }
}
