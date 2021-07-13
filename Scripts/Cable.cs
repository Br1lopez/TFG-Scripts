using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[System.Serializable]
public class CableCurve {
	[SerializeField]
	Vector3 m_start;
	[SerializeField]
	Vector3 m_end;
	[SerializeField]
	float m_slack;
	[SerializeField]
	int m_steps;
	[SerializeField]
	Color m_color;
	[SerializeField]
	bool m_regen;
	[SerializeField]
	bool m_handles;

	static Vector3[] emptyCurve = new Vector3[]{new Vector3 (0.0f, 0.0f, 0.0f), new Vector3 (0.0f, 0.0f, 0.0f)};
	[SerializeField]
	Vector3[] points;


	public bool drawHandles
	{
		get { return m_handles; }
		set
		{
			m_handles = value;
		}
	}

	public bool regenPoints
	{
		get { return m_regen; }
		set {
			m_regen = value;
		}
	}

	public Vector3 start
	{
		get { return m_start; }
		set {
			if (value != m_start)
				m_regen = true;
			m_start = value;
		}
	}

	public Vector3 end
	{
		get { return m_end; }
		set {
			if (value != m_end)
				m_regen = true;
			m_end = value;
		}
	}
	public float slack
	{
		get { return m_slack; }
		set {
			if (value != m_slack)
				m_regen = true;
			m_slack = Mathf.Max(0.0f, value);
		}
	}
	public int steps
	{
		get { return m_steps; }
		set {
			if (value != m_steps)
				m_regen = true;
			m_steps = Mathf.Max (2, value);
		}
	}
	public Color color
	{
		get { return m_color; }
		set {
			if (value != m_color)
				m_regen = true;
			m_color = value;
		}
	}

	public Vector3 midPoint
	{
		get
		{
			Vector3 mid = Vector3.zero;
			if (m_steps == 2) {
				return (points[0] + points[1]) * 0.5f;
			}
			else if (m_steps > 2) {
				int m = m_steps / 2;
			}
			return mid;
		}
	}

	public CableCurve () {
		points = emptyCurve;
		m_start = Vector3.up;
		m_end = Vector3.up + Vector3.forward;
		m_slack = 0.5f;
		m_steps = 20;
		m_regen = true;
		m_color = Color.white;
		m_handles = true;
	}

	public CableCurve (CableCurve v) {
		points = v.Points ();
		m_start = v.start;
		m_end = v.end;
		m_slack = v.slack;
		m_steps = v.steps;
		m_regen = v.regenPoints;
		m_color = v.color;
		m_handles = v.drawHandles;
	}

	public Color[] Colors () {
		Color[] cols = new Color[m_steps];
		for (int c = 0; c < m_steps; c++) {
			cols[c] = m_color;
		}
		return cols;
	}

	public Vector3[] Points ()
	{
		if (!m_regen)
			return points;

		if (m_steps < 2)
			return emptyCurve;

		float lineDist = Vector3.Distance (m_end, m_start);
		float lineDistH = Vector3.Distance (new Vector3(m_end.x, m_start.y, m_end.z), m_start);
		float l = lineDist + Mathf.Max(0.0001f, m_slack);
		float r = 0.0f;
		float s = m_start.y;
		float u = lineDistH;
		float v = end.y;

		if ((u-r) == 0.0f)
			return emptyCurve;

		float ztarget = Mathf.Sqrt(Mathf.Pow(l, 2.0f) - Mathf.Pow(v-s, 2.0f)) / (u-r);

		int loops = 30;
		int iterationCount = 0;
		int maxIterations = loops * 10; // For safety.
		bool found = false;

		float z = 0.0f;
		float ztest = 0.0f;
		float zstep = 100.0f;
		float ztesttarget = 0.0f;
		for (int i = 0; i < loops; i++) {
			for (int j = 0; j < 10; j++) {
				iterationCount++;
				ztest = z + zstep;
				ztesttarget = (float)Math.Sinh(ztest)/ztest;

				if (float.IsInfinity (ztesttarget))
					continue;

				if (ztesttarget == ztarget) {
					found = true;
					z = ztest;
					break;
				} else if (ztesttarget > ztarget) {
					break;
				} else {
					z = ztest;
				}

				if (iterationCount > maxIterations) {
					found = true;
					break;
				}
			}

			if (found)
				break;
			
			zstep *= 0.1f;
		}

		float a = (u-r)/2.0f/z;
		float p = (r+u-a*Mathf.Log((l+v-s)/(l-v+s)))/2.0f;
		float q = (v+s-l*(float)Math.Cosh(z)/(float)Math.Sinh(z))/2.0f;

		points = new Vector3[m_steps];
		float stepsf = m_steps-1;
		float stepf;
		for (int i = 0; i < m_steps; i++) {
			stepf = i / stepsf;
			Vector3 pos = Vector3.zero;
			pos.x = Mathf.Lerp(start.x, end.x, stepf);
			pos.z = Mathf.Lerp(start.z, end.z, stepf);
			pos.y = a * (float)Math.Cosh(((stepf*lineDistH)-p)/a)+q;
			points[i] = pos;
		}

		m_regen = false;
		return points;
	}

