using UnityEngine;
using System.Collections;


[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]

public class Optimizer : MonoBehaviour
{

	public enum OptimizeMethod
	{
		CombineMeshs

	}

	public OptimizeMethod m_OptimizeMethod = OptimizeMethod.CombineMeshs;

	public GameObject[] m_ObjectsToCombine;

	public bool setActive = true;
	// Use this for initialization
	void Awake ()
	{
		switch (m_OptimizeMethod) {
		case OptimizeMethod.CombineMeshs:
			fn_CombineMeshs ();
			break;
		}
	}

	void fn_CombineMeshs ()
	{
		
		Vector3 oldPos = transform.position;
		Quaternion oldRot = transform.rotation;

		transform.rotation = Quaternion.identity;
		transform.position = Vector3.zero;

		//MeshFilter[] MeshsToCombine = gameObject.GetComponentsInChildren <MeshFilter> ();
		Mesh finalMesh = new Mesh ();
		CombineInstance[] combine = new CombineInstance[m_ObjectsToCombine.Length];

		for (int i = 0; i < m_ObjectsToCombine.Length; i++) {
			//combine [i].subMeshIndex = 0;
			combine [i].mesh = m_ObjectsToCombine [i].GetComponent <MeshFilter> ().mesh;
			combine [i].transform = m_ObjectsToCombine [i].transform.localToWorldMatrix;
			Destroy (m_ObjectsToCombine [i].GetComponent <MeshRenderer> ());
			Destroy (m_ObjectsToCombine [i].GetComponent <MeshFilter> ());
		}

		finalMesh.CombineMeshes (combine);
		gameObject.GetComponent <MeshFilter> ().sharedMesh = finalMesh;
		gameObject.GetComponent <MeshRenderer> ().enabled = true;

		transform.rotation = oldRot;
		transform.position = oldPos;

		gameObject.SetActive (setActive);
	}
}
