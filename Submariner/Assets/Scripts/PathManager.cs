using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static PathManager inst;
    void Awake() { inst = this; }

    #region Vars
    public string seed;
    public bool useRandomSeed;

    [SerializeField] float _monstersHigh = 3.15f;
    [SerializeField] float _obstaclesHigh = 0;

    System.Random prng;

    [SerializeField] GameObject[] _pathElems = null;
    [SerializeField] GameObject[] _pathObstacles = null;
    [SerializeField] MonsterMovement _monster = null;

    Vector3 lastElementPivot = Vector3.zero;

    public PathManager(GameObject[] pathObstacles)
    {
        _pathObstacles = pathObstacles;
    }
    #endregion
    #region MonoFunctions
    void Start()
    {
        lastElementPivot = transform.position;

        Generate();

        for (int i = 0; i < 5; i++)
            SpawnNext();
    }
    #endregion
    #region Functions
    /// <summary>Generate the pseudo random number</summary>
    void Generate()
    {
        if (useRandomSeed)
            seed = System.DateTime.Now.ToString() + Time.deltaTime.ToString();

        prng = new System.Random(seed.GetHashCode());
    }
    /// <summary>Spawn the next path object and obstacles</summary>
    public void SpawnNext()
    {
        int index = prng.Next(0, _pathElems.Length);

        Transform t = Instantiate(_pathElems[index], transform).transform;
        t.position = lastElementPivot;
        lastElementPivot = t.GetChild(0).position;

        for(int i = 0; i < 2; i++)
        {
            index = prng.Next(0, _pathObstacles.Length + 1);

            if (index >= _pathObstacles.Length)
            {
                MonsterMovement m = Instantiate(_monster, t);
                Transform t_d = m.transform;

                float x = (float)(prng.NextDouble()) * 2f + 1f;
                float y = _monstersHigh;
                float z = (float)prng.NextDouble() * (t.GetChild(0).localPosition.z * .5f) + (t.GetChild(0).localPosition.z * .5f * i);

                x += Mathf.Lerp(0, t.GetChild(0).localPosition.x, z / t.GetChild(0).localPosition.z) * .5f;

                t_d.localPosition = new Vector3(x, y, z);

                
                //m.Init(new Vector2((float)prng.NextDouble(), (float)prng.NextDouble()).normalized * 2);
            }
            else
            {
                Transform t_d = Instantiate(_pathObstacles[index], t).transform;

                float x = (float)(prng.NextDouble() * 6f) + 1f;
                float y = _obstaclesHigh;
                float z = (float)prng.NextDouble() * (t.GetChild(0).localPosition.z * .5f) + (t.GetChild(0).localPosition.z * .5f * i);

                x += Mathf.Lerp(0, t.GetChild(0).localPosition.x, z / t.GetChild(0).localPosition.z) * .5f;

                t_d.localPosition = new Vector3(x, y, z);

                t_d.localEulerAngles = Vector3.up * 90 * prng.Next(0, 3);
            }
        }
    }
    #endregion
}
