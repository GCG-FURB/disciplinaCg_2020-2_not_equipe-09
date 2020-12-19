using CG_Biblioteca;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace gcgcg
{
    internal class Cubo : ObjetoGeometria
    {
        private bool exibeVetorNormal = false;
        private int texture;
        public Cubo(char rotulo, Objeto paiRef) : base(rotulo, paiRef)
        {
            if (this.paiRef != null)
                this.paiRef.FilhoAdicionar(this);
            base.PontosAdicionar(new Ponto4D(-0.5, -0.5, 0.5)); // PtoA listaPto[0]
            base.PontosAdicionar(new Ponto4D(0.5, -0.5, 0.5)); // PtoB listaPto[1]
            base.PontosAdicionar(new Ponto4D(0.5, 0.5, 0.5)); // PtoC listaPto[2]
            base.PontosAdicionar(new Ponto4D(-0.5, 0.5, 0.5)); // PtoD listaPto[3]
            base.PontosAdicionar(new Ponto4D(-0.5, -0.5, -0.5)); // PtoE listaPto[4]
            base.PontosAdicionar(new Ponto4D(0.5, -0.5, -0.5)); // PtoF listaPto[5]
            base.PontosAdicionar(new Ponto4D(0.5, 0.5, -0.5)); // PtoG listaPto[6]
            base.PontosAdicionar(new Ponto4D(-0.5, 0.5, -0.5)); // PtoH listaPto[7]

            //Precisei descobrir onde está o path do bendito executável
            Console.WriteLine(Directory.GetCurrentDirectory());
            string imgPath = "E:\\Notebook\\FURB\\6 Semestre\\Computação Gráfica\\Unidade 4\\CG_N4\\textures\\box.jpg";
            // O comentários mais importante de todos:
            // Se caso, a imagem não exisir ou não conseguir carregar ou tiver 0px 0px, vai estourar exceção "Parameter is not valid"
            System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(imgPath);

            //TODO: o que faz está linha abaixo?
            GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
            GL.GenTextures(1, out texture);
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D,
                          0,
                          PixelInternalFormat.Rgba,
                          data.Width,
                          data.Height,
                          0,
                          OpenTK.Graphics.OpenGL.PixelFormat.Bgra, 
                          PixelType.UnsignedByte, 
                          data.Scan0);

            bitmap.UnlockBits(data);

        }
    
        protected override void DesenharObjeto()
        {
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, texture);

            // Sentido anti-horário
            GL.Begin(PrimitiveType.Quads);
            // Face da frente (vermelho)
            GL.Color3(1.0f,0.0f,0.0f);
            GL.Normal3(0, 0, 1);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(base.pontosLista[0].X, base.pontosLista[0].Y, base.pontosLista[0].Z);    // PtoA
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(base.pontosLista[1].X, base.pontosLista[1].Y, base.pontosLista[1].Z);    // PtoB
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(base.pontosLista[2].X, base.pontosLista[2].Y, base.pontosLista[2].Z);    // PtoC
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(base.pontosLista[3].X, base.pontosLista[3].Y, base.pontosLista[3].Z);    // PtoD
            // Face do fundo (verde)
            GL.Color3(0.0f,1.0f,0.0f);
            GL.Normal3(0, 0, -1);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(base.pontosLista[4].X, base.pontosLista[4].Y, base.pontosLista[4].Z);    // PtoE
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(base.pontosLista[7].X, base.pontosLista[7].Y, base.pontosLista[7].Z);    // PtoH
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(base.pontosLista[6].X, base.pontosLista[6].Y, base.pontosLista[6].Z);    // PtoG
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(base.pontosLista[5].X, base.pontosLista[5].Y, base.pontosLista[5].Z);    // PtoF
            // Face de cima (azul)
            GL.Color3(0.0f,0.0f,1.0f);
            GL.Normal3(0, 1, 0);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(base.pontosLista[3].X, base.pontosLista[3].Y, base.pontosLista[3].Z);    // PtoD
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(base.pontosLista[2].X, base.pontosLista[2].Y, base.pontosLista[2].Z);    // PtoC
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(base.pontosLista[6].X, base.pontosLista[6].Y, base.pontosLista[6].Z);    // PtoG
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(base.pontosLista[7].X, base.pontosLista[7].Y, base.pontosLista[7].Z);    // PtoH
            // Face de baixo (amarelo)
            GL.Color3(1.0f,1.0f,0.0f);
            GL.Normal3(0, -1, 0);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(base.pontosLista[0].X, base.pontosLista[0].Y, base.pontosLista[0].Z);    // PtoA
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(base.pontosLista[4].X, base.pontosLista[4].Y, base.pontosLista[4].Z);    // PtoE
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(base.pontosLista[5].X, base.pontosLista[5].Y, base.pontosLista[5].Z);    // PtoF
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(base.pontosLista[1].X, base.pontosLista[1].Y, base.pontosLista[1].Z);    // PtoB
            // Face da direita (ciano)
            GL.Color3(0.0f,1.0f,1.0f);
            GL.Normal3(1, 0, 0);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(base.pontosLista[1].X, base.pontosLista[1].Y, base.pontosLista[1].Z);    // PtoB
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(base.pontosLista[5].X, base.pontosLista[5].Y, base.pontosLista[5].Z);    // PtoF
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(base.pontosLista[6].X, base.pontosLista[6].Y, base.pontosLista[6].Z);    // PtoG
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(base.pontosLista[2].X, base.pontosLista[2].Y, base.pontosLista[2].Z);    // PtoC
            // Face da esquerda (magenta)
            GL.Color3(1.0f,0.0f,1.0f);
            GL.Normal3(-1, 0, 0);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex3(base.pontosLista[0].X, base.pontosLista[0].Y, base.pontosLista[0].Z);    // PtoA
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex3(base.pontosLista[3].X, base.pontosLista[3].Y, base.pontosLista[3].Z);    // PtoD
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex3(base.pontosLista[7].X, base.pontosLista[7].Y, base.pontosLista[7].Z);    // PtoH
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex3(base.pontosLista[4].X, base.pontosLista[4].Y, base.pontosLista[4].Z);    // PtoE
            GL.End();

            GL.Disable(EnableCap.Texture2D);
            // if (exibeVetorNormal) //TODO: acho que não precisa.
            //   ajudaExibirVetorNormal(); //TODO: acho que não precisa.
        }

        //TODO: melhorar para exibir não só a lista de pontos (geometria), mas também a topologia ... poderia ser listado estilo OBJ da Wavefrom
        public override string ToString()
        {
            string retorno;
            retorno = "__ Objeto Cubo: " + base.rotulo + "\n";
            for (var i = 0; i < pontosLista.Count; i++)
            {
            retorno += "P" + i + "[" + pontosLista[i].X + "," + pontosLista[i].Y + "," + pontosLista[i].Z + "," + pontosLista[i].W + "]" + "\n";
            }
            return (retorno);
        }

    }
}