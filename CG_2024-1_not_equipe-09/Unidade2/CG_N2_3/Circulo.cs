using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
    internal class Circulo : Objeto
    {
        private const int GenerationCirclePointsCounter = 72; 

        public Circulo(Objeto _paiRef, ref char _rotulo, double radius) : base(_paiRef, ref _rotulo)
        {
            double increasingDrawingAngle = 5.0;
            PrimitivaTipo = PrimitiveType.Points;
            PrimitivaTamanho = 5;

            for (int i = 0; i <= GenerationCirclePointsCounter; i++)
            {
                base.PontosAdicionar(Matematica.GerarPtosCirculo(increasingDrawingAngle, radius));
                Atualizar();
                increasingDrawingAngle += 5.0;
            }
        }

        protected void Atualizar()
        {
            base.ObjetoAtualizar();
        }
    }
}
