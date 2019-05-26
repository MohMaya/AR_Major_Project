using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPS : MonoBehaviour
{
    public static GPS Instance { set; get; }

    public double latitude;
    public double longitude;
    public double targetLat = 28.560060f;
    public double targetLong = 77.2793045f;
    //public Text coordinates;
    public String message = "";
    public int flag = 0;
    public int events_num = 0;
    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;
    public GameObject obj4;
    public GameObject item;

    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(StartLocationService());

        events_num += 1;
        Debug.Log("Redeployment No. : " + events_num.ToString());
        if (events_num == 3)
        {
            //Testing Data
            //flag = 1;
            //latitude = 28.560060f;
            //longitude = 77.2793045f;
            //Testing Data

            obj1.SetActive(false);
            obj2.SetActive(false);
            obj3.SetActive(false);
            obj4.SetActive(false);
            System.Random random = new System.Random();
            GameObject[] arr = new GameObject[4] { obj1, obj2, obj3, obj4 };
            int item_idx = random.Next(0, 4);
            item = arr[item_idx];
            Debug.Log("Item  = " + item.name);
        }
    }

    private void Update()
    {
        if (flag == 1)
        {
            //UpdateCoords();

            double distance = Distance();
            if (distance < 0.0001)
            {
                //Display the AD
                item.SetActive(true);
                String tempstr = "Current -> LAT : " + latitude.ToString() + "\tLONG : " + longitude.ToString();
                tempstr += "\nTarget -> LAT : " + targetLat.ToString() + "\tLONG : " + targetLong.ToString();
                tempstr += "\nDistance : " + Distance().ToString();
                //Debug.Log(tempstr+"\nDISPLAYED");
            }
            else
            {
                //Hide the AD
                item.SetActive(false);
                String tempstr = "Current -> LAT : " + latitude.ToString() + "\tLONG : " + longitude.ToString();
                tempstr += "\nTarget -> LAT : " + targetLat.ToString() + "\tLONG : " + targetLong.ToString();
                tempstr += "\nDistance : " + Distance().ToString();
                //Debug.Log(tempstr + "\nHIDDEN");
            }
        }
    }

    private void UpdateCoords()
    {
        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
    }

    private IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            //User has not enabled input location
            Debug.Log("User has not enabled GPS");
            message = "ERR01:\tUser has not enabled GPS\nHIDDEN";
            item.SetActive(false);
            yield break;//Since in midst of a coroutine
        }

        Input.location.Start();
        int maxWait=20;
        while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            message = "ERR02:\tWaiting ......\nHIDDEN";
            item.SetActive(false);
            Debug.Log("Waiting ......");
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Location Service Failed");
            message = "ERR04:\tLocation Service Failed\nHIDDEN";
            item.SetActive(false);
            yield break;
        }


        if (maxWait <= 0)
        {
            Debug.Log("Timed Out");
            message = "ERR03:\tTimed Out\nHIDDEN";
            item.SetActive(false);
            yield break;
        }



        latitude = Input.location.lastData.latitude;
        longitude = Input.location.lastData.longitude;
        flag = 1;


    }

    private double Distance()
    {
        double x_dist = latitude - targetLat;
        x_dist = x_dist * x_dist;

        double y_dist = longitude - targetLong;
        y_dist = y_dist * y_dist;

        return Math.Sqrt(x_dist+y_dist);
    }


}
