using UnityEngine;
using System.IO; // Json 사용하기 위해 써야함.

/// <summary>
/// JSON 파일로 저장/ 로드
/// </summary>
public class OptionJsonStorage
{
    private string fileName = "option.json";

    string GetFilePath()
    {
        // 파일을 저장할 최종 경로 생성 -> 내부 저장소의 경로 + 파일 이름.
        string path = Path.Combine(Application.persistentDataPath, fileName); // path ?
        Debug.Log("path =   " +  path);
        return path;
    }

    public void Save(OptionData data)
    {
        string path = GetFilePath();
        string json =JsonUtility.ToJson(data); // OptionData를 Json 형태로 변환.

        File.WriteAllText(path, json);
    }

    public OptionData Load()
    {
        string path = GetFilePath() ;

        if(File.Exists(path) == false)
        {
            return null;
        }

        string json = File .ReadAllText(path);

        // string.IsNullOrEmpty -> 문자열이 null 문자열이거나 비어있는 문자열인지 체크.
        if (string.IsNullOrEmpty(json) == true)
        {
            return null;
        }

        OptionData data = JsonUtility.FromJson<OptionData>(json);
        return data;
    }

    public void Delete()
    {
        string path = GetFilePath();
        if (File.Exists(path) == false)
        {
            File.Delete(path);
        }
    }

}
