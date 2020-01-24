using ControleEstoque.Web.Models;
using Rotativa;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    public class RelatPosicaoEstoqueController : Controller
    {
        public ActionResult Index()
        {
            var estoque = ProdutoModel.RecuperarRelatPosicaoEstoque();
            return new ViewAsPdf("~/Views/Relatorio/RelatPosicaoEstoqueView.cshtml", estoque);
        }
    }
}