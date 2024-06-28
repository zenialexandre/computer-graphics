#define CG_Debug

using CG_Biblioteca;
using System;
using OpenTK.Graphics.OpenGL4;

namespace gcgcg
{
  internal class Ponto : Objeto
  {
    public Ponto(Objeto _paiRef, ref char _rotulo) : this(_paiRef, ref _rotulo, new Ponto4D())
    {

    }

    public Ponto(Objeto _paiRef, ref char _rotulo, Ponto4D pto) : base(_paiRef, ref _rotulo)
    {
      PrimitivaTipo = PrimitiveType.Points;
      PrimitivaTamanho = 10;

      base.PontosAdicionar(pto);

      Atualizar();
    }

    public void Atualizar()
    {
      base.ObjetoAtualizar();
    }

    public bool isOffLimitsBiggerCircleBBox(BBox biggerCircleBBox, Ponto4D midCirclePoint, Ponto4D newInnerPoint, double radius)
    {
      if (biggerCircleBBox.Dentro(newInnerPoint) && !isOffLimitsEuclideanDistance(midCirclePoint, newInnerPoint, radius))
      {
        return false;
      } else {
        return true;
      }
    }

    protected bool isOffLimitsEuclideanDistance(Ponto4D midCirclePoint, Ponto4D newInnerPoint, double radius)
    {
      double euclideanDistance = CG_Biblioteca.Matematica.distanciaQuadrado(midCirclePoint, newInnerPoint);
      double radiusPowerTwo = Math.Pow(radius, 2);

      return euclideanDistance > radiusPowerTwo;
    }

    public void isOffLimits(BBox innerRectangleBBox, Retangulo innerRectangle)
    {

      if (innerRectangleBBox.Dentro(this.PontosId(0)))
      {
          innerRectangle.SetPrimitivaTipo(true);
      }
      else
      {
          innerRectangle.SetPrimitivaTipo(false);
      }
    }

#if CG_Debug
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Ponto _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
      retorno += base.ImprimeToString();
      return (retorno);
    }
#endif

  }
}
