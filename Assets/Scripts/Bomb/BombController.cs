﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public bool isOn=false;
    //public bool isExplose=false;

    public Animator ani;

    public float currentTime;
    public float startCountdowntime = 10;

    //public BombSpawner BombSpawner;
    public Rigidbody2D rb;

    public TextMeshProUGUI BombTimeText;
    public Coroutine countdownCoroutine;

    public List<GameObject> BombEffList;
    public int bombType;

    public SpriteRenderer bombColor;
    private Color originalColor;

    #region
    public int bombLife = 3;

    #endregion
    private void Awake()
    {
        if (ani == null) ani = GetComponent<Animator>();
        //if (BombSpawner == null) BombSpawner =FindObjectOfType<BombSpawner>();
        if (rb == null) rb = transform.parent.GetComponent<Rigidbody2D>();
        bombColor = this.GetComponent<SpriteRenderer>();
        if (bombColor != null)
        {
            originalColor = bombColor.material.color; // Lưu lại màu gốc
        }
    }
    
    private void OnEnable()
    {
        //khởi tạo bomb
        isOn = false;
        //isExplose = false;
        bombLife = 3;
        bombType = RandomEff();
        if (bombType + 1 < 3)
        {
            BombTimeText.gameObject.SetActive(true);
            currentTime = startCountdowntime;
            BombTimeText.text = currentTime.ToString();//type 1,2
        }
        else if (bombType + 1 == 3)
        {
            BombTimeText.gameObject.SetActive(false);
        }
        //đặt lại bomb để va chạm và khóa trục z
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        this.transform.parent.GetComponent<Collider2D>().isTrigger = false;

        //StartCoroutine(CountdownAndExplode());
        //isOn = true;
    } 
    public int RandomEff()
    {
        int ran = Random.Range(0, 3);
        if (ran == 1)
        {
            ChangeToColor(Color.Lerp(Color.blue, Color.white, 0.7f));
        }
        else if(ran == 2)
        {
            ChangeToColor(Color.Lerp(Color.red, Color.white, 0.7f));
        }
        else ResetColor();
        //Debug.Log(ran);
        return ran;
    }
    private void Update()
    {
        BombTimeText.text = currentTime.ToString();//BomebTime
        ani.SetBool("isOn", isOn);

        if (bombType+1 != 3) return;//type 3
        TriggerExplode();
    }

    public void ChangeToColor(Color newColor)
    {
        if (bombColor != null)
        {
            bombColor.material.color = newColor; // Thay đổi màu sắc
        }
    }
    public void ResetColor()
    {
        if (bombColor != null)
        {
            bombColor.material.color = originalColor; // Đặt lại màu gốc
        }
    }
    public IEnumerator CountdownAndExplode()//type 1,2
    {
        while (currentTime > 0)
        {
            if (currentTime==0)
            {
                break;
            }
            yield return new WaitForSeconds(1f);
            currentTime--;
        }

        Explose();
    }
    public void TriggerExplode()//type 3
    {
        if (bombLife <= 0 && isOn)
        {
            //isExplose = true;
            Explose();
        }
    }
    public void Explose()
    {
        Vector3 newposforExplose = new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z);
        Instantiate(BombEffList[bombType], newposforExplose, transform.parent.rotation);
        despawnBomb();
    }
    public void despawnBomb()
    {
        // Đảm bảo đối tượng trở về Object Pool
        if (this.transform.parent.parent == null)
        {
            if (BombSpawner.instance.PoolingObj != null)
            {
                Debug.Log("Đảm bảo đối tượng trở về Object Pool");
                this.transform.parent.parent = BombSpawner.instance.PoolingObj.transform;
            }
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        this.transform.parent.gameObject.SetActive(false);
    }

}
