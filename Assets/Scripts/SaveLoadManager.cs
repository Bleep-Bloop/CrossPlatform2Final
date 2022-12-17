using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{

    [SerializeField]static byte[] key = Aes.Create().Key;
    [SerializeField]static byte[] aesIV = Aes.Create().IV; // What is this IV?
    ICryptoTransform encryptor = Aes.Create().CreateEncryptor(key, aesIV);
    ICryptoTransform decryptor = Aes.Create().CreateDecryptor(key, aesIV);

    // Save game data
    [XmlRoot("GameData")]
    public class GameStateData
    {

        public struct DataTransform
        {
            public float posX;
            public float posY;
            public float posZ;
            public float rotX;
            public float rotY;
            public float rotZ;
            public float scaleX;
            public float scaleY;
            public float scaleZ;

        }

        // Data for checkpoints
        public class DataCheckpoint
        {
            // Checkpoint transform location
            public DataTransform posRotScale;

            public int checkpointID;

            public bool isActive;

            public int unityInstanceID;
        }

        public class DataPlayer
        {
            public DataTransform posRotScale;

            public DataTransform activeCheckpointTransform;

            public int lives;

            public int health;
 
        }

        // Instance variables
        public List<DataCheckpoint> levelCheckpoints = new List<DataCheckpoint>();

        public DataPlayer player = new DataPlayer();
    }

    // Game data to save/load
    public GameStateData gameState = new GameStateData();

    public void Save(string fileName = "GameData.xml")
    {

        // Save game data
        XmlSerializer serializer = new XmlSerializer(typeof(GameStateData));
        FileStream stream = new FileStream(fileName, FileMode.Create);

        //ICryptoTransform encryptor = Aes.Create().CreateEncryptor(key, aesIV);

        // Encrypt
        //CryptoStream cryptoStream = new CryptoStream(stream, Aes.Create().CreateEncryptor(), CryptoStreamMode.Write);
        CryptoStream cryptoStream = new CryptoStream(stream, encryptor, CryptoStreamMode.Write); // ToDo move to an encrypt function

        serializer.Serialize(cryptoStream, gameState);
        // serializer.Serialize(stream, gameState);  unencrpyted
        stream.Flush();
        stream.Dispose();
        stream.Close();

    }




    public void Load(string fileName = "GameData.xml")
    {
        XmlSerializer serializer = new XmlSerializer(typeof(GameStateData));
        FileStream stream = new FileStream(fileName, FileMode.Open);

        // Decrypt
        // create decryptor
        //ICryptoTransform decryptor = Aes.Create().CreateDecryptor(key, aesIV);

        CryptoStream cryptoStream = new CryptoStream(stream, decryptor, CryptoStreamMode.Read); // ToDo move to an decrypt function

        gameState = serializer.Deserialize(cryptoStream) as GameStateData;
        //gameState = serializer.Deserialize(stream) as GameStateData;
        stream.Flush();
        stream.Dispose();
        stream.Close();
    }


    public void Encrypt(FileStream incomingStream)
    {

    }

    public void Decrypt(FileStream encry)
    {
       
    }

}