
$(document)
    .ready(function () {
    $("#foneid").mask("(00) 00000-0000");
    $('#submit').click(function Enviar() {
    var nome = $('#nomeid').val();

var fone = $('#foneid').val();
var email = $('#emailid').val();
var mensagem = $('#mensagemid').val();
          if (nome.indexOf(" ") == -1) {
        $("#erros").html('Nome invalido');
    return false;
  }
          if (nome.length < 10) {
        $("#erros").html('Nome invalido');
    return false;
  }
          if (email != "") {
            var filtro = /^([w-]+(?:.[w-]+)*)@((?:[w-]+.)*w[w-]{0,66}).([a-z]{2,6}(?:.[a-z]{2})?)$/i;
            if (filtro.test(email)) {
              return true;
            } else {
        $("#erros").html('O endereço de email fornecido é invalido');
    return false;
  }
          } else {
        $("#erros").html('Forneça um endereço de email');
    return false;
  }
          if (mensagem.length < 1) {
        $("#erros").html(' Digite a mensagem');
    return false;
        }

        });
       
    });