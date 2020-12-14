using OpenTK.Graphics.OpenGL;
using CG_Biblioteca;

namespace gcgcg
{
    internal class Circulo : ObjetoGeometria{

        private Ponto4D origem = new Ponto4D();
        private bool mostrarOrigem;
        public double raio;
        public BBoxInternaCirculo BBoxInternaCirculo;

        public Circulo(char rotulo, Objeto paiRef, double angulo, double raio, bool mostrarOrigem, Ponto4D referencia) : base(rotulo, paiRef){
            base.rotulo = rotulo;
            this.paiRef = paiRef;            
            this.raio = raio;
            this.PrimitivaTipo = PrimitiveType.LineLoop;
            this.mostrarOrigem = mostrarOrigem;
            origem = referencia;

            double totalPontos = 360 / angulo;
            for (int i = 1; i <= totalPontos; i++) {
                Ponto4D p = Matematica.GerarPtosCirculo(i * (360 / totalPontos), raio);
                //p.X += referencia.X;
                //p.Y += referencia.Y;
                
                base.PontosAdicionar(p);
            }
            BBoxInternaCirculo = new BBoxInternaCirculo('B', this, referencia);
        }

        public Ponto4D PontoOrigem(){
            return origem;
        }

        protected override void DesenharObjeto()
        {
            if(mostrarOrigem){
                GL.PointSize(5);
                GL.Begin(PrimitiveType.Points);
                GL.Vertex2(origem.X, origem.Y);
                GL.End();
            }

            ObjetoCor.CorR = 0;
            ObjetoCor.CorG = 0;
            ObjetoCor.CorB = 0;
            GL.LineWidth(2);
            GL.Begin(base.PrimitivaTipo);
            foreach (Ponto4D pto in pontosLista){
                GL.Vertex2(pto.X + origem.X, pto.Y + origem.Y);
            }
            GL.End();

            if (!mostrarOrigem)
                BBoxInternaCirculo.Desenhar();
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