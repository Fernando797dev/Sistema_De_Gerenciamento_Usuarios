USE login;




CREATE TABLE usuarios (

id INT AUTO_INCREMENT PRIMARY KEY,

nome_completo VARCHAR(100) NOT NULL,

email VARCHAR(150) NOT NULL UNIQUE,

usuario VARCHAR(50) NOT NULL UNIQUE,

senha VARCHAR(255) NOT NULL,

IsAdmin VARCHAR(100) NOT NULL,

status VARCHAR(20) NOT NULL DEFAULT 'Ativo',

tipo_usuario VARCHAR(20) NOT NULL DEFAULT 'Usuário',

perfil_acesso VARCHAR(50) NOT NULL DEFAULT 'Usuário',

imagem_perfil VARCHAR(100) NOT NULL,

data_criacao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

data_atualizacao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE current_timestamp,

ultimo_login DATETIME NULL

);




select * from usuarios;




UPDATE usuarios

SET IsAdmin = 1

WHERE id = 28;




ADD COLUMN nome_completo VARCHAR(150) AFTER id,

ADD COLUMN status ENUM('Ativo', 'Inativo', 'Pendente') NOT NULL DEFAULT 'Ativo' AFTER IsAdmin,

ALTER TABLE usuarios MODIFY COLUMN imagem_perfil VARCHAR(255) NULL;

ADD COLUMN data_criacao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP AFTER imagem_perfil,

ADD COLUMN data_atualizacao DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP AFTER data_criacao,

ADD COLUMN ultimo_login DATETIME NULL AFTER data_atualizacao;

ALTER TABLE usuarios MODIFY COLUMN imagem_perfil VARCHAR(255);
