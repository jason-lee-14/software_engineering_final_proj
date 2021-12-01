using UnityEngine;

public class LightTrigger : MonoBehaviour
{
    //light reference
    private Light cam_light;

    //getter methods for acceptance testing
    public Light GetLight { get => cam_light; }
    //once you get into the main wall's area where you need the flash light it will turn on
    public void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.CompareTag("MainCamera"))
       {
            Camera.main.gameObject.AddComponent<Light>();
            cam_light = Camera.main.gameObject.GetComponent<Light>();
            cam_light.type = UnityEngine.LightType.Spot;
            cam_light.intensity = 1.5f;
            cam_light.range = 5;
       }
    }
    //if you leave the main wlal area turn off flashlight
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
            GameObject.Destroy(cam_light);
    }
}
