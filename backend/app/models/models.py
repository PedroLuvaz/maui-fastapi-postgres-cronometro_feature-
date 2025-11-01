from sqlalchemy import Column, Integer, String, Boolean, DateTime, ForeignKey, Text
from sqlalchemy.orm import relationship
from datetime import datetime
from ..database import Base

class Usuario(Base):
    __tablename__ = "usuarios"
    
    id = Column(Integer, primary_key=True, index=True)
    nome = Column(String(100), nullable=False)
    email = Column(String(100), unique=True, nullable=False, index=True)
    senha_hash = Column(String(255), nullable=False)
    tipo = Column(String(50), nullable=False)  # coordenador, analista, admin
    ativo = Column(Boolean, default=True)
    data_criacao = Column(DateTime, default=datetime.utcnow)
    
    # Relacionamentos
    testes_coordenados = relationship("Teste", back_populates="coordenador")
    mensuracoes = relationship("Mensuracao", back_populates="usuario")

class Cliente(Base):
    __tablename__ = "clientes"
    
    id = Column(Integer, primary_key=True, index=True)
    nome = Column(String(100), nullable=False)
    email = Column(String(100), unique=True, nullable=False)
    telefone = Column(String(20))
    ativo = Column(Boolean, default=True)
    data_criacao = Column(DateTime, default=datetime.utcnow)
    
    # Relacionamentos
    produtos = relationship("Produto", back_populates="cliente")
    testes = relationship("Teste", back_populates="cliente")

class Produto(Base):
    __tablename__ = "produtos"
    
    id = Column(Integer, primary_key=True, index=True)
    nome = Column(String(100), nullable=False)
    descricao = Column(Text)
    versao = Column(String(50))
    cliente_id = Column(Integer, ForeignKey("clientes.id"))
    ativo = Column(Boolean, default=True)
    data_criacao = Column(DateTime, default=datetime.utcnow)
    
    # Relacionamentos
    cliente = relationship("Cliente", back_populates="produtos")
    testes = relationship("Teste", back_populates="produto")

class Teste(Base):
    __tablename__ = "testes"
    
    id = Column(Integer, primary_key=True, index=True)
    nome = Column(String(100), nullable=False)
    objetivo = Column(Text)
    cliente_id = Column(Integer, ForeignKey("clientes.id"))
    produto_id = Column(Integer, ForeignKey("produtos.id"))
    coordenador_id = Column(Integer, ForeignKey("usuarios.id"))
    ativo = Column(Boolean, default=True)
    data_criacao = Column(DateTime, default=datetime.utcnow)
    
    # Relacionamentos
    cliente = relationship("Cliente", back_populates="testes")
    produto = relationship("Produto", back_populates="testes")
    coordenador = relationship("Usuario", back_populates="testes_coordenados")
    mensuracoes = relationship("Mensuracao", back_populates="teste")

class Mensuracao(Base):
    __tablename__ = "mensuracoes"
    
    id = Column(Integer, primary_key=True, index=True)
    teste_id = Column(Integer, ForeignKey("testes.id"))
    usuario_id = Column(Integer, ForeignKey("usuarios.id"))
    tempo_total = Column(Integer)  # em segundos
    status = Column(String(50))  # pendente, em_andamento, concluido
    observacoes = Column(Text)
    data_inicio = Column(DateTime)
    data_fim = Column(DateTime)
    data_criacao = Column(DateTime, default=datetime.utcnow)
    
    # Relacionamentos
    teste = relationship("Teste", back_populates="mensuracoes")
    usuario = relationship("Usuario", back_populates="mensuracoes")
    interrupcoes = relationship("Interrupcao", back_populates="mensuracao")
    frustacoes = relationship("Frustacao", back_populates="mensuracao")

class Interrupcao(Base):
    __tablename__ = "interrupcoes"
    
    id = Column(Integer, primary_key=True, index=True)
    mensuracao_id = Column(Integer, ForeignKey("mensuracoes.id"))
    momento = Column(DateTime, nullable=False)
    motivo = Column(Text)
    duracao = Column(Integer)  # em segundos
    
    # Relacionamentos
    mensuracao = relationship("Mensuracao", back_populates="interrupcoes")

class Frustacao(Base):
    __tablename__ = "frustacoes"
    
    id = Column(Integer, primary_key=True, index=True)
    mensuracao_id = Column(Integer, ForeignKey("mensuracoes.id"))
    momento = Column(DateTime, nullable=False)
    nivel = Column(Integer)  # 1 a 5
    descricao = Column(Text)
    
    # Relacionamentos
    mensuracao = relationship("Mensuracao", back_populates="frustacoes")
