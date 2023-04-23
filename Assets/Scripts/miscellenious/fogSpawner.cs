using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fogSpawner : MonoBehaviour
{
    List<GameObject> Clouds = new List<GameObject>();
    public bool isFog;//if its a cloud or fog
    private Color32 colour;
    public GameObject[] CloudPrefabs;
    public GameObject[] PossibleStartCloudsPos;
    public Transform StartCloudPos;
    public Transform EndCloudPos;
    public Transform EndScreen;

    void Start()
    {
        switch(SaveManager.instance.activeSave.XTREME, isFog)
        {
            case (true, false):
                colour = new Color32(69, 0, 110, 255);
                break;
            case (false, false):
                colour = new Color32(255, 255, 255, 255);
                break;
            default:
                colour = new Color32(239, 159, 141, 61);
                break;
        }


        
        foreach(GameObject p in PossibleStartCloudsPos)
        {
            GameObject cloud = 
            Instantiate(CloudPrefabs[
                (int) Random.Range(0, CloudPrefabs.Length)]
                , p.transform.position + new Vector3(0, Random.Range(-1.2f, 1.2f), 0), p.transform.rotation);

            cloud.GetComponent<SpriteRenderer>().color = colour;
            Clouds.Add(cloud);
        }
        
    }

    void Update()
    {
        for(int i = 0; i < Clouds.Count; i++)
        {
            if(Clouds[i].transform.position.x < StartCloudPos.position.x || Clouds[i].transform.position.x > EndCloudPos.position.x)
            {
                Destroy(Clouds[i]);
                Clouds.RemoveAt(i);
            }
        }



        if(Clouds[0].transform.position.x - 5f >= StartCloudPos.position.x)
        {
            GameObject cloud = 
            Instantiate(CloudPrefabs[
                (int) Random.Range(0, CloudPrefabs.Length)],
                StartCloudPos.transform.position + new Vector3 (0, Random.Range(-1.2f, 1.2f), 0), 
                StartCloudPos.transform.rotation);
            cloud.GetComponent<SpriteRenderer>().color = colour;
            Clouds.Insert(0, cloud);
        } 
        
        if(Clouds[Clouds.Count - 1].transform.position.x + 5f <= EndCloudPos.position.x)
        {
            GameObject cloud = 
            Instantiate(CloudPrefabs[
                (int) Random.Range(0, CloudPrefabs.Length)],
                EndCloudPos.transform.position + new Vector3 (0, Random.Range(-1.2f, 1.2f), 0), 
                EndCloudPos.transform.rotation);
            cloud.GetComponent<SpriteRenderer>().color = colour;

            Clouds.Insert(Clouds.Count, cloud);
        }
    }

    
}