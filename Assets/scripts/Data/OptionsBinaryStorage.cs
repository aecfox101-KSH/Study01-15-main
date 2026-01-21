using UnityEngine;
using System.IO;

public class OptionsBinaryStorage
{
    private string fileName = "options.dat";

    string GetFilePath()
    {
        // 파일을 저장할 최종 경로 생성 -> 내부 저장소의 경로 + 파일 이름.
        string path = Path.Combine(Application.persistentDataPath, fileName);
        Debug.Log("path =   " + path);
        return path;
    }

    public void Save(OptionData data)
    {
        string path = GetFilePath();

        // 쓰기용 파일을 생성.
        using(FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write)) // using 문법 규칙
        using(BinaryWriter writer = new BinaryWriter(stream)) // 파일에 이진 데이터를 쓰기.
        {
            writer.Write(data.mainVolume01);
            writer.Write(data.bgmVolume01);
            writer.Write(data.sfxVolume01);
        }

    }
    public OptionData Load()
    {
        string path = GetFilePath();

        if (File.Exists(path) == false)
        {
            return null;
        }

        // 읽기용 파일을 열기.
        using(FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
        using(BinaryReader reader = new BinaryReader(stream))
        {
            OptionData data = new OptionData();

            data.mainVolume01 = reader.ReadSingle(); // 실수형 데이터를 읽는다.
            data.bgmVolume01 = reader.ReadSingle();
            data.sfxVolume01 = reader.ReadSingle();

            return data;
        }
        
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
