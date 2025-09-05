# Bazar Carioca
Conectando comerciante e consumidor

---

## O que é o Bazar Carioca?

### Resumo

Bazar Carioca é um site para comerciantes e consumidores.
* Comerciantes expõem seus negócios, serviços prestados, e produtos.
* Consumidores pesquisam o que querem, e podem encontrar lojas com o que quiserem.

### Os dois papéis

#### - Lojistas
Lojistas podem expor suas lojas, serviços e produtos seguindo esses passos:
1. Criar o seu perfil em "Criar conta"
2. Fazem cadastro com:
    1. **Nome**
    2. **Email**
    3. **Senha**
3. Criam sua(s) loja(s) no site
4. Inserem as informações da **loja**, sendo elas:
    1. **Serviços**
    2. **Tipos de produtos** (para agrupar os produtos)
    3. **Produtos**

(Caso tenham feito o cadastro anteriormente, lojistas podem entrar com seus dados em "Entrar")

#### - Consumidor
Consumidores não precisam fazer nenhum cadastro. Eles apenam veem o que os lojistam publicarem no site. Existem duas formas de achar conteúdo dos lojistas:
1. Pesquisando na barra de busca
2. Vendo as lojas recomendadas na página inicial

---

## Detalhes técnicos

### Sobre o Frontend
#### O principal
O site é uma Single Page Aplication (SPA). Que só tem esse tempo de resposta rápida graças à componentização e o uso de React Router.

#### Tecnologias utilizadas

