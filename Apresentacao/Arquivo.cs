using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Apresentacao
{
    public class Arquivo
    {
        public string Nome { get; set; }
        public string Extensao { get; set; }
        public string CaminhoCompleto { get; set; }
        public string SubPasta { get; set; }
    }

    public class Arquivos: List<Arquivo>
    {
    }
}
