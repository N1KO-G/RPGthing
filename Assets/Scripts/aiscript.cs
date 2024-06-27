
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class aiscript : MonoBehaviour
{

    public float wanderradius;
    public float wandertimer;
    NavMeshAgent navMeshAgent;
    public GameObject player;
    public Transform Target;
    public float amount;
    private float timer;
    healthmanager Healthmanager;
    public float attacktimer = 2f;
    public float attackcooldown = 2f;

    void Awake()
    {
        
        Healthmanager = FindAnyObjectByType<healthmanager>();
        player = GameObject.FindGameObjectWithTag("Player");
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        timer = wandertimer;
    }
    // Update is called once per frame
    void Update()
    {
        attacktimer -= Time.deltaTime;
        timer += Time.deltaTime;
        float distance = Vector3.Distance(player.transform.position, transform.position);
        bool IsplayerCloseEnough = distance <= amount;

        if (IsplayerCloseEnough)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }

            if(!IsplayerCloseEnough && timer >= wandertimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position,wanderradius, -1);
                navMeshAgent.SetDestination(newPos);
                timer = 0;
            }
    }


    public static Vector3 RandomNavSphere(Vector3 origin,float dist, int LayerMask)
    {
            Vector3 randdirection = (Random.insideUnitSphere * dist);

            randdirection += origin;

            NavMeshHit hit;

            NavMesh.SamplePosition(randdirection, out hit, dist, LayerMask);

            return hit.position;
    }

    void OnTriggerStay(Collider other)
    {
        if(GameObject.FindGameObjectWithTag("Player"))
        {
            if (attacktimer <= 0)
            {
            Healthmanager.TakeDamage(10);
            attacktimer = attackcooldown;
            }
        }
    }

    


}
