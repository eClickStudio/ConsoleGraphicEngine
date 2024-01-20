//using System;
//using System.Numerics;

//namespace RayTracingGraphicEngine3D.RayTracingEngine.Tools
//{
//    public static class Vector3Math
//    {
//        public static bool IsNormal(this Vector3 v)
//        {
//            return float.IsNormal(v.X) && float.IsNormal(v.Y) && float.IsNormal(v.Z);
//        }

//        public static Vector3 Sign(this Vector3 v)
//        {
//            return new Vector3(Math.Sign(v.X), Math.Sign(v.Y), Math.Sign(v.Z));
//        }

//        public static Vector3 Step(this Vector3 v, Vector3 edge)
//        {
//            return new Vector3(Step(edge.X, v.X), Step(edge.Y, v.Y), Step(edge.Z, v.Z));
//        }

//        private static float Step(float edge, float x)
//        {
//            if (x > edge)
//            {
//                return 1;
//            }
//            else
//            {
//                return 0;
//            }
//        }
//    }
//}
