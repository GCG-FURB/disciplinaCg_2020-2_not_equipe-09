#define CG_Gizmo
// #define CG_Privado

using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Input;
using OpenTK.Graphics.OpenGL;

using CG_Biblioteca;
using CG_N4;
using System.IO;

namespace gcgcg{

    class Mundo : GameWindow{
 

        private static Mundo instanciaMundo = null;
        private Point WindowCenter = new Point();

        private Mundo(int width, int height) : base(width, height) { }

        public static Mundo GetInstance(int width, int height){
            if (instanciaMundo == null)
                instanciaMundo = new Mundo(width, height);
            return instanciaMundo;
        }

        private CameraPerspective camera = new CameraPerspective();
        protected List<Objeto> objetosLista = new List<Objeto>();
        private ObjetoGeometria objetoSelecionado = null;
        private Ponto4D mousePos = new Ponto4D();
        private char objetoId = '@';
        private String menuSelecao = "";
        private char menuEixoSelecao = 'z';
        private float deslocamento = 0;
        private bool bBoxDesenhar = false;

    #if CG_Privado
        private Cilindro obj_Cilindro;
        private Esfera obj_Esfera;
        private Cone obj_Cone;
    #endif
        private Cubo obj_Cubo;
        private RotacaoObjeto rotacaoObjeto = new RotacaoObjeto();

