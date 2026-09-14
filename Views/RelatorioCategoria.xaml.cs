namespace MauiAppCompras.Views;

public partial class RelatorioCategoria : ContentPage
{
    public RelatorioCategoria()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var produtos = await App.Db.GetAll();

        var relatorio = produtos
            .GroupBy(p => p.Categoria)
            .Select(g => new
            {
                Categoria = g.Key,
                Total = g.Sum(p => p.Total),
                TotalFormatado = $"Total: R$ {g.Sum(p => p.Total):N2}"
            })
            .ToList();

        lista_relatorio.ItemsSource = relatorio;

        double totalGeral = produtos.Sum(p => p.Total);

        label_total.Text = $"Total geral: R$ {totalGeral:N2}";
    }
}