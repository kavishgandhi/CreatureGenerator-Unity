using System;
using System.Collections.Generic;
using UnityEngine;


public class CCMeshData
{
    public List<Vector3> points; // Original mesh points
    public List<Vector4> faces; // Original mesh quad faces
    public List<Vector4> edges; // Original mesh edges
    public List<Vector3> facePoints; // Face points, as described in the Catmull-Clark algorithm
    public List<Vector3> edgePoints; // Edge points, as described in the Catmull-Clark algorithm
    public List<Vector3> newPoints; // New locations of the original mesh points, according to Catmull-Clark
}


// Comparer class to use for equality checks in dictionary used in GetEdges 
public class TwoPointsComparer : EqualityComparer<Tuple<int, int>>
{
    public override bool Equals(Tuple<int, int> x, Tuple<int, int> y)
    {
        // Tuples of two points indices are equal if the indices are the same in any order 
        if ((x.Item1 == y.Item1 && x.Item2 == y.Item2) || (x.Item2 == y.Item1 && x.Item1 == y.Item2))
        {
            return true;
        }

        return false;
    }

    public override int GetHashCode(Tuple<int, int> obj)
    {
        return obj.Item1.GetHashCode() + obj.Item2.GetHashCode();
    }
}

public static class CatmullClark
{
    // Returns a QuadMeshData representing the input mesh after one iteration of Catmull-Clark subdivision.
    public static QuadMeshData Subdivide(QuadMeshData quadMeshData)
    {
        // Create and initialize a CCMeshData corresponding to the given QuadMeshData
        CCMeshData meshData = new CCMeshData();
        meshData.points = quadMeshData.vertices;
        meshData.faces = quadMeshData.quads;
        meshData.edges = GetEdges(meshData);
        meshData.facePoints = GetFacePoints(meshData);
        meshData.edgePoints = GetEdgePoints(meshData);
        meshData.newPoints = GetNewPoints(meshData);

        // Combine facePoints, edgePoints and newPoints into a subdivided QuadMeshData.
        // We first go over all faces, creating a new quad for each face where each point is first in each
        // corresponding quad, and the facePoint is third.
        List<Vector4> newFaces = new List<Vector4>();
        for (int i = 0; i < meshData.faces.Count; i++)
        {
            int indexP1 = (int) meshData.faces[i][0];
            int indexP2 = (int) meshData.faces[i][1];
            int indexP3 = (int) meshData.faces[i][2];
            int indexP4 = (int) meshData.faces[i][3];
            newFaces.Add(new Vector4(indexP1, 0, i, 0));
            newFaces.Add(new Vector4(indexP2, 0, i, 0));
            newFaces.Add(new Vector4(indexP3, 0, i, 0));
            newFaces.Add(new Vector4(indexP4, 0, i, 0));
        }

        // Go over all edges and for each edge insert it in the corresponding quad according to the relevant 
        // face index and point index, creating quads of the form (newPoint,edgePoint1,facePoint,edgePoint2)
        // in a clockwise manner.
        for (int j = 0; j < meshData.edges.Count; j++)
        {
            int indexP1 = (int) meshData.edges[j][0];
            int indexP2 = (int) meshData.edges[j][1];
            int indexF1 = (int) meshData.edges[j][2];
            int indexF2 = (int) meshData.edges[j][3];
            // get offsets in first face quads and update edge points in quads
            var (offSet1F1, offSet2F1) = QuadOffsets(meshData, indexF1, indexP1, indexP2);
            // To keep clockwise manner, for the first face - this edge belongs second for the first point quad, and
            // last in the second point quad
            Vector4 quad1F1 = newFaces[indexF1 * 4 + offSet1F1];
            quad1F1[1] = j;
            newFaces[indexF1 * 4 + offSet1F1] = quad1F1;
            Vector4 quad2F1 = newFaces[indexF1 * 4 + offSet2F1];
            quad2F1[3] = j;
            newFaces[indexF1 * 4 + offSet2F1] = quad2F1;

            // get offsets in second face quads and update edge points in quads
            var (offSet1F2, offSet2F2) = QuadOffsets(meshData, indexF2, indexP1, indexP2);
            // To keep clockwise manner, for the second face - this edge belongs last for the first point quad, and
            // second in the second point quad
            Vector4 quad1F2 = newFaces[indexF2 * 4 + offSet1F2];
            quad1F2[3] = j;
            newFaces[indexF2 * 4 + offSet1F2] = quad1F2;
            Vector4 quad2F2 = newFaces[indexF2 * 4 + offSet2F2];
            quad2F2[1] = j;
            newFaces[indexF2 * 4 + offSet2F2] = quad2F2;
        }

        // Create the list of new vertices starting with facePoints, edgePoints and newPoints
        List<Vector3> newVertex = new List<Vector3>();
        foreach (Vector3 facePoint in meshData.facePoints)
        {
            newVertex.Add(facePoint);
        }

        foreach (Vector3 edgePoint in meshData.edgePoints)
        {
            newVertex.Add(edgePoint);
        }

        foreach (Vector3 newPoint in meshData.newPoints)
        {
            newVertex.Add(newPoint);
        }

        // Calculate the final indices according to the offsets in the new vertices list
        List<Vector4> finalFaces = new List<Vector4>();
        int pointsOffset = meshData.facePoints.Count + meshData.edgePoints.Count;
        int edgePointOffset = meshData.facePoints.Count;
        foreach (Vector4 newface in newFaces)
        {
            finalFaces.Add(new Vector4(newface[0] + pointsOffset, newface[1] + edgePointOffset, newface[2],
                newface[3] + edgePointOffset));
        }

        return new QuadMeshData(newVertex, finalFaces);
    }

