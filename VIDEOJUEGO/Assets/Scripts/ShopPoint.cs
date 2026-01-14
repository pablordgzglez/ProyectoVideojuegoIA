using UnityEngine;

public class ShopPoint : MonoBehaviour
{
    [Header("Open")]
    public KeyCode openKey = KeyCode.E;

    [Header("Reference")]
    public ShopUI shopUI; 

    private bool inRange;
    private Player player;
    private Shooter shooter;
    private FirstPersonController fpc;

    void Update()
    {
        if (!inRange || shopUI == null) return;

        
        if (Input.GetKeyDown(openKey) && !shopUI.IsOpen())
            shopUI.Open(player, shooter, fpc);

    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        inRange = true;
        player = other.GetComponent<Player>();
        shooter = other.GetComponent<Shooter>();
        fpc = other.GetComponent<FirstPersonController>();
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        inRange = false;

        if (shopUI != null && shopUI.IsOpen())
            shopUI.Close();

        player = null;
        shooter = null;
        fpc = null;
    }
}
