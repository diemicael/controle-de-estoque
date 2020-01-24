using ControleEstoque.Web.Models;
using Rotativa;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    public class RelatDespesaController : Controller
    {
        public ActionResult Index()
        {
            var despesa = DespesaModel.RecuperarRelatDespesa();
            return new ViewAsPdf("~/Views/Relatorio/RelatDespesaView.cshtml", despesa);
        }
    }
}