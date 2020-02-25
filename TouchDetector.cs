using System;
using System.Collections.Generic;
using OpenCvSharp;
using OpenCvSharp.Aruco;

namespace TouchDetectionLibrary
{
    public class TouchDetector
    {
        public void DetectTouchByTwoCamera(Action action)
        {
            using (var video0 = new VideoCapture(0))
            using (var video1 = new VideoCapture(1))
            using (var window0 = new Window("capture0"))
            using (var window1 = new Window("capture1"))
            {
                var dictionary = CvAruco.GetPredefinedDictionary(PredefinedDictionaryName.Dict6X6_250);
                var parameters = DetectorParameters.Create();

                var frames = new List<Mat> { new Mat(), new Mat() };
                var videos = new List<VideoCapture>();
                var windows = new List<Window>();

                Point2f[][] corners;
                int[] ids;
                int[] previousIds;
                Point2f[][] rejectedImgPoints;

                videos.Add(video0);
                videos.Add(video1);
                windows.Add(window0);
                windows.Add(window1);

                var wasFoundList = new List<bool> { false, false };
                var isTouchedList = new List<bool> { false, false };
                var wasTouched = false;

                while (true)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        videos[i].Read(frames[i]);

                        CvAruco.DetectMarkers(frames[i], dictionary, out corners, out ids, parameters, out rejectedImgPoints);

                        isTouchedList[i] = wasFoundList[i] && !(ids.Length > 0);

                        if (ids.Length > 0)
                        {
                            wasFoundList[i] = true;
                            CvAruco.DrawDetectedMarkers(frames[i], corners, ids);
                        }
                        else
                        {
                            wasFoundList[i] = false;
                            isTouchedList[i] = true;
                        }

                        windows[i].ShowImage(frames[i]);
                    }

                    if (!isTouchedList.Contains(false))
                    {
                        if (!wasTouched)
                        {
                            Console.WriteLine("Hello world!");
                            for (int i = 0; i < isTouchedList.Count; i++)
                            {
                                isTouchedList[i] = false;
                            }
                        }
                        wasTouched = true;
                    }
                    else
                    {
                        wasTouched = false;
                    }

                    var key = Cv2.WaitKey(1);
                    if (key == 'q')
                    {
                        break;
                    }
                }
            }
        }
    }
}
