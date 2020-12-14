using CG_Biblioteca;
using OpenTK.Graphics.OpenGL;

namespace gcgcg
{
    internal class BBoxInternaCirculo : ObjetoGeometria
    {
        public BBoxInternaCirculo(char rotulo, Circulo paiRef, Ponto4D referencia) : base(rotulo, paiRef)
        {
            base.rotulo = rotulo;
            this.paiRef = paiRef;
            if (paiRef != null){
                Ponto4D p1 = Matematica.GerarPtosCirculo(45, ((Circulo)paiRef).raio);
                p1.X += referencia.X;
                p1.Y += referencia.Y;
                base.PontosAdicionar(p1);
                Ponto4D p2 = Matematica.GerarPtosCirculo(135, ((Circulo)paiRef).raio);
                p2.X += referencia.X;
                p2.Y += referencia.Y;
                base.PontosAdicionar(p2);
                Ponto4D p3 = Matematica.GerarPtosCirculo(225, ((Circulo)paiRef).raio);
                p3.X += referencia.X;
                p3.Y += referencia.Y;
                base.PontosAdicionar(p3);
                Ponto4D p4 = Matematica.GerarPtosCirculo(315, ((Circulo)paiRef).raio);
                p4.X += referencia.X;
                p4.Y += referencia.Y;
                base.PontosAdicionar(p4);
                obterMaiorX = Matematica.GerarPtosCirculo(45, ((Circulo)paiRef).raio).X + referencia.X;
                obterMenorX = Matematica.GerarPtosCirculo(135, ((Circulo)paiRef).raio).X + referencia.X;
                obterMaiorY = Matematica.GerarPtosCirculo(45, ((Circulo)paiRef).raio).Y + referencia.Y;
                obterMenorY = Matematica.GerarPtosCirculo(225, ((Circulo)paiRef).raio).Y + referencia.Y;
            }
            ObjetoCor.CorR = 169;
            ObjetoCor.CorG = 122;
            ObjetoCor.CorB = 171;
        }
        public double obterMenorX { get; set; }
        public double obterMenorY { get; set; }
        public double obterMaiorX { get; set; }
        public double obterMaiorY { get; set; }

        protected override void DesenharObjeto(){
            GL.LineWidth(2);
            GL.Begin(base.PrimitivaTipo);
            foreach (Ponto4D pto in pontosLista){
                GL.Vertex2(pto.X, pto.Y);
            }
            GL.End();
        }
    }
}
