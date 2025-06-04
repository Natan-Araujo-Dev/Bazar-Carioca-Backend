using BazarCarioca.WebAPI.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BazarCarioca.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SQLController : ControllerBase
    {
        private readonly AppDbContext DataBase;
        private readonly ProductsController ProductsController;
        private readonly StoresController StoresController;

        public SQLController(AppDbContext _DataBase, ProductsController _ProductsController, StoresController _StoresController)
        {
            DataBase = _DataBase;
            ProductsController = _ProductsController;
            StoresController = _StoresController;
        }

        [HttpPost]
        public async Task<IActionResult> RepopulateAll()
        {
            // Esses dois são apagados fora do SQL pois possuem imagens no WebService
            await ProductsController.DeleteAll();
            await StoresController.DeleteAll();

            await DataBase.Database.ExecuteSqlRawAsync(
                "-- Liberando a questão das keys antes de apagar\r\n" +
                "SET FOREIGN_KEY_CHECKS = 0;\r\n\r\n" +
                "-- Apagando em si\r\n" +
                "TRUNCATE TABLE `Bazar-Carioca-AWS-RDS`.Shopkeepers;" +
                "TRUNCATE TABLE `Bazar-Carioca-AWS-RDS`.Stores;" +
                "TRUNCATE TABLE `Bazar-Carioca-AWS-RDS`.Services;\r\n" +
                "TRUNCATE TABLE `Bazar-Carioca-AWS-RDS`.ProductTypes;\r\n" +
                "TRUNCATE TABLE `Bazar-Carioca-AWS-RDS`.Products;" +
                " -- Voltando ao normal\r\n" +
                "SET FOREIGN_KEY_CHECKS = 1;\r\n\r\n" +
                "-- /* apague o comentário do comentário se quiser só deletar o BDD\r\n\r\n" +
                "-- Shopkeepers\r\n" +
                "INSERT INTO `Bazar-Carioca-AWS-RDS`.Shopkeepers (Name, Email, Password)\r\n" +
                "VALUES\r\n\t" +
                "('Ana Silva',      'ana.silva@example.com',     'SenhaForte1_'),\r\n\t" +
                "('Bruno Costa',    'bruno.costa@example.com',   'SenhaForte2_'),\r\n\t" +
                "-- seus shopkeepers aqui embaixo\r\n\t" +
                "('Carla Santos',   'carla.santos@example.com',  'SenhaForte3_'),\r\n\t" +
                "('Diego Lima',     'diego.lima@example.com',    'SenhaForte4_'),\r\n\t" +
                "('Eduardo Alves',  'eduardo.alves@example.com', 'SenhaForte5_'),\r\n\t" +
                "('Juliana Oliveira','juliana.oliveira@example.com','SenhaForte6_');\r\n\r\n" +
                "-- Stores\r\n" +
                "INSERT INTO `Bazar-Carioca-AWS-RDS`.Stores\r\n" +
                "  (Name, Description, CellphoneNumber,\r\n" +
                "   Neighborhood, Street, Number,\r\n" +
                "   OpeningTime, ClosingTime, ShopkeeperId)\r\n" +
                "VALUES\r\n" +
                "  ('Dimitri', 'Uma loja especializada em moda feminina', 923456789, 'Barra da tijuca', 'Rua A', 100, '08:00', '18:00', 1),\r\n" +
                "  ('Casa do celular', 'Nós buscamos dar o melhor atendimento para todos que nos visitam!', 928475124, 'Campo Grande', 'Estrada do Tinguí', 218, '09:30', '19:00', 2),\r\n" +
                "  ('Mundial acabamentos', 'Focada em acabamentos e decoração de casas', 901294876, 'Bangu', 'Roque Barbosa', 82, '10:00', '20:00', 2),\r\n" +
                "  -- suas stores aqui embaixo\r\n" +
                "  ('Loja Gourmet',       'Especializada em ingredientes finos e importados',  912345678, 'Leblon',           'Rua Rainha Guilhermina',        55,  '10:00', '22:00', 3),\r\n" +
                "  ('Tech Solutions',     'Suporte e manutenção de hardware e software',       923456780, 'Copacabana',       'Rua Barata Ribeiro',            120, '09:00', '19:00', 3),\r\n" +
                "  ('Ateliê da Costura',  'Conserto e criação de roupas sob medida',           934567801, 'Ipanema',          'Avenida Visconde de Pirajá',     200, '08:30', '18:30', 4),\r\n" +
                "  ('Game Master',        'Revenda de jogos e acessórios para gamers',         945678912, 'Botafogo',         'Rua São Clemente',               80,  '11:00', '21:00', 4),\r\n" +
                "  ('Casa Conforto',      'Decoração e utilidades domésticas',                 956789023, 'Tijuca',           'Rua Conde de Bonfim',            150, '09:00', '18:00', 5),\r\n" +
                "  ('Beleza & Cia',       'Produtos de beleza, estética e perfumaria',         967890134, 'Barra da Tijuca',  'Estrada da Barra',               330, '10:00', '20:00', 6);\r\n\r\n" +
                "-- Services\r\n" +
                "INSERT INTO `Bazar-Carioca-AWS-RDS`.Services (Name, AveragePrice, StoreId)\r\n" +
                "VALUES\r\n" +
                "    ('Remendo', 25.00, 1),\r\n" +
                "    ('Alfaiate', 55.05, 1),\r\n" +
                "    ('Medição', 5.50, 1),\r\n\r\n" +
                "    ('Manutenção de Notebook', 150.05, 2),\r\n" +
                "    ('Reparo de celular', 75.50, 2),\r\n" +
                "    ('Instalação completa Windows 10', 15.25, 2),\r\n\r\n" +
                "    ('Troca de piso', 120, 3),\r\n" +
                "    ('Orçamento', 05.99, 3),\r\n" +
                "    -- seus services aqui embaixo\r\n" +
                "    ('Degustação guiada', 80.00, 4),\r\n" +
                "    ('Montagem de cesta gourmet', 120.00, 4),\r\n" +
                "    ('Curso de culinária internacional', 200.00, 4),\r\n\r\n" +
                "    ('Formatação Express', 100.00, 5),\r\n" +
                "    ('Recuperação de arquivos', 150.00, 5),\r\n" +
                "    ('Instalação de rede doméstica', 180.00, 5),\r\n\r\n" +
                "    ('Ajuste de vestidos de festa', 75.00, 6),\r\n" +
                "    ('Reforma de roupas antigas', 65.00, 6),\r\n" +
                "    ('Customização de acessórios', 50.00, 6),\r\n\r\n" +
                "    ('Venda de jogos importados', 250.00, 7),\r\n" +
                "    ('Aluguel de consoles', 80.00, 7),\r\n" +
                "    ('Configuração de streaming', 120.00, 7),\r\n\r\n" +
                "    ('Consultoria de decoração', 300.00, 8),\r\n" +
                "    ('Montagem de móveis planejados', 400.00, 8),\r\n" +
                "    ('Instalação de luzes LED', 150.00, 8),\r\n\r\n" +
                "    ('Tratamento facial', 200.00, 9),\r\n" +
                "    ('Maquiagem profissional', 180.00, 9);\r\n\r\n" +
                "-- ProductTypes\r\n" +
                "INSERT INTO `Bazar-Carioca-AWS-RDS`.ProductTypes (Name, StoreId)\r\n" +
                "VALUES\r\n" +
                "    ('Saias', 1),\r\n" +
                "    ('Regatas', 1),\r\n\r\n" +
                "    ('Capas de celular', 2),\r\n" +
                "    ('Películas de celular', 2),\r\n\r\n" +
                "    ('Pisos', 3),\r\n    ('Pias', 3),\r\n" +
                "    -- seus producttypes aqui embaixo\r\n" +
                "    ('Queijos e vinhos', 4),\r\n" +
                "    ('Azeites importados', 4),\r\n\r\n" +
                "    ('Peças de PC', 5),\r\n" +
                "    ('Periféricos', 5),\r\n\r\n" +
                "    ('Vestidos de festa', 6),\r\n" +
                "    ('Tecidos exclusivos', 6),\r\n\r\n" +
                "    ('Jogos de tabuleiro', 7),\r\n" +
                "    ('Consoles retrô', 7),\r\n\r\n" +
                "    ('Papéis de parede', 8),\r\n" +
                "    ('Objetos decorativos', 8),\r\n\r\n" +
                "    ('Maquiagem', 9),\r\n" +
                "    ('Produtos de skincare', 9);\r\n\r\n\r\n"
            );

            return Ok("Tudo foi apagado, Ids voltaram ao 0, e tudo foi repovoado");
        }
    }
}
