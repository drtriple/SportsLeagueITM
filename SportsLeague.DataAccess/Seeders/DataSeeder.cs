using SportsLeague.DataAccess.Context;
using SportsLeague.Domain.Entities;
using SportsLeague.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace SportsLeague.DataAccess.Seeders
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(LeagueDbContext context)
        {
            // Only run if there are no computers (empty database)
            if (await context.Teams.AnyAsync()) return;

            // TEAMS (Liga BetPlay 2026)
            var teams = new List<Team>
            {
                new() { Name="Atlético Nacional", City="Medellín", Stadium="Atanasio Girardot", FoundedDate=new DateTime(1947, 3, 7), LogoUrl="https://upload.wikimedia.org/wikipedia/commons/d/d7/Atl%C3%A9tico_Nacional.png" },
                new() { Name="Independiente Medellín", City="Medellín", Stadium="Atanasio Girardot", FoundedDate=new DateTime(1913, 11, 14), LogoUrl="https://upload.wikimedia.org/wikipedia/commons/thumb/c/c2/Escudo_del_Deportivo_Independiente_Medell%C3%ADn.png/500px-Escudo_del_Deportivo_Independiente_Medell%C3%ADn.png" },
                new() { Name="América de Cali", City="Cali", Stadium="Pascual Guerrero", FoundedDate=new DateTime(1927, 2, 13), LogoUrl="https://www.americadecali.com/cdn/shop/files/logo-05_0dc22c0f-05ad-4774-b78e-d47922c5d864.png?v=1717606020" },
                new() { Name="Deportivo Cali", City="Cali", Stadium="Deportivo Cali", FoundedDate=new DateTime(1912, 5, 23), LogoUrl="https://upload.wikimedia.org/wikipedia/commons/4/4a/Escudo_Deportivo_Cali.png" },
                new() { Name="Junior FC", City="Barranquilla", Stadium="Metropolitano", FoundedDate=new DateTime(1924, 8, 7), LogoUrl="https://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Escudo_de_Atl%C3%A9tico_Junior.svg/1280px-Escudo_de_Atl%C3%A9tico_Junior.svg.png" },
                new() { Name="Millonarios FC", City="Bogotá", Stadium="El Campín", FoundedDate=new DateTime(1946, 6, 18), LogoUrl="https://upload.wikimedia.org/wikipedia/commons/8/8d/Millonarios_F%C3%BAtbol_Club_logo.png" },
                new() { Name="Independiente Santa Fe", City="Bogotá", Stadium="El Campín", FoundedDate=new DateTime(1941, 2, 28), LogoUrl="https://upload.wikimedia.org/wikipedia/commons/5/58/Escudo_de_Independiente_Santa_Fe.png" },
                new() { Name="Deportes Tolima", City="Ibagué", Stadium="Manuel Murillo Toro", FoundedDate=new DateTime(1954, 12, 18), LogoUrl="https://upload.wikimedia.org/wikipedia/commons/thumb/4/4a/Escudo_del_Deportes_Tolima.svg/1920px-Escudo_del_Deportes_Tolima.svg.png" },
                new() { Name="Atlético Bucaramanga", City="Bucaramanga", Stadium="Américo Montanini", FoundedDate=new DateTime(1949, 5, 11), LogoUrl="https://upload.wikimedia.org/wikipedia/commons/d/d3/Escudo_Atl%C3%A9tico_Bucaramanga_2024_-_II.png" },
                new() { Name="Once Caldas", City="Manizales", Stadium="Palogrande", FoundedDate=new DateTime(1961, 2, 15), LogoUrl="https://upload.wikimedia.org/wikipedia/commons/thumb/9/9a/Once_Caldas_logo-svg.svg/1280px-Once_Caldas_logo-svg.svg.png" },
                new() { Name="Deportivo Pasto", City="Pasto", Stadium="Departamental Libertad", FoundedDate=new DateTime(1949, 5, 12), LogoUrl="https://upload.wikimedia.org/wikipedia/commons/6/6b/Deportivo_Pasto_logo.png" },
                new() { Name="Deportivo Pereira", City="Pereira", Stadium="Hernán Ramírez Villegas", FoundedDate=new DateTime(1944, 11, 12), LogoUrl="https://upload.wikimedia.org/wikipedia/commons/0/08/EscudoDP.png" },
                new() { Name="Águilas Doradas", City="Sincelejo", Stadium="Arturo Cumplido Sierra", FoundedDate=new DateTime(2008, 1, 1), LogoUrl="https://dimayor.com.co/wp-content/uploads/2026/02/LOOGO-1.png" },
                new() { Name="Boyacá Chicó FC", City="Tunja", Stadium="La Independencia", FoundedDate=new DateTime(2002, 7, 20), LogoUrl="https://dimayor.com.co/wp-content/uploads/2024/06/Boyaca-chico.png" },
                new() { Name="Jaguares de Córdoba", City="Montería", Stadium="Jaraguay", FoundedDate=new DateTime(2012, 7, 15), LogoUrl="https://upload.wikimedia.org/wikipedia/pt/9/9c/JaguaresDeCordoba.png" },
                new() { Name="Alianza FC", City="Valledupar", Stadium="Armando Maestre", FoundedDate=new DateTime(2003, 8, 15), LogoUrl="https://alianzafc.com.co/web/assets/images/equipojuegos/LOGOALIANZAFULL.png" },
                new() { Name="Fortaleza CEIF", City="Bogotá", Stadium="Metropolitano de Techo", FoundedDate=new DateTime(2019, 1, 1), LogoUrl="https://fortalezaceif.com.co/wp-content/uploads/2024/07/Fortaleza-CEIF.png" },
                new() { Name="Llaneros FC", City="Villavicencio", Stadium="Bello Horizonte", FoundedDate=new DateTime(2012, 4, 20), LogoUrl="https://upload.wikimedia.org/wikipedia/en/3/3e/Llaneros_F.C.png" },
                new() { Name="Cúcuta Deportivo", City="Cúcuta", Stadium="General Santander", FoundedDate=new DateTime(1924, 1, 1), LogoUrl="https://upload.wikimedia.org/wikipedia/commons/9/97/Cucuta_Deportivo_2017_Oficial_Logo.png" },
                new() { Name="Internacional de Bogotá", City="Bogotá", Stadium="Estadio Metropolitano de Techo", FoundedDate=new DateTime(2025, 12, 10), LogoUrl="https://upload.wikimedia.org/wikipedia/commons/0/04/Logo_Internacional_de_Bogota_Oficial.png" },
            };

            context.Teams.AddRange(teams);
            await context.SaveChangesAsync();

            // PLAYERS
            var playersData = new (string First, string Last, PlayerPosition Pos, int Number)[][]
            {
                // 1. Atlético Nacional
                new[] {
                    ("David", "Ospina", PlayerPosition.Goalkeeper, 1),
                    ("William", "Tesillo", PlayerPosition.Defender, 3),
                    ("Edwin", "Cardona", PlayerPosition.Midfielder, 10),
                    ("Alfredo", "Morelos", PlayerPosition.Forward, 9),
                    ("Harlen", "Castillo", PlayerPosition.Goalkeeper, 25),
                    ("Juan Manuel", "Zapata", PlayerPosition.Midfielder, 8)
                },
                // 2. Independiente Medellín
                new[] {
                    ("Eder", "Chaux", PlayerPosition.Goalkeeper, 1),
                    ("Fainer", "Torijano", PlayerPosition.Defender, 2),
                    ("Jaime", "Alvarado", PlayerPosition.Midfielder, 15),
                    ("Brayan", "León", PlayerPosition.Forward, 27),
                    ("Leyser", "Chaverra", PlayerPosition.Defender, 13),
                    ("Diego", "Moreno", PlayerPosition.Midfielder, 8)
                },
                // 3. América de Cali
                new[] {
                    ("Joel", "Graterol", PlayerPosition.Goalkeeper, 1),
                    ("Daniel", "Bocanegra", PlayerPosition.Defender, 2),
                    ("Éder", "Álvarez", PlayerPosition.Defender, 3),
                    ("Alexis", "Zapata", PlayerPosition.Midfielder, 21),
                    ("Adrián", "Ramos", PlayerPosition.Forward, 20),
                    ("Duván", "Vergara", PlayerPosition.Forward, 11)
                },
                // 4. Deportivo Cali
                new[] {
                    ("Alejandro", "Rodríguez", PlayerPosition.Goalkeeper, 1),
                    ("Francisco", "Meza", PlayerPosition.Defender, 3),
                    ("Alexander", "Mejía", PlayerPosition.Midfielder, 13),
                    ("Andrés", "Andrade", PlayerPosition.Midfielder, 10),
                    ("Fredy", "Montero", PlayerPosition.Forward, 17),
                    ("Jarlan", "Barrera", PlayerPosition.Midfielder, 7)
                },
                // 5. Junior FC
                new[] {
                    ("Santiago", "Mele", PlayerPosition.Goalkeeper, 77),
                    ("Emanuel", "Olivera", PlayerPosition.Defender, 18),
                    ("Víctor", "Cantillo", PlayerPosition.Midfielder, 24),
                    ("Yairo", "Moreno", PlayerPosition.Midfielder, 30),
                    ("Carlos", "Bacca", PlayerPosition.Forward, 70),
                    ("José", "Enamorado", PlayerPosition.Forward, 99)
                },
                // 6. Millonarios FC
                new[] {
                    ("Álvaro", "Montero", PlayerPosition.Goalkeeper, 31),
                    ("Juan Pablo", "Vargas", PlayerPosition.Defender, 4),
                    ("Daniel", "Giraldo", PlayerPosition.Midfielder, 8),
                    ("David", "Silva", PlayerPosition.Midfielder, 14),
                    ("Radamel Falcao", "García", PlayerPosition.Forward, 9),
                    ("Leonardo", "Castro", PlayerPosition.Forward, 23)
                },
                // 7. Independiente Santa Fe
                new[] {
                    ("Andrés", "Mosquera", PlayerPosition.Goalkeeper, 1),
                    ("Facundo", "Agüero", PlayerPosition.Defender, 4),
                    ("Hugo", "Rodallega", PlayerPosition.Forward, 9),
                    ("Daniel", "Torres", PlayerPosition.Midfielder, 16),
                    ("Juan Pablo", "Zuluaga", PlayerPosition.Midfielder, 23),
                    ("Elvis", "Perlaza", PlayerPosition.Defender, 22)
                },
                // 8. Deportes Tolima
                new[] {
                    ("William", "Cuesta", PlayerPosition.Goalkeeper, 1),
                    ("Marlon", "Torres", PlayerPosition.Defender, 2),
                    ("Yeison", "Guzmán", PlayerPosition.Midfielder, 10),
                    ("Brayan", "Gil", PlayerPosition.Forward, 9),
                    ("Junior", "Hernández", PlayerPosition.Defender, 20),
                    ("Cristian", "Trujillo", PlayerPosition.Midfielder, 6)
                },
                // 9. Atlético Bucaramanga
                new[] {
                    ("Aldair", "Quintana", PlayerPosition.Goalkeeper, 12),
                    ("Jefferson", "Mena", PlayerPosition.Defender, 6),
                    ("Fabry", "Castro", PlayerPosition.Midfielder, 5),
                    ("Frank", "Castañeda", PlayerPosition.Forward, 10),
                    ("Andrés", "Ponce", PlayerPosition.Forward, 9),
                    ("Freddy", "Hinestroza", PlayerPosition.Midfielder, 17)
                },
                // 10. Once Caldas
                new[] {
                    ("James", "Aguirre", PlayerPosition.Goalkeeper, 12),
                    ("Sergio", "Palacios", PlayerPosition.Defender, 34),
                    ("Juan David", "Cuesta", PlayerPosition.Defender, 2),
                    ("Mateo", "García", PlayerPosition.Midfielder, 14),
                    ("Dayro", "Moreno", PlayerPosition.Forward, 17),
                    ("Michael", "Barrios", PlayerPosition.Forward, 7)
                },
                // 11. Deportivo Pasto
                new[] {
                    ("Diego", "Martínez", PlayerPosition.Goalkeeper, 1),
                    ("Nicolás", "Gil", PlayerPosition.Defender, 5),
                    ("Víctor", "Mejía", PlayerPosition.Midfielder, 13),
                    ("Ray", "Vanegas", PlayerPosition.Forward, 10),
                    ("Daniel", "Moreno", PlayerPosition.Forward, 7),
                    ("Cristian", "Arrieta", PlayerPosition.Defender, 20)
                },
                // 12. Deportivo Pereira
                new[] {
                    ("Salvador", "Ichazo", PlayerPosition.Goalkeeper, 1),
                    ("Jean", "Pestaña", PlayerPosition.Defender, 2),
                    ("Juan David", "Ríos", PlayerPosition.Midfielder, 14),
                    ("Darwin", "Quintero", PlayerPosition.Forward, 37),
                    ("Gonzalo", "Lencina", PlayerPosition.Forward, 9),
                    ("Jorge", "Bermúdez", PlayerPosition.Midfielder, 10)
                },
                // 13. Águilas Doradas
                new[] {
                    ("José", "Contreras", PlayerPosition.Goalkeeper, 1),
                    ("Jeison", "Quiñones", PlayerPosition.Defender, 3),
                    ("Guillermo", "Celis", PlayerPosition.Midfielder, 8),
                    ("Jorge", "Ramos", PlayerPosition.Forward, 9),
                    ("Fredy", "Salazar", PlayerPosition.Forward, 20),
                    ("Víctor", "Moreno", PlayerPosition.Defender, 4)
                },
                // 14. Boyacá Chicó FC
                new[] {
                    ("Rogerio", "Caicedo", PlayerPosition.Goalkeeper, 1),
                    ("Henry", "Plazas", PlayerPosition.Defender, 3),
                    ("Frank", "Lozano", PlayerPosition.Midfielder, 6),
                    ("Sebastian", "Támara", PlayerPosition.Midfielder, 10),
                    ("Wilmar ", "Cruz ",    PlayerPosition.Forward, 9),
                    ("Geimer ", "Balanta ", PlayerPosition.Forward, 7)
                },
                // 15. Jaguares de Córdoba
                new[] {
                    ("Geovanni", "Banguera", PlayerPosition.Goalkeeper, 1),
                    ("Oscar", "Vanegas", PlayerPosition.Defender, 4),
                    ("Didier", "Pino", PlayerPosition.Midfielder, 6),
                    ("Juan", "Roa", PlayerPosition.Midfielder, 13),
                    ("Wilson", "Morelo", PlayerPosition.Forward, 19),
                    ("Damir", "Ceter", PlayerPosition.Forward, 9)
                },
                // 16. Alianza FC
               new[] {
                    ("Carlos", "Mosquera", PlayerPosition.Goalkeeper, 1),
                    ("Pedro", "Franco", PlayerPosition.Defender, 22),
                    ("Rubén", "Manjarrés", PlayerPosition.Midfielder, 10),
                    ("Leonardo", "Saldaña", PlayerPosition.Defender, 16),
                    ("Mayer", "Gil", PlayerPosition.Forward, 7),
                    ("Misael", "Martínez", PlayerPosition.Forward, 9)
                },
                // 17. Fortaleza FC
                new[] {
                    ("Juan", "Castillo", PlayerPosition.Goalkeeper, 1),
                    ("Iván", "Anderson", PlayerPosition.Defender, 2),
                    ("Leonardo", "Pico", PlayerPosition.Midfielder, 14),
                    ("Sebastián", "Navarro", PlayerPosition.Midfielder, 10),
                    ("Santiago", "Córdoba", PlayerPosition.Forward, 11),
                    ("Nicolás", "Rodríguez", PlayerPosition.Forward, 7)
                },
                // 18. Llaneros FC
                new[] {
                    ("Arled", "Cadavid", PlayerPosition.Goalkeeper, 1),
                    ("Miller", "Mosquera", PlayerPosition.Defender, 4),
                    ("Marlon", "Sierra", PlayerPosition.Midfielder, 6),
                    ("Bryan", "Urueña", PlayerPosition.Midfielder, 10),
                    ("Néider", "Stiven", PlayerPosition.Forward, 11),
                    ("Jhildrey", "Lasso",   PlayerPosition.Defender ,   2 )
                },
                // 19. Cúcuta Deportivo
                new[] {
                    ("Juanito", "Moreno",   PlayerPosition.Goalkeeper , 1),
                    ("Hernán ", "Pertuz ",  PlayerPosition.Defender ,   23),
                    ("Felipe ", "Gómez ",   PlayerPosition.Midfielder , 8 ),
                    ("Luis ", "Zúñiga ",    PlayerPosition.Forward ,    9 ),
                    ("Cristian ", "Álvarez ",   PlayerPosition.Midfielder , 10),
                    ("Mauricio ", "Duarte ",    PlayerPosition.Defender ,   20)
                },
                // 20. Internacional de Bogotá
                new[] {
                    ("Enrique", "Almeida", PlayerPosition.Goalkeeper, 1),
                    ("Santiago", "Arias", PlayerPosition.Defender, 4),
                    ("José", "Leudo", PlayerPosition.Midfielder, 14),
                    ("Jean", "Colorado", PlayerPosition.Midfielder, 6),
                    ("Carlos", "Darwin", PlayerPosition.Forward, 10),
                    ("Mauricio", "Castaño", PlayerPosition.Defender, 15)
                },
            };

            var players = new List<Player>();
            for (int i = 0; i < teams.Count; i++)
            {
                foreach (var pd in playersData[i])
                {
                    players.Add(new Player
                    {
                        FirstName = pd.First,
                        LastName = pd.Last,
                        Number = pd.Number,
                        Position = pd.Pos,
                        BirthDate = new DateTime(1995, 1, 1).AddMonths(players.Count),
                        TeamId = teams[i].Id
                    });
                }
            }

            context.Players.AddRange(players);
            await context.SaveChangesAsync();

            // REFEREES
            var referees = new List<Referee>
            {
                new() { FirstName="Wilmar", LastName="Roldán", Nationality="Colombia" },
                new() { FirstName="Andrés", LastName="Rojas", Nationality="Colombia" },
                new() { FirstName="Carlos", LastName="Betancur", Nationality="Colombia" },
                new() { FirstName="Jhon", LastName="Hinestroza", Nationality="Colombia" },
            };

            context.Referees.AddRange(referees);
            await context.SaveChangesAsync();

            // TOURNAMENT
            var tournament = new Tournament
            {
                Name = "Liga BetPlay 2026-I",
                Season = "2026-I",
                StartDate = new DateTime(2026, 1, 16),
                EndDate = new DateTime(2026, 6, 5),
                Status = TournamentStatus.InProgress
            };

            context.Tournaments.Add(tournament);
            await context.SaveChangesAsync();

            // REGISTER THE 20 TEAMS
            foreach (var team in teams)
            {
                context.TournamentTeams.Add(new TournamentTeam
                {
                    TournamentId = tournament.Id,
                    TeamId = team.Id
                });
            }

            await context.SaveChangesAsync();
        }
    }
}