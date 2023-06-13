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
            GameObject.Find("Halo").transform.parent.gameObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            Destroy(GameObject.Find("Halo"));
        }
        if (HighlightedGamePlayer != null)
        {
            GameObject player = GameObject.Find(HighlightedGamePlayer);

            GameObject haloObject = new GameObject("Halo");

            haloObject.transform.parent = player.transform;

            haloObject.transform.localPosition = new Vector3(0, 1.25f, 0);
            haloObject.transform.rotation = Quaternion.Euler(90, 0, 0);

            haloObject.AddComponent<Light>();
            Light haloLight = haloObject.GetComponent<Light>();

            haloLight.type = LightType.Point;
            haloLight.range = 0.8f;
            haloLight.intensity = 300;
            haloLight.color = new Color(255, 212, 0, 255);
            haloLight.renderMode = LightRenderMode.ForcePixel;

            haloObject.AddComponent<MeshRenderer>().material = Resources.Load<Material>("Material/Highlight");
            // haloObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Material/Highlight");
            GameObject temp_capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            Mesh capsuleMesh = temp_capsule.GetComponent<MeshFilter>().mesh;
            Destroy(temp_capsule);
            haloObject.AddComponent<MeshFilter>().mesh = capsuleMesh;
            haloObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            haloObject.transform.localScale = new Vector3(50f, 100f, 50f);
            haloObject.transform.localPosition = new Vector3(0, 0.5f, 0);

            player.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            // haloObject.GetComponent<MeshFilter>().mesh

            // haloObject.AddComponent<Halo>();

            // Behaviour halo = (Behaviour)haloLight.GetComponent("Halo");
            // halo.enabled = true;
        }
    }
}
