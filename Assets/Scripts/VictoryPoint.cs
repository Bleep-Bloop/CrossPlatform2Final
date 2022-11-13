using UnityEngine.SceneManagement;
using UnityEngine;

public class VictoryPoint : MonoBehaviour
{

   

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("entered");
        if(other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene("Victory");
        }
    }

}
