using UnityEngine;
using UnityEngine.UI;

public class MiniMission : MonoBehaviour
{
    public Button button;

    [SerializeField]
    int countRespTrain;
    [SerializeField]
    int countTrainDelivered;

    private void Start()
    {
        button.onClick.AddListener(NextLevel);

        countRespTrain = 1;
    }

    private void Update()
    {
        
    }


    void NextLevel()
    {
        button.onClick.RemoveAllListeners();
        button.gameObject.SetActive(false);

        RespawnMode rm = GetComponent<RespawnMode>();

        for (int i = 0; i < countRespTrain; i++)
        {
            rm.RandomRespawn();   
        }
    }


    public void TrainOnStation()
    {
        countTrainDelivered++;
        
        if(countTrainDelivered==countRespTrain)
        {
            countTrainDelivered = 0;

            if (countRespTrain < 4)
            {
                countRespTrain++;
            }

            button.gameObject.SetActive(true);
            button.onClick.AddListener(NextLevel);
        }
    }
}