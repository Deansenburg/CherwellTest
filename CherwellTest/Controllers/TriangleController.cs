using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CherwellTest.Helper;
using CherwellTest.Model;

namespace CherwellTest.Controller
{
    public class TriangleController : ApiController
    {
        int size = 10;

        [Route("api/lookup/{row}/{column}")]
        public HttpResponseMessage GetTriangle(char row, int column)
        {
            row = Char.ToUpper(row);
            int intRow = InterpolateCalc.Interpolate(InterpolateCalc.Map(row, 'A', 'F'), 0, 5);
            if (intRow < 0 || intRow > 5 || column < 1 || column > 12)
            {
                String err = String.Format("Row({0})/Column({1}) is outside the range A-F/1-12", row, column);
                return Request.CreateErrorResponse((HttpStatusCode)422, err);
            }
            int intColumn = column - 1;

            //top left point of the triangle
            int[] p0 = new int[] { size * (int)Math.Floor((double)intColumn / 2), size *  intRow};
            //bottom right point of the triangle
            int[] p1 = new int[] { p0[0] + size, p0[1] + size};
            //when column is even the third point is horizontal from the origin
            int[] p2 = column % 2 == 1 ?
                new int[] { p0[0], p0[1] + size} :
                new int[] { p0[0] + size, p0[1] };

            var triangle = new Triangle(row, column, new int[][] { p0, p1, p2});
            return Request.CreateResponse(HttpStatusCode.OK, triangle);
        }

        private int[][] RearrangeTriangle(int[] p0, int[] p1, int[] p2, int[] v)
        {
            int[][] triangle = new int[3][];
            //since vector will be diagonal we only need to check the x value
            if (v[0] < 0)
            {
                triangle[0] = p0;
                triangle[1] = p1;
                triangle[2] = p2;
            }
            else
            {
                triangle[0] = p1;
                triangle[1] = p0;
                triangle[2] = p2;
            }
            return triangle;
        }

        [Route("api/reverse")]
        public HttpResponseMessage GetRowColumn([FromUri]int[] p0, [FromUri]int[] p1, [FromUri]int[] p2)
        {
            //check if the point lies on the grid
            if (p0[0] % size != 0 || p0[1] % size != 0 ||
                p1[0] % size != 0 || p1[1] % size != 0 ||
                p2[0] % size != 0 || p2[1] % size != 0)
            {
                String err = "Points provided do not lie on the size grid";
                return Request.CreateErrorResponse((HttpStatusCode)422, err);
            }

            //there is no gaurenteed order that the points come in as so we need to
            //find orientation of the triangle, ie which points correspond to which point on the triangle
            int[] v01 = VectorCalc.VectorSub(p0, p1);
            int[] v02 = VectorCalc.VectorSub(p0, p2);
            int s01 = VectorCalc.VectorLength(v01);
            int s02 = VectorCalc.VectorLength(v02);

            int[][] trianglePoints;
            //s01 is the hypot vector
            if (s01 > s02)
            {
                trianglePoints = RearrangeTriangle(p0, p1, p2, v01);
            }
            //s02 is the hypot vector
            else if (s01 < s02)
            {
                trianglePoints = RearrangeTriangle(p0, p2, p1, v02);
            }
            //s03 is the hypot vector
            else
            {
                int[] v03 = VectorCalc.VectorSub(p1, p2);
                trianglePoints = RearrangeTriangle(p1, p2, p0, v03);
            }

            int rowInt = trianglePoints[0][1] / size;
            int columnInt = trianglePoints[0][0] / size;
            if (rowInt < 0 || rowInt > 5 || columnInt < 0 || columnInt > 5)
            {
                String err = "Points provided do not correspond to a triangle";
                return Request.CreateErrorResponse((HttpStatusCode)422, err);
            }

            //take the topLeft point, apply reverse steps
            char row = (char)InterpolateCalc.Interpolate(InterpolateCalc.Map(rowInt, 0, 5), 'A', 'F');
            int column = (columnInt * 2) + 1;
            //if vector from topLeft to rightAngle is horizontal, then add one
            int[] v04 = VectorCalc.VectorSub(trianglePoints[0], trianglePoints[2]);
            if (v04[1] == 0)
            {
                column += 1;
            }

            var triangle = new Triangle(row, column, trianglePoints);
            return Request.CreateResponse(HttpStatusCode.OK, triangle);
        }
    }
}
