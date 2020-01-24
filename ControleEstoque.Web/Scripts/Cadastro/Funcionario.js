function set_dados_form(dados) {
    $('#id_cadastro').val(dados.Id);
    $('#txt_nome').val(dados.Nome);
    $('#txt_num_documento').val(dados.NumDocumento);
    $('#txt_telefone').val(dados.Telefone);
    $('#txt_cargo').val(dados.Cargo);
    $('#txt_salario').val(dados.Salario);
    $('#txt_rua').val(dados.Rua);
    $('#txt_complemento').val(dados.Complemento);
    $('#txt_cep').val(dados.Cep);
    $('#cbx_ativo').prop('checked', dados.Ativo);


    var inclusao = (dados.Id == 0);
    if (inclusao) {
        $('#ddl_estado').empty();
        $('#ddl_estado').prop('disabled', true);

        $('#ddl_cidade').empty();
        $('#ddl_cidade').prop('disabled', true);
    }
    else {
        $('#ddl_pais').val(dados.IdPais);
        mudar_pais(dados.IdEstado, dados.IdCidade);
    }
}


function mudar_pais(id_estado, id_cidade) {
    var ddl_pais = $('#ddl_pais'),
        id_pais = parseInt(ddl_pais.val()),
        ddl_estado = $('#ddl_estado'),
        ddl_cidade = $('#ddl_cidade');

    if (id_pais > 0) {
        var url = url_listar_estados,
            param = { idPais: id_pais };

        ddl_estado.empty();
        ddl_estado.prop('disabled', true);

        ddl_cidade.empty();
        ddl_cidade.prop('disabled', true);

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response && response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    ddl_estado.append('<option value=' + response[i].Id + '>' + response[i].Nome + '</option>');
                }
                ddl_estado.prop('disabled', false);
            }
            sel_estado(id_estado);
            mudar_estado(id_cidade);
        });
    }
}

function mudar_estado(id_cidade) {
    var ddl_estado = $('#ddl_estado'),
        id_estado = parseInt(ddl_estado.val()),
        ddl_cidade = $('#ddl_cidade');

    if (id_estado > 0) {
        var url = url_listar_cidades,
            param = { idEstado: id_estado };

        ddl_cidade.empty();
        ddl_cidade.prop('disabled', true);

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response && response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    ddl_cidade.append('<option value=' + response[i].Id + '>' + response[i].Nome + '</option>');
                }
                ddl_cidade.prop('disabled', false);
            }
            sel_cidade(id_cidade);
        });
    }
}

function sel_estado(id_estado) {
    $('#ddl_estado').val(id_estado);
    $('#ddl_estado').prop('disabled', $('#ddl_estado option').length == 0);
}

function sel_cidade(id_cidade) {
    $('#ddl_cidade').val(id_cidade);
    $('#ddl_cidade').prop('disabled', $('#ddl_cidade option').length == 0);
}

function set_focus_form() {
    $('#txt_nome').focus();
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Nome: '',
        NumDocumento: '',
        Telefone: '',
        Cargo: '',
        Salario: '',
        Rua: '',
        Complemento: '',
        Cep: '',
        IdPais: 0,
        IdEstado: 0,
        IdCidade: 0,
        Ativo: true
    };
}

function get_dados_form() {
    return {
        Id: $('#id_cadastro').val(),
        Nome: $('#txt_nome').val(),
        NumDocumento: $('#txt_num_documento').val(),
        Telefone: $('#txt_telefone').val(),
        Cargo: $('#txt_cargo').val(),
        Salario: $('#txt_salario').val(),
        Rua: $('#txt_rua').val(),
        Complemento: $('#txt_complemento').val(),
        Cep: $('#txt_cep').val(),
        IdPais: $('#ddl_pais').val(),
        IdEstado: $('#ddl_estado').val(),
        IdCidade: $('#ddl_cidade').val(),
        Ativo: $('#cbx_ativo').prop('checked')
    };
}

function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Nome).end()
        .eq(1).html(param.Telefone).end()
        .eq(1).html(param.Contato).end()
        .eq(2).html(param.Ativo ? 'SIM' : 'NÃO');
}

$(document)
.ready(function () {
    $('#txt_telefone').mask('(00) 00000-0000');
    $('#txt_cep').mask('00000-000');
    $('#txt_num_documento').mask('000.000.000-00', { reverse: true });
    $('#txt_salario').mask('##0,00', { reverse: true });
})
.on('change', '#ddl_pais', function () {
    mudar_pais();
})
.on('change', '#ddl_estado', function () {
    mudar_estado();
});