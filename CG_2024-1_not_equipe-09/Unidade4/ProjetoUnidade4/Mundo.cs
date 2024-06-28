#define CG_DEBUG
#define CG_Gizmo      
#define CG_OpenGL      
// #define CG_OpenTK
// #define CG_DirectX      
// #define CG_Privado      

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using OpenTK.Mathematics;
using System.Collections.Generic;

//FIXME: padrão Singleton

namespace gcgcg
{
  public class Mundo : GameWindow
  {
    private static Objeto mundo = null;
    private char rotuloNovo = '?';
    private Objeto _point;
    private Objeto _biggerCube;
    private Objeto _smallerCube;
    private readonly float _smallerCubeOrbitationRadius = 0.050f;
    private float _smallerCubeOrbitationAngle = 1.0f;
    private float _timeAccumulator;

    private readonly float[] _sruEixos =
    [
      -0.5f,  0.0f,  0.0f, /* X- */      0.5f,  0.0f,  0.0f, /* X+ */
       0.0f, -0.5f,  0.0f, /* Y- */      0.0f,  0.5f,  0.0f, /* Y+ */
       0.0f,  0.0f, -0.5f, /* Z- */      0.0f,  0.0f,  0.5f  /* Z+ */
    ];
    private int _vertexBufferObject_sruEixos;
    private int _vertexArrayObject_sruEixos;

    private List<Texture> _textures;
    private List<float[]> _faceVertices;
    private List<uint[]> _faceIndices;
    private readonly List<Ponto4D> _rectangleFirstPoints = new List<Ponto4D> {
      new Ponto4D(-1.0, -1.0, 1.0),
      new Ponto4D(-1.0, -1.0, -1.0),
      new Ponto4D(-1.0, 1.0, -1.0),
      new Ponto4D(-1.0, -1.0, -1.0),
      new Ponto4D(1.0, -1.0, -1.0),
      new Ponto4D(-1.0, -1.0, -1.0)
    };
    private readonly List<Ponto4D> _rectangleSecondPoints = new List<Ponto4D> {
      new Ponto4D(1.0, 1.0, 1.0),
      new Ponto4D(1.0, 1.0, -1.0),
      new Ponto4D(1.0, 1.0, 1.0),
      new Ponto4D(1.0, -1.0, 1.0),
      new Ponto4D(1.0, 1.0, 1.0),
      new Ponto4D(-1.0, 1.0, 1.0)
    };
    private List<Shader> _shadersWithTextures;
    private List<int> _vertexBufferObjects_texture;
    private List<int> _vertexArrayObjects_texture;
    private List<int> _elementBufferObjects_texture;

    private readonly int _vertexBufferObject_texture_frontFace = 0;
    private readonly int _vertexBufferObject_texture_backFace = 0;
    private readonly int _vertexBufferObject_texture_topFace = 0;
    private readonly int _vertexBufferObject_texture_bottomFace = 0;
    private readonly int _vertexBufferObject_texture_rightFace = 0;
    private readonly int _vertexBufferObject_texture_leftFace = 0;

    private readonly int _vertexArrayObject_texture_frontFace = 0;
    private readonly int _vertexArrayObject_texture_backFace = 0;
    private readonly int _vertexArrayObject_texture_topFace = 0;
    private readonly int _vertexArrayObject_texture_bottomFace = 0;
    private readonly int _vertexArrayObject_texture_rightFace = 0;
    private readonly int _vertexArrayObject_texture_leftFace = 0;

    private readonly int _elementBufferObject_texture_frontFace = 0;
    private readonly int _elementBufferObject_texture_backFace = 0;
    private readonly int _elementBufferObject_texture_topFace = 0;
    private readonly int _elementBufferObject_texture_bottomFace = 0;
    private readonly int _elementBufferObject_texture_rightFace = 0;
    private readonly int _elementBufferObject_texture_leftFace = 0;

    private Shader _shaderBranca;
    private Shader _shaderVermelha;
    private Shader _shaderVerde;
    private Shader _shaderAzul;
    private Shader _shaderCiano;
    private Shader _shaderMagenta;
    private Shader _shaderAmarela;
    private Shader _shaderFrontFaceTexture;
    private Shader _shaderBackFaceTexture;
    private Shader _shaderTopFaceTexture;
    private Shader _shaderBottomFaceTexture;
    private Shader _shaderRightFaceTexture;
    private Shader _shaderLeftFaceTexture;

    private Camera _camera;
    private float _angleX;
    private float _angleY;
    private float _previousMouseStateX;
    private float _previousMouseStateY;

