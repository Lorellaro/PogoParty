// reading and writing files
using System.IO;
using UnityEngine;
// binary formatter
using System.Runtime.Serialization.Formatters.Binary;

// https://www.youtube.com/watch?v=XOjd_qU2Ido&t=325s Save system 
public static class SaveSystem
{
    public static void SavePlayer(PogoStickPhysics player){
        BinaryFormatter formatter = new BinaryFormatter();
        // make save consistent across each platform
        string path = Application.persistentDataPath + "/player.data";
        // write data to a new save file
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData(player);
        float x, y, z;
        x = player.transform.position.x;
        y = player.transform.position.y;
        z = player.transform.position.z;
        if (x != 0 && y != 0 && z != 0)
        {
            data.position[0] = x;
            data.position[1] = y;
            data.position[2] = z;
        }
        // write data to binary
        formatter.Serialize(stream, data);
        // close the stream to prevent unwanted errors
        stream.Close();
    }
    public static void SavePlayer(PogoStickPhysics player, float x, float y, float z){
        BinaryFormatter formatter = new BinaryFormatter();
        // make save consistent across each platform
        string path = Application.persistentDataPath + "/player.data";
        // write data to a new save file
        FileStream stream = new FileStream(path, FileMode.Create);
        PlayerData data = new PlayerData(player);
        // float x, y, z;
        // x = player.transform.position.x;
        // y = player.transform.position.y;
        // z = player.transform.position.z;
        if (x != 0 && y != 0 && z != 0)
        {
            data.position[0] = x;
            data.position[1] = y;
            data.position[2] = z;
        }
        // write data to binary
        formatter.Serialize(stream, data);
        // close the stream to prevent unwanted errors
        stream.Close();
    }

    public static PlayerData LoadPlayer(){
        // using same path as saved to
        /*
        string path = Application.persistentDataPath + "/player.data";
        // file found
        if(File.Exists(path)){
            // read data from file
            BinaryFormatter formatter = new BinaryFormatter();
            // read data from path to file and open
            FileStream stream = new FileStream(path, FileMode.Open);
            // set data from stream to the type PlayerData 
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            // close the stream to prevent unwanted errors
            stream.Close();
            return data;
        }
        else{
            // no file found
            Debug.LogError("Save file not found in " + path);
            return null;
        }*/
        return null;
    }
}
