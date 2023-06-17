using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CASP.SoundManager;
public class ObstaclePlay : MonoBehaviour
{
    private List<Transform> DroppedItems;

    [SerializeField] ParticleSystem Smoke;
    BoxCollider Coll;

    private void Start()
    {
        Coll = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Coll.enabled = false; 
        Smoke.Play();

        int startIndex = CollectedCoffeeData.instance.CoffeeList.IndexOf(other.transform);
        if (startIndex <= -1) return;
        int endIndex = CollectedCoffeeData.instance.CoffeeList.Count - startIndex;
        DroppedItems = CollectedCoffeeData.instance.CoffeeList.GetRange(startIndex + 1, endIndex - 1);
        if (startIndex < CollectedCoffeeData.instance.CoffeeList.Count - 1)
        {
            CollectedCoffeeData.instance.CoffeeList.RemoveRange(startIndex, CollectedCoffeeData.instance.CoffeeList.Count - startIndex);
        }
        else
        {
            CollectedCoffeeData.instance.CoffeeList.Remove(other.transform);
        }
        other.gameObject.SetActive(false);
        Destroy(other.gameObject, 0.7f);
        ThrowStackedItems(DroppedItems);

        if (other.CompareTag("Cherry"))
        {
            UIManager.instance.UpdateScore(-10);
        }

        if (other.CompareTag("Chocolate"))
        {
            UIManager.instance.UpdateScore(-8);
        }

        if (other.CompareTag("White"))
        {
            UIManager.instance.UpdateScore(-6);
        }

        if (other.CompareTag("Pink"))
        {
            UIManager.instance.UpdateScore(-4);
        }

        if (other.CompareTag("CollectedCone"))
        {
            UIManager.instance.UpdateScore(-2);
        }
            SoundManager.instance.Play("Obstacle", true);
    }

    private void ThrowStackedItems(List<Transform> droppedItems)
    {
        foreach (var item in droppedItems)
        {
            if (item.CompareTag("Cherry"))
            {
                UIManager.instance.UpdateScore(-10);
            }
            if (item.CompareTag("Chocolate"))
            {
                UIManager.instance.UpdateScore(-8);
            }
            if (item.CompareTag("White"))
            {
                UIManager.instance.UpdateScore(-6);
            }
            if (item.CompareTag("Pink"))
            {
                UIManager.instance.UpdateScore(-4);
            }
            if (item.CompareTag("CollectedCone"))
            {
                UIManager.instance.UpdateScore(-2);
            }
            var currentPos = item.position;

            item.DOJump(new Vector3(Random.Range(-1.7f, 1.7f), currentPos.y, currentPos.z + Random.Range(5f, 7f)), 2, 1, 0.3f)
                .OnComplete(() =>
                {
                    item.gameObject.tag = "Cone";
                    Destroy(item.gameObject.GetComponent<Rigidbody>());
                    Destroy(item.gameObject.GetComponent<CollectedCoffee>());
                });
        }
    }
}
