using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineController : MonoBehaviour
{
    Animator machineAnim;
    void Start()
    {
        machineAnim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            machineAnim.SetBool("Active", false);
        }
        
        else
        {
            machineAnim.SetBool("Active", true);
        }
    }

}
