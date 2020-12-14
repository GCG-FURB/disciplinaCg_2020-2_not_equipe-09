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

namespace gcgcg{
    class Mundo : GameWindow {

        private static Mundo instanciaMundo = null;

        private Mundo(int width, int height) : base(width, height) { }

        public static Mundo GetInstance(int width, int height) {
            if (instanciaMundo == null)
                instanciaMundo = new Mundo(width, height);
            return instanciaMundo;
        }

        private CameraOrtho camera = new CameraOrtho();
        protected List<Objeto> objetosLista = new List<Objeto>();
        private ObjetoGeometria objetoSelecionado = null;
        private bool bBoxDesenhar = false;
        Ponto4D mousePos = new Ponto4D();
        Ponto4D pontoSelecionado = null;
        Ponto4D pontoFilho = null;
        private Poligono objetoNovo = null;
        private bool movimentarVertice = false;
        private char objetoId = '@';
        private bool girarObj = false;
#if CG_Privado
        private Retangulo obj_Retangulo;
        private Privado_SegReta obj_SegReta;
        private Privado_Circulo obj_Circulo;
#endif         

        protected override void OnLoad(EventArgs e) {            

            base.OnLoad(e);

            foreach(JoystickDevice joy in this.Joysticks)
            {
                Console.WriteLine(joy.Description);
                //Console.WriteLine();
                //Console.WriteLine(joy.Button(0));
            }
            

            camera.xmin = 0;
            camera.xmax = 600;
            camera.ymin = 0;
            camera.ymax = 600;

            objetosLista.Add(new Ponto('P', null, mousePos));

            Console.WriteLine(" --- Ajuda / Teclas: ");
            Console.WriteLine(" [  H     ] mostra teclas usadas. ");
#if CG_Privado
            obj_Retangulo = new Retangulo(objetoId + 1, null, new Ponto4D(50, 50, 0), new Ponto4D(150, 150, 0));
            obj_Retangulo.ObjetoCor.CorR = 255; obj_Retangulo.ObjetoCor.CorG = 0; obj_Retangulo.ObjetoCor.CorB = 255;
            objetosLista.Add(obj_Retangulo);
            objetoSelecionado = obj_Retangulo;

            obj_SegReta = new Privado_SegReta(objetoId + 1, null, new Ponto4D(50, 150), new Ponto4D(150, 250));
            obj_SegReta.ObjetoCor.CorR = 255; obj_SegReta.ObjetoCor.CorG = 99; obj_SegReta.ObjetoCor.CorB = 71;
            objetosLista.Add(obj_SegReta);
            objetoSelecionado = obj_SegReta;

            obj_Circulo = new Privado_Circulo(objetoId + 1, null, new Ponto4D(100, 300), 50);
            obj_Circulo.ObjetoCor.CorR = 177; obj_Circulo.ObjetoCor.CorG = 166; obj_Circulo.ObjetoCor.CorB = 136;
            objetosLista.Add(obj_Circulo);
            objetoSelecionado = obj_Circulo;
#endif
            GL.ClearColor(0.5f, 0.5f, 0.5f, 1.0f);
        }
        protected override void OnUpdateFrame(FrameEventArgs e) {
            base.OnUpdateFrame(e);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(camera.xmin, camera.xmax, camera.ymin, camera.ymax, camera.zmin, camera.zmax);
        }
        protected override void OnRenderFrame(FrameEventArgs e) {
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
            if (objetoNovo != null)
                objetoNovo.Desenhar();
            this.SwapBuffers();
        }

