using UnityEngine.Jobs;
using UnityEngine;
using Unity.Collections;
using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;

public class SpheresJob : MonoBehaviour
{

    public int count = 10000;
    public float speed = 20;
    public int spawnRange = 50;
    public bool useJob;
        
    private Transform[] transforms;
    private Vector3[] targets;
    private TransformAccessArray transAccArr;
    private NativeArray<Vector3> nativeTargets;
    private List<GameObject> cubes = new List<GameObject>();

    struct MovementJob : IJobParallelForTransform
    {
        public float deltaTime;
        public NativeArray<Vector3> Targets;
        public float Speed;
        public void Execute(int i, TransformAccess transform)
        {
            transform.position = Vector3.Lerp(transform.position, Targets[i], deltaTime / Speed);
        }
    }
    
    private MovementJob job;
    private JobHandle newJobHandle;
    
    
    // Start is called before the first frame update
    void Start()
    {
        transforms = new Transform[count];
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


    }

    // Update is called once per frame
    void Update()
    {
        transAccArr = new TransformAccessArray(transforms);
        nativeTargets = new NativeArray<Vector3>(targets, Allocator.Temp);
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
                cubes[i].transform.position = Vector3.Lerp(cubes[i].transform.position, targets[i], Time.deltaTime / speed);
        }
    }

    private void LateUpdate()
    {
        newJobHandle.Complete();
        transAccArr.Dispose();
        //nativeTargets.Dispose();
    }

    public IEnumerator GenerateTargets()
    {
        for (int i = 0; i < targets.Length; i++)
            targets[i] = new Vector3(Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange));
        yield return new WaitForSeconds(10);
    }
}
