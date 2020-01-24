using ControleEstoque.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ControleEstoque.Web.Controllers
{
    public class CadDespesasController : Controller
    {
		// GET: CadDespesas
		private const int _quantMaxLinhasPorPagina = 5;

		public ActionResult Index()
		{
			ViewBag.ListaTamPag = new SelectList(new int[] { _quantMaxLinhasPorPagina, 10, 15, 20 }, _quantMaxLinhasPorPagina);
			ViewBag.QuantMaxLinhasPorPagina = _quantMaxLinhasPorPagina;
			ViewBag.PaginaAtual = 1;

			var lista = DespesaModel.RecuperarLista(ViewBag.PaginaAtual, _quantMaxLinhasPorPagina);
			var quant = DespesaModel.RecuperarQuantidade();

			var difQuantPaginas = (quant % ViewBag.QuantMaxLinhasPorPagina) > 0 ? 1 : 0;
			ViewBag.QuantPaginas = (quant / ViewBag.QuantMaxLinhasPorPagina) + difQuantPaginas;

			return View(lista);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public JsonResult DespesaPagina(int pagina, int tamPag, string filtro, string ordem)
		{
			var lista = DespesaModel.RecuperarLista(pagina, tamPag, filtro, ordem);

			return Json(lista);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public JsonResult RecuperarDespesa(int id)
		{
			return Json(DespesaModel.RecuperarPeloId(id));
		}

		[HttpPost]

		[ValidateAntiForgeryToken]
		public JsonResult ExcluirDespesa(int id)
		{
			return Json(DespesaModel.ExcluirPeloId(id));
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public JsonResult SalvarDespesa(DespesaModel model)
		{
			var resultado = "OK";
			var mensagens = new List<string>();
			var idSalvo = string.Empty;

			if (!ModelState.IsValid)
			{
				resultado = "AVISO";
				mensagens = ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList();
			}
			else
			{
				try
				{
					var id = model.Salvar();
					if (id > 0)
					{
						idSalvo = id.ToString();
					}
					else
					{
						resultado = "ERRO";
					}
				}
				catch (Exception ex)
				{
					resultado = "ERRO";
				}
			}

			return Json(new { Resultado = resultado, Mensagens = mensagens, IdSalvo = idSalvo });
		}
	}
}