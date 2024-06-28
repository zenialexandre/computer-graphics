//TODO: testar se estes DEFINEs continuam funcionado
#define CG_Gizmo  // debugar gráfico.
#define CG_OpenGL // render OpenGL.
// #define CG_DirectX // render DirectX.
// #define CG_Privado // código do professor.

using CG_Biblioteca;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
// using OpenTK.Mathematics;

namespace gcgcg
{
    public class Mundo : GameWindow
    {
        private static Objeto mundo = null;
        private char rotuloAtual = '?';
        private Objeto objetoSelecionado = null;

        private readonly float[] _sruEixos =
        {
            -0.5f,  0.0f,  0.0f, /* X- */      0.5f,  0.0f,  0.0f, /* X+ */
            0.0f, -0.5f,  0.0f, /* Y- */      0.0f,  0.5f,  0.0f, /* Y+ */
            0.0f,  0.0f, -0.5f, /* Z- */      0.0f,  0.0f,  0.5f, /* Z+ */
        };

        private int _vertexBufferObject_sruEixos;
        private int _vertexArrayObject_sruEixos;

        private Shader _shaderVermelha;
        private Shader _shaderVerde;
        private Shader _shaderAzul;
        private Shader _shaderCiano;
        private Shader _shaderAmarela;
        private Shader _shaderBranca;

        private bool mouseMovtoPrimeiro = true;
        private Ponto4D mouseMovtoUltimo;
        //private Vector2 _lastPos;
        private int primitiveTypesRouletteCounter = 0;

        #region Variaveis do exercício
        private Ponto point_1;
        private Ponto point_2;
        private Ponto point_3;
        private Ponto point_4;
        private int index_points_list = 0;
        private List<Ponto> point_list = new List<Ponto>();
        private List<Ponto4D> point_list_4D = new List<Ponto4D>();
        private Poligono controlPolyhedron;
        private bool splinePointsChanged;

        private Spline spline;
        #endregion

        public Mundo(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
               : base(gameWindowSettings, nativeWindowSettings)
        {
            mundo ??= new Objeto(null, ref rotuloAtual); //padrão Singleton
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            #region Eixos: SRU  
            _vertexBufferObject_sruEixos = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject_sruEixos);
            GL.BufferData(BufferTarget.ArrayBuffer, _sruEixos.Length * sizeof(float), _sruEixos, BufferUsageHint.StaticDraw);
            _vertexArrayObject_sruEixos = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject_sruEixos);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            _shaderVermelha = new Shader("Shaders/shader.vert", "Shaders/shaderVermelha.frag");
            _shaderVerde = new Shader("Shaders/shader.vert", "Shaders/shaderVerde.frag");
            _shaderAzul = new Shader("Shaders/shader.vert", "Shaders/shaderAzul.frag");
            _shaderCiano = new Shader("Shaders/shader.vert", "Shaders/shaderCiano.frag");
            _shaderAmarela = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
            _shaderBranca = new Shader("Shaders/shader.vert", "Shaders/shaderBranca.frag");
            #endregion

            #region Objeto: Pontos

            point_1 = new Ponto(mundo, ref rotuloAtual, new Ponto4D(-0.25, -0.35));
            point_2 = new Ponto(mundo, ref rotuloAtual, new Ponto4D(-0.25, 0.25));

            point_3 = new Ponto(mundo, ref rotuloAtual, new Ponto4D(0.25, 0.25));
            point_4 = new Ponto(mundo, ref rotuloAtual, new Ponto4D(0.25, -0.35));

            point_1.shaderObjeto = _shaderVermelha;
            objetoSelecionado = point_1;

            point_list.Add(point_1);
            point_list.Add(point_2);
            point_list.Add(point_3);
            point_list.Add(point_4);

            #endregion

            #region Objeto: Segmento de reta

            point_list_4D.Add(new Ponto4D(-0.25, -0.35));
            point_list_4D.Add(new Ponto4D(-0.25, 0.25));
            point_list_4D.Add(new Ponto4D(0.25, 0.25));
            point_list_4D.Add(new Ponto4D(0.25, -0.35));

            controlPolyhedron = new Poligono(mundo, ref rotuloAtual, point_list_4D);
            controlPolyhedron.shaderObjeto = _shaderCiano;
            controlPolyhedron.PrimitivaTipo = PrimitiveType.LineStrip;

            spline = new Spline(mundo, ref rotuloAtual);
            spline.shaderObjeto = _shaderAmarela;
            spline.drawSpline(point_list_4D);
            #endregion

#if CG_Privado
             #region Objeto: circulo - origem
             objetoSelecionado = new Circulo(mundo, ref rotuloAtual, 0.2);
             objetoSelecionado.shaderObjeto = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
             #endregion
             #region Objeto: circulo
             objetoSelecionado = new Circulo(mundo, ref rotuloAtual, 0.1, new Ponto4D(0.0,-0.5));
             objetoSelecionado.shaderObjeto = new Shader("Shaders/shader.vert", "Shaders/shaderAmarela.frag");
             #endregion

             #region Objeto: SrPalito  
             objetoSelecionado = new SrPalito(mundo, ref rotuloAtual);
             #endregion