    public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
           : base(gameWindowSettings, nativeWindowSettings)
    {
      mundo ??= new Objeto(null, ref rotuloNovo); //padrão Singleton
    }

    protected override void OnLoad()
    {
      base.OnLoad();

      //Utilitario.Diretivas();
#if CG_DEBUG      
      Console.WriteLine("Tamanho interno da janela de desenho: " + ClientSize.X + "x" + ClientSize.Y);
#endif

      GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
      GL.Enable(EnableCap.DepthTest);

      #region Cores
      _shaderBranca = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
      _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
      _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
      _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
      _shaderCiano = new Shader("Shaders/shader.vert", "Shaders/shaderCiano.frag");
      _shaderMagenta = new Shader("Shaders/shader.vert", "Shaders/shaderMagenta.frag");
      _shaderAmarela = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
      _shaderFrontFaceTexture = new Shader("Shaders/shaderWithTextures.vert", "Shaders/shaderWithTextures.frag");
      _shaderBackFaceTexture = new Shader("Shaders/shaderWithTextures.vert", "Shaders/shaderWithTextures.frag");
      _shaderTopFaceTexture = new Shader("Shaders/shaderWithTextures.vert", "Shaders/shaderWithTextures.frag");
      _shaderBottomFaceTexture = new Shader("Shaders/shaderWithTextures.vert", "Shaders/shaderWithTextures.frag");
      _shaderRightFaceTexture = new Shader("Shaders/shaderWithTextures.vert", "Shaders/shaderWithTextures.frag");
      _shaderLeftFaceTexture = new Shader("Shaders/shaderWithTextures.vert", "Shaders/shaderWithTextures.frag");
      #endregion

      #region Eixos: SRU  
      _vertexBufferObject_sruEixos = GL.GenBuffer();
      GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_sruEixos);
      GL.BufferData(BufferTarget.ArrayBuffer, _sruEixos.Length * sizeof(float), _sruEixos, BufferUsageHint.StaticDraw);
      _vertexArrayObject_sruEixos = GL.GenVertexArray();
      GL.BindVertexArray(_vertexArrayObject_sruEixos);
      GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
      GL.EnableVertexAttribArray(0);
      #endregion

      #region Object: Point  
      _point = new Ponto(mundo, ref rotuloNovo, new Ponto4D(2.0, 0.0));
      _point.PrimitivaTipo = PrimitiveType.Points;
      _point.PrimitivaTamanho = 5;
      #endregion

      #region Object: Bigger Cube
      _biggerCube = new Cubo(mundo, ref rotuloNovo, 0.0f);
      _biggerCube.shaderCor = _shaderAmarela;
      _textures = Cubo.GetTextures();
      _faceVertices = ((Cubo) _biggerCube).GetFaceVertices();
      _faceIndices = ((Cubo) _biggerCube).GetFaceIndices();

      _vertexArrayObjects_texture = [
        _vertexArrayObject_texture_frontFace,
        _vertexArrayObject_texture_backFace,
        _vertexArrayObject_texture_topFace,
        _vertexArrayObject_texture_bottomFace,
        _vertexArrayObject_texture_rightFace,
        _vertexArrayObject_texture_leftFace
      ];

      _vertexBufferObjects_texture = [
        _vertexBufferObject_texture_frontFace,
        _vertexBufferObject_texture_backFace,
        _vertexBufferObject_texture_topFace,
        _vertexBufferObject_texture_bottomFace,
        _vertexBufferObject_texture_rightFace,
        _vertexBufferObject_texture_leftFace
      ];

      _elementBufferObjects_texture = [
        _elementBufferObject_texture_frontFace,
        _elementBufferObject_texture_backFace,
        _elementBufferObject_texture_topFace,
        _elementBufferObject_texture_bottomFace,
        _elementBufferObject_texture_rightFace,
        _elementBufferObject_texture_leftFace
      ];

      _shadersWithTextures = [
        _shaderFrontFaceTexture,
        _shaderBackFaceTexture,
        _shaderTopFaceTexture,
        _shaderBottomFaceTexture,
        _shaderRightFaceTexture,
        _shaderLeftFaceTexture
      ];

      OnLoadUseTextures();
      #endregion

      #region Object: Smaller Cube
      _smallerCube = new Cubo(mundo, ref rotuloNovo, 0.8f);
      _smallerCube.shaderCor = _shaderCiano;
      _smallerCube.MatrizTranslacaoXYZ(0, -2.5, 0);
      #endregion

      _camera = new Camera(Vector3.UnitZ * 5, ClientSize.X / (float) ClientSize.Y);
      _angleX = -90;
      _angleY = 0;
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
      base.OnRenderFrame(e);

      GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
      mundo.Desenhar(new Transformacao4D(), _camera);

      OnRenderFrameUseTextures();

#if CG_Gizmo      
      Gizmo_Sru3D();
#endif
      SwapBuffers();
    }

    protected override void OnUpdateFrame(FrameEventArgs e)
    {
      base.OnUpdateFrame(e);
      OnUpdateFrameOrbitateSmallerCube(e);

      const float cameraSpeed = 2.5f;
      const float cameraSensivity = 0.020f;
      var estadoTeclado = KeyboardState;

      // ☞ 396c2670-8ce0-4aff-86da-0f58cd8dcfdc   TODO: forma otimizada para teclado.
      #region Teclado
      if (estadoTeclado.IsKeyDown(Keys.Escape))
        Close();
      if (estadoTeclado.IsKeyPressed(Keys.Space))
      {
        if (_biggerCube == null)
          _biggerCube = mundo;
        _biggerCube.shaderCor = _shaderBranca;
        _biggerCube = mundo.GrafocenaBuscaProximo(_biggerCube);
        _biggerCube.shaderCor = _shaderAmarela;
      }
      if (estadoTeclado.IsKeyPressed(Keys.G))
        mundo.GrafocenaImprimir("");
      if (estadoTeclado.IsKeyPressed(Keys.P) && _biggerCube != null)
        Console.WriteLine(_biggerCube.ToString());
      if (estadoTeclado.IsKeyPressed(Keys.M) && _biggerCube != null)
        _biggerCube.MatrizImprimir();
      if (estadoTeclado.IsKeyPressed(Keys.I) && _biggerCube != null)
        _biggerCube.MatrizAtribuirIdentidade();
      if (estadoTeclado.IsKeyPressed(Keys.Left) && _biggerCube != null)
        _biggerCube.MatrizTranslacaoXYZ(-0.3, 0, 0);
      if (estadoTeclado.IsKeyPressed(Keys.Right) && _biggerCube != null)
        _biggerCube.MatrizTranslacaoXYZ(0.3, 0, 0);
      if (estadoTeclado.IsKeyPressed(Keys.Up) && _biggerCube != null)
        _biggerCube.MatrizTranslacaoXYZ(0, 0.3, 0);
      if (estadoTeclado.IsKeyPressed(Keys.Down) && _biggerCube != null)
        _biggerCube.MatrizTranslacaoXYZ(0, -0.3, 0);
      if (estadoTeclado.IsKeyPressed(Keys.O) && _biggerCube != null)
        _biggerCube.MatrizTranslacaoXYZ(0, 0, 0.05);
      if (estadoTeclado.IsKeyPressed(Keys.L) && _biggerCube != null)
        _biggerCube.MatrizTranslacaoXYZ(0, 0, -0.05);
      if (estadoTeclado.IsKeyPressed(Keys.PageUp) && _biggerCube != null)
        _biggerCube.MatrizEscalaXYZ(2, 2, 2);
      if (estadoTeclado.IsKeyPressed(Keys.PageDown) && _biggerCube != null)
        _biggerCube.MatrizEscalaXYZ(0.5, 0.5, 0.5);
      if (estadoTeclado.IsKeyPressed(Keys.Home) && _biggerCube != null)
        _biggerCube.MatrizEscalaXYZBBox(0.5, 0.5, 0.5);
      if (estadoTeclado.IsKeyPressed(Keys.End) && _biggerCube != null)
        _biggerCube.MatrizEscalaXYZBBox(2, 2, 2);
      if (estadoTeclado.IsKeyPressed(Keys.D1) && _biggerCube != null)
        _biggerCube.MatrizRotacao(10);
      if (estadoTeclado.IsKeyPressed(Keys.D2) && _biggerCube != null)
        _biggerCube.MatrizRotacao(-10);
      if (estadoTeclado.IsKeyPressed(Keys.D3) && _biggerCube != null)
        _biggerCube.MatrizRotacaoZBBox(10);
      if (estadoTeclado.IsKeyPressed(Keys.D4) && _biggerCube != null)
        _biggerCube.MatrizRotacaoZBBox(-10);

      if (estadoTeclado.IsKeyDown(Keys.Z))
        _camera.Position = Vector3.UnitZ * 5;
      if (estadoTeclado.IsKeyDown(Keys.W))
        _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
      if (estadoTeclado.IsKeyDown(Keys.S))
        _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
      if (estadoTeclado.IsKeyDown(Keys.A))
        _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
      if (estadoTeclado.IsKeyDown(Keys.D))
        _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
      if (estadoTeclado.IsKeyDown(Keys.RightShift))
        _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
      if (estadoTeclado.IsKeyDown(Keys.LeftShift))
        _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
      if (estadoTeclado.IsKeyDown(Keys.D9))
        _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
      if (estadoTeclado.IsKeyDown(Keys.D0))
        _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down

      #endregion

      #region Mouse
      if (MouseState.IsButtonPressed(MouseButton.Left))
      {
        _previousMouseStateX = MouseState.X;
        _previousMouseStateY = MouseState.Y;
      }

      if (MouseState.IsButtonDown(MouseButton.Left))
      {
        OnUpdateFrameReloadCameraPerspective(cameraSensivity, MouseState.X, MouseState.Y);
      }

      if (MouseState.IsButtonDown(MouseButton.Right) && _biggerCube != null)
      {
        Console.WriteLine("MouseState.IsButtonDown(MouseButton.Right)");

        int janelaLargura = ClientSize.X;
        int janelaAltura = ClientSize.Y;
        Ponto4D mousePonto = new Ponto4D(MousePosition.X, MousePosition.Y);
        Ponto4D sruPonto = Utilitario.NDC_TelaSRU(janelaLargura, janelaAltura, mousePonto);

        _biggerCube.PontosAlterar(sruPonto, 0);
      }

      if (MouseState.IsButtonReleased(MouseButton.Right))
      {
        Console.WriteLine("MouseState.IsButtonReleased(MouseButton.Right)");
      }
      #endregion
    }

