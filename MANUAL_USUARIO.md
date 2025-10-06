# DeviantArt Scrapper - Manual do Usuário

## Sumário
1. [Introdução](#introducao)
2. [Requisitos do Sistema](#requisitos-do-sistema)
3. [Instalação e Configuração](#instalacao-e-configuracao)
4. [Primeiros Passos](#primeiros-passos)
5. [Configuração da API](#configuracao-da-api)
6. [Raspagem de Galeria](#raspagem-de-galeria)
7. [Raspagem de Favoritos](#raspagem-de-favoritos)
8. [Formatos de Exportação](#formatos-de-exportacao)
9. [Monitoramento de Progresso](#monitoramento-de-progresso)
10. [Solução de Problemas](#solucao-de-problemas)
11. [Dicas e Boas Práticas](#dicas-e-boas-praticas)
12. [Perguntas Frequentes (FAQ)](#faq)

---

## Introdução

O **DeviantArt Scrapper** é um aplicativo de desktop profissional desenvolvido para ajudar você a coletar e exportar dados de obras de arte do site DeviantArt. Seja você pesquisador, artista, colecionador ou analista de dados, esta ferramenta oferece uma solução completa para reunir informações sobre o conteúdo do DeviantArt.

### Principais Recursos
- **Funcionalidade Dupla**: Raspagem de galerias de usuários e de favoritos
- **Múltiplos Formatos de Exportação**: CSV para análise, HTML para visualização e Excel para relatórios
- **Progresso em Tempo Real**: Atualizações ao vivo com taxa de transferência e contagem de itens
- **Qualidade Profissional**: Lógica de repetição, limitação de taxa e tratamento de erros
- **Segurança**: Armazenamento criptografado de credenciais e autenticação OAuth2

---

## Requisitos do Sistema

### Requisitos Mínimos
- **Sistema Operacional**: Windows 10 versão 1809 (build 17763) ou superior
- **Framework**: .NET 9 Runtime (instalado automaticamente, se ausente)
- **Memória**: 2 GB de RAM
- **Armazenamento**: 100 MB de espaço disponível (mais espaço para dados exportados)
- **Internet**: Conexão estável para acesso à API

### Requisitos Recomendados
- **Sistema Operacional**: Windows 11
- **Memória**: 4 GB de RAM ou mais
- **Armazenamento**: 1 GB de espaço disponível
- **Internet**: Conexão banda larga para raspagem mais rápida

---

## Instalação e Configuração

### Download e Instalação
1. Baixe a versão mais recente do repositório do projeto.
2. Extraia o aplicativo para a pasta desejada.
3. Execute `DeviantArtScrapper.exe` para iniciar o aplicativo.
4. O runtime .NET 9 será baixado automaticamente, se não estiver presente.

### Primeiro Início
Ao iniciar pela primeira vez, você verá a janela principal do aplicativo com:
- **Status**: "Não configurado - Configure as configurações da API"
- **Aba Galeria**: Pronta para uso, mas necessitando configuração da API
- **Aba Favoritos**: Disponível após configuração da API
- **Botão Configurações**: Localizado no canto inferior direito

---

## Configuração da API

Antes de poder raspar qualquer dado, você precisa configurar suas credenciais da API do DeviantArt.

### Obtendo Acesso à API do DeviantArt

1. **Acesse o Portal de Desenvolvedores do DeviantArt**
   - Acesse [DeviantArt Developer Portal](https://www.deviantart.com/developers/apps)
   - Faça login com sua conta DeviantArt

2. **Crie um Novo Aplicativo**
   - Clique em "Register your Application"
   - Preencha os detalhes do aplicativo:
     - **Nome do Aplicativo**: "Personal Scraper" (ou outro de sua escolha)
     - **Descrição**: "Ferramenta de coleta de dados pessoais"
     - **Website**: Seu site ou "N/A"
   - **Grant Type**: Selecione "Client Credentials"
   - Aceite os termos e crie o aplicativo

3. **Obtenha Suas Credenciais**
   - Após criado, você receberá:
     - **Client ID**: Uma string longa de caracteres
     - **Client Secret**: Outra string longa (mantenha em sigilo!)

### Configurando o Aplicativo

1. **Abra as Configurações**
   - Clique no botão "Settings" no canto inferior direito
   - A janela de configurações será aberta

2. **Insira Suas Credenciais**
   - **Client ID**: Cole seu Client ID obtido no DeviantArt
   - **Client Secret**: Cole seu Client Secret
   - Clique em "Test Connection" para verificar suas credenciais
   - Se bem-sucedido, aparecerá a mensagem "Conexão bem-sucedida!"

3. **Salve a Configuração**
   - Clique em "OK" para salvar
   - Suas credenciais são criptografadas e armazenadas com segurança localmente
   - O status principal será atualizado para "Pronto - API configurada e autenticada"

### Notas de Segurança
- Credenciais criptografadas com **Windows DPAPI**
- Armazenadas apenas no computador local
- Usa autenticação **OAuth2** para acesso seguro
- Nenhuma senha ou dado pessoal é armazenado

---

## Raspagem de Galeria

A raspagem de galeria coleta todas as obras públicas enviadas por um usuário específico do DeviantArt.

### Passo a Passo

1. **Acesse a Aba Galeria**
   - Selecionada por padrão
   - Certifique-se de que a API está configurada (status: "Pronto")

2. **Insira o Nome de Usuário**
   - No campo "DeviantArt Username", insira o nome do usuário
   - Exemplo: "sakimichan", "loish", "david-revoy"
   - **Obs.**: Apenas galerias públicas podem ser raspadas

3. **Escolha o Nome do Arquivo de Saída**
   - Informe o nome do arquivo (sem extensão)
   - Padrão: "gallery_export"
   - Use o botão "..." para escolher o local de gravação

4. **Selecione o Formato de Exportação**
   - **CSV**: Melhor para análise de dados
   - **HTML**: Cria uma galeria visual
   - **Excel (XLSX)**: Relatórios profissionais com formatação avançada

5. **Inicie a Raspagem**
   - Clique em "Start Scraping"
   - A barra de progresso aparecerá com atualizações ao vivo
   - Serão exibidas estatísticas em tempo real:
     - Itens coletados
     - Taxa de transferência em KB/s
     - Status atual da operação

### Dados Coletados
- **Informações Básicas**: Título, URL, data de publicação
- **Detalhes do Artista**: Nome de usuário e perfil
- **Estatísticas**: Curtidas, comentários
- **Mídia**: URLs de download e miniaturas
- **Conteúdo Sensível**: Indicadores de nudez/maduro
- **Metadados**: Todos os dados públicos disponíveis

### Ordenação dos Dados
Os itens da galeria são organizados por data (mais recentes primeiro).

---

## Raspagem de Favoritos

A raspagem de favoritos coleta todas as obras públicas que um usuário favoritou de outros artistas.

### Como Funciona

1. **Mude para a Aba Favoritos**
   - Clique na aba "Favorites"
   - Interface semelhante à da galeria

2. **Configure a Coleta**
   - **Usuário**: Nome de usuário alvo
   - **Nome do Arquivo**: Nome da exportação (padrão: "favorites_export")
   - **Formato**: CSV, HTML ou Excel

3. **Inicie a Coleta**
   - Clique em "Start Scraping"
   - Progresso exibido em tempo real

### Recursos Específicos de Favoritos
- **Dados de Múltiplos Artistas**
- **Ordenação Especial**: Por artista e data
- **Modelos HTML exclusivos**
- **Análise de Coleções**: Identifica padrões e preferências

### Casos de Uso
- **Pesquisa de Inspiração**
- **Análise de Tendências**
- **Mapeamento de Comunidades**
- **Estudos de Curadoria**

---

## Formatos de Exportação

### CSV (Valores Separados por Vírgula)

**Melhor Para**: Análise de dados e planilhas

**Estrutura do Arquivo**:
```csv
Title,Author,URL,Published Date,Mature Content,Stats Favourites,Stats Comments,Download URL,Thumbnail URL
"Fantasy Landscape","artist123","https://...","2025-01-15 14:30:00",false,"1250","45","https://...","https://..."
```

**Recursos**:
- Codificação UTF-8
- Compatível com Excel, Google Sheets e ferramentas de análise
- Pequeno tamanho de arquivo para grandes volumes

### HTML (Galeria Web)

**Melhor Para**: Visualização e apresentações

**Recursos**:
- Tema escuro e design moderno
- Layout responsivo em grade
- Miniaturas clicáveis
- Paginação: 100 itens por página
- Exibição de metadados
- Navegação entre páginas

### Excel (XLSX)

**Melhor Para**: Relatórios profissionais e análises detalhadas

**Recursos**:
- Formatação profissional
- URLs clicáveis
- Formatação condicional para conteúdo maduro
- Colunas autoajustáveis
- Cabeçalho fixo e filtros automáticos

---

## Monitoramento de Progresso

### Feedback em Tempo Real

O aplicativo fornece informações detalhadas durante as operações de raspagem:

**Indicadores de Progresso**:
- **Barra de Progresso**: Indicação visual de atividade
- **Contador de Itens**: "Coletados X itens..."
- **Taxa de Transferência**: Medida em KB/s
- **Mensagens de Status**: Detalhes da operação

**Estados do Progresso**:
- **Inicializando**: Configurando conexão com a API
- **Coletando**: Reunindo dados
- **Exportando**: Processando e salvando
- **Completo**: Operação concluída
- **Erro**: Problema encontrado

### Cancelamento

**Como Cancelar**:
- Clique em "Cancelar" durante a raspagem
- A operação para com segurança após a requisição atual
- Opções de exportar resultados parciais

**Exportação Parcial**:
- **Exportar Dados Coletados**: Salva o que foi obtido
- **Descartar Resultados**: Cancela sem salvar
- **Retomar Depois**: Não suportado - reinicie do início

### Monitoramento de Desempenho

**Cálculo de Taxa**:
- Baseado em itens coletados por segundo
- Estimado em ~2KB por obra
- Atualiza em tempo real

**Desempenho Típico**:
- **Pequenas Coleções** (<100 itens): 30–60s
- **Médias** (100–1000 itens): 2–10min
- **Grandes** (1000+ itens): 10+min
- **Fatores**: Velocidade da internet, resposta da API, tamanho

---

## Solução de Problemas

### Problemas Comuns

#### "Status: Não configurado"
**Problema**: API não configurada  
**Solução**: Abra Configurações → insira credenciais → teste e salve

#### "Connection Test Failed"
**Problema**: Credenciais inválidas ou problema de rede  
**Soluções**: Verifique ID/Secret, conexão e ative o app no DeviantArt

#### "No gallery items found"
**Problema**: Usuário sem galeria pública  
**Soluções**: Verifique nome, visibilidade ou teste outro usuário

#### "Request failed (attempt 1/3)"
**Problema**: Falha temporária da API  
**Solução**: Aguarde - o app tenta automaticamente

#### Travamentos
**Soluções**: Reinicie, verifique memória, espaço e antivírus

#### "Access Denied"
**Problema**: Arquivo aberto ou sem permissão  
**Soluções**: Feche o arquivo, use outro nome ou execute como admin

---

## Dicas e Boas Práticas

### Maximizar o Sucesso
- **Pesquise Usuários**: Verifique nomes no perfil do DeviantArt
- **Planeje Exportações**: Use nomes descritivos e datas
- **Desempenho**: Conexão estável e feche outros apps
- **Backup**: Armazene cópias em nuvem

### Ética de Uso
- **Somente Dados Públicos**
- **Não viole limites da API**
- **Dê Crédito aos Artistas**
- **Respeite Privacidade e Direitos Autorais**

---

## Perguntas Frequentes (FAQ)

**O aplicativo é gratuito?**  
Sim, mas você precisa de suas próprias credenciais da API DeviantArt.

**Posso raspar conteúdo privado?**  
Não, apenas conteúdo público.

**Meus dados estão seguros?**  
Sim, credenciais criptografadas localmente via Windows.

**Posso rodar várias instâncias?**  
Não, apenas uma por vez.

**E se a API mudar?**  
Baixe a nova versão atualizada.

---

## Suporte e Atualizações

### Ajuda
- Consulte este manual
- Relate bugs no repositório

### Atualizações
- Verifique novas versões regularmente
- Mudanças na API podem exigir atualização

### Contribuição
- Envie relatórios detalhados de bugs
- Sugira novas funcionalidades
- Ajude a melhorar a documentação

---

*DeviantArt Scrapper v0.1.25.1005 - Ferramenta Profissional de Coleta de Dados do DeviantArt*