        protected override void OnLoad(EventArgs e){
            base.OnLoad(e);           

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");

            WindowCenter.X = 300;
            WindowCenter.Y = 300;

            obj_Cubo = new Cubo('D', null);
            objetosLista.Add(obj_Cubo);
            objetoSelecionado = obj_Cubo;
            objetoSelecionado.Translacao(8, 'x');
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos

            obj_Cubo = new Cubo('C', null);
            objetosLista.Add(obj_Cubo);
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos
            
            obj_Cubo = new Cubo('D', null);
            objetosLista.Add(obj_Cubo);
            objetoSelecionado = obj_Cubo;
            objetoSelecionado.Translacao(-4, 'x');
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos

            obj_Cubo = new Cubo('D', null);
            objetosLista.Add(obj_Cubo);
            objetoSelecionado = obj_Cubo;
            objetoSelecionado.Translacao(-8, 'x');
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos

            obj_Cubo = new Cubo('D', null);
            objetosLista.Add(obj_Cubo);
            objetoSelecionado = obj_Cubo;
            objetoSelecionado.Translacao(8, 'x');
            objetoSelecionado.Translacao(-4, 'z');
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos

            obj_Cubo = new Cubo('D', null);
            objetosLista.Add(obj_Cubo);
            objetoSelecionado = obj_Cubo;
            objetoSelecionado.Translacao(4, 'x');
            objetoSelecionado.Translacao(-4, 'z');
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos

            obj_Cubo = new Cubo('D', null);
           objetosLista.Add(obj_Cubo);
            objetoSelecionado = obj_Cubo;
            objetoSelecionado.Translacao(-4, 'z');
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos

            obj_Cubo = new Cubo('D', null);
            objetosLista.Add(obj_Cubo);
            objetoSelecionado = obj_Cubo;
            objetoSelecionado.Translacao(-4, 'x');
            objetoSelecionado.Translacao(-4, 'z');
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos

            obj_Cubo = new Cubo('D', null);
            objetosLista.Add(obj_Cubo);
            objetoSelecionado = obj_Cubo;
            objetoSelecionado.Translacao(-8, 'x');
            objetoSelecionado.Translacao(-4, 'z');
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos

            obj_Cubo = new Cubo('D', null);
            objetosLista.Add(obj_Cubo);
            objetoSelecionado = obj_Cubo;
            objetoSelecionado.Translacao(8, 'x');
            objetoSelecionado.Translacao(-8, 'z');
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos

            obj_Cubo = new Cubo('D', null);
            objetosLista.Add(obj_Cubo);
            objetoSelecionado = obj_Cubo;
            objetoSelecionado.Translacao(4, 'x');
            objetoSelecionado.Translacao(-8, 'z');
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos

            obj_Cubo = new Cubo('D', null);
            objetosLista.Add(obj_Cubo);
            objetoSelecionado = obj_Cubo;
            objetoSelecionado.Translacao(-8, 'z');
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos

            obj_Cubo = new Cubo('D', null);
            objetosLista.Add(obj_Cubo);
            objetoSelecionado = obj_Cubo;
            objetoSelecionado.Translacao(-4, 'x');
            objetoSelecionado.Translacao(-8, 'z');
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos

            obj_Cubo = new Cubo('D', null);
            objetosLista.Add(obj_Cubo);
            objetoSelecionado = obj_Cubo;
            objetoSelecionado.Translacao(-8, 'x');
            objetoSelecionado.Translacao(-8, 'z');
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos

            obj_Cubo = new Cubo('D', null);
            objetosLista.Add(obj_Cubo);
            objetoSelecionado = obj_Cubo;
            objetoSelecionado.Translacao(8, 'x');
            objetoSelecionado.Translacao(-12, 'z');
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos

            obj_Cubo = new Cubo('D', null);
            objetosLista.Add(obj_Cubo);
            objetoSelecionado = obj_Cubo;
            objetoSelecionado.Translacao(4, 'x');
            objetoSelecionado.Translacao(-12, 'z');
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos

            obj_Cubo = new Cubo('D', null);
            objetosLista.Add(obj_Cubo);
            objetoSelecionado = obj_Cubo;
            objetoSelecionado.Translacao(-12, 'z');
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos

            obj_Cubo = new Cubo('D', null);
            objetosLista.Add(obj_Cubo);
            objetoSelecionado = obj_Cubo;
            objetoSelecionado.Translacao(-4, 'x');
            objetoSelecionado.Translacao(-12, 'z');
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos

            obj_Cubo = new Cubo('D', null);
            objetosLista.Add(obj_Cubo);
            objetoSelecionado = obj_Cubo;
            objetoSelecionado.Translacao(-8, 'x');
            objetoSelecionado.Translacao(-12, 'z');
            rotacaoObjeto.AdicionarObjeto(obj_Cubo); // Rotacionar sozinho os objetos

            Objeto Esfera = new Esfera('E', null);
            Esfera.ObjetoCor.CorR = 177;
            Esfera.ObjetoCor.CorG = 166;
            Esfera.ObjetoCor.CorB = 136;
            objetosLista.Add(Esfera);
            Esfera.Translacao(4, 'x');
            rotacaoObjeto.AdicionarObjeto(Esfera); // Rotacionar sozinho os objetos

#if CG_Privado  //FIXME: arrumar os outros objetos
            objetoId = Utilitario.charProximo(objetoId);
            obj_Cilindro = new Cilindro(objetoId, null);
            obj_Cilindro.ObjetoCor.CorR = 177; obj_Cilindro.ObjetoCor.CorG = 166; obj_Cilindro.ObjetoCor.CorB = 136;
            objetosLista.Add(obj_Cilindro);
            obj_Cilindro.Translacao(2, 'x');

            objetoId = Utilitario.charProximo(objetoId);
            obj_Esfera = new Esfera(objetoId, null);
            obj_Esfera.ObjetoCor.CorR = 177; obj_Esfera.ObjetoCor.CorG = 166; obj_Esfera.ObjetoCor.CorB = 136;
            objetosLista.Add(obj_Esfera);
            obj_Esfera.Translacao(4, 'x');

            objetoId = Utilitario.charProximo(objetoId);
            obj_Cone = new Cone(objetoId, null);
            obj_Cone.ObjetoCor.CorR = 177; obj_Cone.ObjetoCor.CorG = 166; obj_Cone.ObjetoCor.CorB = 136;
            objetosLista.Add(obj_Cone);
            obj_Cone.Translacao(6, 'x');
#endif

            GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
            GL.Enable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.CullFace);
        }
        protected override void OnResize(EventArgs e){
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(camera.Fovy, Width / (float)Height, camera.Near, camera.Far);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);
        }

        protected override void OnUpdateFrame(FrameEventArgs e){
            base.OnUpdateFrame(e);
        }
        protected override void OnRenderFrame(FrameEventArgs e){
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            Matrix4 modelview = Matrix4.LookAt(camera.Eye, camera.At, camera.Up);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);
    #if CG_Gizmo      
            Sru3D();
