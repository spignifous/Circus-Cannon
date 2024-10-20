using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class UnitManager : StaticInstance<UnitManager>
{
    [Header("Units prefab")]
    [SerializeField] private Unit _bulletUnit;
    [SerializeField] private Unit _ballUnit;

    [Space(5)]
    [SerializeField] private Unit _bulletImpactUnit;
    [SerializeField] private Unit _fireRateUnit;
    [SerializeField] private Unit _explosionUnit;

    private ObjectPool<Unit> _bulletUnits;
    private ObjectPool<Unit> _ballUnits;

    private ObjectPool<Unit> _bulletImpactUnits;
    private ObjectPool<Unit> _fireRateUnits;
    private ObjectPool<Unit> _explosionUnits;

    // List of all units
    public static List<Unit> Units { get; private set; } = new List<Unit>();

    // Start is called before the first frame update
    protected void Start()
    {
        // Create Bullet pool
        _bulletUnits = new ObjectPool<Unit>(CreateBullet, PullCallback, PushCallback, DestroyCallback, true, 10, 20);

        // Create Ball pool
        _ballUnits = new ObjectPool<Unit>(CreateBall, PullCallback, PushCallback, DestroyCallback, true, 10, 20);

        // Create Bullet Impact pool
        _bulletImpactUnits = new ObjectPool<Unit>(CreateBulletImpact, PullCallback, PushCallback, DestroyCallback, true, 10, 20);

        // Create Fire Rate pool
        _fireRateUnits = new ObjectPool<Unit>(CreateFireRate, PullCallback, PushCallback, DestroyCallback, true, 10, 20);

        // Create Fire Rate pool
        _explosionUnits = new ObjectPool<Unit>(CreateExplosion, PullCallback, PushCallback, DestroyCallback, true, 10, 20);
    }

    #region PULL UNITS FUNCTIONS

    // BULLET
    public Unit PullBullet(Vector3 position, Quaternion rotation) => PullUnit(_bulletUnits, position, rotation);

    // CIRCUS BULLET
    public Unit PullBall(Vector3 position, Quaternion rotation) => PullUnit(_ballUnits, position, rotation);

    // BULLET IMPACT FX
    public Unit PullBulletImpact(Vector3 position, Quaternion rotation) => PullUnit(_bulletImpactUnits, position, rotation);

    // FIRE RATE FX
    public Unit PullFireRate(Vector3 position, Quaternion rotation) => PullUnit(_fireRateUnits, position, rotation);

    // EXPLOSION FX
    public Unit PullExplosion(Vector3 position, Quaternion rotation) => PullUnit(_explosionUnits, position, rotation);

    #endregion

    #region CREATE OBJECTS

    private Unit CreateBullet() => InstantiateUnit(_bulletUnit);
    private Unit CreateBall() => InstantiateUnit(_ballUnit);
    private Unit CreateBulletImpact() => InstantiateUnit(_bulletImpactUnit);
    private Unit CreateFireRate() => InstantiateUnit(_fireRateUnit);
    private Unit CreateExplosion() => InstantiateUnit(_explosionUnit);

    private Unit InstantiateUnit(Unit unit)
    {
        if (unit == null) return null;

        // Instantiate new instance of the unit
        return Instantiate(unit);
    }

    #endregion

    #region UNIT SETUP

    private Unit PullUnit(ObjectPool<Unit> units, Vector3 position, Quaternion rotation)
    {
        if (units == null) return null;

        // Pull Object from pool and update position and rotation
        Unit unit = units.Get();

        if (unit != null)
        {
            unit.transform.position = position;
            unit.transform.rotation = rotation;

            // Pass reference
            unit.InitializeUnit(this, units);
        }

        return unit;
    }

    private void AddUnit(Unit unit)
    {
        if (unit == null) return;

        // Add to list
        if (!Units.Contains(unit))
        {
            Units.Add(unit);
        }
    }

    private void RemoveUnit(Unit unit)
    {
        if (unit == null) return;

        // Remove from list
        if (Units.Contains(unit))
        {
            Units.Remove(unit);
        }
    }

    #endregion

    #region CALLBACK FUNCTIONS

    private void PullCallback(Unit unit)
    {
        if (unit == null) return;

        AddUnit(unit);

        // Activate
        unit.gameObject.SetActive(true);
    }

    private void PushCallback(Unit unit)
    {
        if (unit == null) return;

        RemoveUnit(unit);
    }

    private void DestroyCallback(Unit unit)
    {
        if (unit == null) return;

        RemoveUnit(unit);

        // Destroy
        Destroy(unit.gameObject);
    }

    #endregion

    #region UNITY FUNCTIONS

    private void OnDestroy()
    {
        /* Units.Clear();
        _ballUnits.Clear();
        _bulletUnits.Clear();
        _ballUnits.Clear();

        _bulletImpactUnits.Clear(); ;
        _fireRateUnits.Clear();
        _explosionUnits.Clear();*/
    }

    #endregion
}
