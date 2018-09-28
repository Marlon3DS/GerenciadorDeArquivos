using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Apresentacao.Gerenciar;

namespace Apresentacao
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public string CaminhoOrigem { get; set; }
        public string CaminhoDestino { get; set; }
        GridViewColumnHeader _ultimoCabecalhoClicado = null;
        ListSortDirection _ultimaDirecao = ListSortDirection.Ascending;

        public MainWindow()
        {
            InitializeComponent();
        }

        private string SelecionaDiretorio()
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog
                {
                    Description = "Selecione o Caminho do Diretório"
                };
                folderBrowserDialog.ShowDialog();
                return folderBrowserDialog.SelectedPath;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        private void BtnCopiar_Click(object sender, RoutedEventArgs e)
        {
            ExecutarOperacao(Operacao.Copiar);
        }

        private void BtnMover_Click(object sender, RoutedEventArgs e)
        {
            ExecutarOperacao(Operacao.Mover);
        }

        private void BtnSelecOrigem_Click(object sender, RoutedEventArgs e)
        {
            if (TxtOrigem.Text != "")
            {
                AtualizarTela(Diretorio.Origem);
            }
            else
            {
                CaminhoOrigem = SelecionaDiretorio();
                TxtOrigem.Text = Gerenciar.PegarPasta(CaminhoOrigem);
                if (TxtOrigem.Text != "")
                {
                    BtnSelecOrigem.Content = "x";
                    PesquisarArquivos();
                    if (TxtDestino.Text != "")
                    {
                        BtnCopiar.IsEnabled = true;
                        BtnMover.IsEnabled = true;
                    }
                }
            }
        }

        private void BtnSelecDestino_Click(object sender, RoutedEventArgs e)
        {
            if (TxtDestino.Text != "")
            {
                AtualizarTela(Diretorio.Destino);
            }
            else
            {
                CaminhoDestino = SelecionaDiretorio();
                TxtDestino.Text = Gerenciar.PegarPasta(CaminhoDestino);
                if (TxtDestino.Text != "")
                {
                    BtnSelecDestino.Content = "x";
                    if (TxtOrigem.Text != "")
                    {
                        BtnCopiar.IsEnabled = true;
                        BtnMover.IsEnabled = true;
                    }
                }
            }
        }

        private void LvArquivos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LblSelecionado.Content = "Selecionados: " + LvArquivos.SelectedItems.Count.ToString();
        }

        private void GridViewColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            var cabecalhoClicado = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direcao;

            if (cabecalhoClicado != null && cabecalhoClicado.Role != GridViewColumnHeaderRole.Padding)
            {
                if (cabecalhoClicado != _ultimoCabecalhoClicado)
                {
                    direcao = ListSortDirection.Ascending;
                }
                else
                {
                    if (_ultimaDirecao == ListSortDirection.Ascending)
                    {
                        direcao = ListSortDirection.Descending;
                    }
                    else
                    {
                        direcao = ListSortDirection.Ascending;
                    }
                }

                var columnBinding = cabecalhoClicado.Column.DisplayMemberBinding as System.Windows.Data.Binding;
                var sortBy = columnBinding?.Path.Path ?? cabecalhoClicado.Column.Header as string;

                Ordenar(sortBy, direcao);

                _ultimoCabecalhoClicado = cabecalhoClicado;
                _ultimaDirecao = direcao;
            }
        }

        private void PesquisarArquivos()
        {
            try
            {
                Arquivos arquivos = new Arquivos();
                arquivos = Gerenciar.ListarArquivos(CaminhoOrigem);
                LvArquivos.Items.Clear();
                foreach (Arquivo arquivo in arquivos)
                {
                    LvArquivos.Items.Add(arquivo);
                }
                LblTotal.Content = "Total: " + LvArquivos.Items.Count.ToString();
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.Message);
            }
        }

        private void Ordenar(string sortBy, ListSortDirection direction)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(LvArquivos.Items);

            dataView.SortDescriptions.Clear();
            SortDescription sd = new SortDescription(sortBy, direction);
            dataView.SortDescriptions.Add(sd);
            dataView.Refresh();
        }

        private void AtualizarTela(Diretorio diretorio)
        {
            if (diretorio == Diretorio.Origem)
            {
                TxtOrigem.Text = "";
                BtnSelecOrigem.Content = "...";
                LvArquivos.Items.Clear();
            }
            else if (diretorio == Diretorio.Destino)
            {
                TxtDestino.Text = "";
                BtnSelecDestino.Content = "...";
            }
            BtnCopiar.IsEnabled = false;
            BtnMover.IsEnabled = false;
        }

        private void LimparTela()
        {
            AtualizarTela(Diretorio.Origem);
            AtualizarTela(Diretorio.Destino);
            LblTotal.Content = "0";
            LblSelecionado.Content = "0";
        }

        private void ExecutarOperacao(Operacao operacao)
        {
            try
            {
                Arquivos arquivos = new Arquivos();
                if (LvArquivos.SelectedItems.Count == 0)
                {
                    LvArquivos.SelectAll();
                }

                foreach (Arquivo item in LvArquivos.SelectedItems)
                {
                    arquivos.Add(item);
                }

                Processamento processamento = new Processamento(arquivos, CaminhoDestino, operacao);
                processamento.ShowDialog();
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show(exception.Message);
            }
            finally
            {
                LimparTela();
            }
        }
    }
}
