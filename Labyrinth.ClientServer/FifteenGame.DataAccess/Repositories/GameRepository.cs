using FifteenGame.Common.Definitions;
using FifteenGame.Common.Dtos;
using FifteenGame.Common.Repositories;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FifteenGame.DataAccess.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly string ConnectionString = ConfigurationManager.ConnectionStrings["Main"].ConnectionString;
            //@"Server=localhost;Port=5432;Database=FifteenGame1Dev22;User Id=postgres;Password=Qwerty123;";

        public GameDto GetByGameId(int gameId)
        {
            var selectQuery = @"
select
    g.""Id"",
    g.""UserId"",
    g.""MoveCount"",
    g.""GameBegin"",
    g.""HealCount"",
    g.""ExitRow"", 
    g.""ExitCol"", 
    g.""PlaerRow"",
    g.""PlaerCol"",
    c.""Row"",
    c.""Column"",
    c.""Value""
from 
    ""Games"" g
    join ""Cells"" c on g.""Id"" = c.""GameId""
where
    g.""Id"" = @gameId
";

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                GameDto result = null;
                connection.Open();

                using (var command = new NpgsqlCommand(selectQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("gameId", gameId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (result == null)
                            {
                                result = new GameDto();
                                result.Id = reader.GetInt32(0);
                                result.UserId = reader.GetInt32(1);
                                result.MoveCount = reader.GetInt32(2);
                                result.GameBegin = reader.GetDateTime(3);
                                result.HealCount =reader.GetInt32(4);
                                result.ExitRow = reader.GetInt32(5);  
                                result.ExitCol = reader.GetInt32(6);  
                                result.PlaerRow = reader.GetInt32(7);  
                                result.PlaerCol= reader.GetInt32(8);

                               
                            }

                            var rowVal = reader.GetInt32(9);
                            var columnVal = reader.GetInt32(10);
                            var value = reader.GetString(11);

                            result.Cells[rowVal, columnVal] = value;
                        }
                    }
                }

                return result;
            }
        }

        public IEnumerable<GameDto> GetByUserId(int userId)
        {
            var selectQuery = @"
select
    g.""Id"",
    g.""UserId"",
    g.""MoveCount"",
    g.""GameBegin"",
    g.""HealCount"",
    g.""ExitRow"", 
    g.""ExitCol"", 
    g.""PlaerRow"",
    g.""PlaerCol"",
    c.""Row"",
    c.""Column"",
    c.""Value""  
from 
    ""Games"" g
    join ""Cells"" c on g.""Id"" = c.""GameId""
where
    g.""UserId"" = @userId
order by
    g.""Id""
";

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                List<GameDto> result = new List<GameDto>();
                connection.Open();

                using (var command = new NpgsqlCommand(selectQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("userId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        GameDto gameDto = null;

                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);

                            gameDto = result.FirstOrDefault(g => g.Id == id);
                            if (gameDto == null)
                            {
                                gameDto = new GameDto();
                                result.Add(gameDto);

                                gameDto.Id = id;
                                gameDto.UserId = reader.GetInt32(1);
                                gameDto.MoveCount = reader.GetInt32(2);
                                gameDto.GameBegin = reader.GetDateTime(3);
                                gameDto.HealCount = reader.GetInt32(4);
                                gameDto.ExitRow= reader.GetInt32(5);
                                gameDto.ExitCol = reader.GetInt32(6);
                                gameDto.PlaerRow= reader.GetInt32(7);
                                gameDto.PlaerCol = reader.GetInt32(8);
                               
                            }

                            var rowVal = reader.GetInt32(9);
                            var columnVal = reader.GetInt32(10);
                            var value = reader.GetString(11);

                            gameDto.Cells[rowVal, columnVal] = value;
                        }
                    }
                }

                return result;
            }
        }

        public void Remove(int gameId)
        {
            var removeCellsQuery = @"
delete from ""Cells""
where ""GameId"" = @gameId
";

            var removeGameQuery = @"
delete from ""Games""
where ""Id"" = @gameId
";

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = new NpgsqlCommand(removeCellsQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("gameId", gameId);

                    var insertResult = command.ExecuteNonQuery();
                }

                using (var command = new NpgsqlCommand(removeGameQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("gameId", gameId);

                    var insertResult = command.ExecuteNonQuery();
                }
            }
        }

        public int Save(GameDto gameDto)
        {
            if (gameDto.Id == 0)
            {
                return Create(gameDto);
            }

            return Update(gameDto);
        }

        private int Create(GameDto gameDto)
        {
            var insertGameQuery = @"
insert into ""Games"" (""UserId"", ""MoveCount"",""GameBegin"",""HealCount"" ,""ExitRow"", ""ExitCol"", ""PlaerRow"",""PlaerCol"")
values (@userId, @moveCount, @gameBegin,@healcount,@exitrow,@exitcol,@plaerrow,@plaercol)
returning ""Id""
";

            var insertCellQuery = @"
insert into ""Cells"" (""GameId"", ""Row"", ""Column"", ""Value"")
values (@gameId, @row, @column, @value)
";

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                int gameId;
                connection.Open();

                using (var command = new NpgsqlCommand(insertGameQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("userId", gameDto.UserId);
                    command.Parameters.AddWithValue("moveCount", gameDto.MoveCount);
                    command.Parameters.AddWithValue("gameBegin", gameDto.GameBegin);
                    command.Parameters.AddWithValue("healcount", gameDto.HealCount);
                    command.Parameters.AddWithValue("exitrow", gameDto.ExitRow);
                    command.Parameters.AddWithValue("exitcol", gameDto.ExitCol);
                    command.Parameters.AddWithValue("plaerrow", gameDto.PlaerRow);
                    command.Parameters.AddWithValue("plaercol", gameDto.PlaerCol);

                    var insertResult = command.ExecuteScalar();
                    gameId = (int)insertResult;
                }

                for (int row = 0; row < Constants.RowCount; row++)
                {
                    for (int column = 0; column < Constants.ColumnCount; column++)
                    {
                      

                        using (var command = new NpgsqlCommand(insertCellQuery, connection))
                        {
                            command.CommandType = System.Data.CommandType.Text;
                            command.Parameters.AddWithValue("gameId", gameId);
                            command.Parameters.AddWithValue("row", row);
                            command.Parameters.AddWithValue("column", column);
                            command.Parameters.AddWithValue("value", gameDto.Cells[row, column]??string.Empty);
                            

                            command.ExecuteNonQuery();
                        }
                    }
                }

                return gameId;
            }
        }

        private int Update(GameDto gameDto)
        {
            var updateGameQuery = @"
update ""Games""
set
    ""MoveCount"" = @moveCount,
    ""HealCount"" = @healcount,
    ""PlaerRow"" = @plaerrow,
    ""PlaerCol"" = @plaercol
    
    
where
    ""Id"" = @gameId
";

            var removeCellsQuery = @"
delete from ""Cells""
where ""GameId"" = @gameId
";

            var insertCellQuery = @"
insert into ""Cells"" (""GameId"", ""Row"", ""Column"", ""Value"")
values (@gameId, @row, @column, @value)
";

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                int gameId = gameDto.Id; ;
                connection.Open();

                using (var command = new NpgsqlCommand(updateGameQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("gameId", gameId);
                    command.Parameters.AddWithValue("moveCount", gameDto.MoveCount);
                    command.Parameters.AddWithValue("healcount", gameDto.HealCount);
                    command.Parameters.AddWithValue("plaerrow", gameDto.PlaerRow);
                    command.Parameters.AddWithValue("plaercol", gameDto.PlaerCol);

                    var insertResult = command.ExecuteNonQuery();
                }

                using (var command = new NpgsqlCommand(removeCellsQuery, connection))
                {
                    command.CommandType = System.Data.CommandType.Text;
                    command.Parameters.AddWithValue("gameId", gameId);

                    var insertResult = command.ExecuteNonQuery();
                }

                for (int row = 0; row < Constants.RowCount; row++)
                {
                    for (int column = 0; column < Constants.ColumnCount; column++)
                    {
                       

                        using (var command = new NpgsqlCommand(insertCellQuery, connection))
                        {
                            command.CommandType = System.Data.CommandType.Text;
                            command.Parameters.AddWithValue("gameId", gameId);
                            command.Parameters.AddWithValue("row", row);
                            command.Parameters.AddWithValue("column", column);
                            command.Parameters.AddWithValue("value", gameDto.Cells[row, column]);

                            command.ExecuteNonQuery();
                        }
                    }
                }

                return gameId;
            }
        }
    }
}
