using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Collider[] player;
    [SerializeField]
    float range;
    [SerializeField]
    LayerMask layer;

    [SerializeField]
    float aimingTime;
    [SerializeField]
    float shootTime;
    [SerializeField]
    float reloadingTime;

    public float Range => range;

    bool playerInRange = false;
    GameObject bullet;
    LineRenderer lineRenderer;

    private void Awake()
    {
        bullet = transform.GetChild(0).gameObject;
        bullet.SetActive(false);
        bullet.transform.position = transform.position;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (!playerInRange)
        {
            player = Physics.OverlapSphere(transform.position, range, layer);
        }
        if (player.Length > 0 && !playerInRange)
        {
            LookOtPlayer();
        }
    }

    void LookOtPlayer()
    {
        playerInRange = true;
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        float time = 0;

        while (time < aimingTime)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, player[0].transform.position);
            time += Time.deltaTime;
            yield return null;
        }

        bullet.transform.LookAt(player[0].transform.position);

        yield return new WaitForSeconds(shootTime);

        //�� ���
        lineRenderer.enabled = false;
        bullet.SetActive(true);
        Debug.Log("��");

        yield return new WaitForSeconds(0.2f);

        bullet.SetActive(false);

        yield return new WaitForSeconds(reloadingTime);

        bullet.transform.position = transform.position;
        playerInRange = false;

        yield break;
    }
}
