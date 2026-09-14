using MauiAppCompras.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MauiAppCompras.Views;


public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();
    public ListaProduto()
    {
        InitializeComponent();

        lst_produtos.ItemsSource = lista;
    }

    protected override async void OnAppearing()
    {
        try
        {

            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private async void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new Views.NovoProduto());
        }
        catch (Exception ex)
        {

            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {

            string q = e.NewTextValue;

            lst_produtos.IsRefreshing = true;

            lista.Clear();

            List<Produto> tmp = await App.Db.Search(q);

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        } finally

        { lst_produtos.IsRefreshing = false; }
    }

    private async void AplicarFiltros()
    {
        string pesquisa = txt_search.Text?.ToLower() ?? "";
        string categoria = picker_categoria.SelectedItem?.ToString();

        var produtos = await App.Db.GetAll();

        if (!string.IsNullOrEmpty(pesquisa))
        {
            produtos = produtos
                .Where(p => p.Descricao.ToLower().Contains(pesquisa))
                .ToList();
        }

        if (!string.IsNullOrEmpty(categoria) && categoria != "Todas")
        {
            produtos = produtos
                .Where(p => p.Categoria == categoria)
                .ToList();
        }

        lista.Clear();

        foreach (var produto in produtos)
        {
            lista.Add(produto);
        }
    }

    private async void txt_search_TextChanged_1(object sender, TextChangedEventArgs e)
    {
        try
        {
            AplicarFiltros();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private async void picker_categoria_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            AplicarFiltros();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }


    private async void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
        try
        {


            double soma = lista.Sum(i => i.Total);

            string msg = $"Total de produtos: {lista.Count}\nSoma dos produtos: {soma:C}";

            await DisplayAlert("Total", msg, "Ok");
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {

            MenuItem selecionado = sender as MenuItem;

            Produto p = selecionado.BindingContext as Produto;

            bool confirm = await DisplayAlert(
                "Tem certeza?", $"Deseja excluir o produto {p.Descricao}?", "Sim", "Não");


            if (confirm)
            {
                await App.Db.Delete(p.Id);
                lista.Remove(p);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private async void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            
            Produto p = e.SelectedItem as Produto;

            await Navigation.PushAsync(new Views.EditarProduto
            {
                BindingContext = p,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "Ok");
        }
    }

    private async void lst_produtos_Refreshing(object sender, EventArgs e)
    {
        try
        {

            lista.Clear();

            List<Produto> tmp = await App.Db.GetAll();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "Ok");

        } finally 

        { lst_produtos.IsRefreshing = false; }
    }

    private async void ToolbarItem_Clicked_2(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RelatorioCategoria());
    }
}