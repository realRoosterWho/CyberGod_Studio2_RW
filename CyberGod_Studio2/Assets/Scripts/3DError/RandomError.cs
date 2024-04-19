using UnityEngine;
using System.Collections;
using UnityEditor.Rendering.PostProcessing;
using System.Net.NetworkInformation;

public class RandomError : MonoBehaviour
{
    Ray ray; // random ray to hit neko generating the position of error
    public GameObject prefabError;// prefab of error
    public GameObject neko; // neko
    private Vector3 nekoCenter; // store neko's center
    private Vector3 origin;// ray's origin
    private Vector3 end;// ray's end

    private void RandomRay()
    {
        // ray's origin box
        nekoCenter = neko.transform.position;// get neko's center
        float centerX = nekoCenter.x;
        float centerY = nekoCenter.y;
        float centerZ = nekoCenter.z;
        int a = 2;//box's length

        // random origin of the ray
        int ranx = Random.Range((int)nekoCenter.x - a, (int)nekoCenter.x + a);
        int rany = Random.Range((int)nekoCenter.y - a, (int)nekoCenter.y + a);
        int ranz = Random.Range((int)nekoCenter.z - a, (int)nekoCenter.z + a);

        // inite the ray
        ray = new Ray();

        // calculate its end and origin
        origin = new Vector3(ranx, rany, ranz);
        end = nekoCenter;

        // inite its end and origin
        ray.origin = origin;
        ray.direction = end - origin;
    }
    void Start()
    {
        
    }
    void Update()
    {
        // show the hitting point when clicked
        if (Input.GetMouseButtonDown(0))
        {
            // create random ray
            RandomRay();
            // draw the random ray
            Debug.DrawRay(ray.origin, (end - origin), Color.red);
            // inite the hit
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                // draw the ray
                Debug.DrawLine(ray.origin, hit.point);

                //create a son object error from prefab
                GameObject error = Instantiate(prefabError, transform);  
                error.transform.localScale /= 2; // set the scale of error

                // get the hitting imformation
                error.transform.position = hit.point;
                error.transform.up = hit.normal;

                // settle the error sphere at the surface of neko
                error.transform.Translate(Vector3.up * 0.5f * error.transform.localScale.y, Space.Self);
            }
        }
    }

}

