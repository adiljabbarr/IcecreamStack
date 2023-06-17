using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using CASP.SoundManager;

public class CollectedCoffee : MonoBehaviour
{
    Sequence seq;

    private void Start()
    {
        if (transform.GetChild(0).gameObject.activeSelf)
        {
            gameObject.tag = "Pink";
        }
        if (transform.GetChild(1).gameObject.activeSelf)
        {
            gameObject.tag = "White";
        }
        if (transform.GetChild(2).gameObject.activeSelf)
        {
            gameObject.tag = "Chocolate";
        }
        if (transform.GetChild(3).gameObject.activeSelf)
        {
            gameObject.tag = "Cherry";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandFree"))
        {
            SoundManager.instance.Play("HandFree", true);

            CollectedCoffeeData.instance.CoffeeList.Remove(transform);
            // && this.gameObject.layer != 6
        }

        if (other.CompareTag("Cone"))
        {
            SoundManager.instance.Play("Collect", true);
            //UIManager.instance.UpdateScore(2);
            other.tag = "CollectedCone";
            CollectedCoffeeData.instance.CoffeeList.Add(other.transform);
            other.gameObject.AddComponent<CollectedCoffee>();
            other.gameObject.AddComponent<Rigidbody>().isKinematic = true;

            // var seq = DOTween.Sequence();
            seq = DOTween.Sequence();
            seq.Kill();
            seq = DOTween.Sequence();
            for (int i = CollectedCoffeeData.instance.CoffeeList.Count - 1; i > 0; i--)
            {
                seq.WaitForCompletion(true);
                seq.Join(CollectedCoffeeData.instance.CoffeeList[i].DOScale(new Vector3(2.38f, 1.35f, 2.5f), 0.1f));
                seq.AppendInterval(0.02f);
                seq.Join(CollectedCoffeeData.instance.CoffeeList[i].DOScale(new Vector3(1.87f, 1.06f, 1.94f), 0.1f));
            }
        }

        //2 coneda verir,+2+2+2+2
        if (other.CompareTag("CreamMachine"))
        {
        SoundManager.instance.Play("Cream", true);

            if (transform.CompareTag("Pink"))
            {
                UIManager.instance.UpdateScore(2);
                transform.GetChild(1).gameObject.SetActive(true);
                gameObject.tag = "White"; //ag olur
            }

            if (transform.CompareTag("CollectedCone"))
            {
                UIManager.instance.UpdateScore(2);
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).DOScale(0.84974f, 0.1f);

                gameObject.tag = "Pink"; //pembe olur


            }

        }
        if (other.CompareTag("ChocolateMachine"))
        {
            SoundManager.instance.Play("Chocolate", true);

            if (transform.CompareTag("White"))
            {
                UIManager.instance.UpdateScore(2);
                transform.GetChild(2).gameObject.SetActive(true); //sokolad olur
                gameObject.tag = "Chocolate";
            }
        }

        if (other.CompareTag("CherryMachine"))
        {
            SoundManager.instance.Play("Cherry", true);

            if (transform.CompareTag("Chocolate"))
            {
                UIManager.instance.UpdateScore(2);
                transform.GetChild(3).gameObject.SetActive(true); //cherry olur
                gameObject.tag = "Cherry";
            }
        }


        if (other.CompareTag("SellPortal"))
        {
            SoundManager.instance.Play("SellPortal", true);

            if (transform.CompareTag("Cherry"))
            {
                UIManager.instance.UpdateScore(-10);
                UIManager.instance.UpdateEarnedMoney(+10);

            }

            if (transform.CompareTag("Chocolate"))
            {
                UIManager.instance.UpdateScore(-8);
                UIManager.instance.UpdateEarnedMoney(+8);

            }

            if (transform.CompareTag("White"))
            {
                UIManager.instance.UpdateScore(-6);
                UIManager.instance.UpdateEarnedMoney(+6);

            }

            if (transform.CompareTag("Pink"))
            {
                UIManager.instance.UpdateScore(-4);
                UIManager.instance.UpdateEarnedMoney(+4);

            }

            if (transform.CompareTag("CollectedCone"))
            {
                UIManager.instance.UpdateScore(-2);
                UIManager.instance.UpdateEarnedMoney(+2);
            }

            transform.DOMoveX(transform.position.x - 20f, 0.5f);
            CollectedCoffeeData.instance.CoffeeList.Remove(transform);
            Destroy(gameObject, 1f);
        }

        if (other.CompareTag("Finish"))
        {
            StartCoroutine(ConeSell());
        }

    }

    IEnumerator ConeSell()
    {
        yield return new WaitForSeconds(1.1f);
        SoundManager.instance.Play("FinishLine", true);
        if (transform.CompareTag("Cherry"))
        {
            UIManager.instance.UpdateScore(-10);
            UIManager.instance.UpdateEarnedMoney(+10);

        }

        if (transform.CompareTag("Chocolate"))
        {
            UIManager.instance.UpdateScore(-8);
            UIManager.instance.UpdateEarnedMoney(+8);

        }

        if (transform.CompareTag("White"))
        {
            UIManager.instance.UpdateScore(-6);
            UIManager.instance.UpdateEarnedMoney(+6);

        }

        if (transform.CompareTag("Pink"))
        {
            UIManager.instance.UpdateScore(-4);
            UIManager.instance.UpdateEarnedMoney(+4);

        }

        if (transform.CompareTag("CollectedCone"))
        {
            UIManager.instance.UpdateScore(-2);
            UIManager.instance.UpdateEarnedMoney(+2);
        }
        transform.DOMove(CollectedCoffeeData.instance.HandHoldTransform[CollectedCoffeeData.instance.finishCount].position, 0.3f);
        transform.parent = CollectedCoffeeData.instance.HandParent[CollectedCoffeeData.instance.finishCount];
        CollectedCoffeeData.instance.finishCount++;
        CollectedCoffeeData.instance.CoffeeList.Remove(transform);
    }
}