             #region Objeto: Spline
             objetoSelecionado = new Spline(mundo, ref rotuloAtual);
             #endregion
#endif

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit);

#if CG_Gizmo
            Sru3D();
#endif
            mundo.Desenhar();
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            #region Teclado
            var input = KeyboardState;
            if (input.IsKeyPressed(Keys.Escape))
            {
                Close();
            }
            else if (input.IsKeyPressed(Keys.Space))
            {
                objetoSelecionado.shaderObjeto = _shaderBranca;
                index_points_list += 1;
                if (index_points_list > 3)
                {
                    index_points_list = 0;
                }

                objetoSelecionado = point_list[index_points_list];
                objetoSelecionado.shaderObjeto = _shaderVermelha;
            }
            else if (input.IsKeyPressed(Keys.C))
            {   
                UpdateFigures(index_points_list, controlPolyhedron, point_list_4D, point_list, 0.0 ,0.05);
            }
            else if (input.IsKeyPressed(Keys.B))
            {   
                UpdateFigures(index_points_list, controlPolyhedron, point_list_4D, point_list, 0.0 , -0.05);
            }
            else if (input.IsKeyPressed(Keys.D))
            {   
                UpdateFigures(index_points_list, controlPolyhedron, point_list_4D, point_list, 0.05 , 0.0);
            }
            else if (input.IsKeyPressed(Keys.E))
            {   
                UpdateFigures(index_points_list, controlPolyhedron, point_list_4D, point_list, -0.05 , 0.0);
            }
            else if (input.IsKeyPressed(Keys.A))
            {   
                splinePointsChanged = spline.SplinePoints(0.001, false);

                if(splinePointsChanged){
                    spline.drawSpline(point_list_4D);
                    //Console.WriteLine(spline.qtdPontosSpline);
                }

            }
            else if (input.IsKeyPressed(Keys.Z))
            {   
                splinePointsChanged = spline.SplinePoints(0.001, true);
                
                if(splinePointsChanged){
                    spline.drawSpline(point_list_4D);
                }
            }
            #endregion

            #region  Mouse
            int janelaLargura = Size.X;
            int janelaAltura = Size.Y;
            Ponto4D mousePonto = new Ponto4D(MousePosition.X, MousePosition.Y);
            Ponto4D sruPonto = Utilitario.NDC_TelaSRU(janelaLargura, janelaAltura, mousePonto);

            //FIXME: o movimento do mouse em relação ao eixo X está certo. Mas tem um erro no eixo Y,,, aumentar o valor do Y aumenta o erro.
            if (input.IsKeyDown(Keys.LeftShift))
            {
                if (mouseMovtoPrimeiro)
                {
                    mouseMovtoUltimo = sruPonto;
                    mouseMovtoPrimeiro = false;
                }
                else
                {
                    var deltaX = sruPonto.X - mouseMovtoUltimo.X;
                    var deltaY = sruPonto.Y - mouseMovtoUltimo.Y;
                    mouseMovtoUltimo = sruPonto;

                    objetoSelecionado.PontosAlterar(new Ponto4D(objetoSelecionado.PontosId(0).X + deltaX, objetoSelecionado.PontosId(0).Y + deltaY, 0), 0);
                    objetoSelecionado.ObjetoAtualizar();
                }
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                objetoSelecionado.PontosAlterar(sruPonto, 0);
                objetoSelecionado.ObjetoAtualizar();
            }
            #endregion

        }
        #region Atualiza figuras
        private void UpdateFigures(int index_point, Poligono polyhedron, List<Ponto4D> points_4D, List<Ponto> point_list, double x, double y){
            point_list[index_point].PontosAlterar(new Ponto4D(point_list[index_point].PontosId(0).X + x, point_list[index_point].PontosId(0).Y + y), 0);

            points_4D[index_point] = new Ponto4D(points_4D[index_point].X + x, points_4D[index_point].Y + y);
            polyhedron.Atualizar(points_4D);
            spline.drawSpline(points_4D);
        }
        #endregion

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
        }

        protected override void OnUnload()
        {
            mundo.OnUnload();

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(_vertexBufferObject_sruEixos);
            GL.DeleteVertexArray(_vertexArrayObject_sruEixos);

            GL.DeleteProgram(_shaderVermelha.Handle);
            GL.DeleteProgram(_shaderVerde.Handle);
            GL.DeleteProgram(_shaderAzul.Handle);

            base.OnUnload();
        }

        protected void setNextPrimiteTypeFromRoulette()
        {
            PrimitiveType[] primitiveTypesRoulette = {
                PrimitiveType.Points,
                PrimitiveType.Lines,
                PrimitiveType.LineStrip,
                PrimitiveType.Triangles,
                PrimitiveType.TriangleStrip,
                PrimitiveType.TriangleFan,
                PrimitiveType.LineLoop
            };
            objetoSelecionado.PrimitivaTipo = primitiveTypesRoulette[primitiveTypesRouletteCounter];
            primitiveTypesRouletteCounter += 1;

            if (primitiveTypesRouletteCounter > 6) primitiveTypesRouletteCounter = 0;
        }

#if CG_Gizmo
        private void Sru3D()
        {
#if CG_OpenGL && !CG_DirectX
            GL.BindVertexArray(_vertexArrayObject_sruEixos);
            // EixoX
            _shaderVermelha.Use();
            GL.DrawArrays(PrimitiveType.Lines, 0, 2);
            // EixoY
            _shaderVerde.Use();
            GL.DrawArrays(PrimitiveType.Lines, 2, 2);
            // EixoZ
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
