using System.Collections.Generic;
using UnityEngine;

public class LaserShooterScript : LogicObject
{
	private List<GameObject> laserBeamList = new(); //---name
	[SerializeField] private float maxRayDistance = 15000;
	[SerializeField] private float raySpread = 0;
	[SerializeField] private LayerMask layerMask;
	[SerializeField] private LayerMask playerMask;
	private UnityEngine.Pool.ObjectPool<GameObject> beamPool;// ---name
	public GameObject laserBeam;
	public int reflections;

	private GameObject lastReceiver = null;

	private ParticleSystem particleSystemt;

	/// <summary>
	/// Function that clears lasers
	/// </summary>
	private void ClearLasers()
	{
		foreach (var go in laserBeamList) beamPool.Release(go);
		laserBeamList.Clear();
	}
	/// <summary>
	/// laser visualization
	/// </summary>
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.green;

		var a = (Vector2)transform.position + Vector2.Perpendicular(transform.right) * raySpread / 2;
		var b = (Vector2)transform.position - Vector2.Perpendicular(transform.right) * raySpread / 2;
		Gizmos.DrawLine(a, b);
	}
	/// <summary>
	/// starts the whole script 
	/// </summary>
	protected override void Start()
	{
		beamPool = new UnityEngine.Pool.ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool,
			OnDestroyPoolObject);

		ParticleSystem particleSystem;
		if (transform.GetChild(0).TryGetComponent<ParticleSystem>(out particleSystem) && transform.childCount > 0)
		{

			particleSystemt = particleSystem;// ----getchild 0 gali nebut ir component (null)
		}
		
	}
	/// <summary>
	/// updates every frame	for laser
	/// </summary>
	private void Update()
	{
		if (!state)
			FirstLaserShot();
		else if (laserBeamList.Count > 0) ClearLasers();
	}
	/// <summary>
	/// shoots first laser
	/// </summary>
	private void FirstLaserShot()
	{
		ClearLasers();
		ShootLaser(reflections, transform.position, transform.right);
	}

	private bool particlesPlaying = false;
	private Vector2 lastHitPoint;
	private Vector2 lastDir;
	/// <summary>
	/// sets particles for laser
	/// </summary>
	/// <param name="val">bool for laser particle status</param>
	private void SetParticles(bool val)
	{
		if (particleSystemt != null)
		{
			particleSystemt.transform.position = lastHitPoint;// --------psystem null gali
			particleSystemt.transform.rotation =
				Quaternion.Euler(0, 0, Mathf.Atan2(-lastDir.y, -lastDir.x) * Mathf.Rad2Deg);
			if (val && !particlesPlaying)
			{
				particlesPlaying = true;
				particleSystemt.Play(true);
			}
			else if (!val && particlesPlaying)
			{
				particlesPlaying = false;
				particleSystemt.Stop(true, ParticleSystemStopBehavior.StopEmitting);
			}
		}
	}

	/// <summary>
	/// shoots lasers
	/// </summary>
	/// <param name="reflectionsLeft">how many reflections are left</param>
	/// <param name="origin">origin point from where the  laser was shot</param>
	/// <param name="dir">direction in where the  laser is going</param>
	private void ShootLaser(int reflectionsLeft, Vector2 origin, Vector2 dir)
	{
		if (reflectionsLeft == 0) return;

		var hit = Physics2D.Raycast(origin, dir, maxRayDistance, layerMask);

		var dist = hit.collider == null ? maxRayDistance : hit.distance;


		if (dist > Mathf.Epsilon) Draw2DRay(origin, origin + dir * dist);
		GameObject tempReceiver = null;
		var isLast = true;
		if (hit.collider != null)
		{
			if (hit.collider.CompareTag("Mirror"))
			{
				var norm = (hit.point - (Vector2)hit.collider.transform.position).normalized;



				ShootLaser(reflectionsLeft - 1, hit.point - dir * 0.001f, Vector2.Reflect(dir, norm));
				isLast = false;
			}
			else if (hit.collider.CompareTag("LaserReceiver"))
			{
				tempReceiver = hit.collider.gameObject;
			}
		}

		if (isLast)
		{
			if (hit.collider != null)
			{
				lastHitPoint = hit.point;
				lastDir = dir;
				SetParticles(true);
			}
			else
			{
				SetParticles(false);
			}

			if (tempReceiver != lastReceiver)
			{
				if (lastReceiver != null) lastReceiver.GetComponent<LaserReceiverScript>().SetValue(false);
				if (tempReceiver != null) tempReceiver.GetComponent<LaserReceiverScript>().SetValue(true);
				lastReceiver = tempReceiver;
			}
		}
	}
	/// <summary>
	/// draws the laser beam
	/// </summary>
	/// <param name="startPos">position where laser  starts drawing</param>
	/// <param name="hitPos">position  where  laser hits something</param>
	private void Draw2DRay(Vector2 startPos, Vector2 hitPos)
	{
		var middle = (hitPos - startPos) / 2 + startPos;

		var dis = (startPos - hitPos).magnitude;


		Vector3 norm;
		if (dis == 0)
		{

			norm = Vector3.up;
		}
		else
		{
			norm = (hitPos - startPos) / dis;
		}
		

		var rot = Mathf.Atan2(norm.y, norm.x) * Mathf.Rad2Deg;
		var quat = Quaternion.Euler(0, 0, rot);

		var laser = beamPool.Get();
		laserBeamList.Add(laser);

		laser.transform.position = middle;

		laser.transform.rotation = quat;

		laser.GetComponent<SpriteRenderer>().size = new Vector2(dis, 0.5f);


		var hit = Physics2D.BoxCast(middle, new Vector2(dis, raySpread), rot, Vector2.zero, 0.1f, playerMask);

		if (hit.collider != null) GameManager.inst.Death();
	}

	/// <summary>
	/// creates a pooled item in the  game
	/// </summary>
	/// <returns>created object</returns>
	private GameObject CreatePooledItem()
	{
		if (laserBeam != null)
		{

			var temp = Instantiate(laserBeam, Vector2.zero, Quaternion.identity);//---laserb null

			return temp;
		}
		return null;
	}

	/// <summary>
	/// returns an object to the pool
	/// </summary>
	/// <param name="system">the game object, that is returned to the pool</param>
	private void OnReturnedToPool(GameObject system)
	{
		system.gameObject.SetActive(false);
	}

	/// <summary>
	/// the game object that is taken from the pool
	/// </summary>
	/// <param name="system"></param>
	private void OnTakeFromPool(GameObject system)
	{
		system.gameObject.SetActive(true);
	}

	/// <summary>
	/// destroys the object from the pool
	/// </summary>
	/// <param name="system"></param>
	private void OnDestroyPoolObject(GameObject system)
	{
		Destroy(system.gameObject);
	}
	/// <summary>
	/// removes laser from the pool	
	/// </summary>
	/// <param name="obj"></param>
	public void RemoveLaser(GameObject obj)
	{
		beamPool.Release(obj);
	}
}