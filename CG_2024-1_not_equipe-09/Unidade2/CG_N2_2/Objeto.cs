#define CG_OpenGL
#define CG_Debug
// #define CG_DirectX

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;

namespace gcgcg
{
  internal class Objeto  //TODO: deveria ser uma class abstract ..??
  {
    // Objeto
    private readonly char rotulo;
    protected Objeto paiRef;
    private List<Objeto> objetosLista = new List<Objeto>();
    private PrimitiveType primitivaTipo = PrimitiveType.LineLoop;
    public PrimitiveType PrimitivaTipo { get => primitivaTipo; set => primitivaTipo = value; }
    private float primitivaTamanho = 1;
    public float PrimitivaTamanho { get => primitivaTamanho; set => primitivaTamanho = value; }
    private Shader _shaderObjeto = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
    public Shader shaderObjeto { set => _shaderObjeto = value; }

    // Vértices do objeto
    //TODO: o objeto mundo deveria ter estes atributos abaixo?
    protected List<Ponto4D> pontosLista = new List<Ponto4D>();
    private int _vertexBufferObject;
    private int _vertexArrayObject;

    // BBox do objeto
    private BBox bBox = new BBox();
    public BBox Bbox()  //TODO: readonly
    {
      return bBox;
    }

    // Transformações do objeto
    private Transformacao4D matriz = new Transformacao4D();

    public Objeto(Objeto _paiRef, ref char _rotulo, Objeto objetoFilho = null)
    {
      this.paiRef = _paiRef;
      rotulo = _rotulo = Utilitario.CharProximo(_rotulo);
      if (_paiRef != null)
      {
        ObjetoAdicionar(objetoFilho);
      }
    }

    private void ObjetoAdicionar(Objeto objetoFilho)
    {
      if (objetoFilho == null)
      {
        paiRef.objetosLista.Add(this);
      }
      else
      {
        paiRef.FilhoAdicionar(objetoFilho);
      }
    }

    public void ObjetoAtualizar()
    {
      float[] vertices = new float[pontosLista.Count * 3];
      int ptoLista = 0;
      for (int i = 0; i < vertices.Length; i += 3)
      {
        vertices[i] = (float)pontosLista[ptoLista].X;
        vertices[i + 1] = (float)pontosLista[ptoLista].Y;
        vertices[i + 2] = (float)pontosLista[ptoLista].Z;
        ptoLista++;
      }
      bBox.Atualizar(matriz, pontosLista);

      GL.PointSize(primitivaTamanho);

      _vertexBufferObject = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
      GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);
      _vertexArrayObject = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject);
      GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
      GL.EnableVertexAttribArray(0);
    }

    public void Desenhar()
    {
#if CG_OpenGL && !CG_DirectX
      GL.PointSize(primitivaTamanho);

      GL.BindVertexArray(_vertexArrayObject);
      _shaderObjeto.Use();
      GL.DrawArrays(primitivaTipo, 0, pontosLista.Count);
#elif CG_DirectX && !CG_OpenGL
      Console.WriteLine(" .. Coloque aqui o seu código em DirectX");
#elif (CG_DirectX && CG_OpenGL) || (!CG_DirectX && !CG_OpenGL)
      Console.WriteLine(" .. ERRO de Render - escolha OpenGL ou DirectX !!");
#endif
      for (var i = 0; i < objetosLista.Count; i++)
      {
        objetosLista[i].Desenhar();
      }
    }

    #region Objeto: CRUD

    public void FilhoAdicionar(Objeto filho)
    {
      this.objetosLista.Add(filho);
    }

    public Ponto4D PontosId(int id)
    {
      return pontosLista[id];
    }

    public void PontosAdicionar(Ponto4D pto)
    {
      pontosLista.Add(pto);
      ObjetoAtualizar();
    }

    public void PontosAlterar(Ponto4D pto, int posicao)
    {
      pontosLista[posicao] = pto;
      ObjetoAtualizar();
    }

    #endregion

    #region Objeto: Grafo de Cena

    public Objeto GrafocenaBusca(char _rotulo)
    {
      if (rotulo == _rotulo)
      {
        return this;
      }
      foreach (var objeto in objetosLista)
      {
        var obj = objeto.GrafocenaBusca(_rotulo);
        if (obj != null)
        {
          return obj;
        }
      }
      return null;
    }

    public Objeto GrafocenaBuscaProximo(Objeto objetoAtual)
    {
      objetoAtual = GrafocenaBusca(Utilitario.CharProximo(objetoAtual.rotulo));
      if (objetoAtual != null)
      {
        return objetoAtual;
      }
      else
      {
        return GrafocenaBusca(Utilitario.CharProximo('@'));
      }
    }

    public void GrafocenaImprimir(String idt)
    {
      System.Console.WriteLine(idt + rotulo);
      foreach (var objeto in objetosLista)
      {
        objeto.GrafocenaImprimir(idt + "  ");
      }
    }

    #endregion



    public void OnUnload()
    {
      foreach (var objeto in objetosLista)
      {
        objeto.OnUnload();
      }

      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      GL.BindVertexArray(0);
      GL.UseProgram(0);

      GL.DeleteBuffer(_vertexBufferObject);
      GL.DeleteVertexArray(_vertexArrayObject);

      GL.DeleteProgram(_shaderObjeto.Handle);
    }

#if CG_Debug
    protected string ImprimeToString()
    {
      string retorno;
      retorno = "__ Objeto: " + rotulo + "\n";
      for (var i = 0; i < pontosLista.Count; i++)
      {
        retorno += "P" + i + "[ " +
        string.Format("{0,10}", pontosLista[i].X) + " | " +
        string.Format("{0,10}", pontosLista[i].Y) + " | " +
        string.Format("{0,10}", pontosLista[i].Z) + " | " +
        string.Format("{0,10}", pontosLista[i].W) + " ]" + "\n";
      }
      retorno += bBox.ToString();
      return (retorno);
    }
#endif

  }
}