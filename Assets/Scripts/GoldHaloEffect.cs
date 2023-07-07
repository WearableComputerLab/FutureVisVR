using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldHaloEffect : MonoBehaviour
{
    // public static string HighlightedGamePlayer;
    private static string previousObserver = "RightPlayer0";

    public static void createHighlightedPlayer(string HighlightedGamePlayer)
    {
        if (GameObject.Find("Halo") != null)
        {
            // GameObject.Find("Halo").transform.parent.gameObject.transform.localScale = new Vector3(1f, 2f, 1f);
            GameObject.Find("mHalo").transform.parent.gameObject.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            Destroy(GameObject.Find("Halo"));
            Destroy(GameObject.Find("mHalo"));
        }
        if (HighlightedGamePlayer != null)
        {
            GameObject player = GameObject.Find(HighlightedGamePlayer);
            GameObject mPlayer = GameObject.Find("m" + HighlightedGamePlayer);

            GameObject haloObject = new GameObject("Halo");
            GameObject mHaloObject = new GameObject("mHalo");

            haloObject.transform.parent = player.transform;
            mHaloObject.transform.parent = mPlayer.transform;

            // haloObject.transform.localPosition = new Vector3(0, 1.25f, 0);
            haloObject.transform.rotation = Quaternion.Euler(90, 0, 0);
            mHaloObject.transform.localPosition = new Vector3(0, 0.125f, 0);
            mHaloObject.transform.rotation = Quaternion.Euler(90, 0, 0);

            // haloObject.AddComponent<Light>();
            // Light haloLight = haloObject.GetComponent<Light>();
            haloObject.AddComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off; //Remove shadow of highlighted player

            mHaloObject.AddComponent<Light>();
            Light mHaloLight = mHaloObject.GetComponent<Light>();

            // haloLight.type = LightType.Point;
            // haloLight.range = 0.8f;
            // haloLight.intensity = 300;
            // haloLight.color = new Color(255, 212, 0, 255);
            // haloLight.renderMode = LightRenderMode.ForcePixel;
            mHaloLight.type = LightType.Point;
            mHaloLight.range = 0.008f;
            mHaloLight.intensity = 300;
            mHaloLight.color = new Color(255, 212, 0, 255);
            mHaloLight.renderMode = LightRenderMode.ForcePixel;

            haloObject.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Material/Highlight");
            GameObject temp_capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            Mesh capsuleMesh = temp_capsule.GetComponent<MeshFilter>().mesh;
            Destroy(temp_capsule);
            haloObject.AddComponent<MeshFilter>().mesh = capsuleMesh;
            haloObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            // haloObject.transform.localScale = new Vector3(100f, 250f, 100f);
            haloObject.transform.localScale = new Vector3(1.1f, 1f, 1.1f);
            haloObject.transform.localPosition = new Vector3(0, 0f, 0);
            mHaloObject.AddComponent<MeshRenderer>().material = Resources.Load<Material>("Material/Highlight");
            GameObject mTemp_capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            Mesh mCapsuleMesh = mTemp_capsule.GetComponent<MeshFilter>().mesh;
            Destroy(mTemp_capsule);
            mHaloObject.AddComponent<MeshFilter>().mesh = mCapsuleMesh;
            mHaloObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            mHaloObject.transform.localScale = new Vector3(100f, 100f, 100f);
            mHaloObject.transform.localPosition = new Vector3(0, 0.5f, 0);

            // player.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            mPlayer.transform.localScale = new Vector3(0.0001f, 0.0001f, 0.0001f);
        }
    }

    public static void createHighlightedObserver(string observer)
    {
        GameObject mPlayer = GameObject.Find("m" + observer);

        if (GameObject.Find("mObserver") != null)
        {
            Destroy(GameObject.Find("mObserver"));
        }

        GameObject mObserver = new GameObject("mObserver");
        mObserver.transform.parent = mPlayer.transform;
        mObserver.transform.localPosition = new Vector3(mPlayer.transform.localPosition.x, 0.4f, mPlayer.transform.localPosition.z);
        Light mObserverLight = mObserver.AddComponent<Light>();
        mObserverLight.type = LightType.Point;
        mObserverLight.range = 0.01f;
        mObserverLight.intensity = 300;
        mObserverLight.color = new Color(255, 212, 0, 255);
        mObserverLight.renderMode = LightRenderMode.ForcePixel;

    }

    public static void hideObserverPlayerObject(string observer)
    {
        // for (int i = 0; i < 11; i++)
        // {
        //     GameObject.Find("RightPlayer" + i).transform.localScale = new Vector3(0.65f, 2.2f, 0.65f);
        //     GameObject.Find("LeftPlayer" + i).transform.localScale = new Vector3(0.65f, 2.2f, 0.65f);
        // }


        // GameObject.Find(previousObserver).transform.localScale = new Vector3(0.65f, 2.2f, 0.65f);
        GameObject.Find(observer).transform.localScale = new Vector3(0.001f, 0.001f, 0.001f);
        // previousObserver = observer;
    }
}
