using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
    internal class Circulo : ObjetoGeometria{
        public Circulo(string rotulo, Objeto paiRef, double angulo, int xOrigem, int yOrigem) : base(rotulo, paiRef){
            this.PrimitivaTipo = PrimitiveType.Points;
            double totalPontos = 360 / angulo;
            Ponto4D ponto4D;
            for (int i = 1; i <= totalPontos; i++) {
                ponto4D = Matematica.GerarPtosCirculo(i * (360 / totalPontos), 100);
                ponto4D.X += xOrigem;
                ponto4D.Y += yOrigem;
                base.PontosAdicionar(ponto4D);
            }
        }
        protected override void DesenharObjeto()
        {
            GL.PointSize(5);
            GL.Begin(base.PrimitivaTipo);
            foreach (Ponto4D pto in pontosLista){
                GL.Vertex2(pto.X, pto.Y);
            }
            GL.End();
        }
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Circulo: " + base.rotulo + "\n";
            for (var i = 0; i < pontosLista.Count; i++){
                retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
            }
            return (retorno);
        }
    }
}