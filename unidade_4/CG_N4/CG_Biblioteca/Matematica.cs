/**
  Autor: Dalton Solano dos Reis
**/

using gcgcg;
using System;
using System.Collections.Generic;

namespace CG_Biblioteca{
    /// <summary>
    /// Classe com funções matemáticas.
    /// </summary>
    public abstract class Matematica{
    /// <summary>
    /// Função para calcular um ponto sobre o perímetro de um círculo informando um ângulo e raio.
    /// </summary>
    /// <param name="angulo"></param>
    /// <param name="raio"></param>
    /// <returns></returns>
        public static Ponto4D GerarPtosCirculo(double angulo, double raio){
            Ponto4D pto = new Ponto4D();
            pto.X = (raio * Math.Cos(Math.PI * angulo / 180.0));
            pto.Y = (raio * Math.Sin(Math.PI * angulo / 180.0));
            pto.Z = 0;
            return (pto);
        }

        public static double GerarPtosCirculoSimétrico(double raio){         
            return (raio * Math.Cos(Math.PI * 45 / 180.0));
        }

        public static double distanciaEuclidiana(Ponto4D p1, Ponto4D p2){
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2)+ Math.Pow(p1.Y - p2.Y,2));
        }

        private static double calculati(Ponto4D p1, Ponto4D p2, Ponto4D mousePos){
            return (mousePos.Y - p1.Y) / (p2.Y - p1.Y); // Fórmula, parte 1
        }

        private static double calculaxi(Ponto4D p1, Ponto4D p2, double ti){
            return p1.X + (p2.X - p1.X) * ti; // Fórmula, parte 2
        }
        public static bool ScanLine(List<Ponto4D> pontosLista, Ponto4D mousePos){
            if (pontosLista.Count < 2){
                return false;
            }else{
                List<Ponto4D> listaIntersecao = new List<Ponto4D>();
                double ti = 0;
                double xi = 0;
                Ponto4D primeiroPonto = pontosLista[0];
                Ponto4D ultimoPonto = pontosLista[pontosLista.Count - 1];
                for (int i = 0; i < pontosLista.Count - 1; i++){
                    ti = Matematica.calculati(pontosLista[i], pontosLista[i + 1], mousePos);
                    if (ti > 0 && ti < 1){
                        xi = calculaxi(pontosLista[i], pontosLista[i + 1], ti);
                        if (xi > mousePos.X)
                            listaIntersecao.Add(new Ponto4D(xi, ti));
                    }
                }
                ti = Matematica.calculati(ultimoPonto, primeiroPonto, mousePos);
                if (ti > 0 && ti < 1){
                    xi = calculaxi(ultimoPonto, primeiroPonto, ti);
                    if (xi > mousePos.X)
                        listaIntersecao.Add(new Ponto4D(xi, ti));
                }
                return listaIntersecao.Count % 2 != 0; // Fórmula, parte 3
            }
        }        
    }
}