#endif

            for (var i = 0; i < objetosLista.Count; i++)
                objetosLista[i].Desenhar();
            if (bBoxDesenhar && (objetoSelecionado != null))
                objetoSelecionado.BBox.Desenhar();

            this.SwapBuffers();
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e){
            // Console.Clear(); //TODO: não funciona.
            if (e.Key == Key.H) Utilitario.AjudaTeclado();
            else if (e.Key == Key.Escape) Exit();
            //--------------------------------------------------------------
            else if (e.Key == Key.Number9)
                objetoSelecionado = null;                     // desmacar objeto selecionado
            else if (e.Key == Key.B)
                bBoxDesenhar = !bBoxDesenhar;     //FIXME: bBox não está sendo atualizada.
            else if (e.Key == Key.E) {
                Console.WriteLine("--- Objetos / Pontos: ");
                for (var i = 0; i < objetosLista.Count; i++)
                    Console.WriteLine(objetosLista[i]);
            }
            else if (e.Key == Key.W || e.Key == Key.S || e.Key == Key.A || e.Key == Key.D) {
                camera.Andar(e.Key);
            }
            //--------------------------------------------------------------
            else if (e.Key == Key.X) menuEixoSelecao = 'x';
            else if (e.Key == Key.Y) menuEixoSelecao = 'y';
            else if (e.Key == Key.Z) menuEixoSelecao = 'z';
            else if (e.Key == Key.Minus) deslocamento--;
            else if (e.Key == Key.Plus) deslocamento++;
            else if (e.Key == Key.C) menuSelecao = "[menu] C: Câmera";
            else if (e.Key == Key.O) menuSelecao = "[menu] O: Objeto";

            // Menu: seleção
            else if (menuSelecao.Equals("[menu] C: Câmera")) 
                camera.MenuTecla(e.Key, menuEixoSelecao, deslocamento);
            else if (menuSelecao.Equals("[menu] O: Objeto")) {
                if (objetoSelecionado != null) objetoSelecionado.MenuTecla(e.Key, menuEixoSelecao, deslocamento, bBoxDesenhar);
                else Console.WriteLine(" ... Objeto NÃO selecionado.");
            }

            else
                Console.WriteLine(" __ Tecla não implementada.");

            if (!(e.Key == Key.LShift)) //FIXME: não funciona.
                Console.WriteLine("__ " + menuSelecao + "[" + deslocamento + "]");
        }

        protected override void OnMouseMove(MouseMoveEventArgs e){            
            mousePos.Y = 600 - e.Position.Y; // Inverti eixo Y
            if (mousePos.X < e.Position.X)
                camera.movimentarCabeca(Key.Right);
            else
                if(mousePos.X > e.Position.X)
                    camera.movimentarCabeca(Key.Left);
            mousePos.X = e.Position.X;
        }

#if CG_Gizmo
        private void Sru3D(){

            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            // GL.Color3(1.0f,0.0f,0.0f);
            GL.Color3(Convert.ToByte(255), Convert.ToByte(0), Convert.ToByte(0));
            GL.Vertex3(0, 0, 0); GL.Vertex3(200, 0, 0);
            // GL.Color3(0.0f,1.0f,0.0f);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(255), Convert.ToByte(0));
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 200, 0);
            // GL.Color3(0.0f,0.0f,1.0f);
            GL.Color3(Convert.ToByte(0), Convert.ToByte(0), Convert.ToByte(255));
            GL.Vertex3(0, 0, 0); GL.Vertex3(0, 0, 200);
            GL.End();
        }
#endif    
    }
    class Program{
        static void Main(string[] args){
            // não achei um método global que descubra a resolução do meu monitor,
            // por isso está fixo
            Mundo window = Mundo.GetInstance(1920, 1080);
            window.WindowBorder = WindowBorder.Hidden;
            //esconder o mouse
            //window.CursorVisible = false;
            window.Title = "CG_N4";
            window.Run(1.0 / 60.0);
        }
    }
}
