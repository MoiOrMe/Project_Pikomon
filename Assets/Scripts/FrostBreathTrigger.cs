using UnityEngine;

public class FrostBreathTrigger : MonoBehaviour
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

    private void OnTriggerEnter(Collider other)
    {
        // Tu peux ajouter une condition ici, par exemple pour n'activer que si le joueur entre
        if (particleSystems != null)
        {
            foreach (var ps in particleSystems)
            {
                ps.gameObject.SetActive(true); // active le GameObject
                ps.Play(); // joue le système de particules
            }
        }
    }
}
