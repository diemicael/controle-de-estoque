﻿using ControleEstoque.Web.Models;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    public class OperSaidaProdutoController : OperEntradaSaidaProdutoController
    {
        protected override string SalvarPedido(EntradaSaidaProdutoViewModel dados)
        {
            return ProdutoModel.SalvarPedidoSaida(dados.Data, dados.Produtos);
        }
    }
}