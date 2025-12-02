using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBase : MonoBehaviour
{
        public ProjectileBase prefabProjectlie;
        public Transform positionToShoot;
        public float timeBetweenShoot = 3f;

        public Transform playerSideReference;
        private Coroutine _currentCoroutine;

    private void Awake()
    {
        if (playerSideReference == null)
        {
            // Garante que achamos o Player real, não só o root
            var player = GetComponentInParent<Player>();
            if (player != null)
            {
                playerSideReference = player.transform;
            }
            else
            {
                Debug.LogWarning("GunBase não encontrou um Player no parent!");
            }
        }
    }

    void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                _currentCoroutine = StartCoroutine(StartShoot());
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                if (_currentCoroutine != null)
                    StopCoroutine(_currentCoroutine);
            }
        }

        IEnumerator StartShoot()
        {
            while (true)
            {
                Shoot();
                yield return new WaitForSeconds(timeBetweenShoot);
            }
        }

        public void Shoot()
        {
            var projectile = Instantiate(prefabProjectlie);
            projectile.transform.position = positionToShoot.position;

            // Direção correta, sem necessidade de referência no inspector
            projectile.side = playerSideReference.transform.localScale.x;
        }
    }