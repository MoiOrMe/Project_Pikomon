using UnityEngine;

public class FrostBreathOnClick : MonoBehaviour
{
    public GameObject frostBreathContainer;
    private ParticleSystem[] particleSystems;

    void Start()
    {
        if (frostBreathContainer != null)
        {
            particleSystems = frostBreathContainer.GetComponentsInChildren<ParticleSystem>(true);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PlayParticles();
        }

        void PlayParticles()
        {
            foreach (var ps in particleSystems)
            {
                ps.gameObject.SetActive(true);
                ps.Play();
            }
        }
    }
}
