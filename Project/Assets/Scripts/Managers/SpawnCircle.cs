using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCircle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float radius = 1;

        for (int i = 0; i < 4; i++)
        {
            float angle = i * Mathf.PI * 2f;
            Vector3 checkedPosition = new Vector3(Mathf.Cos(angle) * radius, transform.position.y, Mathf.Sin(angle) * radius);
            checkedPosition = transform.position + checkedPosition;
            Debug.Log(checkedPosition);

            if(checkedPosition != Vector3.zero)
            {
               
                ////GameObject go = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), checkedPosition, Quaternion.identity);
                //if (checkedPosition == Vector3.zero)
                //    Destroy(go);
            }
                   
        }
        

       
    }

    
}
