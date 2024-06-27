using UnityEngine;

public class healthtest : MonoBehaviour
{

    public float health = 100f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    
        public void healthamount()
        {
        if (health >= 0)
        {
            Destroy(this.gameObject);
        }
        }

        public void takeDamage(float damage)
        {
            health -= damage;
        }
     
}
