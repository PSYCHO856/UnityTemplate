using System;
using System.Collections.Generic;
using UnityEngine;

public static class PathHelper
{
    //根据提供的路径获取平滑路径
    public static void GetWayPoints(Vector3[] points, int amountRadio, List<Vector3> wayPoints)
    {
        if (points == null || points.Length <= 1)
        {
            Debug.Log("points is empty!");
            return;
        }

        wayPoints.Clear();

        var vector3s = PathControlPointGenerator(points);

        var prevPt = Interp(vector3s, 0);
        var SmoothAmount = (points.Length - 1) * amountRadio;
        for (var i = 1; i <= SmoothAmount; i++)
        {
            var pm = (float) i / SmoothAmount;
            var currPt = Interp(vector3s, pm);
            wayPoints.Add(currPt);
            prevPt = currPt;
        }
    }

    //Gizmos平滑的绘制提供的路径
    public static void DrawPathHelper(Vector3[] path, Color color)
    {
        var vector3s = PathControlPointGenerator(path);

        var prevPt = Interp(vector3s, 0);
        Gizmos.color = color;
        var SmoothAmount = path.Length * 20;
        for (var i = 1; i <= SmoothAmount; i++)
        {
            var pm = (float) i / SmoothAmount;
            var currPt = Interp(vector3s, pm);
            Gizmos.DrawLine(currPt, prevPt);
            prevPt = currPt;
        }
    }

    //计算路径的长度
    public static float PathLength(Vector3[] path)
    {
        float pathLength = 0;

        var vector3s = PathControlPointGenerator(path);

        var prevPt = Interp(vector3s, 0);
        var SmoothAmount = path.Length * 20;
        for (var i = 1; i <= SmoothAmount; i++)
        {
            var pm = (float) i / SmoothAmount;
            var currPt = Interp(vector3s, pm);
            pathLength += Vector3.Distance(prevPt, currPt);
            prevPt = currPt;
        }

        return pathLength;
    }

    //生成曲线控制点,path.length>=2（为路径添加首尾点，便于绘制Cutmull-Rom曲线）
    private static Vector3[] PathControlPointGenerator(Vector3[] path)
    {
        Vector3[] suppliedPath;
        Vector3[] vector3s;

        suppliedPath = path;

        var offset = 2;
        vector3s = new Vector3[suppliedPath.Length + offset];
        Array.Copy(suppliedPath, 0, vector3s, 1, suppliedPath.Length);

        //计算第一个控制点和最后一个控制点位置
        vector3s[0] = vector3s[1] + (vector3s[1] - vector3s[2]);
        vector3s[vector3s.Length - 1] = vector3s[vector3s.Length - 2] +
                                        (vector3s[vector3s.Length - 2] - vector3s[vector3s.Length - 3]);

        //首位点重合时，形成闭合的Catmull-Rom曲线
        if (vector3s[1] == vector3s[vector3s.Length - 2])
        {
            var tmpLoopSpline = new Vector3[vector3s.Length];
            Array.Copy(vector3s, tmpLoopSpline, vector3s.Length);
            tmpLoopSpline[0] = tmpLoopSpline[tmpLoopSpline.Length - 3];
            tmpLoopSpline[tmpLoopSpline.Length - 1] = tmpLoopSpline[2];
            vector3s = new Vector3[tmpLoopSpline.Length];
            Array.Copy(tmpLoopSpline, vector3s, tmpLoopSpline.Length);
        }

        return vector3s;
    }

    //Catmull-Rom曲线 
    private static Vector3 Interp(Vector3[] pts, float t)
    {
        var numSections = pts.Length - 3;
        var currPt = Mathf.Min(Mathf.FloorToInt(t * numSections), numSections - 1);
        var u = t * numSections - currPt;

        var a = pts[currPt];
        var b = pts[currPt + 1];
        var c = pts[currPt + 2];
        var d = pts[currPt + 3];

        return .5f * (
            (-a + 3f * b - 3f * c + d) * (u * u * u) +
            (2f * a - 5f * b + 4f * c - d) * (u * u) +
            (-a + c) * u +
            2f * b
        );
    }
}