        protected override void OnKeyDown(OpenTK.Input.KeyboardKeyEventArgs e) {
            if (e.Key == Key.H)
                Utilitario.AjudaTeclado();
            else if (e.Key == Key.Escape)
                Exit();
            else if (e.Key == Key.E){
                Console.WriteLine("--- Objetos / Pontos: ");
                for (var i = 0; i < objetosLista.Count; i++)
                    Console.WriteLine(objetosLista[i]);
            }
            else if (e.Key == Key.O)
                bBoxDesenhar = !bBoxDesenhar;
            else if (e.Key == Key.Enter)
                EfetivarPontosPoligono();
            else if (e.Key == Key.Space)
                AdicionarNovoPoligonoPonto();
            else if (e.Key == Key.D)
                RemoverPontoMaisProximo();
            else if (e.Key == Key.S)
                AlterarPrimitivaTipo();
            else if (e.Key == Key.V)
                MoverPontoMaisProximo();
            else if (e.Key == Key.Number8)
                SelecionarObjetoMaisProximo();
            else if (objetoSelecionado != null){
                if (e.Key == Key.M)
                    Console.WriteLine(objetoSelecionado.Matriz);
                else if (e.Key == Key.P)
                    Console.WriteLine(objetoSelecionado);
                else if (e.Key == Key.I)
                    objetoSelecionado.AtribuirIdentidade();
                else if (e.Key == Key.Left)
                    objetoSelecionado.TranslacaoXYZ(-10, 0, 0);
                else if (e.Key == Key.Right)
                    objetoSelecionado.TranslacaoXYZ(10, 0, 0);
                else if (e.Key == Key.Up)
                    objetoSelecionado.TranslacaoXYZ(0, 10, 0);
                else if (e.Key == Key.Down)
                    objetoSelecionado.TranslacaoXYZ(0, -10, 0);
                else if (e.Key == Key.PageUp)
                    objetoSelecionado.EscalaXYZ(2, 2, 2);
                else if (e.Key == Key.PageDown)
                    objetoSelecionado.EscalaXYZ(0.5, 0.5, 0.5);
                else if (e.Key == Key.Home)
                    objetoSelecionado.EscalaXYZBBox(0.5, 0.5, 0.5);
                else if (e.Key == Key.End)
                    objetoSelecionado.EscalaXYZBBox(2, 2, 2);
                else if (e.Key == Key.Number1)
                    objetoSelecionado.Rotacao(10);
                else if (e.Key == Key.Number2)
                    objetoSelecionado.Rotacao(-10);
                else if (e.Key == Key.Number3)
                    objetoSelecionado.RotacaoZBBox(10);
                else if (e.Key == Key.Number4)
                    objetoSelecionado.RotacaoZBBox(-10);
                else if (e.Key == Key.Number9)
                    objetoSelecionado = null;   // desmarcar objeto selecionado
                else if (e.Key == Key.C)
                    RemoverPoligono();
                else if (e.Key == Key.AltLeft)
                    girarObj = !girarObj;
                else if (e.Key == Key.R || e.Key == Key.G || e.Key == Key.B)
                    AtualizaCorObjeto(objetoSelecionado, (byte)(Convert.ToInt32(e.Key == Key.R) * 255), (byte)(Convert.ToInt32(e.Key == Key.G) * 255), (byte)(Convert.ToInt32(e.Key == Key.B) * 255));
                else if (e.Key == Key.K)
                    movimentarVertice = !movimentarVertice;
                else
                    Console.WriteLine("Tecla " + e.Key + " não implementada.");
            }
            else
                Console.WriteLine("Tecla " + e.Key + " não implementada.");
        }

        private void EfetivarPontosPoligono(){
            if (objetoNovo != null){
                objetoNovo.PontosRemoverUltimo();   
                if (objetoSelecionado != null)
                    objetoSelecionado.FilhoAdicionar(objetoNovo);
                else{
                    objetosLista.Add(objetoNovo);
                    objetoSelecionado = objetoNovo;                    
                }
                    
                objetoNovo = null;
            }
        }

        private void RemoverPoligono(){
            if (objetosLista.Remove(objetoSelecionado)){
                objetoSelecionado.RemoverTodosFilhos();
                objetoSelecionado = null;
            }
        }

        private void AdicionarNovoPoligonoPonto(){
            if (objetoNovo == null){
                objetoNovo = new Poligono(objetoId, null);
                objetosLista.Add(objetoNovo);
                objetoNovo.PontosAdicionar(new Ponto4D(mousePos.X, mousePos.Y));
                objetoNovo.PontosAdicionar(new Ponto4D(mousePos.X, mousePos.Y));
            }
            else
                objetoNovo.PontosAdicionar(new Ponto4D(mousePos.X, mousePos.Y));
        }

        private void RemoverPontoMaisProximo(){
            if (pontoSelecionado == null)
                pontoSelecionado = RetornaPontoMaisProximo(mousePos, false);
            if (pontoSelecionado == null) {
                pontoSelecionado.X = mousePos.X;
                pontoSelecionado.Y = mousePos.Y;
            }
            if (pontoSelecionado != null)
                foreach (ObjetoGeometria obj in objetosLista)
                    if (obj.pontosLista.Remove(pontoSelecionado)){
                        pontoSelecionado = null;
                        break;
                    }
        }

        private void AlterarPrimitivaTipo(){
            if (objetoNovo != null)
                if (objetoNovo.PrimitivaTipo == PrimitiveType.LineLoop)
                    objetoNovo.PrimitivaTipo = PrimitiveType.Lines;
                else
                    objetoNovo.PrimitivaTipo = PrimitiveType.LineLoop;
        }

