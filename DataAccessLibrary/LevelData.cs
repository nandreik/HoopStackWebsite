using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary
{
    public class LevelData
    {
        private readonly ISqlDataAccess _db;

        public LevelData(ISqlDataAccess db)
        {
            _db = db;
        }

        public Task<List<LevelModel>> GetLevels()
        {
            string sql = "select * from dbo.Level";
            return _db.LoadData<LevelModel, dynamic>(sql, new { });
        }

        public Task InsertLevel(LevelModel level)
        {
            string sql = @"insert into dbo.Level (LevelNum, NumStacks, Stack1, Stack2, Stack3, Stack4, Stack5, Stack6, Stack7, Stack8, Stack9, Stack10)
                            values (@LevelNum, @NumStacks, @Stack1, @Stack2, @Stack3, @Stack4, @Stack5, @Stack6, @Stack7, @Stack8, @Stack9, @Stack10);";

            return _db.SaveData(sql, level);
        }
    }
}
