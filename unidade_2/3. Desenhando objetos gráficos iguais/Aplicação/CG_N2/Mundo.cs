﻿/**
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
        private Circulo obj_Circulo;
        private SegReta obj_Reta;
#if CG_Privado
        private Privado_SegReta obj_SegReta;
        private Privado_Circulo obj_Circulo;
#endif

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            camera.xmin = -300; 
            camera.xmax = 300; 
            camera.ymin = -300; 
            camera.ymax = 300;

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");
          
            obj_Reta = new SegReta("R1",
                                    null, 
                                    new Ponto4D(-100, -100, 0), 
                                    new Ponto4D(100, -100, 0));
            obj_Reta.ObjetoCor.CorR = 120;
            obj_Reta.ObjetoCor.CorG = 223;
            obj_Reta.ObjetoCor.CorB = 224;
            objetosLista.Add(obj_Reta);            
            
            obj_Circulo = new Circulo("C1",
                                      null,                                      
                                      5,
                                      -100,
                                      -100);
            obj_Circulo.ObjetoCor.CorR = 0;
            obj_Circulo.ObjetoCor.CorG = 0;
            obj_Circulo.ObjetoCor.CorB = 0;
            objetosLista.Add(obj_Circulo);            

            obj_Reta = new SegReta("R2",
                                    null,
                                    new Ponto4D(-100, -100, 0),
                                    new Ponto4D(0, 100, 0));
            obj_Reta.ObjetoCor.CorR = 120;
            obj_Reta.ObjetoCor.CorG = 223;
            obj_Reta.ObjetoCor.CorB = 224;
            objetosLista.Add(obj_Reta);
            objetoSelecionado = obj_Reta;

            obj_Circulo = new Circulo("C2",
                                       null,                                        
                                       5,
                                       100,
                                       -100);
            obj_Circulo.ObjetoCor.CorR = 0;
            obj_Circulo.ObjetoCor.CorG = 0;
            obj_Circulo.ObjetoCor.CorB = 0;
            objetosLista.Add(obj_Circulo);            

            obj_Reta = new SegReta("R3",
                                    null,
                                    new Ponto4D(100, -100, 0),
                                    new Ponto4D(0, 100, 0));
            obj_Reta.ObjetoCor.CorR = 120;
            obj_Reta.ObjetoCor.CorG = 223;
            obj_Reta.ObjetoCor.CorB = 224;
            objetosLista.Add(obj_Reta);            

            obj_Circulo = new Circulo("C3",
                                       null,                                        
                                       5,
                                       0,
                                       100);
            obj_Circulo.ObjetoCor.CorR = 0;
            obj_Circulo.ObjetoCor.CorG = 0;
            obj_Circulo.ObjetoCor.CorB = 0;
            objetosLista.Add(obj_Circulo);
            objetoSelecionado = obj_Circulo;

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
            else if (e.Key == Key.O)
                bBoxDesenhar = !bBoxDesenhar;
            else if (e.Key == Key.V)
                mouseMoverPto = !mouseMoverPto;
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
