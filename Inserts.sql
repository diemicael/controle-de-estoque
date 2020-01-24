insert into grupo_produto
(nome,ativo)
values
(111,1),
(222,1),
(333,1);
GO

insert into marca_produto
(nome, ativo)
values
('Intel', 1),
('Dell', 1),
('Asus', 1),
('Acer', 1);
GO

insert into local_armazenamento
(nome, ativo)
values
('Central', 1),
('Porto Alegre', 1),
('Novo Hamburgo', 1),
('Campo Bom', 1);
GO

insert into unidade_medida
(nome, sigla, ativo)
values
('unidade', 'UND', 1),
('peça', 'PC', 1),
('pacote', 'PT', 1);
GO

insert into pais
(nome, codigo, ativo)
values
('Brasil', 'BRA', 1),
('Estados Unidos', 'USA', 1)
GO

insert into estado
(nome, uf, ativo, id_pais)
values
('Rio grande do Sul', 'RS', 1, 1),
('Nova York', 'NY', 1, 2)
GO

insert into cidade
(nome, ativo, id_estado)
values
('Novo Hamburgo', 1, 1),
('Nova York', 1, 2)
GO

insert into fornecedor
(nome, razao_social, num_documento, tipo, telefone, contato, logradouro, numero, complemento, cep, id_pais, id_estado, id_cidade, ativo)
values
('Dell', 'Dell, Inc.', '54.656.456/4561-56', 1, '(51) 998346556', 'dellcontato@gmail.com', 'Rua General Daltro Filho', '107', 'APTO 02', '93336-170', 1, 1, 1, 1) 
GO

insert into funcionario
(nome, num_documento, telefone, cargo, salario, rua, numero, complemento, cep, id_pais, id_estado, id_cidade, ativo)
values
('Ismael Cunha Deolinda da Silva', '026.648.410-76', '(51) 993442527', 'Administrativo', 2200.00, 'Caçador', '481', 'APTO 231', '9333-170', 1, 1, 1, 1)
GO

