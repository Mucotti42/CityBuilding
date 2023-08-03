using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using UnityEngine;
using Random = UnityEngine.Random;

public class FirebaseProvider : MonoBehaviour
{
    private DatabaseReference databaseReference;
    [SerializeField] private string UserId;
    public string socialId;
    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        //SetBuilding(new MapData("testBuilding2", BuildingType.Market1.ToString(),
            //new Vector2Int(5,5),4,DateTime.Now,2));
        //SetBuildingData("testBuilding","state",3);

        //GetBuildingsData(Convert);
        CheckUserExists();
    }

    private async void CheckUserExists()
    {
        DataSnapshot snapshot = await databaseReference.Child("Users/" + UserId +"/GeneralData").GetValueAsync();

        if (snapshot is { Exists: true })
        {
            string jsonData = snapshot.GetRawJsonValue();
            Debug.Log(jsonData);
            GeneralData generalData = JsonUtility.FromJson<GeneralData>(jsonData);
            socialId = generalData.socialId;
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                string socialId = Random.Range(100000, 999999).ToString();
                DataSnapshot snapshot2 = await databaseReference.Child("Social/" + socialId).GetValueAsync();

                if (snapshot2 is { Exists: true })
                    continue;
                
                SetUserData("/GeneralData",new GeneralData(100,0,1,"Eskil",socialId,new List<string>()));
                SetData("Social/" + socialId, UserId);
            }
        }
    }
    public void Convert(DataSnapshot snapshot)
    {
        List<BuildingData> buildingsDataList = new List<BuildingData>();
        foreach (var childSnapshot in snapshot.Children)
        {
            string jsonData = childSnapshot.GetRawJsonValue();
            Debug.Log(jsonData);
            BuildingData buildingData = JsonUtility.FromJson<BuildingData>(jsonData);
            buildingsDataList.Add(buildingData);
        }

        Debug.LogWarning(buildingsDataList[0].buildingId);
        Debug.LogWarning(buildingsDataList[1].endTime);
    }

    public void SetData(string path, object data)
    {
        string jsonData = JsonUtility.ToJson(data);
        databaseReference.Child(path).SetRawJsonValueAsync(jsonData);
    }
    private async void GetData(Action<object> callback, string path)
    {
        DataSnapshot snapshot = await databaseReference.Child(path).GetValueAsync();

        if (snapshot is { Exists: true })
        {
            callback?.Invoke(snapshot.Value);
        }
        else
        {
            Debug.Log("Error: No data at: " + path + " path.");
        }
    }

    private void SetUserData(string path,object data)
    {
        string previousPath = "Users/" + UserId + "/";
        SetData(previousPath + path,data);
    }

    private void GetUserData(Action<object> callback, string path)
    {
        string previousPath = "Users/" + UserId + "/";
        GetData(callback ,previousPath + path);
    }
    
    public void SetGeneralData(Utils.GeneralDatas dataType, object data)
    {
        SetUserData("GeneralData/" + dataType.ToString(), data);
    }

    public void GetGeneralData()
    {
        
    }

    public async Task GetBuildingsData(Action<DataSnapshot> callback, string _userId = null)
    {
        var id = UserId;
            
        if (_userId != null)
            id = _userId;
        
        DataSnapshot snapshot = await databaseReference.Child("Users").Child(id).Child("Buildings").GetValueAsync();
        if (snapshot != null)
        {
            callback?.Invoke(snapshot);
        }
    }
    
    public void SetBuilding(BuildingData value)
    {
        string buildingId = value.buildingId;
        string jsonData = JsonUtility.ToJson(value);
        databaseReference.Child("Users/" + UserId + "/Buildings/" + buildingId).SetRawJsonValueAsync(jsonData);
    }

    public void UpdateBuildingData(string buildingId, string data, object value)
    {
        var updates = new Dictionary<string, object>
        {
            {"Users/" + UserId +"/Buildings/" + buildingId + "/" + data.ToString(), value}
        };
        
        databaseReference.UpdateChildrenAsync(updates);
    }
}