    protected override void OnMouseWheel(MouseWheelEventArgs mouseWheelEventArgs)
    {
      base.OnMouseWheel(mouseWheelEventArgs);
      _camera.Fov -= mouseWheelEventArgs.OffsetY;
    }

    protected override void OnResize(ResizeEventArgs e)
    {
      base.OnResize(e);

#if CG_DEBUG      
      Console.WriteLine("Tamanho interno da janela de desenho: " + ClientSize.X + "x" + ClientSize.Y);
#endif
      GL.Viewport(0, 0, ClientSize.X, ClientSize.Y);
      _camera.AspectRatio = ClientSize.X / (float) ClientSize.Y;
    }

    protected override void OnUnload()
    {
      mundo.OnUnload();

      GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
      GL.BindVertexArray(0);
      GL.UseProgram(0);

      GL.DeleteBuffer(_vertexBufferObject_sruEixos);
      GL.DeleteVertexArray(_vertexArrayObject_sruEixos);

      OnUnloadDeleteArrays();
      OnUnloadDeleteBuffers();
      OnUnloadDeleteElementBuffers();

      GL.DeleteProgram(_shaderBranca.Handle);
      GL.DeleteProgram(_shaderVermelha.Handle);
      GL.DeleteProgram(_shaderVerde.Handle);
      GL.DeleteProgram(_shaderAzul.Handle);
      GL.DeleteProgram(_shaderCiano.Handle);
      GL.DeleteProgram(_shaderMagenta.Handle);
      GL.DeleteProgram(_shaderAmarela.Handle);
      GL.DeleteProgram(_shaderFrontFaceTexture.Handle);
      GL.DeleteProgram(_shaderBackFaceTexture.Handle);
      GL.DeleteProgram(_shaderTopFaceTexture.Handle);
      GL.DeleteProgram(_shaderBottomFaceTexture.Handle);
      GL.DeleteProgram(_shaderRightFaceTexture.Handle);
      GL.DeleteProgram(_shaderLeftFaceTexture.Handle);

      base.OnUnload();
    }

    protected void OnLoadUseTextures()
    {
      for (int i = 0; i < _textures.Count; i++)
      {
        GL.Enable(EnableCap.Texture2D);
        _vertexArrayObjects_texture[i] = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObjects_texture[i]);

        _vertexBufferObjects_texture[i] = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObjects_texture[i]);
        GL.BufferData(BufferTarget.ArrayBuffer, _faceVertices[i].Length * sizeof(float), _faceVertices[i], BufferUsageHint.StaticDraw);

