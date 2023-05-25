using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldHaloEffect : MonoBehaviour
{
    // public static string HighlightedGamePlayer;

    public static void createHightedPlayer(string HighlightedGamePlayer)
    {
        if (GameObject.Find("Halo") != null)
        {
            Destroy(GameObject.Find("Halo"));
        }
        if (HighlightedGamePlayer != null)
        {
            GameObject player = GameObject.Find(HighlightedGamePlayer);
            GameObject haloObject = new GameObject("Halo");

            haloObject.transform.parent = player.transform;

            haloObject.transform.localPosition = new Vector3(0, 0.3f, 0);
            haloObject.transform.rotation = Quaternion.Euler(90, 0, 0);

            haloObject.AddComponent<Light>();
            Light haloLight = haloObject.GetComponent<Light>();

            haloLight.type = LightType.Point;
            haloLight.range = 0.6f;
            haloLight.color = new Color(255, 212, 0, 255);
            haloLight.renderMode = LightRenderMode.ForcePixel;

            // Behaviour halo = (Behaviour)haloLight.GetComponent("Halo");
            // halo.enabled = true;
        }
    }
}
