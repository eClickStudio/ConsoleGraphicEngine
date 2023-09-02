namespace System.Numerics
{
    public static class Vector3Math
    {
        public static Vector3 Sign(Vector3 v)
        {
            return new Vector3(Math.Sign(v.X), Math.Sign(v.Y), Math.Sign(v.Z));
        }

        public static Vector3 Step(Vector3 edge, Vector3 v)
        {
            return new Vector3(Step(edge.X, v.X), Step(edge.Y, v.Y), Step(edge.Z, v.Z));
        }

        private static float Step(float edge, float x)
        {
            if (x > edge)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
