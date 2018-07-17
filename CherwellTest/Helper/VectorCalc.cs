namespace CherwellTest.Helper
{
    public class VectorCalc
    {
        public static int[] VectorSub(int[] v0, int[] v1)
        {
            return new int[] { v0[0] - v1[0], v0[1] - v1[1] };
        }

        //only using for comparision so no root needed here
        public static int VectorLength(int[] v0)
        {
            return (v0[0] * v0[0]) + (v0[1] * v0[1]);
        }
    }
}