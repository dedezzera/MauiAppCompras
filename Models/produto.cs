using SQLite;

namespace MauiAppCompras.Models
{
    public class Produto
    {

        string _descricao;
        double _quantidade;
        double _preco;


        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descricao { 
            get => _descricao;
            set 
            { 
                if(value == null)
                {
                    throw new Exception("Descrição não pode ser vazia");
                }
                _descricao = value;
            }
        }
        public double Quantidade { 
            get => _quantidade; 
            set
            {
                if (value < 1)
                {
                    throw new Exception("Preencha uma quantidade válida");
                }
                _quantidade = value;
            }
        }
        public double Preco { 
            get => _preco;
            set
            {
                if (value <= 0)
                {
                    throw new Exception("Preencha um preço válido");
                }
                _preco = value;
            }
        }
        public double Total { get => Quantidade * Preco; }

    }
}
