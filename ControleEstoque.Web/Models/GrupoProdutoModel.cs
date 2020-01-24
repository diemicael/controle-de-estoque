﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dapper;

namespace ControleEstoque.Web.Models
{
    public class GrupoProdutoModel
    {
        #region Atributos
        public int Id { get; set; }

        [Required(ErrorMessage = "Preencha o nome.")]
        public string Nome { get; set; }

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
                ret = conexao.ExecuteScalar<int>("select count(*) from grupo_produto");               
            }
            return ret;
        }

        public static List<GrupoProdutoModel> RecuperarLista(int pagina, int tamPagina, string filtro = "", string ordem = "")
        {
            var ret = new List<GrupoProdutoModel>();

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();
                using (var comando = new SqlCommand())
                {
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
                        " from grupo_produto" +
                        filtroWhere +
                        " order by " + (!string.IsNullOrEmpty(ordem) ? ordem : "nome") +
                        paginacao;

                    ret = conexao.Query<GrupoProdutoModel>(sql).ToList();
                }
            }
            return ret;
        }

        public static GrupoProdutoModel RecuperarPeloId(int id)
        {
            GrupoProdutoModel ret = null;

            using (var conexao = new SqlConnection())
            {
                conexao.ConnectionString = ConfigurationManager.ConnectionStrings["principal"].ConnectionString;
                conexao.Open();

                var sql = "select * from grupo_produto where (id = @id)";
                var parametros = new { id };
                ret = conexao.Query<GrupoProdutoModel>(sql, parametros).SingleOrDefault();              
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

                    var sql = "delete from grupo_produto where (id = @id)";
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
                        var sql = "insert into grupo_produto (nome, ativo) values (@nome, @ativo); select convert(int, scope_identity())";
                        var parametros = new { nome = this.Nome, ativo = (this.Ativo ? 1 : 0) };
                        ret = conexao.ExecuteScalar<int>(sql, parametros);
                    }
                    else
                    {
                        var sql = "update grupo_produto set nome=@nome, ativo=@ativo where id = @id";
                        var parametros = new { id = this.Id, nome = this.Nome, ativo = (this.Ativo ? 1 : 0) };
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