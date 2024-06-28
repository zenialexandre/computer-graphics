//https://github.com/mono/opentk/blob/main/Source/Examples/Shapes/Old/Cube.cs

#define CG_Debug
using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using System.Collections.Generic;

namespace gcgcg
{
  internal class Cubo : Objeto
  {
    private readonly Ponto4D[] _vertices;
    // int[] indices;
    // Vector3[] normals;
    // int[] colors;

    private readonly float[] _frontFaceVertices =
    [
      // Position         Texture coordinates
      -1.0005f, -1.0005f, 1.0005f, 0.0f, 0.0f,
      1.0005f, -1.0005f, 1.0005f, 1.0f, 0.0f,
      1.0005f, 1.0005f, 1.0005f, 1.0f, 1.0f,
      -1.0005f, 1.0005f, 1.0005f, 0.0f, 1.0f,
    ];
    
    private readonly float[] _backFaceVertices =
    [
      // Position         Texture coordinates
      -1.0005f, -1.0005f, -1.0005f, 0.0f, 0.0f,
      1.0005f, -1.0005f, -1.0005f, 1.0f, 0.0f,
      1.0005f, 1.0005f, -1.0005f, 1.0f, 1.0f,
      -1.0005f, 1.0005f, -1.0005f, 0.0f, 1.0f
    ];

    private readonly float[] _topFaceVertices =
    [
      // Position         Texture coordinates
      -1.0005f, 1.0005f, 1.0005f, 0.0f, 0.0f,
      1.0005f, 1.0005f, -1.0005f, 1.0f, 1.0f,
      1.0005f, 1.0005f, 1.0005f, 1.0f, 0.0f,
      -1.0005f, 1.0005f, -1.0005f, 0.0f, 1.0f
    ];

    private readonly float[] _bottomFaceVertices =
    [
      // Position         Texture coordinates
      1.0005f, -1.0005f, -1.0005f, 0.0f, 0.0f,
      -1.0005f, -1.0005f, 1.0005f, 1.0f, 1.0f,
      -1.0005f, -1.0005f, -1.0005f, 1.0f, 0.0f,
      1.0005f, -1.0005f, 1.0005f, 0.0f, 1.0f
    ];

    public readonly float[] _rightFaceVertices =
    [
      // Position         Texture coordinates
      1.0005f, -1.0005f, -1.0005f, 0.0f, 0.0f,
      1.0005f, 1.0005f, 1.0005f, 1.0f, 1.0f,
      1.0005f, -1.0005f, 1.0005f, 1.0f, 0.0f,
      1.0005f, 1.0005f, -1.0005f, 0.0f, 1.0f
    ];

    public readonly float[] _leftFaceVertices =
    [
      // Position         Texture coordinates
      -1.0005f, -1.0005f, 1.0005f, 0.0f, 0.0f,
      -1.0005f, 1.0005f, -1.0005f, 1.0f, 1.0f,
      -1.0005f, -1.0005f, -1.0005f, 1.0f, 0.0f,
      -1.0005f, 1.0005f, 1.0005f, 0.0f, 1.0f
    ];

    private readonly uint[] _frontFaceIndices =
    [
      1, 2, 3,
      0, 1, 3,
    ];

    private readonly uint[] _backFaceIndices =
    [
      1, 2, 3,
      0, 1, 3
    ];

    private readonly uint[] _topFaceIndices =
    [
      3, 0, 1,
      0, 2, 1
    ];

    private readonly uint[] _bottomFaceIndices =
    [
      3, 0, 1,
      0, 2, 1
    ];

    private readonly uint[] _rightFaceIndices =
    [
      1, 0, 2,
      3, 0, 1
    ];

    private readonly uint[] _leftFaceIndices =
    [
      1, 0, 2,
      3, 0, 1
    ];