![React](https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=61DAFB)
![React Router](https://img.shields.io/badge/React_Router-CA4245?style=for-the-badge&logo=react-router&logoColor=white)
![React Hooks](https://img.shields.io/badge/React%20Hooks-61DAFB?style=for-the-badge&logo=react&logoColor=white)
![TailwindCSS](https://img.shields.io/badge/Tailwind_CSS-38B2AC?style=for-the-badge&logo=tailwind-css&logoColor=white)
![Vite](https://img.shields.io/badge/Vite-646CFF?style=for-the-badge&logo=vite&logoColor=FFD62E)
![Vercel](https://img.shields.io/badge/Vercel-000000?style=for-the-badge&logo=vercel&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-007ACC?style=for-the-badge&logo=typescript&logoColor=white)
![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?style=for-the-badge&logo=javascript&logoColor=black)
![Context API](https://img.shields.io/badge/Context_API-61DAFB?style=for-the-badge&logo=react&logoColor=white)
![CVA](https://img.shields.io/badge/CVA-000000?style=for-the-badge&logo=typescript&logoColor=white)
![Axios](https://img.shields.io/badge/Axios-671ddf?style=for-the-badge&logo=axios&logoColor=white)
![Git](https://img.shields.io/badge/Git-F05033?style=for-the-badge&logo=git&logoColor=white)
![GitHub](https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white)


### Sobre o Backend
#### Resumo
- O backend foi feito em ASP.NET (.NET 8).
- GitHub actions para fazer o CI/CD (deploy automático no servidor)
- Uso de Bearer Token e ASP.NET User para autênticação e autorização
- A API foi hospedada em um Linux Ubuntu em um servidor AWS EC2
- Banco de dados: AWS RDS
- Armazenamento de imagem: AWS S3
- Utilizei programação assíncrona (async await) para garantir um tempo de resposta rápido
- Padronizei a comunicação Frontend X Backend Params pelo Body do request
- Utilizei IActionResult para uma resposta dinâmica com o Frontend

#### Tecnologias

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-512BD4?style=for-the-badge&logo=.net&logoColor=white)
![.NET 8](https://img.shields.io/badge/.NET_8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![AWS](https://img.shields.io/badge/AWS-232F3E?style=for-the-badge&logo=amazon-aws&logoColor=FF9900)
![SOLID](https://img.shields.io/badge/SOLID_Principles-2C3E50?style=for-the-badge&logo=buffer&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity_Framework_Core-512BD4?style=for-the-badge&logo=ef&logoColor=white)
![Postman](https://img.shields.io/badge/Postman-FF6C37?style=for-the-badge&logo=postman&logoColor=white)
![AutoMapper](https://img.shields.io/badge/AutoMapper-FF5733?style=for-the-badge&logo=nuget&logoColor=white)
![Git](https://img.shields.io/badge/Git-F05033?style=for-the-badge&logo=git&logoColor=white)
![GitHub](https://img.shields.io/badge/GitHub-181717?style=for-the-badge&logo=github&logoColor=white)
![GitHub Actions](https://img.shields.io/badge/GitHub_Actions-2088FF?style=for-the-badge&logo=github-actions&logoColor=white)

#### Endpoints

##### - Search (Busca)
| Método | Rota | Descrição |
|---:|---|---|
| GET | `/bazar-carioca/busca` | Pesquisa pelo nome |

##### - Auth
| Método | Rota | Descrição |
|---:|---|---|
| POST | `/bazar-carioca/autenticacao/registrar` | Registrar novo usuário |
| POST | `/bazar-carioca/autenticacao/login` | Autenticar usuário — retorna `accessToken` / `refreshToken` / `expiricy` |
| POST | `/bazar-carioca/autenticacao/refresh-token` | Trocar refresh token por novo access token |
| POST | `/bazar-carioca/autenticacao/revoke/{username}` | Revogar autenticação de um usuário |
| POST | `/bazar-carioca/autenticacao/create-role` | Criar nova role |
| POST | `/bazar-carioca/autenticacao/add-user-to-role` | Atribuir role a usuário |
| POST | `/bazar-carioca/autenticacao/add-user-to-shopkeeper` | Vincular usuário a lojista |

##### - Shopkeepers (Lojistas)
| Método | Rota | Descrição |
|---:|---|---|
| GET | `/bazar-carioca/lojistas` | Listar lojistas |
| GET | `/bazar-carioca/lojistas/email` | Buscar lojista por e-mail |
| GET | `/bazar-carioca/lojistas/{Id}` | Buscar lojista por ID |
| POST | `/bazar-carioca/lojistas` | Criar lojista |
| PATCH | `/bazar-carioca/lojistas/{Id}` | Atualizar lojista |
| DELETE | `/bazar-carioca/lojistas/{Id}` | Remover lojista |

##### - Stores (Lojas)
| Método | Rota | Descrição |
|---:|---|---|
| POST | `/bazar-carioca/lojas` | Criar loja |
| GET | `/bazar-carioca/lojas/{Id}` | Buscar loja por ID |
| PATCH | `/bazar-carioca/lojas/{Id}` | Atualizar loja |
| DELETE | `/bazar-carioca/lojas/{Id}` | Remover loja |

##### - Services (Serviços)
| Método | Rota | Descrição |
|---:|---|---|
| GET | `/bazar-carioca/servicos` | Listar serviços |
| GET | `/bazar-carioca/servicos/{Id}` | Buscar serviço por ID |
| POST | `/bazar-carioca/servicos` | Criar serviço |
| PATCH | `/bazar-carioca/servicos/{Id}` | Atualizar serviço |
| DELETE | `/bazar-carioca/servicos/{Id}` | Remover serviço |

##### - Product Types (Tipos de produtos)
| Método | Rota | Descrição |
|---:|---|---|
| GET | `/bazar-carioca/tipos-de-produtos` | Listar tipos de produto |
| GET | `/bazar-carioca/tipos-de-produtos/loja` | Listar tipos de produto por loja |
| GET | `/bazar-carioca/tipos-de-produtos/{Id}` | Buscar tipo por ID |
| POST | `/bazar-carioca/tipos-de-produtos` | Criar tipo de produto |
| PATCH | `/bazar-carioca/tipos-de-produtos/{Id}` | Atualizar tipo de produto |
| DELETE | `/bazar-carioca/tipos-de-produtos/{Id}` | Remover tipo de produto |

##### - Products (Produtos)
| Método | Rota | Descrição |
|---:|---|---|
| GET | `/bazar-carioca/produtos` | Listar produtos (paginado / filtro) |
| GET | `/bazar-carioca/produtos/tipo-de-produto/{Id}` | Listar produtos por tipo |
| GET | `/bazar-carioca/produtos/{Id}` | Buscar produto por ID |
| POST | `/bazar-carioca/produtos` | Criar produto |
| PATCH | `/bazar-carioca/produtos/{Id}` | Atualizar parcialmente produto |
| DELETE | `/bazar-carioca/produtos/{Id}` | Remover produto |
