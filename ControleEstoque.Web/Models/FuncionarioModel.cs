using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using Dapper;

namespace ControleEstoque.Web.Models
{
    public class FuncionarioModel
    {
        #region Atributos
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        [MaxLength(60, ErrorMessage = "O nome pode ter no máximo 60 caracteres.")]
        public string Nome { get; set; }

        [MaxLength(20, ErrorMessage = "O número do documento pode ter no máximo 20 caracteres.")]
        public string NumDocumento { get; set; }

        [Required(ErrorMessage = "Preencha o telefone.")]
        [MaxLength(20, ErrorMessage = "O telefone deve ter 20 caracteres.")]      
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Preencha o cargo.")]
        [MaxLength(50, ErrorMessage = "O cargo deve ter 50 caracteres.")]
        public string Cargo { get; set; }

        [Required(ErrorMessage = "Preencha o salário.")]
        public decimal Salario { get; set; }

        [MaxLength(100, ErrorMessage = "A rua do endereço pode ter no máximo 100 caracteres.")]
        public string Rua { get; set; }

        [MaxLength(20, ErrorMessage = "O número do endereço pode ter no máximo 20 caracteres.")]
        public string Numero { get; set; }

        [MaxLength(100, ErrorMessage = "O complemento do endereço pode ter no máximo 100 caracteres.")]
        public string Complemento { get; set; }

        [MaxLength(10, ErrorMessage = "O CEP do endereço pode ter no máximo 10 caracteres.")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "Selecione o país.")]
        public int IdPais { get; set; }

        [Required(ErrorMessage = "Selecione o estado.")]
        public int IdEstado { get; set; }

        [Required(ErrorMessage = "Selecione a cidade.")]
        public int IdCidade { get; set; }

        public bool Ativo { get; set; }
        #endregion

        #region Métodos
        public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                ret = conexao.ExecuteScalar<int>("select count(*) from funcionario");             
            }
            return ret;
        }

        public static List<FuncionarioModel> RecuperarLista(int pagina = 0, int tamPagina = 0, string filtro = "", string ordem = "")
        {
            var ret = new List<FuncionarioModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                
                    var filtroWhere = "";
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        filtroWhere = string.Format(" where lower(nome) like '%{0}%'", filtro.ToLower());
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
                        " from funcionario" +
                        filtroWhere +
                        " order by " + (!string.IsNullOrEmpty(ordem) ? ordem : "nome") +
                        paginacao;

                    ret = conexao.Query<FuncionarioModel>(sql).ToList();             
            }
            return ret;
        }

        public static FuncionarioModel RecuperarPeloId(int id)
        {
            FuncionarioModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();

                var sql = "select * from funcionario where (id = @id)";
                var parametros = new { id };
                ret = conexao.Query<FuncionarioModel>(sql, parametros).SingleOrDefault();
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

                    var sql = "delete from funcionario where (id = @id)";
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
                       var sql = "insert into funcionario (nome, num_documento, telefone, cargo, salario, rua," +
                            " numero, complemento, cep, id_pais, id_estado, id_cidade, ativo) values (@nome, @num_documento," +
                            " @telefone, @cargo, @salario, @rua, @numero, @complemento, @cep, @id_pais, @id_estado, @id_cidade, @ativo);" +
                            " select convert(int, scope_identity())";
                       var parametros = new
                       {
                           nome = this.Nome,
                           num_documento = this.NumDocumento ?? "",
                           telefone = this.Telefone ?? "",
                           cargo = this.Cargo ?? "",
                           salario = this.Salario,
                           rua = this.Rua ?? "",
                           numero = this.Numero ?? "",
                           complemento = this.Complemento ?? "",
                           cep = this.Cep ?? "",
                           id_pais = this.IdPais,
                           id_estado = this.IdEstado,
                           id_cidade = this.IdCidade,
                           ativo = (this.Ativo ? 1 : 0)
                       };
                    ret = conexao.ExecuteScalar<int>(sql, parametros);
                }
                else
                    {
                       var sql = "update funcionario set nome=@nome, num_documento=@num_documento," +
                            " telefone=@telefone, cargo=@cargo, salario=@salario, rua=@rua, numero=@numero, complemento=@complemento," +
                            " cep=@cep, id_pais=@id_pais, id_estado=@id_estado, id_cidade=@id_cidade, ativo=@ativo where id = @id";
                       var parametros = new
                       {
                          id = this.Id,
                          nome = this.Nome,
                          num_documento = this.NumDocumento ?? "",
                          telefone = this.Telefone ?? "",
                          cargo = this.Cargo ?? "",
                          salario = this.Salario,
                          rua = this.Rua ?? "",
                          numero = this.Numero ?? "",
                          complemento = this.Complemento ?? "",
                          cep = this.Cep ?? "",
                          id_pais = this.IdPais,
                          id_estado = this.IdEstado,
                          id_cidade = this.IdCidade,
                          ativo = (this.Ativo ? 1 : 0)
                    };
                    
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