        _elementBufferObjects_texture[i] = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObjects_texture[i]);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _faceIndices[i].Length * sizeof(uint), _faceIndices[i], BufferUsageHint.StaticDraw);

        _shadersWithTextures[i].Use();

        var vertexLocation = _shadersWithTextures[i].GetAttribLocation("aPosition");
        GL.EnableVertexAttribArray(vertexLocation);
        GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);

        var texCoordLocation = _shadersWithTextures[i].GetAttribLocation("aTexCoord");
        GL.EnableVertexAttribArray(texCoordLocation);
        GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

        _textures[i].Use(TextureUnit.Texture0);

        Retangulo faceRectangle = new Retangulo(_biggerCube, ref rotuloNovo, _rectangleFirstPoints[i], _rectangleSecondPoints[i], true);
        faceRectangle.shaderCor = _shadersWithTextures[i];
      }
    }

    protected void OnRenderFrameUseTextures()
    {
      for (int i = 0; i < _textures.Count; i++)
      {
        GL.BindVertexArray(_vertexArrayObjects_texture[i]);
        _textures[i].Use(TextureUnit.Texture0);
        _shadersWithTextures[i].Use();

        GL.DrawElements(PrimitiveType.Triangles, _faceIndices[i].Length, DrawElementsType.UnsignedInt, 0);
      }
    }

    protected void OnUpdateFrameOrbitateSmallerCube(FrameEventArgs frameEventArgs)
    {
      float updateFrameDelay = 0.030f;

      _timeAccumulator += (float) frameEventArgs.Time;

      if (_timeAccumulator >= updateFrameDelay) 
      {
        Ponto4D smallerCubeOrbitationPoint = Matematica.GerarPtosCirculo(_smallerCubeOrbitationAngle, _smallerCubeOrbitationRadius);
        _smallerCube.MatrizTranslacaoXYZ(smallerCubeOrbitationPoint.X, smallerCubeOrbitationPoint.Y, smallerCubeOrbitationPoint.Z);
        _smallerCubeOrbitationAngle += 1.0f;
        _timeAccumulator = 0.0f;
      }
    }

    protected void OnUpdateFrameReloadCameraPerspective(float cameraSensivity, float actualMouseStateX, float actualMouseStateY)
    {
      float deltaX = (_previousMouseStateX - actualMouseStateX) * cameraSensivity / 80f;
      float deltaY = (_previousMouseStateY - actualMouseStateY) * cameraSensivity / 80f;

      _angleX += deltaX;
      _angleY += deltaY;

      Ponto4D pointX = Matematica.GerarPtosCirculo(-_angleX, 5);
      Ponto4D pointY = Matematica.GerarPtosCirculo(-_angleY, 5);

      _camera.Position = new Vector3((float) pointX.X, (float) pointY.Y, (float) pointX.Y);
      _camera.Yaw -= deltaX;
      _camera.Pitch += deltaY;
    }

    protected void OnUnloadDeleteArrays()
    {
      foreach (int _vertexArrayObject_texture in _vertexArrayObjects_texture)
      {
        GL.DeleteVertexArray(_vertexArrayObject_texture);
      }
    }

    protected void OnUnloadDeleteBuffers()
    {
      foreach (int _vertexBufferObject_texture in _vertexBufferObjects_texture)
      {
        GL.DeleteBuffer(_vertexBufferObject_texture);
      }
    }

    protected void OnUnloadDeleteElementBuffers()
    {
      foreach (int _elementBufferObject_texture in _elementBufferObjects_texture)
      {
        GL.DeleteBuffer(_elementBufferObject_texture);
      }
    }

#if CG_Gizmo
    private void Gizmo_Sru3D()
    {
#if CG_OpenGL && !CG_DirectX
      var model = Matrix4.Identity;
      GL.BindVertexArray(_vertexArrayObject_sruEixos);
      // EixoX
      _shaderVermelha.SetMatrix4("model", model);
      _shaderVermelha.SetMatrix4("view", _camera.GetViewMatrix());
      _shaderVermelha.SetMatrix4("projection", _camera.GetProjectionMatrix());
      _shaderVermelha.Use();
      GL.DrawArrays(PrimitiveType.Lines, 0, 2);
      // EixoY
      _shaderVerde.SetMatrix4("model", model);
      _shaderVerde.SetMatrix4("view", _camera.GetViewMatrix());
      _shaderVerde.SetMatrix4("projection", _camera.GetProjectionMatrix());
      _shaderVerde.Use();
      GL.DrawArrays(PrimitiveType.Lines, 2, 2);
      // EixoZ
      _shaderAzul.SetMatrix4("model", model);
      _shaderAzul.SetMatrix4("view", _camera.GetViewMatrix());
      _shaderAzul.SetMatrix4("projection", _camera.GetProjectionMatrix());
      _shaderAzul.Use();
      GL.DrawArrays(PrimitiveType.Lines, 4, 2);
#elif CG_DirectX && !CG_OpenGL
      Console.WriteLine(" .. Coloque aqui o seu código em DirectX");
#elif (CG_DirectX && CG_OpenGL) || (!CG_DirectX && !CG_OpenGL)
      Console.WriteLine(" .. ERRO de Render - escolha OpenGL ou DirectX !!");
#endif
    }
#endif    

  }
}
