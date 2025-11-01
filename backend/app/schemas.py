from pydantic import BaseModel, EmailStr
from typing import Optional, List
from datetime import datetime
from enum import Enum

# Enums
class TipoUsuarioEnum(str, Enum):
    ADMIN = "admin"
    TECNICO = "tecnico"
    COORDENADOR = "coordenador"
    VOLUNTARIO = "voluntario"

class StatusMensuracaoEnum(str, Enum):
    AGUARDANDO = "aguardando"
    RODANDO = "rodando"
    PAUSADA = "pausada"
    FINALIZADA = "finalizada"

# ==================== USUÁRIO ====================
class UsuarioBase(BaseModel):
    nome: str
    email: EmailStr
    tipo: str
    ativo: bool = True

class UsuarioCreate(UsuarioBase):
    senha_hash: str

class UsuarioResponse(UsuarioBase):
    id: int
    data_criacao: datetime
    
    class Config:
        from_attributes = True

# ==================== CLIENTE ====================
class ClienteBase(BaseModel):
    nome: str
    email: EmailStr
    telefone: Optional[str] = None
    ativo: bool = True

class ClienteCreate(ClienteBase):
    pass

class ClienteResponse(ClienteBase):
    id: int
    data_criacao: datetime
    
    class Config:
        from_attributes = True

# ==================== PRODUTO ====================
class ProdutoBase(BaseModel):
    nome: str
    descricao: Optional[str] = None
    versao: Optional[str] = None
    cliente_id: int
    ativo: bool = True

class ProdutoCreate(ProdutoBase):
    pass

class ProdutoResponse(ProdutoBase):
    id: int
    data_criacao: datetime
    
    class Config:
        from_attributes = True

# ==================== TESTE ====================
class TesteBase(BaseModel):
    nome: str
    objetivo: Optional[str] = None
    cliente_id: int
    produto_id: int
    coordenador_id: int
    ativo: bool = True

class TesteCreate(TesteBase):
    pass

class TesteResponse(TesteBase):
    id: int
    data_criacao: datetime
    
    class Config:
        from_attributes = True

# ==================== MENSURAÇÃO ====================
class MensuracaoBase(BaseModel):
    teste_id: int
    usuario_id: int
    tempo_total: Optional[int] = None
    status: str = "pendente"
    observacoes: Optional[str] = None

class MensuracaoCreate(MensuracaoBase):
    pass

class MensuracaoResponse(MensuracaoBase):
    id: int
    data_inicio: Optional[datetime] = None
    data_fim: Optional[datetime] = None
    data_criacao: datetime
    
    class Config:
        from_attributes = True

class MensuracaoIniciar(BaseModel):
    pass

class MensuracaoFinalizar(BaseModel):
    tempo_total: int