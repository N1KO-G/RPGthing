using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Animations;
using System.Collections;

public class healthmanager : MonoBehaviour
{
    public Slider slider;
    public Slider slowhealthslider;
    public string scene;
    public float maxhealth = 100f;
    public float health;
    public float lerpspeed = 0.05f;
    Animator animator;
    public GameObject playergameobject;

    void Awake()
    {
        health = maxhealth;
        animator = playergameobject.GetComponent<Animator>();
    }

    void Update()
    { 

        if (slider.value != health)
        {
            slider.value = health;
        }

        if(slider.value != slowhealthslider.value)
        {
            slowhealthslider.value = Mathf.Lerp(slowhealthslider.value, health, lerpspeed);
        }
    }

   public  void TakeDamage(float amount)
    {
        health -= amount;
    }

    public IEnumerator death()
    {
        if (health <= 0)
        {
            animator.SetBool("isDead", true);
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene(scene);
        }
    }




  
}