    // Returns a list of all edges in the mesh defined by given points and faces.
    // Each edge is represented by Vector4(p1, p2, f1, f2)
    // p1, p2 are the edge vertices
    // f1, f2 are faces incident to the edge. If the edge belongs to one face only, f2 is -1
    public static List<Vector4> GetEdges(CCMeshData mesh)
    {
        // Use a dictionary for quick lookup in case we already came across the edge and want
        // to update the faces it belongs to. Using the TwoPointsComparer here for equality checks
        TwoPointsComparer c = new TwoPointsComparer();
        Dictionary<Tuple<int, int>, Tuple<int, int>> d = new Dictionary<Tuple<int, int>, Tuple<int, int>>(c);

        // Go over all faces and for each face insert new edge or update existing with face.  
        for (int j = 0; j < mesh.faces.Count; j++)
        {
            // Go over all 4 edges in this face
            for (int i = 0; i < 4; i++)
            {
                int p1 = (int) mesh.faces[j][i];
                int p2 = (int) mesh.faces[j][(i + 1) % 4]; // when i=3 we want p0 
                Tuple<int, int> twoPoint = new Tuple<int, int>(p1, p2);
                if (d.ContainsKey(twoPoint)) // already have this edge, update faces
                {
                    int f1 = d[twoPoint].Item1;
                    Tuple<int, int> newFaces = new Tuple<int, int>(f1, j);
                    d[twoPoint] = newFaces;
                }
                else // new face, add to dictionary
                {
                    d[twoPoint] = new Tuple<int, int>(j, -1);
                }
            }
        }

        // Convert dictionary to list of Vector4
        List<Vector4> edges = new List<Vector4>();
        foreach (KeyValuePair<Tuple<int, int>, Tuple<int, int>> entry in d)
        {
            edges.Add(new Vector4(entry.Key.Item1, entry.Key.Item2, entry.Value.Item1, entry.Value.Item2));
        }

        return edges;
    }

    // Returns a list of "face points" for the given CCMeshData, as described in the Catmull-Clark algorithm 
    public static List<Vector3> GetFacePoints(CCMeshData mesh)
    {
        // Go over all faces and for each face calculate the facePoint which is the average of original points in face 
        List<Vector3> facePoints = new List<Vector3>();
        for (int i = 0; i < mesh.faces.Count; i++)
        {
            Vector4 face = mesh.faces[i];
            facePoints.Add((mesh.points[(int) face[0]] + mesh.points[(int) face[1]] + mesh.points[(int) face[2]] +
                            mesh.points[(int) face[3]]) / 4);
        }

        return facePoints;
    }

