using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int cherries = 0;
    [SerializeField] private TextMeshProUGUI cherriesText;
    
    [SerializeField] private AudioSource collectionSoundEffect; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cherry"))
        {
            collectionSoundEffect.Play();
            cherries++;
                cherriesText.text = $"Cherries : {cherries}";
        }
    }
    
}
