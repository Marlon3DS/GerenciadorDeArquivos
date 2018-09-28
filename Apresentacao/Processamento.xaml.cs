using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static Apresentacao.Gerenciar;

namespace Apresentacao
{
    /// <summary>
    /// Lógica interna para Processamento.xaml
    /// </summary>
    public partial class Processamento : Window
    {
        public Arquivos _Arquivos { get; set; }
        public string _Caminho { get; set; }
        public Operacao _Operacao { get; set; }

        public Processamento()
        {
            InitializeComponent();
        }

        public Processamento(Arquivos arquivos, string caminhoDestino, Operacao operacao)
        {
            InitializeComponent();
            _Arquivos = arquivos;
            _Caminho = caminhoDestino;
            _Operacao = operacao;
        }

        private void BtnFechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            switch (_Operacao)
            {
                case Operacao.Copiar:
                    CopiarArquivos();
                    break;
                case Operacao.Mover:
                    MoverArquivos();
                    break;
                default:
                    break;
            }
        }

        private void CopiarArquivos()
        {
            PbStatus.Maximum = _Arquivos.Count;
            PbStatus.Value = 0;
            LblOperacao.Content = "Copiando";
            LblProgresso.Content = _Arquivos.Count + " arquivos";
            var arquivosTarefa = _Arquivos.Select(arquivo =>
            {
                return Task.Factory.StartNew(() =>
                {
                    Gerenciar gerenciar = new Gerenciar();
                    gerenciar.Copiar(arquivo, _Caminho);
                });
            }).ToArray();

            Task.WhenAll(arquivosTarefa);
            LblOperacao.Content = "Copiado";
            this.Close();
        }

        private void MoverArquivos()
        {
            PbStatus.Maximum = _Arquivos.Count;
            PbStatus.Value = 0;
            LblOperacao.Content = "Movendo";
            LblProgresso.Content = _Arquivos.Count + " arquivos";
            var arquivosTarefa = _Arquivos.Select(arquivo =>
            {
                return Task.Factory.StartNew(() =>
                {
                    Gerenciar gerenciar = new Gerenciar();
                    gerenciar.Mover(arquivo, _Caminho);
                });
            }).ToArray();

            Task.WhenAll(arquivosTarefa);
            LblOperacao.Content = "Movido";
            this.Close();
        }
    }
}