    public Vector3[] DrawingPoints;
}

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Cable : MonoBehaviour {

	[SerializeField]
	public List<CableCurve> cables;
	public Material cableMatr;
	public Space space;
    public bool pruebita;

    bool firstRun = true;

	public void GenerateMesh () {
		Transform tr = transform;
		MeshFilter meshFilter = GetComponent <MeshFilter>();
		MeshRenderer meshRenderer = GetComponent <MeshRenderer>();

		Mesh cableMesh = meshFilter.sharedMesh;
		if (cableMesh == null)
			cableMesh = new Mesh();
		cableMesh.Clear();

		int numCables = (cables == null) ? 0 : cables.Count;
		cableMesh.subMeshCount = numCables;

		if (firstRun) {
			for (int c = 0; c < numCables; c++) {
				cables[c].regenPoints = true;
			}
			firstRun = false;
		}

		List<Vector3> points = new List<Vector3> ();
		List<Color> colors = new List<Color> ();
		for (int c = 0; c < numCables; c++) {
			points.AddRange (cables[c].Points ());
			colors.AddRange (cables[c].Colors ());
		}

		if (space == Space.World) {
			int count = points.Count;
			for (int p = 0; p < count; p++) {
				Matrix4x4 ltw = tr.worldToLocalMatrix;
				points[p] = ltw.MultiplyPoint (points[p]);
			}
		}

		cableMesh.SetVertices(points);
		cableMesh.SetColors(colors);

		int indice = 0;
		for (int c = 0; c < numCables; c++) {
			int numIndices = cables[c].steps;
			int[] indices = new int[numIndices];
			for (int i = 0; i < numIndices; i++) {
				indices[i] = indice;
				indice++;
			}
			cableMesh.SetIndices (indices, MeshTopology.LineStrip, c);
		}

		cableMesh.RecalculateBounds ();
		meshFilter.sharedMesh = cableMesh;
		AssignMaterials();
	}

	public void AssignMaterials () {
		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
		int numCables = (cables == null) ? 0 : cables.Count;
		Material[] mats = new Material[numCables];
		for (int c = 0; c < numCables; c++) {
			mats[c] = cableMatr;
		}
		meshRenderer.sharedMaterials = mats;
	}

    
    public void DrawLines()
    {
        int i = 1;
        foreach (CableCurve c in cables)
        {            
            GameObject g = new GameObject();
            g.transform.SetParent(gameObject.transform);
            g.name = "Cable " +i;            

            //g.layer = 11;
            LineRenderer lr = g.AddComponent<LineRenderer>();
            lr.positionCount = c.DrawingPoints.Length;
            lr.SetPositions(c.DrawingPoints);
            Material m = new Material(Shader.Find("Sprites/Default"));
            m.color = Color.yellow;

            for (i = 8; i < 11; i++)
            {
                if (CheckParentLayer(gameObject, i))
                {
                    switch (i)
                    {
                        case 8:
                            m.color = new Color(0.3f, 0, 0, 1);
                            break;
                        case 9:
                            m.color = new Color(0,0.3f,0,1);
                            break;
                        case 10:
                            m.color = new Color(0, 0, 0.3f, 1);
                            break;
                    }

                    print("Index: " + i);
                    break;
                }
            }
            //m.color = Color.yellow;
            lr.material = m;
            AnimationCurve ac = new AnimationCurve();
            ac.AddKey(0f, 0.1f);
            ac.AddKey(1f, 0.1f);
            lr.widthCurve = ac;
            lr.sortingLayerName = "Fondo";
            i++;
        }
    }

    public bool CheckParentLayer(GameObject gameObject, int LayerIndex)
    {
        //Dado un cierto objeto, sube en la jerarquía hasta encontrar otro objeto en cierta capa.
        while (1<2)
        {            
            if (gameObject.layer == LayerIndex)
            {
                return true;
            }
            else if (gameObject.transform.parent != null)
            {
                gameObject = gameObject.transform.parent.gameObject;
            }
            else
            {
                return false;
            }
        }

       
    }

    /*public void UpdateColor()
    {
        foreach (CableCurve c in cables)
        {
            GameObject g = new GameObject();
            g.transform.SetParent(gameObject.transform);
            g.name = "Cable " + i;
            //g.layer = 11;
            LineRenderer lr = g.AddComponent<LineRenderer>();
            lr.positionCount = c.DrawingPoints.Length;
            lr.SetPositions(c.DrawingPoints);
            Material m = new Material(Shader.Find("Sprites/Default"));
            m.color = Color.black;
            lr.material = m;
            AnimationCurve ac = new AnimationCurve();
            ac.AddKey(0f, 0.1f);
            ac.AddKey(1f, 0.1f);
            lr.widthCurve = ac;
            lr.sortingLayerName = "Fondo";
            i++;
        }
    }*/

	public void Start () {
		GenerateMesh ();
        DrawLines();
	}

    public void Update()
    {

    }
}
