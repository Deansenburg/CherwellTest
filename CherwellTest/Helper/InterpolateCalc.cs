namespace CherwellTest.Helper
{
    public class InterpolateCalc
    {
        //maps 0 to min, 1 to max
        public static double Map(double value, double min, double max)
        {
            return (value - min) / (max - min);
        }
        //given a value, usually between 0-1, gives a value between min and max
        public static int Interpolate(double value, int min, int max)
        {
            return (int)((max - min) * value) + min;
        }
    }
}