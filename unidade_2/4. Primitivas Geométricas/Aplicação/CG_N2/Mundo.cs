/**
  Autor: Dalton Solano dos Reis
**/

#define CG_Gizmo
// #define CG_Privado

using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using OpenTK.Input;
using CG_Biblioteca;

namespace gcgcg
{
    class Mundo : GameWindow
    {
        private static Mundo instanciaMundo = null;

        private Mundo(int width, int height) : base(width, height) { }

        public static Mundo GetInstance(int width, int height)
        {
            if (instanciaMundo == null)
            instanciaMundo = new Mundo(width, height);
            return instanciaMundo;
        }

        private CameraOrtho camera = new CameraOrtho();
        protected List<Objeto> objetosLista = new List<Objeto>();
        private ObjetoGeometria objetoSelecionado = null;
        private bool bBoxDesenhar = false;
        int mouseX, mouseY;   
        private bool mouseMoverPto = false;        
        private Retangulo obj_Retangulo;        
    #if CG_Privado
        private Privado_SegReta obj_SegReta;
        private Privado_Circulo obj_Circulo;
    #endif

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            camera.xmin = -400; 
            camera.xmax = 400; 
            camera.ymin = -400; 
            camera.ymax = 400;

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");

            obj_Retangulo = new Retangulo("R", null, new Ponto4D(-200, -200), new Ponto4D(200,200));            
            objetosLista.Add(obj_Retangulo);
            obj_Retangulo.PrimitivaTipo = PrimitiveType.Points;
            obj_Retangulo.PrimitivaTamanho = 3;
            objetoSelecionado = obj_Retangulo;

#if CG_Privado
            obj_SegReta = new Privado_SegReta("B", null, new Ponto4D(50, 150), new Ponto4D(150, 250));
            obj_SegReta.ObjetoCor.CorR = 255; obj_SegReta.ObjetoCor.CorG = 255; obj_SegReta.ObjetoCor.CorB = 0;
            objetosLista.Add(obj_SegReta);
            objetoSelecionado = obj_SegReta;

            obj_Circulo = new Privado_Circulo("C", null, new Ponto4D(100, 300), 50);
            obj_Circulo.ObjetoCor.CorR = 0; obj_Circulo.ObjetoCor.CorG = 255; obj_Circulo.ObjetoCor.CorB = 255;
            objetosLista.Add(obj_Circulo);
            objetoSelecionado = obj_Circulo;
#endif
            GL.ClearColor(0.5f,0.5f,0.5f,1.0f);
        }
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(camera.xmin, camera.xmax, camera.ymin, camera.ymax, camera.zmin, camera.zmax);
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
    #if CG_Gizmo      
            Sru3D();
    #endif
            for (var i = 0; i < objetosLista.Count; i++)
                objetosLista[i].Desenhar();
            if (bBoxDesenhar && (objetoSelecionado != null))
                objetoSelecionado.BBox.Desenhar();
            this.SwapBuffers();
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e)
        {
            if (e.Key == Key.Space){                             
                switch (objetoSelecionado.PrimitivaTipo)
                {
                    case PrimitiveType.Points:
                        objetoSelecionado.PrimitivaTipo = PrimitiveType.Lines;
                        break;
                    case PrimitiveType.Lines:
                        objetoSelecionado.PrimitivaTipo = PrimitiveType.LineLoop;
                        break;
                    case PrimitiveType.LineLoop:
                        objetoSelecionado.PrimitivaTipo = PrimitiveType.LineStrip;
                        break;
                    case PrimitiveType.LineStrip:
                        objetoSelecionado.PrimitivaTipo = PrimitiveType.Triangles;
                        break;
                    case PrimitiveType.Triangles:
                        objetoSelecionado.PrimitivaTipo = PrimitiveType.TriangleStrip;
                        break;
                    case PrimitiveType.TriangleStrip:
                        objetoSelecionado.PrimitivaTipo = PrimitiveType.TriangleFan;
                        break;
                    case PrimitiveType.TriangleFan:
                        objetoSelecionado.PrimitivaTipo = PrimitiveType.Quads;
                        break;
                    case PrimitiveType.Quads:
                        objetoSelecionado.PrimitivaTipo = PrimitiveType.QuadStrip;
                        break;
                    default:
                        objetoSelecionado.PrimitivaTipo = PrimitiveType.Points;
                        break;
                }                
            }
            else
                Console.WriteLine(" __ Tecla não implementada.");
        }
        
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            mouseX = e.Position.X; mouseY = 600 - e.Position.Y; // Inverti eixo Y
            if (mouseMoverPto && (objetoSelecionado != null)){
                objetoSelecionado.PontosUltimo().X = mouseX;
                objetoSelecionado.PontosUltimo().Y = mouseY;
            }
        }

#if CG_Gizmo
        private void Sru3D()
        {
            GL.LineWidth(3);
            GL.Begin(PrimitiveType.Lines);
            // Linha 1
            GL.Color3(Convert.ToByte(235), Convert.ToByte(62), Convert.ToByte(50));
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(200, 0, 0);
            // Linha 2
            GL.Color3(Convert.ToByte(61), Convert.ToByte(121), Convert.ToByte(0));
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0, 200, 0);
            GL.End();
        }
#endif
    }    

    class Program
    {
        static void Main(string[] args)
        {
            Mundo window = Mundo.GetInstance(600, 600);
            window.Title = "CG_N2";
            window.Run(1.0 / 60.0);
        }
    }
}
