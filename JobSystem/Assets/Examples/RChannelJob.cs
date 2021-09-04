using UnityEngine.Jobs;
using UnityEngine;
using Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;

public class RChannelJob : MonoBehaviour
{

    public bool useJob;
        
    //private NativeArray<Vector3> nativeRChannel;

    private Texture2D myGUITexture;


    struct sumRJob : IJobParallelFor
    {
        // Jobs declare all data that will be accessed in the job
        // By declaring it as read only, multiple jobs are allowed to access the data in parallel
        [ReadOnly]
        public NativeArray<Color> RChannelTexture;

        // By default containers are assumed to be read & write
        public NativeArray<float> SumRChannel;

        // Delta time must be copied to the job since jobs generally don't have concept of a frame.
        // The main thread waits for the job same frame or next frame, but the job should do work deterministically
        // independent on when the job happens to run on the worker threads.
        public float deltaTime;

        public float sum;

        public void Execute(int i)
        {
          // RChannelTexture[i].Load(int3(texelU, texelV, 0)).r;
          for ( i = 0; i < RChannelTexture.Length; i++)
                sum = SumRChannel[i] + RChannelTexture[i][0];
            
            Debug.Log("ejemplo");
            
            
        }
    
    }
    
    private sumRJob job;
    private JobHandle newJobHandle;
    
    
    // Start is called before the first frame update
    void Start()
    {
        myGUITexture = (Texture2D)Resources.Load("Grass");
        //Load a Texture (Assets/Resources/texture01.png)
        var texture = Resources.Load<Texture2D>("Grass");
        //public NativeArray<Color> rChannelTexture;
        //rChannelTexture = texture.GetPixels(0, 0, texture.width, texture.height);
        //RChannelTexture = rChannelTexture;



        //GetComponent<Renderer>().material.mainTexture = texture;
        //var fillColor =  texture.GetPixels32();
        Debug.Log(Resources.Load("Grass"));

        Color[] pix1 = texture.GetPixels(0, 0, texture.width/2, texture.height/2);
        Texture2D pixTex1 = new Texture2D(texture.width/2, texture.height/2);
        pixTex1.SetPixels(pix1);

        Color[] pix2 = texture.GetPixels(0, texture.height/2, texture.width/2, texture.height/2);
        Texture2D pixTex2 = new Texture2D(texture.width/2, texture.height/2);
        pixTex2.SetPixels(pix2);

        Color[] pix3 = texture.GetPixels(texture.width/2, 0, texture.width/2, texture.height/2);
        Texture2D pixTex3 = new Texture2D(texture.width/2, texture.height/2);
        pixTex3.SetPixels(pix3);

        Color[] pix4 = texture.GetPixels(texture.width/2, texture.height/2, texture.width/2, texture.height/2);
        Texture2D pixTex4 = new Texture2D(texture.width/2, texture.height/2);
        pixTex4.SetPixels(pix4);


        var sum_red = 0.0;
        foreach (var px in pix1){
            sum_red += px[0];
            Debug.Log("Red: [" + px[2] + "]");    
        }

        Debug.Log("Red: [" + sum_red + "]");
        Debug.Log("****************");            


        Debug.Log("Valor de Rojo");
        //Debug.Log(fillColor[0]); 

        // for (int i = 0; i< texture.Length/2 i++)
        //   text1
        //var pix = texture.GetPixels32();
        

   
        

       /* transforms = new Transform[count];
        for (int i = 0; i < count; i++)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            cubes.Add(obj);
            obj.transform.position = new Vector3(Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange));
            obj.GetComponent<MeshRenderer>().material.color = Color.green;
            transforms[i] = obj.transform;
        }
        targets = new Vector3[transforms.Length];
        StartCoroutine(GenerateTargets());
*/

    }

    // Update is called once per frame
    void Update()
    {
        var RChannelTexture = new NativeArray<Vector3>(500, Allocator.Persistent);

        var SumRChannel = new NativeArray<Vector3>(500, Allocator.Persistent);

     //   transAccArr = new TransformAccessArray(transforms);
        /*nativeTargets = new NativeArray<Vector3>(targets, Allocator.Temp);
        if (useJob == true)
        {
            job = new MovementJob();
            job.deltaTime = Time.deltaTime;
            job.Targets = nativeTargets;
            job.Speed = speed;
            newJobHandle = job.Schedule(transAccArr);
        }
        else
        {
            for (int i = 0; i < transAccArr.length; i++)
            ;
                //cubes[i].transform.position = Vector3.Lerp(cubes[i].transform.position, targets[i], Time.deltaTime / speed);
        }*/

        RChannelTexture.Dispose();
        SumRChannel.Dispose();
        //nativeRChannel.Dispose();
    }

    private void LateUpdate()
    {
        newJobHandle.Complete();
        //transAccArr.Dispose();
       // nativeTargets.Dispose();
    }

    public IEnumerator GenerateTargets()
    {
       // for (int i = 0; i < targets.Length; i++)
         //   targets[i] = new Vector3(Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange));
        yield return new WaitForSeconds(2);
    }
}