    public Cubo(Objeto _paiRef, ref char _rotulo, float decreaseSizeBy) : base(_paiRef, ref _rotulo)
    {
      PrimitivaTipo = PrimitiveType.TriangleFan;
      float cubeSize = 1.0f - decreaseSizeBy;

      _vertices =
      [
        new Ponto4D(-cubeSize, -cubeSize, cubeSize),
        new Ponto4D(cubeSize, -cubeSize, cubeSize),
        new Ponto4D(cubeSize, cubeSize, cubeSize),
        new Ponto4D(-cubeSize, cubeSize, cubeSize),
        new Ponto4D(-cubeSize, -cubeSize, -cubeSize),
        new Ponto4D(cubeSize, -cubeSize, -cubeSize),
        new Ponto4D(cubeSize, cubeSize, -cubeSize),
        new Ponto4D(-cubeSize, cubeSize, -cubeSize)
      ];

      // // 0, 1, 2, 3 Face da frente
      base.PontosAdicionar(_vertices[0]);
      base.PontosAdicionar(_vertices[1]);
      base.PontosAdicionar(_vertices[2]);
      base.PontosAdicionar(_vertices[3]);

      // // 3, 2, 6, 7 Face de cima
      base.PontosAdicionar(_vertices[3]);
      base.PontosAdicionar(_vertices[2]);
      base.PontosAdicionar(_vertices[6]);
      base.PontosAdicionar(_vertices[7]);

      // Ajuste de rendericazao da parte de cima do Cubo.
      base.PontosAdicionar(_vertices[3]);
      base.PontosAdicionar(_vertices[2]);
      base.PontosAdicionar(_vertices[7]);
      base.PontosAdicionar(_vertices[6]);

      // // 4, 7, 6, 5 Face do fundo
      base.PontosAdicionar(_vertices[4]);
      base.PontosAdicionar(_vertices[7]);
      base.PontosAdicionar(_vertices[6]);
      base.PontosAdicionar(_vertices[5]);
      
      // // 0, 3, 7, 4 Face direita
      base.PontosAdicionar(_vertices[0]);
      base.PontosAdicionar(_vertices[3]);
      base.PontosAdicionar(_vertices[7]);
      base.PontosAdicionar(_vertices[4]);

      // // 0, 4, 5, 1 Face de baixo
      base.PontosAdicionar(_vertices[0]);
      base.PontosAdicionar(_vertices[4]);
      base.PontosAdicionar(_vertices[5]);
      base.PontosAdicionar(_vertices[1]);

      // // 1, 5, 6, 2 Face direita
      base.PontosAdicionar(_vertices[1]);
      base.PontosAdicionar(_vertices[5]);
      base.PontosAdicionar(_vertices[6]);
      base.PontosAdicionar(_vertices[2]);

      Atualizar();
    }

    private void Atualizar()
    {
      base.ObjetoAtualizar();
    }

    public Ponto4D[] GetVertices()
    {
      return this._vertices;
    }

    public static List<Texture> GetTextures()
    {
      List<Texture> textures = [];
      Texture alexandreTexture = Texture.LoadFromFile("assets/alexandre.png");
      Texture brunoTexture = Texture.LoadFromFile("assets/bruno.png");
      Texture joshuaTexture = Texture.LoadFromFile("assets/joshua.png");
      Texture leonardoTexture = Texture.LoadFromFile("assets/leonardo.png");
      Texture lorhanTexture = Texture.LoadFromFile("assets/lorhan.png");
      Texture containerTexture = Texture.LoadFromFile("assets/container.png");

      textures.AddRange([
        lorhanTexture,
        containerTexture,
        joshuaTexture,
        leonardoTexture,
        alexandreTexture,
        brunoTexture
      ]);
      return textures;
    }

    public List<float[]> GetFaceVertices()
    {
      List<float[]> faceVertices =
      [
        _frontFaceVertices,
        _backFaceVertices,
        _topFaceVertices,
        _bottomFaceVertices,
        _rightFaceVertices,
        _leftFaceVertices
      ];
      return faceVertices;
    }

    public List<uint[]> GetFaceIndices()
    {
      List<uint[]> faceIndices =
      [
        _frontFaceIndices,
        _backFaceIndices,
        _topFaceIndices,
        _bottomFaceIndices,
        _rightFaceIndices,
        _leftFaceIndices
      ];
      return faceIndices;
    }

#if CG_Debug
    public override string ToString()
    {
      string retorno;
      retorno = "__ Objeto Cubo _ Tipo: " + PrimitivaTipo + " _ Tamanho: " + PrimitivaTamanho + "\n";
      retorno += base.ImprimeToString();
      return (retorno);
    }
#endif

  }
}
