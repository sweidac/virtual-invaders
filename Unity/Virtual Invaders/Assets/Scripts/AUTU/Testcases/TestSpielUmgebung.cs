using AUTU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class TestSpielUmgebung : CTestGruppe
{
  public GameObject Stadt = null;
  public GameObject Turm = null;

  private Vector3 berechneHoechstesVertex(Transform Objekt)
  {
    MeshFilter[] MeshListe = Objekt.GetComponentsInChildren<MeshFilter>();

    Vector3 HoechstesVertex = new Vector3(0, float.NegativeInfinity, 0);

    foreach(MeshFilter Mesh in MeshListe)
    {
      Vector3[] Vertices = Mesh.sharedMesh.vertices;
      foreach(Vector3 Vertex in Vertices)
      {
        Vector3 VertexInWeltkorrdinaten = Mesh.transform.TransformPoint(Vertex);

        if (VertexInWeltkorrdinaten.y > HoechstesVertex.y)
        {
          HoechstesVertex = VertexInWeltkorrdinaten;
        }
      }
    }

    return HoechstesVertex;
  }

  [Test]
  public IEnumerator TurmIstHoechstesGebäude()
  {
    Debug.Assert(Stadt != null, "Dem Testcase wurde kein valides Stadt-Objekt übergeben!");
    Debug.Assert(Turm != null, "Dem Testcase wurde kein valides Turm-Objekt übergeben!");

    Vector3 HoechstesVertexVomTurm = berechneHoechstesVertex(Turm.transform);

    foreach (Transform StadtObjekt in Stadt.transform)
    {
      Pruefer.istGroesser(berechneHoechstesVertex(StadtObjekt.transform).y, HoechstesVertexVomTurm.y, StadtObjekt.name+" ist höher als "+ Turm.name+"!");
    }

    yield return true;
  }
}