        private void MoverPontoMaisProximo(){
            if (pontoSelecionado == null)
                pontoSelecionado = RetornaPontoMaisProximo(mousePos, false);
            if (pontoSelecionado == null){
                pontoSelecionado.X = mousePos.X;
                pontoSelecionado.Y = mousePos.Y;
            }
        }

        private void AtualizaCorObjeto(ObjetoGeometria obj, byte R, byte G, byte B){
            obj.ObjetoCor.CorR = R;
            obj.ObjetoCor.CorG = G;
            obj.ObjetoCor.CorB = B;
        }

        protected override void OnMouseMove(MouseMoveEventArgs e) {
            if (objetoSelecionado != null){
                if (!girarObj){
                    if (mousePos.X < e.Position.X){
                        objetoSelecionado.TranslacaoXYZ(e.Position.X - mousePos.X, 0, 0);
                    }else
                        objetoSelecionado.TranslacaoXYZ(e.Position.X - mousePos.X, 0, 0);
                    if (mousePos.Y < (600 - e.Position.Y)){
                        objetoSelecionado.TranslacaoXYZ(0, (600 - e.Position.Y) - mousePos.Y, 0);
                    }else
                        objetoSelecionado.TranslacaoXYZ(0, (600 - e.Position.Y) - mousePos.Y, 0);
                }else{
                    if(mousePos.X < e.Position.X){                        
                        objetoSelecionado.RotacaoZBBox(-10);
                    }else{
                        objetoSelecionado.RotacaoZBBox(10);
                    }
                }
            }
            
            mousePos.X = e.Position.X;
            mousePos.Y = 600 - e.Position.Y; // Inverti eixo Y
            if (objetoNovo != null){
                objetoNovo.PontosUltimo().X = mousePos.X;
                objetoNovo.PontosUltimo().Y = mousePos.Y;
            }

            // SelecionarObjetoMaisProximo();
            if (pontoSelecionado != null && movimentarVertice){
                pontoSelecionado.X = mousePos.X;
                pontoSelecionado.Y = mousePos.Y;
            }                
        }

        private void SelecionarObjetoMaisProximo(){
            if (objetoSelecionado == null)
                foreach (ObjetoGeometria obj in objetosLista)
                    if(mousePos.X >= obj.BBox.obterMenorX
                    && mousePos.X <= obj.BBox.obterMaiorX 
                    && mousePos.Y >= obj.BBox.obterMenorY
                    && mousePos.Y <= obj.BBox.obterMaiorY) {
                        if (Matematica.ScanLine(obj.pontosLista, mousePos)){
                            objetoSelecionado = obj;
                            break;
                        }
                    }
        }

        protected override void OnMouseDown(MouseButtonEventArgs e){
            if (e.Mouse.LeftButton == ButtonState.Pressed){
                //Ponto4D p = RetornaPontoMaisProximo(mousePos, true);
                //if (p != null)
                //    pontoSelecionado = p;

                if (objetoNovo == null)
                {
                    objetoNovo = new Poligono(objetoId, null);
                    //if(objetoSelecionado == null)
                    //    objetosLista.Add(objetoNovo);
                    objetoNovo.PontosAdicionar(new Ponto4D(mousePos.X, mousePos.Y));
                    objetoNovo.PontosAdicionar(new Ponto4D(mousePos.X, mousePos.Y));  // N3-Exe6: "truque" para deixar o rastro
                }
                else
                    objetoNovo.PontosAdicionar(new Ponto4D(mousePos.X, mousePos.Y));
            }

        }
        protected override void OnMouseWheel(MouseWheelEventArgs e){
            if (objetoSelecionado != null){
                if (e.DeltaPrecise < 0)
                    objetoSelecionado.EscalaXYZ(0.5, 0.5, 0.5);
                if (e.DeltaPrecise > 0)
                    objetoSelecionado.EscalaXYZ(2, 2, 2);
            }
        }

        private Ponto4D RetornaPontoMaisProximo(Ponto4D mouse, bool objetoSelec){
            Ponto4D result = null;
            double menorDistancia = 9999;
            double distanciaEuclidiana = 0;
            foreach (ObjetoGeometria obj in objetosLista)
                if((objetoSelec && objetoSelecionado == obj)
                || !objetoSelec)
                    foreach (Ponto4D p in obj.pontosLista){
                        distanciaEuclidiana = Matematica.distanciaEuclidiana(mousePos, p);
                        if (distanciaEuclidiana < menorDistancia){
                            menorDistancia = distanciaEuclidiana;
                            result = p;
                        }
                    }
            return result;
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
            Mundo window = Mundo.GetInstance(600, 600);
            window.Title = "CG_N3";
            window.Run(1.0 / 60.0);
        }
    }
}
