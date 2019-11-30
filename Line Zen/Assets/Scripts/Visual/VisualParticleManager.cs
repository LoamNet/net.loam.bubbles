using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualParticleManager : MonoBehaviour
{
    public Data data;
    public Events events;
    public ParticleSystem bubbleExplosionTemplate;
    public List<ParticleSystem> spawnOnStart;

    private List<ParticleSystem> spawnedOnStart;
    private List<ParticleSystem> bubbleExplosionParticles;

    // Start is called before the first frame update
    void Start()
    {
        spawnedOnStart = new List<ParticleSystem>();
        bubbleExplosionParticles = new List<ParticleSystem>();

        events.OnShowParticlesToggle += (isOn) => { HandleStartupSystems(isOn); };
        events.OnGameInitialized += () => { HandleStartupSystems(data.GetDataGeneral().displayParticles); };
    }


    void HandleStartupSystems(bool isDoingSpawn)
    {
        foreach (ParticleSystem sys in spawnedOnStart)
        {
            Destroy(sys.gameObject);
        }

        spawnedOnStart.Clear();
        if (isDoingSpawn)
        {
            foreach (ParticleSystem sys in spawnOnStart)
            {
                ParticleSystem ps = Instantiate(sys, this.gameObject.transform);
                spawnedOnStart.Add(ps);
            }
        }
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
        if (data.GetDataGeneral().displayParticles)
        {
            ParticleSystem obj = Instantiate(bubbleExplosionTemplate, this.gameObject.transform);
            obj.transform.position = location;

            bubbleExplosionParticles.Add(obj);
        }
    }
}
