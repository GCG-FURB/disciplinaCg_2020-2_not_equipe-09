using System;
using OpenTK;
using OpenTK.Input;

namespace CG_Biblioteca
{
    public class CameraPerspective
    {
        private float fovy;
        private float aspect;
        private float near;
        private float far;
        private Vector3 eye;
        private Vector3 at;
        private Vector3 up;

        private float deslocamentoX = 270;
        private enum menuCameraEnum { eye, at, near, far, fovy }
        private menuCameraEnum menuCameraOpcao;

        public CameraPerspective(float fovy = (float)Math.PI / 4, float aspect = 1.0f, float near = 1.0f, float far = 50.0f)
        {
            this.fovy   = fovy;
            this.aspect = aspect;
            this.near   = near;
            this.far    = far * 3; // Analisando o código, foi visto que alterando o far em tempo de execução, ele não aumenta o campo de visão... tem que criar uma nova estância de CameraPerspective

            eye   = Vector3.Zero; 
            eye.Z = 15;   // ( 0, 0,15)
            at    = Vector3.Zero;                // ( 0, 0, 0)
            up    = Vector3.UnitY;               // ( 0, 1, 0)

            menuCameraOpcao = menuCameraEnum.eye;
        }

        public float Fovy { get => fovy; set => fovy = value; }
        public float Aspect { get => aspect; set => aspect = value; }
        public float Near { get => near; set => near = value; }
        public float Far { get => far; set => far = value; }
        public Vector3 Eye { get => eye; set => eye = value; }
        public Vector3 At { get => at; set => at = value; }
        public Vector3 Up { get => up; }

        private void FovyDes(float deslocamento) { fovy += deslocamento; }
        private void NearDes(float deslocamento) { near += deslocamento; }
        private void FarDes(float deslocamento) { far += deslocamento; }

        private void EyeDes(float deslocamento, char eixo){
            switch (eixo){
                case 'x': eye.X += deslocamento; break;
                case 'y': eye.Y += deslocamento; break;
                case 'z': eye.Z += deslocamento; break;
            }
        }

        private void AtDes(float deslocamento, char eixo){
            switch (eixo){
                case 'x': at.X += deslocamento; break;
                case 'y': at.Y += deslocamento; break;
                case 'z': at.Z += deslocamento; break;
            }
        }

        public void MenuTecla(OpenTK.Input.Key tecla, char eixo, float deslocamento)
        {
            if (tecla == Key.P)
                Console.WriteLine(this);
            else if (tecla == Key.R)
            {
                fovy = (float)Math.PI / 4;   //TODO: existe algo do tipo this(); em java
                aspect = 1.0f;
                near = 1.0f;
                far = 50.0f;
                eye = Vector3.Zero; eye.Z = 15;   // ( 0, 0,15)
                at = Vector3.Zero;                // ( 0, 0, 0)
                up = Vector3.UnitY;               // ( 0, 1, 0)
            }
            else if (tecla == Key.Up) menuCameraOpcao++;
            else if (tecla == Key.Down) menuCameraOpcao--; //TODO: qdo chega indice 0 não vai para o final
            else if (tecla == Key.Left) deslocamento = -deslocamento;

            if (!Enum.IsDefined(typeof(menuCameraEnum), menuCameraOpcao))
                menuCameraOpcao = menuCameraEnum.eye;
            Console.WriteLine("__ Câmera (" + menuCameraOpcao + "," + eixo + "," + deslocamento + ")");
            if ((tecla == Key.Left) || (tecla == Key.Right))
            {
                switch (menuCameraOpcao)
                {
                    case menuCameraEnum.eye: EyeDes(deslocamento, eixo); break;
                    case menuCameraEnum.at: AtDes(deslocamento, eixo); break;
                    case menuCameraEnum.near: NearDes(deslocamento); break;
                    case menuCameraEnum.far: FarDes(deslocamento); break;
                    case menuCameraEnum.fovy: FovyDes(deslocamento); break;
                }
            }
        }

        public void Andar(OpenTK.Input.Key tecla)
        {
            // Câmera em primeira pessoa (FPS)
            if (tecla == Key.W){
                eye.X = eye.X + (at.X * ((float)0.5)) / 100;
                eye.Z = eye.Z + (at.Z * ((float)0.5)) / 100;
            }
            else if (tecla == Key.S){
                eye.X = eye.X - (at.X * ((float)0.5)) / 100;
                eye.Z = eye.Z - (at.Z * ((float)0.5)) / 100;
            }
            else if(tecla == Key.A){
                eye.X = (float)(100 * Math.Cos(Math.PI * 0 / 180)) + deslocamentoX;
                eye.Z = (float)(100 * Math.Sin(Math.PI * 0 / 180)) + deslocamentoX;
            }
            else if(tecla == Key.D){

            }
        }
        
        public void movimentarCabeca(OpenTK.Input.Key tecla)
        {
            if (tecla == Key.Left)
            {
                deslocamentoX--;
                deslocamentoX--;
                at.X = (float)(100 * Math.Cos(Math.PI * deslocamentoX / 180));
                at.Z = (float)(100 * Math.Sin(Math.PI * deslocamentoX / 180));
                if (deslocamentoX < 0)
                    deslocamentoX = 360;

            }
            else if (tecla == Key.Right)
            {
                deslocamentoX++;
                deslocamentoX++;
                at.X = (float)(100 * Math.Cos(Math.PI * deslocamentoX / 180));
                at.Z = (float)(100 * Math.Sin(Math.PI * deslocamentoX / 180));
                if (deslocamentoX > 360)
                    deslocamentoX = 0;
            }
        }

        public override string ToString()
        {
            string retorno;
            retorno = "__ CameraPerspective: " + "\n";
            retorno += "eye [" + eye.X + "," + eye.Y + "," + eye.Z + "]" + "\n";
            retorno += "at [" + at.X + "," + at.Y + "," + at.Z + "]" + "\n";
            retorno += "up [" + up.X + "," + up.Y + "," + up.Z + "]" + "\n";
            retorno += "near: " + near + "\n";
            retorno += "deslocamentoX? " + deslocamentoX + "\n";
            retorno += "far: " + far + "\n";
            retorno += "fovy: " + fovy + "\n";
            retorno += "aspect: " + aspect + "\n";
            return (retorno);
        }

    }
}
