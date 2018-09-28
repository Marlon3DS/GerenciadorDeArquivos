using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Apresentacao
{
    public class Gerenciar
    {
        public static Arquivos ListarArquivos(string caminho)
        {
            try
            {
                string[] todosArquivos = CarregarArquivos(caminho);
                Arquivos arquivos = new Arquivos();
                foreach (string caminhoArquivo in todosArquivos)
                {
                    arquivos.Add(ConverterArquivo(caminhoArquivo));
                }
                return arquivos;
            }
            catch (Exception excecao)
            {
                throw new Exception(excecao.Message);
            }
        }

        public static string PegarPasta(string caminho)
        {
            try
            {
                int indice = caminho.LastIndexOf("\\") + 1;
                string pasta = caminho.Substring(indice);
                return pasta;
            }
            catch (Exception excecao)
            {
                throw new Exception(excecao.Message);
            }
        }

        private static string[] CarregarArquivos(string caminho)
        {
            try
            {
                string nomeArquivo = "*.*";
                string[] todosArquivos = Directory.GetFiles(caminho, nomeArquivo, SearchOption.AllDirectories);
                return todosArquivos;
            }
            catch (Exception excecao)
            {
                throw new Exception(excecao.Message);
            }
        }

        private static Arquivo ConverterArquivo(string caminhoArquivo)
        {
            try
            {
                Arquivo arquivo = new Arquivo();
                int indiceInicial=-1, indiceFinal=-1, comprimento=0;
                string subPasta = "";

                indiceInicial = caminhoArquivo.LastIndexOf("\\") + 1;
                indiceFinal = caminhoArquivo.LastIndexOf(".");
                comprimento = indiceFinal - indiceInicial;
                subPasta = PegarPasta(caminhoArquivo.Substring(0, indiceInicial - 1));

                arquivo.CaminhoCompleto = caminhoArquivo;
                arquivo.Nome = caminhoArquivo.Substring(indiceInicial, comprimento);
                arquivo.Extensao = caminhoArquivo.Substring(indiceFinal + 1);
                arquivo.SubPasta = subPasta;

                return arquivo;
            }
            catch (Exception excecao)
            {
                throw new Exception(excecao.Message);
            }
        }
        
        public void Copiar(Arquivo arquivo, string destino)
        {
            try
            {
                string nomeDestino = destino + "\\" + arquivo.Nome + "." + arquivo.Extensao;
                File.Copy(arquivo.CaminhoCompleto, nomeDestino, true);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public void Mover(Arquivo arquivo, string destino)
        {
            try
            {
                string nomeArquivo = arquivo.Nome + "." + arquivo.Extensao;
                string caminhoDestino = destino + "\\" + nomeArquivo;
                if (File.Exists(caminhoDestino))
                {
                    File.Delete(caminhoDestino);
                }
                File.Move(arquivo.CaminhoCompleto, caminhoDestino);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public enum Diretorio
        {
            Origem,
            Destino
        }

        public enum Operacao
        {
            Pesquisar,
            Copiar,
            Mover
        }
    }
}
