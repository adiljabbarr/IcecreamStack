using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using CASP.SoundManager;
public class PlayerController : MonoBehaviour
{
    public float Horizontal;
    [SerializeField] float VerticalSpeed;
    public float SpeedMultiplier;
    public List<Transform> Coffees;
    [SerializeField] float OffsetZ = 1;
    [SerializeField] float LerpSpeed = 1;
    [SerializeField] TMP_Text HandCoinText;
    [SerializeField] Transform IceCreamFirst;
    [SerializeField] Transform HoldTransform;
    [SerializeField] Transform MoneyHead;
    //[SerializeField] Vector3 offset1;
    // private float speed = 0.02f;
   private Touch touch;

    public bool isFinished = false;


    Sequence seq;
    void Start()
    {
        CollectedCoffeeData.instance.CoffeeList.Add(transform.GetChild(0));
        CollectedCoffeeData.instance.CoffeeList.Add(transform.GetChild(1));
    }

    void Update()
    {
        if (!isFinished)
        {
            Horizontal = Input.GetAxis("Horizontal");
            transform.position += new Vector3(Horizontal, 0, VerticalSpeed) * SpeedMultiplier * Time.deltaTime;


            transform.position += Vector3.forward * VerticalSpeed *SpeedMultiplier* Time.deltaTime;


        // if (Input.touchCount > 0)
        // {
        //     touch = Input.GetTouch(0);
        //     if (touch.phase == TouchPhase.Moved)
        //     {
        //         transform.position = new Vector3(transform.position.x + touch.deltaPosition.x * speed, transform.position.y, transform.position.z);
        //         transform.position = new Vector3(Mathf.Clamp(transform.position.x, -6f, 5.3f), transform.position.y, transform.position.z);

        //     }
        // }




        }
        else
        {
            Horizontal = 0;
            transform.position += new Vector3(Horizontal, 0, VerticalSpeed) * SpeedMultiplier * Time.deltaTime;

        }

        float clampLimit = Mathf.Clamp(transform.position.x, -10.5f, 11f);


        if (CollectedCoffeeData.instance.CoffeeList.Count > 1)
        {
            CoffeeFollow();
        }


        transform.position = new Vector3(clampLimit, transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HeadMoney"))
        {
            SoundManager.instance.Play("FinishWall",true);

            var seq = DOTween.Sequence();
            other.transform.SetParent(transform);
            seq.Append(transform.DOMoveY(UIManager.instance.EarnedMoney, 2f)).AppendInterval(1.5f).OnComplete(() =>
            {
                UIManager.instance.OpenWinPanel();
            });
        }

        if (other.CompareTag("Cone"))
        {
            SoundManager.instance.Play("Collect",true);

            UIManager.instance.UpdateScore(2);
            CollectedCoffeeData.instance.CoffeeList.Add(other.transform);

            if (CollectedCoffeeData.instance.CoffeeList.Count == 2)
            {
                CollectedCoffeeData.instance.CoffeeList[1].transform.parent = HoldTransform;
            }
            other.tag = "CollectedCone";
            other.gameObject.AddComponent<CollectedCoffee>();
            if (other.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true;
            }
            else
            {
                other.gameObject.AddComponent<Rigidbody>().isKinematic = true;

            }

            var seq = DOTween.Sequence();
            seq = DOTween.Sequence();
            seq.Kill();
            seq = DOTween.Sequence();
            for (int i = CollectedCoffeeData.instance.CoffeeList.Count - 1; i > 0; i--)
            {
                seq.WaitForCompletion(true);
                seq.Join(CollectedCoffeeData.instance.CoffeeList[i].DOScale(new Vector3(2.38f, 1.35f, 2.5f), 0.2f));
                seq.AppendInterval(0.05f);
                seq.Join(CollectedCoffeeData.instance.CoffeeList[i].DOScale(new Vector3(1.87f, 1.06f, 1.94f), 0.2f));
            }
        }
    }

    void CoffeeFollow()
    {

        for (int i = 1; i < CollectedCoffeeData.instance.CoffeeList.Count; i++)
        {
            Vector3 PrePos = CollectedCoffeeData.instance.CoffeeList[i - 1].transform.position + Vector3.forward * OffsetZ;
            Vector3 CurPos = CollectedCoffeeData.instance.CoffeeList[i].position;
            CollectedCoffeeData.instance.CoffeeList[i].transform.position = Vector3.Lerp(CurPos, PrePos, LerpSpeed * Time.deltaTime);
        }
    }
}
