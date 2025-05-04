using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class ActiveLazer : MonoBehaviour
{
    [SerializeField] private float defDistanceRay = 100;
    public Transform laserFirePoint;
    public LineRenderer m_lineRenderer;
    public PlayerHealth currentTarget;
    public float damagePerSecond = 20f; // Sát thương mỗi giây
    //public bool isLazering = false;
    //public float maxLazerTime = 5f; // Thời gian duy trì laser
    private Coroutine lazerCoroutine; // Lưu trạng thái coroutine
    public LayerMask targetLayer;
    public Transform huongMat;
    public Volume volume;
    public ImpactOnPlayer impact;
    public playerAvatar avatar;
    private void Awake()
    {
         impact = GetComponentInParent<ImpactOnPlayer>();
    }
    private void Start()
    {
        volume.enabled = false;
    }
    void Update()
    {
        if (impact.isUsingSkillCanMove)
        {
            ShootLaser();
        }
    }
    private void OnDisable()
    {
        StopLazer();
        //Debug.Log("ngungwban");
    }
    public void ToggleLazer(float durationTime)
    {
        if (impact.isUsingSkillCanMove)
        {
            StopLazer();
        }
        else
        {
            StartLazer(durationTime);
        }
    }

    void StartLazer(float durationTime)
    {
        //isLazering = true;
        m_lineRenderer.enabled = true;
        volume.enabled = true;
        impact.isUsingSkillCanMove = true;
        impact.SkillInUse.Add(1);
        lazerCoroutine = StartCoroutine(LazerDuration(durationTime));
        avatar.avatarList[avatar.index].GetComponent<Animator>().SetBool("isSkill2", true);

    }

    void StopLazer()
    {
        //isLazering = false;
        m_lineRenderer.enabled = false;
        if (lazerCoroutine != null)
        {
            StopCoroutine(lazerCoroutine);
            impact.isUsingSkillCanMove = false;
            impact.SkillInUse.Remove(1);
            volume.enabled = false;
            lazerCoroutine = null;
            currentTarget = null;
        }
        avatar.avatarList[avatar.index].GetComponent<Animator>().SetBool("isSkill2", false);
    }

    IEnumerator LazerDuration(float durationTime)
    {
        float elapsedTime = 0f;
        while (elapsedTime < durationTime && impact.isUsingSkillCanMove)
        {
            //if (currentTarget != null)
            //{
            //    currentTarget.takeDame(5);
            //}
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        StopLazer();
    }

    void ShootLaser()
    {
        Vector3 dir = (huongMat.transform.localScale.x > 0) ? transform.right : -transform.right;
        RaycastHit2D _hit = Physics2D.Raycast(laserFirePoint.position, dir, Mathf.Infinity, targetLayer);

        if (_hit)
        {

            Draw2DRay(laserFirePoint.position, _hit.point);
            //Debug.Log(_hit.collider);
            if (_hit.collider.transform?.parent?.GetComponentInParent<PlayerHealth>())
            {

                currentTarget = _hit.collider.transform.parent.GetComponentInParent<PlayerHealth>();
                currentTarget.GetComponent<PlayerHealth>().takeDame(0.5f);
            }
            else
            {
               currentTarget = null;
                    return;
            }

            //currentTarget= _hit.collider.gameObject;
        }
        else
        {
            Draw2DRay(laserFirePoint.position, laserFirePoint.transform.right * defDistanceRay);
            currentTarget=null;
        }
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);
        //Debug.Log("vex");
    }

}
