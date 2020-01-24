using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;

namespace ControleEstoque.Web.Models
{
	public class DespesaModel
	{
		#region Atributos
		public int Id { get; set; }

		public DateTime Data { get; set; }

		[Required]
		public decimal Agua { get; set; }

		[Required]
		public decimal Luz { get; set; }

		[Required]
		public decimal Internet { get; set; }

		[Required]
		public decimal Aluguel { get; set; }

		[Required]
		public decimal Seguranca { get; set; }

		[Required]
		public decimal Telefone { get; set; }

		[Required]
		public decimal Marketing { get; set; }

		public decimal Salarios { get; set; }

		public decimal Total { get; set; }

		#endregion

		#region Métodos
		public static int RecuperarQuantidade()
		{
			var ret = 0;

			using (var conexao = new SqlConnection())
			{
				conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
				conexao.Open();
				ret = conexao.ExecuteScalar<int>("select count(*) from despesa");

			}
			return ret;
		}

		public static List<DespesaModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "")
		{
			var ret = new List<DespesaModel>();

			using (var conexao = new SqlConnection())
			{
				conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
				conexao.Open();

				var filtroWhere = "";
				if (!string.IsNullOrEmpty(filtro))
				{
					filtroWhere = string.Format(" where lower(data) like '%{0}%'", filtro.ToLower());
				}

				var pos = (pagina - 1) * tamPagina;
				var paginacao = "";
				if (pagina > 0 && tamPagina > 0)
				{
					paginacao = string.Format(" offset {0} rows fetch next {1} rows only",
						pos > 0 ? pos - 1 : 0, tamPagina);
				}

				var sql =
					"select *" +
					" from despesa" +
					filtroWhere +
					" order by " + (!string.IsNullOrEmpty(ordem) ? ordem : "data") +
					paginacao;

				ret = conexao.Query<DespesaModel>(sql).ToList();
			}
			return ret;
		}

		public static DespesaModel RecuperarPeloId(int id)
		{
			DespesaModel ret = null;

			using (var conexao = new SqlConnection())
			{
				conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
				conexao.Open();

				var sql = "select * from despesa where (id = @id)";
				var parametros = new { id };
				ret = conexao.Query<DespesaModel>(sql, parametros).SingleOrDefault();
			}
			return ret;
		}

		public static List<DespesaModel> RecuperarRelatDespesa()
		{
			var ret = new List<DespesaModel>();

			using (var conexao = new SqlConnection())
			{
				conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
				conexao.Open();

				var sql =

				"select d.data, d.agua, d.luz, d.internet, d.aluguel, d.seguranca, d.telefone, d.marketing, Sum(f.salario) as Salarios, " +
				"d.agua + d.luz + d.internet + d.aluguel + d.seguranca + d.telefone + d.marketing + Sum(f.salario) as total" +
				" from despesa d,funcionario f" +
				" group by data, agua, luz, internet, aluguel, seguranca, d.telefone, marketing";

				ret = conexao.Query<DespesaModel>(sql).ToList();
			}
			return ret;
		}

		public static bool ExcluirPeloId(int id)
		{
			var ret = false;

			if (RecuperarPeloId(id) != null)
			{
				using (var conexao = new SqlConnection())
				{
					conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
					conexao.Open();

					var sql = "delete from despesa where (id = @id)";
					var parametros = new { id };
					ret = (conexao.Execute(sql, parametros) > 0);
				}
			}
			return ret;
		}

		public int Salvar()
		{
			var ret = 0;

			var model = RecuperarPeloId(this.Id);

			using (var conexao = new SqlConnection())
			{
				conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
				conexao.Open();

				if (model == null)
				{
					var sql = "insert into despesa (data, agua, luz, internet, aluguel, seguranca, telefone, marketing) values (@data, @agua, @luz, @internet, @aluguel, @seguranca, @telefone, @marketing); select convert(int, scope_identity())";
					var parametros = new { data = this.Data, agua = this.Agua, luz = this.Luz, internet = this.Internet, aluguel = this.Aluguel, seguranca = this.Seguranca, telefone = this.Telefone, marketing = this.Marketing};
					ret = conexao.ExecuteScalar<int>(sql, parametros);
				}
				else
				{
					var sql = "update despesa set data=@data, agua=@agua, luz=@luz, internet=@internet, aluguel=@aluguel, seguranca=@seguranca, telefone=@telefone, marketing=@marketing where id = @id";
					var parametros = new { id = this.Id, data = this.Data, agua = this.Agua, luz = this.Luz, internet = this.Internet, aluguel = this.Aluguel, seguranca = this.Seguranca, telefone = this.Telefone, marketing = this.Marketing };
					if (conexao.Execute(sql, parametros) > 0)
					{
						ret = this.Id;
					}
				}				
			}
			return ret;
		}
		#endregion
	}
}