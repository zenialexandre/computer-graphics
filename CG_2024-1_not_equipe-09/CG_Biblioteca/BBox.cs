#define CG_Debug
#define CG_Gizmo  // debugar gráfico.
#define CG_OpenGL // render OpenGL.

using System.Collections.Generic;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CG_Biblioteca
{
  public class BBox
  {
    private double menorX, menorY, menorZ, maiorX, maiorY, maiorZ;
    private readonly Ponto4D centro = new();

    private int _vertexBufferObject_bbox;
    private int _vertexArrayObject_bbox;

    private readonly Shader _shaderAmarela;

    public BBox()
    {
      _shaderAmarela = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
      // FIXME: falta deletar ..
      // GL.DeleteProgram(_shaderAmarela.Handle);
      // FIXME: deveria ser removido na classe BBox.
      //GL.DeleteBuffer(_vertexBufferObject_bbox);
      //GL.DeleteVertexArray(_vertexArrayObject_bbox);

    }

    public void Atualizar(Transformacao4D matriz, List<Ponto4D> pontosLista)
    {
      Ponto4D pto = pontosLista[0];
      pto = matriz.MultiplicarPonto(pto);

      this.menorX = pto.X; this.menorY = pto.Y; this.menorZ = pto.Z;
      this.maiorX = pto.X; this.maiorY = pto.Y; this.maiorZ = pto.Z;

      for (var i = 1; i < pontosLista.Count; i++)
      {
        pto = pontosLista[i];
        pto = matriz.MultiplicarPonto(pto);
        Atualizar(matriz, pto);
      }

      ProcessarCentro();
    }

    private void Atualizar(Transformacao4D matriz, Ponto4D pto)
    {
      if (pto.X < menorX)
        menorX = pto.X;
      else
      {
        if (pto.X > maiorX) maiorX = pto.X;
      }
      if (pto.Y < menorY)
        menorY = pto.Y;
      else
      {
        if (pto.Y > maiorY) maiorY = pto.Y;
      }
      if (pto.Z < menorZ)
        menorZ = pto.Z;
      else
      {
        if (pto.Z > maiorZ) maiorZ = pto.Z;
      }
    }

    /// Calcula o ponto do centro da BBox.
    public void ProcessarCentro()
    {
      centro.X = (maiorX + menorX) / 2;
      centro.Y = (maiorY + menorY) / 2;
      centro.Z = (maiorZ + menorZ) / 2;
    }

    /// Verifica se um ponto está dentro da BBox.
    public bool Dentro(Ponto4D pto)
    {
      if ((pto.X >= ObterMenorX && pto.X <= ObterMaiorX) &&
          (pto.Y >= ObterMenorY && pto.Y <= ObterMaiorY) &&
          (pto.Z >= ObterMenorZ && pto.Z <= ObterMaiorZ))
      {
        return true;
      }
      return false;
    }

    /// Obter menor valor X da BBox.
    public double ObterMenorX => menorX;

    /// Obter menor valor Y da BBox.
    public double ObterMenorY => menorY;

    /// Obter menor valor Z da BBox.
    public double ObterMenorZ => menorZ;

    /// Obter maior valor X da BBox.
    public double ObterMaiorX => maiorX;

    /// Obter maior valor Y da BBox.
    public double ObterMaiorY => maiorY;

    /// Obter maior valor Z da BBox.
    public double ObterMaiorZ => maiorZ;

    /// Obter ponto do centro da BBox.
    public Ponto4D ObterCentro => centro;

#if CG_Gizmo
    public void Desenhar(Transformacao4D matrizGrafo)
    {

#if CG_OpenGL && !CG_DirectX

      float[] _bbox =
      {
        (float) menorX,   (float) menorY,   0.0f, // A - canto esquerdo/inferior
        (float) maiorX,   (float) menorY,   0.0f, // B - canto direito/inferior
        (float) maiorX,   (float) maiorY,   0.0f, // C - canto direito/superior
        (float) menorX,   (float) maiorY,   0.0f, // D - canto esquerdo/superior
        (float) centro.X, (float) centro.Y, 0.0f  // E - centro BBox
      };

      _vertexBufferObject_bbox = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_bbox);
      GL.BufferData(BufferTarget.ArrayBuffer, _bbox.Length * sizeof(float), _bbox, BufferUsageHint.StaticDraw);
      _vertexArrayObject_bbox = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject_bbox);
      GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
      GL.EnableVertexAttribArray(0);

      //      var transform = Matrix4.Identity;
      // matrizGrafo = matrizGrafo.MultiplicarMatriz(matriz);

      GL.BindVertexArray(_vertexArrayObject_bbox);
      // _shaderAmarela.SetMatrix4("transform", transform);
      _shaderAmarela.SetMatrix4("transform", matrizGrafo.ObterDadosOpenTK());

      _shaderAmarela.Use();
      GL.DrawArrays(PrimitiveType.LineLoop, 0, ((_bbox.Length - 1) / 3));   // desenha a BBox
      GL.PointSize(20);
      GL.DrawArrays(PrimitiveType.Points, ((_bbox.Length - 1) / 3), 1);     // desenha ponto centro BBox

#elif CG_DirectX && !CG_OpenGL
      Console.WriteLine(" .. Coloque aqui o seu código em DirectX");
#elif (CG_DirectX && CG_OpenGL) || (!CG_DirectX && !CG_OpenGL)
      Console.WriteLine(" .. ERRO de Render - escolha OpenGL ou DirectX !!");
#endif
    }
#endif

#if CG_Debug
    public override string ToString()
    {
      string retorno;
      retorno = "_____ BBox: \n";
      retorno += "menorX: " + menorX + " - maiorX: " + maiorX + "\n";
      retorno += "menorY: " + menorY + " - maiorY: " + maiorY + "\n";
      retorno += "menorZ: " + menorZ + " - maiorZ: " + maiorZ + "\n";
      retorno += "  centroX: " + centro.X + " - centroY: " + centro.Y + " - centroZ: " + centro.Z + "\n";
      retorno += "__________________________________ \n";
      return (retorno);
    }
#endif

  }
}