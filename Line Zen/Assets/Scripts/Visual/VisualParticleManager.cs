using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualParticleManager : MonoBehaviour
{
    public ParticleSystem bubbleExplosionTemplate;

    private List<ParticleSystem> bubbleExplosionParticles;

    // Start is called before the first frame update
    void Start()
    {
        bubbleExplosionParticles = new List<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = bubbleExplosionParticles.Count - 1; i >= 0; --i)
        {
            if(!bubbleExplosionParticles[i].IsAlive())
            {
                Destroy(bubbleExplosionParticles[i].gameObject);
                bubbleExplosionParticles.RemoveAt(i);
            }
        }
    }

    public void CreateBubbleExplosion(DataPoint location)
    {
        ParticleSystem obj = Instantiate(bubbleExplosionTemplate, this.gameObject.transform);
        obj.transform.position = location;

        bubbleExplosionParticles.Add(obj);
    }
}
