using System;
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
    internal class SrPalito : Objeto
    {
        private const int FinalPointAngle = 45;
        private const double FinalPointRadius = 0.5;

        public SrPalito(Objeto _paiRef, ref char _rotulo) : base(_paiRef, ref _rotulo)
        {
            PrimitivaTipo = PrimitiveType.Lines;
            PrimitivaTamanho = 1;

            Ponto4D initialPoint = new Ponto4D(0.0, 0.0, 0.0);
            Ponto4D finalPoint = CG_Biblioteca.Matematica.GerarPtosCirculo(FinalPointAngle, FinalPointRadius);

            base.PontosAdicionar(initialPoint);
            base.PontosAdicionar(finalPoint);
            base.ObjetoAtualizar();
        }
    }
}
