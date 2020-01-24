function set_dados_form(dados) {
    $('#id_despesa').val(dados.Id);
    $('#txt_agua').val(dados.Agua);
    $('#txt_luz').val(dados.Luz);
    $('#txt_internet').val(dados.Internet);
    $('#txt_aluguel').val(dados.Aluguel);
    $('#txt_seguranca').val(dados.Seguranca);
    $('#txt_telefone').val(dados.Telefone);
    $('#txt_marketing').val(dados.Marketing);
}
function formatar_data(data) {
    var dia = ('0' + data.getDate()).slice(-2);
    var mes = ('0' + (data.getMonth() + 1)).slice(-2);
    return data.getFullYear() + "-" + mes + "-" + dia;
}

function set_focus_form() {
    $('#txt_data').focus();
}

function get_dados_inclusao() {
    return {
        Id: 0,
        Data: '',
        Agua: 0,
        Luz: 0,
        Internet: 0,
        Aluguel: 0,
        Seguranca: 0,
        Telefone: 0,
        Marketing: 0
    };
}

function get_dados_form() {
    return {
        Id: $('#id_despesa').val(),
        Data: $('#txt_data').val(),
        Agua: $('#txt_agua').val(),
        Luz: $('#txt_luz').val(),
        Internet: $('#txt_internet').val(),
        Aluguel: $('#txt_aluguel').val(),
        Seguranca: $('#txt_seguranca').val(),
        Telefone: $('#txt_telefone').val(),
        Marketing: $('#txt_marketing').val(),
    }
}


function preencher_linha_grid(param, linha) {
    linha
        .eq(0).html(param.Data).end()
}

$(document)
    .ready(function () {
    var hoje = new Date();
    $('#txt_data').val(formatar_data(hoje));
    $('#txt_agua,#txt_luz,#txt_internet,#txt_aluguel,#txt_seguranca,#txt_telefone,#txt_marketing').mask('##0,00', { reverse: true });
})