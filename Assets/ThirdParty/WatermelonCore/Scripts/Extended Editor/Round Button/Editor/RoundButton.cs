using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Watermelon
{
    public class RoundButton
    {
        private Texture2D baseTexture;
        private int buttonSegments;

        public byte[] buttonValues;

        private Point center;
        private int radius;
        private int radiusSquared;

        private Sector[] sectors;
        private Texture2D tempTexture;

        private float uiSize;

        public void Init(int buttonSegments, Texture2D baseTexture)
        {
            this.buttonSegments = buttonSegments;
            this.baseTexture = baseTexture;

            buttonValues = new byte[buttonSegments];

            ResetTempTexture();

            var imageSize = baseTexture.width;
            radiusSquared = imageSize * imageSize / 4;

            radius = imageSize / 2;
            center = new Point(radius, radius);

            sectors = GetSectors();

            CutSectorsToButtons();
        }

        public void ResetTempTexture()
        {
            tempTexture = new Texture2D(baseTexture.width, baseTexture.height);
            tempTexture.SetPixels(baseTexture.GetPixels());
            tempTexture.Apply();
        }

        public void VisualizeSector(int segmentIndex)
        {
            if (sectors.IsInRange(segmentIndex))
            {
                var nextSector = segmentIndex + 1;
                if (nextSector >= sectors.Length)
                    nextSector = 0;

                for (var x = sectors[segmentIndex].leftSideX; x < sectors[segmentIndex].rightSideX; x++)
                for (var y = sectors[segmentIndex].bottomSideY; y < sectors[segmentIndex].topSideY; y++)
                    if (IsInsideSector(new Point(x, y), center, sectors[segmentIndex].coordinates - center,
                        sectors[nextSector].coordinates - center, radiusSquared))
                        tempTexture.SetPixel(x, y, Color.black);

                tempTexture.Apply();
            }
        }

        public void VisualizeDots()
        {
            for (var i = 0; i < sectors.Length; i++)
            {
                tempTexture.DrawBigDot(sectors[i].coordinates.x, sectors[i].coordinates.y, 10, EditorColor.blue01);
                tempTexture.DrawLine(new Vector2(center.x, center.y),
                    new Vector2(sectors[i].coordinates.x, sectors[i].coordinates.y), Color.black);

                for (var j = 0; j < sectors[i].calculateDots.Count; j++)
                    tempTexture.DrawBigDot(sectors[i].calculateDots[j].x, sectors[i].calculateDots[j].y, 10,
                        EditorColor.green01);
            }

            tempTexture.Apply();
        }

        public void VisualizeSegmentBox(int segmentIndex)
        {
            if (sectors.IsInRange(segmentIndex))
            {
                for (var x = sectors[segmentIndex].leftSideX; x < sectors[segmentIndex].rightSideX; x++)
                for (var y = sectors[segmentIndex].bottomSideY; y < sectors[segmentIndex].topSideY; y++)
                    tempTexture.SetPixel(x, y, Color.red);

                tempTexture.Apply();
            }
        }

        public void DrawCirlceButtons()
        {
            var button = false;

            DrawCirlceButtons(ref button);
        }

        public void DrawCirlceButtons(ref bool button)
        {
            button = false;

            if (tempTexture != null)
            {
                var e = Event.current;
                var zoneRect = EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true),
                    GUILayout.Height(Screen.width), GUILayout.MinHeight(Screen.width));
                uiSize = zoneRect.width;
                var mousePosition = e.mousePosition;

                var sizeDiff = uiSize / baseTexture.width;
                for (var i = 0; i < sectors.Length; i++)
                {
                    var isSelected = buttonValues[i] == 1;
                    if (isSelected)
                        GUI.color = Color.black;

                    if (sectors[i].cuttetButton != null)
                    {
                        var finishRect = new Rect(zoneRect.x + sectors[i].buttonRect.x * sizeDiff,
                            zoneRect.y + sectors[i].buttonRect.y * sizeDiff, sectors[i].buttonRect.width * sizeDiff,
                            sectors[i].buttonRect.height * sizeDiff);

                        GUI.DrawTexture(finishRect, sectors[i].cuttetButton);
                        if (finishRect.Contains(mousePosition))
                        {
                            HandleUtility.Repaint();

                            if (e.type == EventType.MouseDown)
                            {
                                var relativeClick = new Vector2((mousePosition.x - finishRect.x) / sizeDiff,
                                    (finishRect.height - (mousePosition.y - finishRect.y)) / sizeDiff);

                                if (sectors[i].cuttetButton.GetPixel((int) relativeClick.x, (int) relativeClick.y).a >
                                    0.5f)
                                {
                                    buttonValues[i] = buttonValues[i] == 0 ? (byte) 1 : (byte) 0;

                                    button = true;
                                }
                            }
                        }
                    }

                    if (isSelected)
                        GUI.color = Color.white;
                }

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndVertical();
            }
        }

        private void CutSectorsToButtons()
        {
            for (var i = 0; i < sectors.Length; i++)
            {
                var nextSector = i + 1;
                if (nextSector >= sectors.Length)
                    nextSector = 0;

                double centerAngle = 0;
                var halfPoint = GetPositionsHalfAngle(sectors[i].angle, sectors[nextSector].angle, ref centerAngle);

                double leftCenterAngle = 0;
                var leftHalfPoint = GetPositionsHalfAngle(centerAngle, sectors[nextSector].angle, ref leftCenterAngle);

                double rightCenterAngle = 0;
                var rightHalfPoint = GetPositionsHalfAngle(sectors[i].angle, centerAngle, ref rightCenterAngle);

                sectors[i].calculateDots = new List<Point>();
                sectors[i].calculateDots.Add(center); // Add center dot
                sectors[i].calculateDots.Add(sectors[i].coordinates); // Add sector side coordinates
                sectors[i].calculateDots.Add(sectors[nextSector].coordinates); // Add next sector side coordinates
                sectors[i].calculateDots.Add(halfPoint); // Add half point of arc
                sectors[i].calculateDots.Add(leftHalfPoint); // Add left half point of half arc
                sectors[i].calculateDots.Add(rightHalfPoint); // Add right half point of half arc

                var calculatedDotsCount = sectors[i].calculateDots.Count;

                //Calculate box
                sectors[i].leftSideX = 0;
                sectors[i].bottomSideY = 0;
                sectors[i].rightSideX = baseTexture.width;
                sectors[i].topSideY = baseTexture.height;

                //Get left side
                do
                {
                    var changeSize = false;
                    for (var d = 0; d < calculatedDotsCount; d++)
                        if (sectors[i].leftSideX < sectors[i].calculateDots[d].x)
                        {
                            changeSize = true;
                        }
                        else
                        {
                            changeSize = false;

                            break;
                        }

                    if (changeSize)
                        sectors[i].leftSideX++;
                    else
                        break;
                } while (sectors[i].leftSideX < baseTexture.width);

                //Get right side
                do
                {
                    var changeSize = false;
                    for (var d = 0; d < calculatedDotsCount; d++)
                        if (sectors[i].rightSideX > sectors[i].calculateDots[d].x)
                        {
                            changeSize = true;
                        }
                        else
                        {
                            changeSize = false;

                            break;
                        }

                    if (changeSize)
                        sectors[i].rightSideX--;
                    else
                        break;
                } while (sectors[i].rightSideX > 0);

                //Get top side
                do
                {
                    var changeSize = false;
                    for (var d = 0; d < calculatedDotsCount; d++)
                        if (sectors[i].topSideY > sectors[i].calculateDots[d].y)
                        {
                            changeSize = true;
                        }
                        else
                        {
                            changeSize = false;

                            break;
                        }

                    if (changeSize)
                        sectors[i].topSideY--;
                    else
                        break;
                } while (sectors[i].topSideY > 0);

                //Get bottom side
                do
                {
                    var changeSize = false;
                    for (var d = 0; d < calculatedDotsCount; d++)
                        if (sectors[i].bottomSideY < sectors[i].calculateDots[d].y)
                        {
                            changeSize = true;
                        }
                        else
                        {
                            changeSize = false;

                            break;
                        }

                    if (changeSize)
                        sectors[i].bottomSideY++;
                    else
                        break;
                } while (sectors[i].bottomSideY < baseTexture.height);

                //Cut button
                var width = Mathf.Abs(sectors[i].rightSideX - sectors[i].leftSideX);
                var height = Mathf.Abs(sectors[i].topSideY - sectors[i].bottomSideY);

                var coordinates = new Vector2(sectors[i].leftSideX, baseTexture.height - sectors[i].topSideY);
                sectors[i].buttonRect = new Rect(coordinates.x, coordinates.y, width, height);

                var tempButtonTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);
                var tempX = 0;
                var tempY = 0;
                for (var x = sectors[i].leftSideX; x < sectors[i].rightSideX; x++)
                {
                    for (var y = sectors[i].bottomSideY; y < sectors[i].topSideY; y++)
                    {
                        if (IsInsideSector(new Point(x, y), center, sectors[i].coordinates - center,
                            sectors[nextSector].coordinates - center, radiusSquared))
                            tempButtonTexture.SetPixel(tempX, tempY, tempTexture.GetPixel(x, y));
                        else
                            tempButtonTexture.SetPixel(tempX, tempY, Color.clear);

                        tempY++;
                    }

                    tempX++;
                }

                tempButtonTexture.Apply();

                sectors[i].cuttetButton = tempButtonTexture;

                var buttonStyle = new GUIStyle();
                buttonStyle.normal.background = tempButtonTexture;

                sectors[i].buttonStyle = buttonStyle;
            }
        }

        private Sector[] GetSectors()
        {
            double angle = 0;

            var tempSectors = new Sector[buttonSegments];
            for (var i = 0; i < buttonSegments; i++)
            {
                angle = i * (360 / buttonSegments);

                var tempPoint = new Point((int) (center.x + radius * Math.Cos(Math.PI * angle / 180.0)),
                    (int) (center.y + radius * Math.Sin(Math.PI * angle / 180.0)));

                tempSectors[i] = new Sector(tempPoint, angle);
            }

            return tempSectors;
        }

        private Point GetPositionsHalfAngle(double startAngle, double endAngle, ref double centerAngle)
        {
            if (startAngle >= 180 && endAngle == 0)
                endAngle = 360;

            centerAngle = startAngle + (endAngle - startAngle) / 2;

            return new Point((int) (center.x + radius * Math.Cos(Math.PI * centerAngle / 180.0)),
                (int) (center.y + radius * Math.Sin(Math.PI * centerAngle / 180.0)));
        }

        private bool IsInsideSector(Point point, Point center, Point sectorStart, Point sectorEnd, float radius)
        {
            var relPoint = new Point(point.x - center.x, point.y - center.y);

            return !AreClockwise(sectorStart, relPoint) && AreClockwise(sectorEnd, relPoint) &&
                   IsWithingRadius(relPoint, radius);
        }

        private bool AreClockwise(Point sectorStart, Point relPoint)
        {
            return -sectorStart.x * relPoint.y + sectorStart.y * relPoint.x > 0;
        }

        private bool IsWithingRadius(Point relPoint, float radius)
        {
            return relPoint.x * relPoint.x + relPoint.y * relPoint.y <= radius;
        }

        private class Point
        {
            public readonly int x;
            public readonly int y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public override string ToString()
            {
                return GetType() + " (" + x + ":" + y + ")";
            }

            public static Point operator +(Point point1, Point point2)
            {
                return new(point1.x + point2.x, point1.y + point2.y);
            }

            public static Point operator -(Point point1, Point point2)
            {
                return new(point1.x - point2.x, point1.y - point2.y);
            }
        }

        private class Sector
        {
            public readonly double angle;
            public int bottomSideY;

            public Rect buttonRect;

            public GUIStyle buttonStyle;

            public List<Point> calculateDots = new();
            public readonly Point coordinates;
            public Texture2D cuttetButton;

            public int leftSideX;
            public int rightSideX;
            public int topSideY;

            public Sector(Point coordinates, double angle)
            {
                this.coordinates = coordinates;
                this.angle = angle;
            }
        }
    }
}