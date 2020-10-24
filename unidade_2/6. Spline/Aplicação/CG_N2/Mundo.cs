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
        private Spline obj_Spline;
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

            obj_Spline = new Spline("S", null, 20);
            obj_Spline.ObjetoCor.CorR = 255;
            obj_Spline.ObjetoCor.CorG = 255;
            obj_Spline.ObjetoCor.CorB = 0;
            objetosLista.Add(obj_Spline);
            objetoSelecionado = obj_Spline;

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
            if (e.Key == Key.H)
                Utilitario.AjudaTeclado();
            else if (e.Key == Key.Escape)
                Exit();
            else if (e.Key == Key.Keypad1
                || e.Key == Key.Number1)
                obj_Spline.atualizaIndicePontoSelecionado(1);
            else if (e.Key == Key.Keypad2
                || e.Key == Key.Number2)
                obj_Spline.atualizaIndicePontoSelecionado(2);
            else if (e.Key == Key.Keypad3
                || e.Key == Key.Number3)
                obj_Spline.atualizaIndicePontoSelecionado(3);
            else if (e.Key == Key.Keypad4
                || e.Key == Key.Number4)
                obj_Spline.atualizaIndicePontoSelecionado(4);
            else if (e.Key == Key.KeypadAdd)
                obj_Spline.aumentarPontos();
            else if (e.Key == Key.KeypadSubtract)
                obj_Spline.diminuirPontos();
            else if (e.Key == Key.R)
                obj_Spline.resetarPontos();
            else if (e.Key == Key.O)
                bBoxDesenhar = !bBoxDesenhar;
            else if (e.Key == Key.V)
                mouseMoverPto = !mouseMoverPto;
            else
                Console.WriteLine(" __ Tecla não implementada.");
        }
    
        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            mouseX = e.Position.X - 325; 
            mouseY = 275 - e.Position.Y; // Inverti eixo Y
            if (mouseMoverPto && (obj_Spline.pontoSelecionado() != null)){
                obj_Spline.pontoSelecionado().X = mouseX;
                obj_Spline.pontoSelecionado().Y = mouseY;
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
