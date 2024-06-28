using CG_Biblioteca;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
    internal class Circulo : Objeto
    {

        private const int GenerationCirclePointsCounter = 390; 

        public Circulo(Objeto _paiRef, ref char _rotulo, double radius) : base(_paiRef, ref _rotulo)
        {
            double increasingDrawingAngle = 1.0;
            PrimitivaTipo = PrimitiveType.LineStrip;
            PrimitivaTamanho = 5;

            UpdateObject(null, null, radius, increasingDrawingAngle);
        }

        public void UpdateObject(Circulo circulo, Ponto4D innerPoint, double radius, double increasingDrawingAngle)
        {
            for (int i = 0; i <= GenerationCirclePointsCounter; i++)
            {
                Ponto4D circlePoint = Matematica.GerarPtosCirculo(increasingDrawingAngle, radius);

                if (circulo == null)
                {
                    base.PontosAdicionar(new Ponto4D(circlePoint.X, circlePoint.Y));
                }
                else
                {
                    circulo.PontosAlterar(new Ponto4D(circlePoint.X + innerPoint.X, circlePoint.Y + innerPoint.Y), i);
                }
                increasingDrawingAngle += 1.0;
            }
        }

        public List<Ponto4D> getPointList()
        {
            return base.pontosLista;
        }
    }
}
