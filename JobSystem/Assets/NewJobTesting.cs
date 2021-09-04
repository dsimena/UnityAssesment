using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine.Jobs;


public class NewJobTesting : MonoBehaviour
{

    [SerializeField] private bool useJobs;
    //private Color[] pix1, pix2, pix3, pix4; 
    Texture2D pixTex1, pixTex2, pixTex3, pixTex4;
    // Update is called once per frame


    public void Update()
    {
        
        float startTime = Time.realtimeSinceStartup;
        //This is the jobs working on several threads
        if (useJobs)
        { 
            NativeList<JobHandle> jobHandleList = new NativeList<JobHandle>(Allocator.Temp);
            for (int i =0; i<10; i++)
            {
                JobHandle jobHandle = ReallyToughTaskJob();
                jobHandleList.Add(jobHandle);
            }
            
            JobHandle.CompleteAll(jobHandleList);
            jobHandleList.Dispose();
        }
        //Whitout theads, this code takes more time, this is only traditional task
        else
        {
            for (int i =0; i<10; i++)
            {
                Start();
            }
        }
        

        Debug.Log(((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
    }

    //For Task
    //The textures are loaded in variables an divided into parts, then the loops create the result
    private void Start()
    {   //Complex execution tasks
        Color color = new Color();
        float sum_red = 0f;

        /*-----------------------------*/
        var texture = Resources.Load<Texture2D>("Grass");
       
        Color[] pix1 = texture.GetPixels(0, 0, texture.width/2, texture.height/2);
        pixTex1 = new Texture2D(texture.width/2, texture.height/2);
        pixTex1.SetPixels(pix1);
       
        Color[] pix2 = texture.GetPixels(0, texture.height/2, texture.width/2, texture.height/2);
        pixTex2 = new Texture2D(texture.width/2, texture.height/2);
        pixTex2.SetPixels(pix2);

        Color[] pix3 = texture.GetPixels(texture.width/2, 0, texture.width/2, texture.height/2);
        pixTex3 = new Texture2D(texture.width/2, texture.height/2);
        pixTex3.SetPixels(pix3);

        Color[] pix4 = texture.GetPixels(texture.width/2, texture.height/2, texture.width/2, texture.height/2);
        pixTex4 = new Texture2D(texture.width/2, texture.height/2);
        pixTex4.SetPixels(pix4);
       
        /*-----------------------------*/

        
        for (int i = 0; i< pixTex1.height; i++){
            for (int j = 0; j< pixTex1.height; j++){
                color = pixTex1.GetPixel(i,j);
                sum_red += color.r;
            }
        }
        for (int i = 0; i< pixTex2.height; i++){
            for (int j = 0; j< pixTex2.height; j++){
                color = pixTex2.GetPixel(i,j);
                sum_red += color.r;
            }
        }
        for (int i = 0; i< pixTex3.height; i++){
            for (int j = 0; j< pixTex3.height; j++){
                color = pixTex3.GetPixel(i,j);
                sum_red += color.r;
            }
        }
        for (int i = 0; i< pixTex4.height; i++){
            for (int j = 0; j< pixTex4.height; j++){
                color = pixTex4.GetPixel(i,j);
                sum_red += color.r;
            }
        }
         Debug.Log(("R Channel Sum From Task: ") + sum_red );
    }

    public JobHandle ReallyToughTaskJob()
    {
        ReallyToughJob job = new ReallyToughJob();
        return job.Schedule();
    }
                 
}


    public struct ReallyToughJob : IJob
    {

        
        public void Execute()
        {
            Color color;
            float sum_red = 0f;
            var texture = Resources.Load<Texture2D>("Grass");
           
            Color[] pix1 = texture.GetPixels(0, 0, texture.width/2, texture.height/2);
            Texture2D pixTex1 = new Texture2D(texture.width/2, texture.height/2);
            pixTex1.SetPixels(pix1);
 
            Color[] pix2 = texture.GetPixels(0, 0, texture.width/2, texture.height/2);
            Texture2D pixTex2 = new Texture2D(texture.width/2, texture.height/2);
            pixTex2.SetPixels(pix2);

            Color[] pix3 = texture.GetPixels(0, 0, texture.width/2, texture.height/2);
            Texture2D pixTex3 = new Texture2D(texture.width/2, texture.height/2);
            pixTex3.SetPixels(pix3);

            Color[] pix4 = texture.GetPixels(0, 0, texture.width/2, texture.height/2);
            Texture2D pixTex4 = new Texture2D(texture.width/2, texture.height/2);
            pixTex4.SetPixels(pix4);
            
            //Color color = load4Textures.pixTex1.GetPixel(i,j);
                for (int i = 0; i< pixTex1.height; i++){
                    for (int j = 0; j< pixTex1.height; j++){
                        color = pixTex1.GetPixel(i,j);
                        sum_red += color.r;
                    }
                }
                for (int i = 0; i< pixTex2.height; i++){
                    for (int j = 0; j< pixTex2.height; j++){
                        color = pixTex2.GetPixel(i,j);
                        sum_red += color.r;
                    }
                }
                for (int i = 0; i< pixTex3.height; i++){
                    for (int j = 0; j< pixTex3.height; j++){
                        color = pixTex3.GetPixel(i,j);
                        sum_red += color.r;
                    }
                }
                for (int i = 0; i< pixTex4.height; i++){
                    for (int j = 0; j< pixTex4.height; j++){
                        color = pixTex4.GetPixel(i,j);
                        sum_red += color.r;
                    }
                }
             Debug.Log(("R Channel Sum From IJOB-Execute: ") + sum_red );
        }
        
    }