    // Returns a list of "edge points" for the given CCMeshData, as described in the Catmull-Clark algorithm 
    public static List<Vector3> GetEdgePoints(CCMeshData mesh)
    {
        // Go over all edges and for each edge calculate the edgePoint which is the average of the original 
        // endPoints and neighbouring facePoints
        List<Vector3> edgePoints = new List<Vector3>();
        for (int i = 0; i < mesh.edges.Count; i++)
        {
            Vector3 point1 = mesh.points[(int) mesh.edges[i][0]];
            Vector3 point2 = mesh.points[(int) mesh.edges[i][1]];
            Vector3 facePoint1 = mesh.facePoints[(int) mesh.edges[i][2]];
            Vector3 facePoint2 = mesh.facePoints[(int) mesh.edges[i][3]];
            edgePoints.Add((point1 + point2 + facePoint1 + facePoint2) / 4);
        }

        return edgePoints;
    }

    // Returns a list of new locations of the original points for the given CCMeshData, as described in the CC algorithm 
    public static List<Vector3> GetNewPoints(CCMeshData mesh)
    {
        // Create an array the size of the original points to hold for each point a List of neighbor point indices
        // and a Set of adjacent face indices
        Tuple<List<int>, HashSet<int>>[]
            neighborsPointsAndFaces = new Tuple<List<int>, HashSet<int>>[mesh.points.Count];
        for (int k = 0; k < mesh.points.Count; k++)
        {
            neighborsPointsAndFaces[k] = new Tuple<List<int>, HashSet<int>>(new List<int>(), new HashSet<int>());
        }

        // Go over edges and update neighbor points and faces for each point
        List<Vector3> newPoints = new List<Vector3>();
        foreach (Vector4 edge in mesh.edges)
        {
            int p1 = (int) edge[0];
            int p2 = (int) edge[1];
            int f1 = (int) edge[2];
            int f2 = (int) edge[3];
            neighborsPointsAndFaces[p1].Item1.Add(p2); // updates p1 neighbor vertex 
            neighborsPointsAndFaces[p2].Item1.Add(p1); // updates p2 neighbor  vertex
            neighborsPointsAndFaces[p1].Item2
                .Add(f1); // updates p1 neighbor face, we may already added it this is why we chose set 
            neighborsPointsAndFaces[p1].Item2.Add(f2);
            neighborsPointsAndFaces[p2].Item2
                .Add(f1); // updates p2 neighbor face, we may already added it this is why we chose set 
            neighborsPointsAndFaces[p2].Item2.Add(f2);
        }

        // Go over all points and calculate for each point the new position according to the algorithms formula
        for (int i = 0; i < neighborsPointsAndFaces.Length; i++)
        {
            Vector3 p = mesh.points[i];
            int n = neighborsPointsAndFaces[i].Item1.Count;
            Vector3 r = new Vector3();
            Vector3 f = new Vector3();
            // Use neighbor points to calculate edge midpoints and their average r
            foreach (int neibhorIndexd in neighborsPointsAndFaces[i].Item1)
            {
                Vector3 midPoint = (p + mesh.points[neibhorIndexd]) / 2;
                r += midPoint;
            }

            r = r / n;

            // Use adjacent faces to calculate the average of those facePoints f
            foreach (int faceIndexd in neighborsPointsAndFaces[i].Item2)
            {
                f += mesh.facePoints[faceIndexd];
            }

            f = f / n;
            // Find new position of each point and add to list
            newPoints.Add((f + (2 * r) + ((n - 3) * p)) / n);
        }

        return newPoints;
    }

    // ----------------Private helper functions--------------------

    // Find in which of the 4 quads of the face the points are.
    // faceIndex is the index of the current face, and the points indices correspond to the relevant points
    private static (int, int) QuadOffsets(CCMeshData meshData, int faceIndex, int p1Index, int p2Index)
    {
        int offSet1F1 = 0;
        int offSet2F1 = 0;
        for (int k = 0; k < 4; k++)
        {
            if ((int) meshData.faces[faceIndex][k] == p1Index)
            {
                offSet1F1 = k;
            }

            if ((int) meshData.faces[faceIndex][k] == p2Index)
            {
                offSet2F1 = k;
            }
        }

        return (offSet1F1, offSet2F1);
    }
}