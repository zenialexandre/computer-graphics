#define CG_Debug

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace gcgcg
{
    internal class Spline : Objeto
    {
        public double qtdPontosSpline;
        public Spline(Objeto _paiRef, ref char _rotulo) : base(_paiRef, ref _rotulo)
        {
            PrimitivaTipo = PrimitiveType.LineStrip;
            qtdPontosSpline = 0.1;
        }

        public bool SplinePoints(double points, bool isMinus)
        {
            double result = qtdPontosSpline;
            bool isChanged = false;
            if (isMinus)
            {
                result -= points;
            }
            else
            {
                result += points;
            }

            if (result < 0.8 && result > 0.001)
            {
                qtdPontosSpline = result;
                isChanged = true;
            }

            return isChanged;
        }

        public void drawSpline(List<Ponto4D> list_points_polyhedron)
        {
            List<Ponto4D> points_iteration;
            List<Ponto4D> points_spline = new List<Ponto4D>();

            for (double i = 0; i < 1; i += qtdPontosSpline)
            {
                points_iteration = list_points_polyhedron;
                while (points_iteration.Count != 1)
                {
                    points_iteration = iterationSpline(points_iteration, i);
                }
                points_spline.Add(points_iteration[0]);
            }

            base.pontosLista = points_spline;
            base.ObjetoAtualizar();
        }

        private List<Ponto4D> iterationSpline(List<Ponto4D> iteration_list, double t)
        {
            List<Ponto4D> result_iteration = new List<Ponto4D>();
            double rX = 0.0;
            double rY = 0.0;

            for (int i = 1; i < iteration_list.Count; i++)
            {
                rX = iteration_list[i - 1].X + ((iteration_list[i].X - iteration_list[i - 1].X) * t);
                rY = iteration_list[i - 1].Y + ((iteration_list[i].Y - iteration_list[i - 1].Y) * t);
                result_iteration.Add(new Ponto4D(rX, rY));
            }

            return result_iteration;
        }
    }
}
