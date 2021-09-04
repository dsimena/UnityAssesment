using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using UnityEngine.Jobs;


public class NewJobTesting : MonoBehaviour
{

    [SerializeField] 
    private bool useJobs;

    [SerializeField] 
    public Texture2D texture;

    Texture2D pixTex1, pixTex2, pixTex3, pixTex4;
    // Update is called once per frame
    //private Texture2D texture;


    NativeList<Vector4> extractColors(Texture2D texture, int x, int y)
    {
        Color[] pix1 = texture.GetPixels(x, y, texture.width / 2, texture.height / 2);
        NativeList<Vector4> colorsV4 = new NativeList<Vector4>(Allocator.TempJob);
        foreach (var color in pix1)
        {
            colorsV4.Add(color);
        }
        return colorsV4;
    }

    void Update()
    {
        
        float startTime = Time.realtimeSinceStartup;
        //This is the jobs working on several threads
        if (useJobs)
        { 
            NativeList<JobHandle> jobHandleList = new NativeList<JobHandle>(Allocator.Temp);

            //
            var texture = Resources.Load<Texture2D>("Grass");

            //
            int[] xvals = {0, texture.width / 2};
            int[] yvals = {0, texture.height / 2};
            List<NativeList<Vector4>> allVectors = new List<NativeList<Vector4>>();
            foreach (int x in xvals)
            {
                foreach (int y in yvals) {
                    var colorsV4 = extractColors(texture, x, y);
                    allVectors.Add(colorsV4);
                    int[] limits = { x, y };
                    JobHandle jobHandle = ReallyToughTaskJob(colorsV4, allVectors.Count, limits);
                    jobHandleList.Add(jobHandle);

                }
            }
            
            JobHandle.CompleteAll(jobHandleList);
            
            jobHandleList.Dispose();
            foreach(var colorsV4 in allVectors) {
                colorsV4.Dispose();
            }
            
        }
        //Whitout theads, this code takes more time, this is only traditional task
        else
        {
            Start();            
        }
        

        Debug.Log(((Time.realtimeSinceStartup - startTime) * 1000f) + "ms");
    }

    //For Task
    //The textures are loaded in variables an divided into parts, then the loops create the result
    void Start()
    {   //Complex execution tasks
        //Color color = new Color();
        float sum_red = 0f;

        /*-----------------------------*/
        Texture2D texture = Resources.Load<Texture2D>("Grass");

        int[] xvals = { 0, texture.width / 2 };
        int[] yvals = { 0, texture.height / 2 };
        List<NativeList<Vector4>> allVectors = new List<NativeList<Vector4>>();
        foreach (int x in xvals)
        {
            foreach (int y in yvals)
            {
                var colorsV4 = extractColors(texture, x, y);
                allVectors.Add(colorsV4);
                foreach (var color in colorsV4)
                {
                    sum_red += color[0];
                }
                Debug.Log(("R Channel Sum START ") + sum_red + "x: " + x +  "y: "+ y);
            }
        }

        foreach (var colorsV4 in allVectors)
        {
            colorsV4.Dispose();
        }
        
    }

    public JobHandle ReallyToughTaskJob(NativeList<Vector4> pix, int jobIndex, int[] limits)
    {
        
        ReallyToughJob job = new ReallyToughJob( pix , jobIndex, limits[0], limits[1]);
        return job.Schedule();
    }
                 
}


    public struct ReallyToughJob : IJob
    {


        NativeList<Vector4> pix ;
        int jobIndex;
        int x;
        int y;
        float sum_red;

        public ReallyToughJob(NativeList<Vector4> _pix, int _jobIndex, int _x, int _y){
            pix = _pix;
            jobIndex = _jobIndex;
            sum_red = 0;
            x = _x;
            y = _y;
        }


        override
        public string ToString(){
            return jobIndex+" [ "+ x+" " +y+"] => " + sum_red;
        }

        public void Execute(){
            
            foreach (var px in pix)
            {
                sum_red += px[0];
                
            }
            Debug.Log(this);
        
        }   
    }
